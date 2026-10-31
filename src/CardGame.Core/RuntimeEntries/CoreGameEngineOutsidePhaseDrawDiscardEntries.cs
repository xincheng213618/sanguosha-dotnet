#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _resolve;
    private CoreGameEngineOutsidePhaseDrawDiscardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOutsidePhaseDrawDiscardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> resolve) => new(resolve);
}
