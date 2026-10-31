#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> _resolve;
    private CoreGameEngineSoleTargetReplacementCandidatesEntries(Func<EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetReplacementCandidatesEntries Create(Func<EngineOwners.CoreGameEngineSoleTargetReplacementCandidatesRuntime> resolve) => new(resolve);
}
