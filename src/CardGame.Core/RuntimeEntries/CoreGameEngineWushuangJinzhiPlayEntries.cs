#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiPlayEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> _resolve;
    private CoreGameEngineWushuangJinzhiPlayEntries(Func<EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangJinzhiPlayEntries Create(Func<EngineOwners.CoreGameEngineWushuangJinzhiPlayRuntime> resolve) => new(resolve);
}
