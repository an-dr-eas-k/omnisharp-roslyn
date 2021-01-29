using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using OmniSharp.Roslyn.CSharp.Services.Diagnostics;

namespace OmniSharp.Roslyn.CSharp.Workers.Diagnostics
{
    public interface ICsDiagnosticWorker
    {
        Task<ImmutableArray<DocumentDiagnostics>> GetDiagnostics(ImmutableArray<string> documentPaths, CancellationToken cancellationToken = default);
        Task<ImmutableArray<DocumentDiagnostics>> GetAllDiagnosticsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Diagnostic>> AnalyzeDocumentAsync(Document document, CancellationToken cancellationToken);
        Task<IEnumerable<Diagnostic>> AnalyzeProjectsAsync(Project project, CancellationToken cancellationToken);
        ImmutableArray<DocumentId> QueueDocumentsForDiagnostics();
        ImmutableArray<DocumentId> QueueDocumentsForDiagnostics(ImmutableArray<ProjectId> projectId);
    }
}
