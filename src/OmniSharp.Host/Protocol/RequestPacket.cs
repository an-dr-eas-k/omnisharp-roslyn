using System;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace OmniSharp.Protocol
{
    public class RequestPacket : Packet
    {
        public static RequestPacket Parse(string json)
        {
            var obj = JObject.Parse(json);
            var result = obj.ToObject<RequestPacket>();

            if (result.Seq <= 0)
            {
                throw new ArgumentException("invalid seq-value");
            }

            if (string.IsNullOrWhiteSpace(result.Command))
            {
                throw new ArgumentException("missing command");
            }

            JToken arguments;
            if (obj.TryGetValue("arguments", StringComparison.OrdinalIgnoreCase, out arguments))
            {
                result.ArgumentsStream = new MemoryStream(Encoding.UTF8.GetBytes(arguments.ToString()));
            }
            else
            {
                result.ArgumentsStream = Stream.Null;
            }
            result.cancellationTokenSource = new CancellationTokenSource();
            return result;
        }

        public string Command { get; set; }

        public Stream ArgumentsStream { get; set; }

        private CancellationTokenSource cancellationTokenSource;

        public RequestPacket() : base("request") { }

        public ResponsePacket Reply()
        {
            if (Command == OmniSharpEndpoints.CancelRequest)
            {
                return null;
            }
            return new ResponsePacket()
            {
                Request_seq = Seq,
                Success = true,
                Running = true,
                Command = Command
            };
        }

        public void CancelRequest()
        {
            cancellationTokenSource.Cancel();
        }

        public CancellationToken GetCancellationToken() => cancellationTokenSource.Token;
    }
}
