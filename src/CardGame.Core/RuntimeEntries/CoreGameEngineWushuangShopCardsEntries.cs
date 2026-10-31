#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangShopCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _resolve;
    private CoreGameEngineWushuangShopCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangShopCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> resolve) => new(resolve);
}
