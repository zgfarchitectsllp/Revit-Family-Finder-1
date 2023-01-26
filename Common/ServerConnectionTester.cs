using System;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;


namespace ZGF.Server
{
    class ServerConnectionTester
    {

        //public static bool TestServerConnection(string serverName, int timeout)
        //{
        //    return false;
        //}


        public static bool TestServerConnection(string serverName, int timeout, out long replyTimeInMilliseconds)
        {
            replyTimeInMilliseconds = 0;
            bool replySuccess = false;
            
            Ping testPing = new Ping(); 

            PingOptions options = new PingOptions();

            options.DontFragment = true;

            // 32 bytes of data for the buffer
            string bufferData = "12345678901234567890123456789012";
            byte[] buffer = Encoding.ASCII.GetBytes(bufferData);
            int _timeout = timeout < 1 ? 1000 : timeout;

            try
            {
                PingReply reply = testPing.Send(serverName, _timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
#if DEBUG
                    Debug.WriteLine("Ping status: ");
                    Debug.WriteLine("\tAddress:       \t{0}", reply.Address.ToString());
                    Debug.WriteLine("\tRoundTrip time:\t{0}", reply.RoundtripTime);
                    Debug.WriteLine("\tTime to Live:  \t{0}", reply.Options.Ttl);
                    Debug.WriteLine("\tDon't fragment:\t{0}", reply.Options.DontFragment);
                    Debug.WriteLine("\tBuffer length: \t{0}", reply.Buffer.Length);
                    Debug.WriteLine("");
#endif
                    replyTimeInMilliseconds = reply.RoundtripTime;
                    replySuccess = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;                   
            }

            return replySuccess;
        }
    }
}
