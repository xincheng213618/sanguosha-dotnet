#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangAiRuntime> _resolve;
    private CoreGameEngineWushuangAiEntries(Func<EngineOwners.CoreGameEngineWushuangAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAiEntries Create(Func<EngineOwners.CoreGameEngineWushuangAiRuntime> resolve) => new(resolve);
}
