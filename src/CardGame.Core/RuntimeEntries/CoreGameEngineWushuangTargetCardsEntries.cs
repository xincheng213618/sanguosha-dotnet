#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTargetCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> _resolve;
    private CoreGameEngineWushuangTargetCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangTargetCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> resolve) => new(resolve);
}
