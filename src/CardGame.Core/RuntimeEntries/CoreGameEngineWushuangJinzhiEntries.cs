#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed partial class CoreGameEngineWushuangJinzhiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _resolve;
    private CoreGameEngineWushuangJinzhiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangJinzhiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> resolve) => new(resolve);
}
