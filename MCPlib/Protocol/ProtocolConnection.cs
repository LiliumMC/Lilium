using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MCPlib.Protocol
{
    class ProtocolConnection
    {
        public ProtocolConnection(TcpClient Client) : this(Client, 0,null) { }
        public ProtocolConnection(TcpClient Client, int ProtocolVersion,IMinecraftCo Handle)
        {
            this.c = Client;
            this.protocolversion = ProtocolVersion;
            this.handler = Handle;
        }

        private int protocolversion;
        private int compression_treshold = 0;
        private bool login_phase = true;
        TcpClient c;
        Thread netRead;

        private IMinecraftCo handler;
        public string Username;

        private void Receive(byte[] buffer, int start, int offset, SocketFlags f)
        {
            int read = 0;
            while (read < offset)
            {
                read += c.Client.Receive(buffer, start + read, offset - read, f);
            }
        }
        public bool Login(string Username)
        {
            this.Username = Username;
            byte[] protocol_version = getVarInt(protocolversion);
            string server_address = handler.getServerHost();
            byte[] server_port = BitConverter.GetBytes(handler.getServerPort()); Array.Reverse(server_port);
            byte[] next_state = getVarInt(2);
            byte[] handshake_packet = concatBytes(protocol_version, getString(server_address), server_port, next_state);
            SendPacket(0x00, handshake_packet);
            byte[] login_packet = getString(Username);
            SendPacket(0x00, login_packet);

            int packetID = -1;
            List<byte> packetData = new List<byte>();
            while (true)
            {
                readNextPacket(ref packetID, packetData);
                if (packetID == 0x00)
                {
                    handler.OnConnectionLost(Conn.DisconnectReason.LoginRejected, readNextString(packetData));
                    return false;
                }
                else if (packetID == 0x01)//Encrypt
                {
                    return false;
                }
                else if (packetID == 0x02)//Logined
                {
                    Debug.Log("Login Success");
                    login_phase = false;
                    handler.receivePacket(packetID, packetData);
                    StartUpdating();
                    return true;
                }
                else
                {
                    if (packetID == 0x03 && login_phase)
                    {
                        if(protocolversion >= Protocol.MC18Version)
                            compression_treshold = readNextVarInt(packetData);
                    }
                    handler.receivePacket(packetID, packetData);
                }
            }
        }
        public void StartUpdating()
        {
            netRead = new Thread(() =>
              {
                  try
                  {
                      int packetID = -1;
                      List<byte> packetData = new List<byte>();
                      while (true)
                      {
                          readNextPacket(ref packetID, packetData);
                          handler.receivePacket(packetID, packetData);
                      }
                  }
                  catch(Exception e)
                  {
                      handler.OnConnectionLost(Conn.DisconnectReason.ConnectionLost, e.Message);
                  }
              });
            netRead.Start();
        }
        private static byte[] concatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            return result.ToArray();
        }
        private static byte[] getVarInt(int paramInt)
        {
            List<byte> bytes = new List<byte>();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }
        private static byte[] getString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return concatBytes(getVarInt(bytes.Length), bytes);
        }
        private static byte readNextByte(List<byte> cache)
        {
            byte result = cache[0];
            cache.RemoveAt(0);
            return result;
        }
        private static byte[] readData(int offset, List<byte> cache)
        {
            byte[] result = cache.Take(offset).ToArray();
            cache.RemoveRange(0, offset);
            return result;
        }
        private static string readNextString(List<byte> cache)
        {
            int length = readNextVarInt(cache);
            if (length > 0)
            {
                return Encoding.UTF8.GetString(readData(length, cache));
            }
            else return "";
        }
        public int readNextVarIntRAW()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            byte[] tmp = new byte[1];
            while (true)
            {
                Receive(tmp, 0, 1, SocketFlags.None);
                k = tmp[0];
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        private static int readNextVarInt(List<byte> cache)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = readNextByte(cache);
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        private byte[] readDataRAW(int offset)
        {
            if (offset > 0)
            {
                try
                {
                    byte[] cache = new byte[offset];
                    Receive(cache, 0, offset, SocketFlags.None);
                    return cache;
                }
                catch (OutOfMemoryException) { }
            }
            return new byte[] { };
        }
        private void readNextPacket(ref int packetID, List<byte> packetData)
        {
            packetData.Clear();
            int size = readNextVarIntRAW(); //Packet size
            packetData.AddRange(readDataRAW(size)); //Packet contents
            //Handle packet decompression
            if (protocolversion >= Protocol.MC18Version
                && compression_treshold > 0)
            {
                int sizeUncompressed = readNextVarInt(packetData);
                if (sizeUncompressed != 0) // != 0 means compressed, let's decompress
                {
                    byte[] toDecompress = packetData.ToArray();
                    byte[] uncompressed = ZlibUtils.Decompress(toDecompress, sizeUncompressed);
                    packetData.Clear();
                    packetData.AddRange(uncompressed);
                }
            }

            packetID = readNextVarInt(packetData); //Packet ID
        }
        public void SendPacket(int packetID, IEnumerable<byte> packetData)
        {
            byte[] the_packet = concatBytes(getVarInt(packetID), packetData.ToArray());
            if (compression_treshold > 0)
            {
                if (the_packet.Length >= compression_treshold)
                {
                    byte[] compressed_packet = ZlibUtils.Compress(the_packet);
                    the_packet = concatBytes(getVarInt(the_packet.Length), compressed_packet);
                }
                else
                {
                    byte[] uncompressed_length = getVarInt(0); //Not compressed (short packet)
                    the_packet = concatBytes(uncompressed_length, the_packet);
                }
            }
            SendRAW(concatBytes(getVarInt(the_packet.Length), the_packet));
        }
        public void SendRAW(byte[] buffer)
        {
            c.Client.Send(buffer);
        }
        public static void doPing(string host,ushort port,ref byte[] data)
        {
            TcpClient tcp = new TcpClient();
            tcp.Connect(host,port);
            byte[] packet_id = getVarInt(0);
            byte[] protocol_version = getVarInt(-1);
            byte[] server_port = BitConverter.GetBytes(port); Array.Reverse(server_port);
            byte[] next_state = getVarInt(1);
            byte[] packet = concatBytes(packet_id, protocol_version, getString(host), server_port, next_state);
            byte[] tosend = concatBytes(getVarInt(packet.Length), packet);

            tcp.Client.Send(tosend, SocketFlags.None);

            byte[] status_request = getVarInt(0);
            byte[] request_packet = concatBytes(getVarInt(status_request.Length), status_request);

            tcp.Client.Send(request_packet, SocketFlags.None);

            ProtocolConnection ComTmp = new ProtocolConnection(tcp);
            int packetLength = ComTmp.readNextVarIntRAW();
            if (packetLength > 0)
            {
                data = ComTmp.readDataRAW(packetLength);
            }
            tcp.Close();
        }
        public void Dispose()
        {
            if (netRead != null)
                netRead.Abort();
            c.Close();
        }
    }
}
