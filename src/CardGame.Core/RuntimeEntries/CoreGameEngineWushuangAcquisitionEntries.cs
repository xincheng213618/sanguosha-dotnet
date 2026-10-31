#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangAcquisitionRuntime))]
internal sealed partial class CoreGameEngineWushuangAcquisitionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> _resolve;
    private CoreGameEngineWushuangAcquisitionEntries(Func<EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAcquisitionEntries Create(Func<EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve) => new(resolve);
}
