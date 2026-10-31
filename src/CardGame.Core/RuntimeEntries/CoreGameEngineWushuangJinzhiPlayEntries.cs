#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiPlayEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> _resolve;
    private CoreGameEngineWushuangJinzhiPlayEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangJinzhiPlayEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> resolve) => new(resolve);
}
