#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAcquisitionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> _resolve;
    private CoreGameEngineWushuangAcquisitionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAcquisitionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> resolve) => new(resolve);
}
