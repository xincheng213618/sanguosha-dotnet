#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime))]
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> resolve) => new(resolve);
}
