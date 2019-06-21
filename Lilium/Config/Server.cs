namespace Lilium.Config
{
    public sealed class Server
    {
        public string Name { set; get; }
        public string Address { set; get; }
        public int Port { set; get; }
        public bool Restricted { set; get; }
    }
}
