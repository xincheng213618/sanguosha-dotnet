#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> _resolve;
    private CoreGameEngineSoleTargetReplacementCandidatesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetReplacementCandidatesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> resolve) => new(resolve);
}
