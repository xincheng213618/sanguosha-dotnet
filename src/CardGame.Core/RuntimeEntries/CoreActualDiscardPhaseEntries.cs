#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualDiscardPhaseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _resolve;
    private CoreActualDiscardPhaseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualDiscardPhaseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> resolve) => new(resolve);
}
