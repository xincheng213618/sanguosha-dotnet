#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDeckCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> _resolve;
    private CoreGameEngineWushuangDeckCardsEntries(Func<EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangDeckCardsEntries Create(Func<EngineOwners.CoreGameEngineWushuangDeckCardsRuntime> resolve) => new(resolve);
}
