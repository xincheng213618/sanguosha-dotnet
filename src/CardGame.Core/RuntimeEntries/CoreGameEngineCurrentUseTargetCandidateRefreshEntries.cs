#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime))]
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> _resolve;
    private CoreGameEngineCurrentUseTargetCandidateRefreshEntries(Func<EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentUseTargetCandidateRefreshEntries Create(Func<EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> resolve) => new(resolve);
}
