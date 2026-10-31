#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _resolve;
    private CoreGameEngineCardDamageMaximumHpReplacementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDamageMaximumHpReplacementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> resolve) => new(resolve);
}
