#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed partial class CoreGameEngineWushuangJinzhiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _resolve;
    private CoreGameEngineWushuangJinzhiEntries(Func<EngineOwners.CoreGameEngineWushuangJinzhiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangJinzhiEntries Create(Func<EngineOwners.CoreGameEngineWushuangJinzhiRuntime> resolve) => new(resolve);
}
