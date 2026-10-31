#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangPublicPredictionEntries
{
    internal global::CardGame.Core.PublicDamagePrediction AttachWushuangPublicCombatPrediction(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat, int amount, global::CardGame.Core.CardKind kind, int paidHand, global::CardGame.Core.PublicDamagePrediction legacy) => _resolve().AttachWushuangPublicCombatPrediction(view, actorSeat, action, targetSeat, amount, kind, paidHand, legacy);
    internal int WushuangPublicNativeDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, global::CardGame.Core.CardKind? kind, global::CardGame.Core.Suit suit, int additionalIncomingSegments, int priorTargetSegments, long? issuedUseId = default) => _resolve().WushuangPublicNativeDamageBonus(view, sourceSeat, targetSeat, kind, suit, additionalIncomingSegments, priorTargetSegments, issuedUseId);
}
