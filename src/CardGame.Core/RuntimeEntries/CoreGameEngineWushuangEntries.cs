#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> _resolve;
    private CoreGameEngineWushuangEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> resolve) => new(resolve);
}
