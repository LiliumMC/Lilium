using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MCPlib
{
    public class MCServer
    {
        public MCServer(ushort port):this(IPAddress.Any, port) { }
        public MCServer(IPAddress addr,ushort port)
        {
            this.Addr = addr;
            this.Port = port;
        }
        public ushort Port { get; private set; }
        public IPAddress Addr { get; private set; }
        private TcpListener _Listener;
        public bool _IsStarted { get; set; } = false;

        public void Start()
        {
            if (!_IsStarted)
            {
                this._Listener = new TcpListener(Addr, Port);
                this._Listener.Start();
                Debug.Log("服务端已启用:"+Port);
                this._Listener.BeginAcceptSocket(this.OnBeginAcceptSocket, this._Listener);
                this._IsStarted = true;
            }
        }
        public void Stop()
        {
            if (_IsStarted)
            {
                this._Listener.Stop();
                this._IsStarted = false;
            }
        }
        private void OnBeginAcceptSocket(IAsyncResult async)
        {
            TcpListener listener = async.AsyncState as TcpListener;
            try
            {
                Socket client = listener.EndAcceptSocket(async);
                Protocol.ProtocolHandler.Handler(client, this);
                if (this._IsStarted)
                {
                    listener.BeginAcceptSocket(this.OnBeginAcceptSocket, listener);
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
