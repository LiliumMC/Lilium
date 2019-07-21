using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net
{
    interface ConnectionListener
    {
        Task Bind(int inetPort);
        Task Close();
        string getHost();
        int getPort();
    }
}
