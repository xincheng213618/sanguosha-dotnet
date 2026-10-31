#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed partial class CoreGameEngineWushuangShopCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _resolve;
    private CoreGameEngineWushuangShopCardsEntries(Func<EngineOwners.CoreGameEngineWushuangShopCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangShopCardsEntries Create(Func<EngineOwners.CoreGameEngineWushuangShopCardsRuntime> resolve) => new(resolve);
}
