#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime))]
internal sealed partial class CoreGameEnginePreventionDrawObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> _resolve;
    private CoreGameEnginePreventionDrawObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePreventionDrawObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> resolve) => new(resolve);
}
