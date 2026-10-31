#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultEntries
{
    internal void AssertUnexpectedAssaultReceipts() => _resolve().AssertUnexpectedAssaultReceipts();
    internal void ContinueUnexpectedAssaultPublicReveal(global::CardGame.Core.CardUseFrame use) => _resolve().ContinueUnexpectedAssaultPublicReveal(use);
    internal void ResolveUnexpectedAssault(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => _resolve().ResolveUnexpectedAssault(source, target, card, playedCardKind);
    internal void ResolveUnexpectedAssaultEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveUnexpectedAssaultEffect(pending);
    internal bool TryApplyUnexpectedAssaultTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone) => _resolve().TryApplyUnexpectedAssaultTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
}
