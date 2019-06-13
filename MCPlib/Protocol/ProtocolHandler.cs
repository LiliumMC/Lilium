using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MCPlib.Protocol.PacketLib;

namespace MCPlib.Protocol
{
    class ProtocolHandler:IMinecraftCo
    {
        public ProtocolHandler(Socket client,MCServer server)
        {
            this.Client = client;
            this._Server = server;
            Lobby = Data.Servers.servers[ServerData.LobbyServer];
        }
        private Socket Client;
        private ProtocolConnection Proxy;
        private MCServer _Server;
        private Data.Servers.Server Lobby;
        private PacketProtocol protocol;
        private bool login_phase = true;
        private int compression_treshold=0;

        private Thread cRead;

        private void Receive(byte[] buffer, int start, int offset, SocketFlags f)
        {
            int read = 0;
            while (read < offset)
            {
                read += Client.Receive(buffer, start + read, offset - read, f);
            }
        }
        private void DoShakeHands(object state)
        {
            if (_Server._IsStarted)
            {
                int packet_len = readNextVarIntRAW();
                if (packet_len > 0)
                {
                    List<byte> packetData = new List<byte>(readDataRAW(packet_len));
                    int packet_id = readNextVarInt(packetData);
                    int protocol_ver= readNextVarInt(packetData);
                    string host = readNextString(packetData);
                    ushort port = readNextUShort(packetData);
                    Debug.Log(string.Format("Connected with {0}:{1}", host, port));
                    byte next_state = packetData[packetData.Count - 1];
                    if (next_state == 0x01)//PING
                    {
                        byte[] the_state = readDataRAW(readNextVarIntRAW());
                        responsePing();
                    }else if (next_state == 0x02)//Player
                    {
                        getProtocol(protocol_ver);
                        CreateProxyBridge(Lobby);
                    }
                }
            }
        }
        private void CreateProxyBridge(Data.Servers.Server server)
        {
            if (this.Client.Connected)
            {
                List<byte> packetData = new List<byte>(readDataRAW(readNextVarIntRAW()));
                int packet_id = readNextVarInt(packetData);
                string username = readNextString(packetData);

                Debug.Log("Player " + username + " Join the game.","Server");

                TcpClient remote = new TcpClient();
                try
                {
                    remote.Connect(server.Host,server.Port);
                    if (remote.Connected)
                    {
                        Proxy = new ProtocolConnection(remote, server.Protocol, this);
                        if(Proxy.Login(username))
                            handlePacket();
                    }
                }catch(Exception e)
                {
                    Debug.Log(e.Message, "Exception");
                    this.Close();
                }
            }
        }
        private void handlePacket()
        {
            cRead = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        int data_len = readNextVarIntRAW();
                        int packetID = 0;
                        List<byte> packetData = new List<byte>(readDataRAW(data_len));
                        if (compression_treshold > 0)
                        {
                            int packet_len = readNextVarInt(packetData);

                            if (packet_len > 0)//封包已压缩
                            {
                                byte[] uncompress = ZlibUtils.Decompress(packetData.ToArray());
                                packetData = new List<byte>(uncompress);
                                packetID = readNextVarInt(packetData);
                            }
                            else
                            {
                                packetID = readNextVarInt(packetData);
                            }
                        }
                        else
                        {
                            packetID = readNextVarInt(packetData);
                        }
                        var type = protocol.getPacketOutgoingType(packetID);
                        if (packetID == 0x03 && login_phase)
                        {
                            if (protocol.protocolVersion >= Protocol.MC18Version)
                                compression_treshold = readNextVarInt(packetData);
                        }
                        switch (type)
                        {
                            case Protocol.PacketOutgoingType.ChatMessage:
                                string chatmsg = readNextString(packetData);
                                Debug.Log("Chat:" + chatmsg, Proxy.Username);
                                if (chatmsg.StartsWith("/"))
                                {
                                    if (OnCommand(chatmsg))
                                        continue;
                                }
                                packetData = new List<byte>(getString(chatmsg));
                                break;

                        }
                        Proxy.SendPacket(packetID, packetData);
                    }
                }
                catch
                {
                    Close();
                }
            });
            cRead.Start();
        }

        private void getProtocol(int protocol_ver)
        {
            switch (protocol_ver)
            {
                case 5:protocol =new MC1710(); return;
            }
        }

        private void responsePing()
        {
            try
            {
                byte[] packet_ping = new byte[0];
                if (ServerData.CustomMOTD)
                {
                    string dataStr = "{\"description\":\"" + ServerData.ServerName + "\",\"players\":{\"max\":1,\"online\":0},\"version\":{\"name\":\"TEST\",\"protocol\":5}}";
                    byte[] packet_id = getVarInt(0);
                    packet_ping = concatBytes(packet_id, getString(dataStr));
                }
                else
                {
                    ProtocolConnection.doPing(Lobby.Host, Lobby.Port, ref packet_ping);
                }
                Client.Send(concatBytes(getVarInt(packet_ping.Length), packet_ping));
                byte[] response = readDataRAW(readNextVarIntRAW());
                Client.Send(concatBytes(getVarInt(response.Length), response));
            }
            catch
            {
                Close();
            }
        }

        public static void Handler(Socket client, MCServer server)
        {
            ProtocolHandler connection = new ProtocolHandler(client, server);
            ThreadPool.QueueUserWorkItem(new WaitCallback(connection.DoShakeHands));
        }
        private int readNextVarIntRAW()
        {
            int n = 0;
            int r = 0;
            byte[] tmp = new byte[1];
            do
            {
                Receive(tmp, 0, 1, SocketFlags.None);
                r |= ((tmp[0] & 0x7F) << (7 * n));
                n++;
                if (n > 5)
                    throw new OverflowException("VarInt is too big");
            } while ((r & 128) != 0);
            return r;
        }
        private static int readNextVarInt(List<byte> cache)
        {
            int n = 0;
            int r = 0;
            int k = 0;
            do
            {
                k = readNextByte(cache);
                r |= ((k & 0x7F) << (7 * n));
                n++;
                if (n > 5)
                    throw new OverflowException("VarInt is too big");
            } while ((r & 128) != 0);
            return r;
        }
        private static byte readNextByte(List<byte> cache)
        {
            byte result = cache[0];
            cache.RemoveAt(0);
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
        private static byte[] readData(int offset, List<byte> cache)
        {
            byte[] result = cache.Take(offset).ToArray();
            cache.RemoveRange(0, offset);
            return result;
        }
        private static short readNextShort(List<byte> cache)
        {
            byte[] rawValue = readData(2, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt16(rawValue, 0);
        }
        private static int readNextInt(List<byte> cache)
        {
            byte[] rawValue = readData(4, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt32(rawValue, 0);
        }
        private static ushort readNextUShort(List<byte> cache)
        {
            byte[] rawValue = readData(2, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToUInt16(rawValue, 0);
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
        private void SendMessage(string message)
        {
            if (String.IsNullOrEmpty(message))
                return;
            string result = String.Empty;
            if (message.Contains(ServerData.vColorChar))
            {
                string[] data = message.Split(ServerData.vColorChar);
                List<string> extra = new List<string>();
                result = "{\"extra\":[";
                foreach (string t in data)
                {
                    if (!string.IsNullOrEmpty(t))
                    {
                        string colorTag = ServerData.getColorTag(t[0]);
                        if (colorTag != "")
                        {
                            extra.Add("{" + string.Format("\"color\":\"{0}\",\"text\":\"{1}\"", colorTag, t.Remove(0, 1)) + "}");
                        }
                        else
                            extra.Add("{" + string.Format("\"text\":\"{0}\"", t) + "}");
                    }
                }
                result += string.Join(",", extra) + "],\"text\":\"\"}";
            }
            else
            {
                result = "{\"text\":\"" + message + "\"}";
            }
            SendPacket(Protocol.PacketIncomingType.ChatMessage, getString(result));
        }
        private void SendPacket(Protocol.PacketIncomingType type, IEnumerable<byte> packetData)
        {
            SendPacket(protocol.getPacketIncomingID(type), packetData);
        }
        private void SendPacket(int packetID, IEnumerable<byte> packetData)
        {
            byte[] the_packet = concatBytes(getVarInt(packetID), packetData.ToArray());
            if (protocol.protocolVersion>Protocol.MC18Version && the_packet.Length > compression_treshold)
            {
                int sizeUncompressed = the_packet.Length;
                the_packet = concatBytes(getVarInt(sizeUncompressed), ZlibUtils.Compress(the_packet));
            }
            SendRAW(concatBytes(getVarInt(the_packet.Length), the_packet));
        }
        private void SendRAW(byte[] buffer)
        {
            Client.Send(buffer);
        }
        private void ServerTransfer(List<string> args)
        {
            if (args.Count >= 2)
            {
                if (Data.Servers.servers.ContainsKey(args[1]))
                {
                    CreateProxyBridge(Data.Servers.servers[args[1]]);
                }
                else
                    SendMessage(ServerData.MsgServerNotFound);
            }
        }
        private void Close()
        {
            if (cRead != null)
            {
                cRead.Abort();
            }
            if (this.Client != null)
            {
                this.Client.Close();
            }
            if (this.Proxy != null)
            {
                this.Proxy.Dispose();
            }
        }
        public void receivePacket(int packetID, List<byte> packetData)
        {
            SendPacket(packetID, packetData);
        }
        public string getServerHost()
        {
            return Lobby.Host;
        }

        public ushort getServerPort()
        {
            return Lobby.Port;
        }

        public void OnConnectionLost(Conn.DisconnectReason reason, string message)
        {
            Debug.Log("Connection Lost.");
            this.Close();
        }
        public void OnLogin()
        {
            login_phase = false;
        }
        public bool OnCommand(string command)
        {
            if (command.StartsWith("/server"))
            {
                if (Command.hasArg(command))
                {
                    List<string> args = Command.getArg(command);
                    switch (args[0])
                    {
                        case "tp":
                            ServerTransfer(args);
                            return true;
                    }
                    return true;
                }      
            }
            return false;
        }
    }
}
