using Lilium.Net.Event;
using Lilium.Protocol.Message;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net
{
    public class HandleServer
    {
        public string Host { get; set; }
        public int Port { get; set; }
        private PacketProtocol protocol;
        private SessionFactory factory;
        private ConnectionListener listener;

        private List<Session> sessions = new List<Session>();
        private Dictionary<int, object> flags = new Dictionary<int, object>();
        private List<IServerListener> listeners = new List<IServerListener>();
        public bool isListening
        {
            get
            {
                return this.listener != null && this.listener.isListening;
            }
        }
        public HandleServer(string host,int port,PacketProtocol protocol,SessionFactory factory)
        {
            this.Host = host;
            this.Port = port;
            this.protocol = protocol;
            this.factory = factory;
        }
        public PacketProtocol CreatePacketProtocol()
        {
            return this.protocol;
        }
        public bool hasGlobalFlag(int key)
        {
            return this.flags.ContainsKey(key);
        }
        public T GetGlobalFlag<T>(int key)
        {
            if (!hasGlobalFlag(key))
                return default;
            object value = this.flags[key];
            return (T)value;
        }
        public void SetGlobalFlag(int key,object value)
        {
            this.flags.Add(key, value);
        }
        public void AddSession(Session session)
        {
            this.sessions.Add(session);
        }
        public void RemoveSession(Session session)
        {
            this.sessions.Remove(session);
            if (session.Connected)
                session.Disconnect(DisconnectReason.ConnectionLost, "Connection Closed.");
        }
        public void AddListener(IServerListener listener)
        {
            this.listeners.Add(listener);
        }
        public void RemoveListener(IServerListener listener)
        {
            this.listeners.Remove(listener);
        }
        public async Task Bind()
        {
            this.listener = this.factory.createServerListener(this);
            await this.listener.Bind();
        }
        public void CallEvent(ServerEvent Event)
        {
            foreach(IServerListener listener in listeners)
            {
                Event.Call(listener);
            }
        }
        public async Task Close()
        {
            foreach(Session session in sessions)
            {
                if (session.Connected)
                    session.Disconnect(DisconnectReason.InGameKick, "Server Closed.");
            }
            await this.listener.Close();
        }
    }
}
