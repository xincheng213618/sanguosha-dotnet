#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime))]
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionObserversEntries(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionObserversEntries Create(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionObserversRuntime> resolve) => new(resolve);
}
