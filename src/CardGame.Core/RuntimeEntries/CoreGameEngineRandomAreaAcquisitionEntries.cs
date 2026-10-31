#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime))]
internal sealed partial class CoreGameEngineRandomAreaAcquisitionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionEntries(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionEntries Create(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> resolve) => new(resolve);
}
