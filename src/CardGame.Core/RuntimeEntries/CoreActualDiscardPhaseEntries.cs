#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed partial class CoreActualDiscardPhaseEntries
{
    private readonly Func<EngineOwners.CoreActualDiscardPhaseRuntime> _resolve;
    private CoreActualDiscardPhaseEntries(Func<EngineOwners.CoreActualDiscardPhaseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualDiscardPhaseEntries Create(Func<EngineOwners.CoreActualDiscardPhaseRuntime> resolve) => new(resolve);
}
