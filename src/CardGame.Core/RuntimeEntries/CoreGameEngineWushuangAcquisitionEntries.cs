#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime))]
internal sealed partial class CoreGameEngineWushuangAcquisitionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> _resolve;
    private CoreGameEngineWushuangAcquisitionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAcquisitionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve) => new(resolve);
}
