#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementEntries
{
    internal int CalculateCardDamageLayer(GameEngine.IDamageAttempt attack) => _resolve().CalculateCardDamageLayer(attack);
    internal bool IsExactMaximumHpReplacementDeathParent(DeathFrame death, ResolutionFrame parent) => _resolve().IsExactMaximumHpReplacementDeathParent(death, parent);
    internal void PublishMaximumHpDamageReplacement(long ownerId) => _resolve().PublishMaximumHpDamageReplacement(ownerId);
    internal bool TryReplaceCardDamageWithMaximumHpLoss(GameEngine.IDamageAttempt attack, scoped out bool pending) => _resolve().TryReplaceCardDamageWithMaximumHpLoss(attack, out pending);
}
