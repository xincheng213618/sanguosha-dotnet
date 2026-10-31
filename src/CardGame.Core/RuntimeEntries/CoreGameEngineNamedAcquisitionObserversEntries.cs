#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime))]
internal sealed partial class CoreGameEngineNamedAcquisitionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineNamedAcquisitionObserversEntries(Func<EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAcquisitionObserversEntries Create(Func<EngineOwners.CoreGameEngineNamedAcquisitionObserversRuntime> resolve) => new(resolve);
}
