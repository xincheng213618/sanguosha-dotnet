#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDeckCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> _resolve;
    private CoreGameEngineWushuangDeckCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangDeckCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> resolve) => new(resolve);
}
