using OmniSharp.Mef;

namespace OmniSharp.Models
{
    [OmniSharpEndpoint(OmniSharpEndpoints.CancelRequest, typeof(CancellationRequest), typeof(object))]
    public class CancellationRequest : IRequest
    {
        public int Request_seq { get; set; }

    }
}