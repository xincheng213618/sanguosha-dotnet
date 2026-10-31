#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime))]
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> _resolve;
    private CoreGameEngineCurrentUseTargetCandidateRefreshEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentUseTargetCandidateRefreshEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> resolve) => new(resolve);
}
