#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAiRuntime> _resolve;
    private CoreGameEngineWushuangAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAiRuntime> resolve) => new(resolve);
}
