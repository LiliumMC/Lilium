using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net
{
    public interface ConnectionListener
    {
        Task Bind();
        Task Close();
        bool isListening { get; }
        string getHost();
        int getPort();
    }
}
