#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangPublicPredictionEntries
{
    internal PublicDamagePrediction AttachWushuangPublicCombatPrediction(GameSnapshot view, int actorSeat, LegalAction action, int targetSeat, int amount, CardKind kind, int paidHand, PublicDamagePrediction legacy) => _resolve().AttachWushuangPublicCombatPrediction(view, actorSeat, action, targetSeat, amount, kind, paidHand, legacy);
    internal int WushuangPublicNativeDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat, CardKind? kind, Suit suit, int additionalIncomingSegments, int priorTargetSegments, long? issuedUseId = default) => _resolve().WushuangPublicNativeDamageBonus(view, sourceSeat, targetSeat, kind, suit, additionalIncomingSegments, priorTargetSegments, issuedUseId);
}
