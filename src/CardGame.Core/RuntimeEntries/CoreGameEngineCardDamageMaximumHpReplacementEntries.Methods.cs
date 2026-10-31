#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementEntries
{
    internal int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CalculateCardDamageLayer(attack);
    internal bool IsExactMaximumHpReplacementDeathParent(global::CardGame.Core.DeathFrame death, global::CardGame.Core.ResolutionFrame parent) => _resolve().IsExactMaximumHpReplacementDeathParent(death, parent);
    internal void PublishMaximumHpDamageReplacement(long ownerId) => _resolve().PublishMaximumHpDamageReplacement(ownerId);
    internal bool TryReplaceCardDamageWithMaximumHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out bool pending) => _resolve().TryReplaceCardDamageWithMaximumHpLoss(attack, out pending);
}
