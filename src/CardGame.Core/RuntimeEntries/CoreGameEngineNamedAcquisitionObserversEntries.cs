#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime))]
internal sealed partial class CoreGameEngineNamedAcquisitionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineNamedAcquisitionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAcquisitionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> resolve) => new(resolve);
}
