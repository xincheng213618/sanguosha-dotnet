#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _resolve;
    private CoreGameEngineOutsidePhaseDrawDiscardEntries(Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOutsidePhaseDrawDiscardEntries Create(Func<EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> resolve) => new(resolve);
}
