using Lilium.Config;
using Lilium.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Plugins
{
    public abstract class Plugin
    {
        private HandleServer server;
        public void Init(HandleServer server)
        {
            this.server = server;
        }
        public abstract string Name { get; }
        public abstract string Version { get; }

        protected HandleServer Server { get
            {
                return this.server;
            } }
        protected YamlConfig DefaultConfig { get
            {
                return Program.config;
            } }
        public virtual void OnLoad() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

        public override string ToString()
        {
            return Name + " [" + Version + "]";
        }
    }
}
