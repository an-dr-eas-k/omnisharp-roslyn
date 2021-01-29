using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using OmniSharp.Mef;
using OmniSharp.Models;

namespace OmniSharp
{
    [Shared, Export]
    [OmniSharpHandler(OmniSharpEndpoints.CancelRequest, LanguageNames.CSharp)]
    public class OmniSharpClientRequestService : IRequestHandler<CancellationRequest, object>
    {

        private readonly Dictionary<IRequest, int> _requestIds = new();
        private readonly Dictionary<int, CancellationTokenSource> _cancellationSourcesForRequestIds = new();

        [ImportingConstructor]
        public OmniSharpClientRequestService()
        {
        }

        public Task<object> Handle(CancellationRequest request)
        {
            if (_cancellationSourcesForRequestIds.TryGetValue(request.Request_seq, out CancellationTokenSource cts))
            {
                cts.Cancel();
            }
            return Task.FromResult<object>(null);
        }

        public CancellationToken GetToken(IRequest request)
        {
            if (_requestIds.TryGetValue(request, out int requestId))
            {
                var tokenSource = new CancellationTokenSource();
                _cancellationSourcesForRequestIds.Add(requestId, tokenSource);
                return tokenSource.Token;
            }
            return CancellationToken.None;
        }

        public void RegisterRequest(int requestSeq, IRequest request)
        {
            _requestIds.Add(request, requestSeq);
        }

        public void UnregisterRequest(int requestSeq)
        {
            _requestIds
              .Where(ri => ri.Value == requestSeq)
              .Select(ri => ri.Key)
              .ToList()
              .ForEach(r => _requestIds.Remove(r));

            _cancellationSourcesForRequestIds.Remove(requestSeq);
        }
    }
}