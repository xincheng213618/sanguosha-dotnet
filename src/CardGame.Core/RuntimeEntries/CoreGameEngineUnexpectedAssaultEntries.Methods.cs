#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultEntries
{
    internal void ContinueUnexpectedAssaultPublicReveal(CardUseFrame use) => _resolve().ContinueUnexpectedAssaultPublicReveal(use);
    internal void ResolveUnexpectedAssault(CharacterState source, CharacterState target, Card card, CardKind? playedCardKind = default) => _resolve().ResolveUnexpectedAssault(source, target, card, playedCardKind);
    internal void ResolveUnexpectedAssaultEffect(NullificationWindowFrame pending) => _resolve().ResolveUnexpectedAssaultEffect(pending);
    internal bool TryApplyUnexpectedAssaultTargetCardEffect(long resolutionId, Card effectCard, CardKind effectiveCardKind, int sourceSeat, int targetSeat, LegalActionKind actionKind, Card targetCard, CardLocation targetCardFromZone) => _resolve().TryApplyUnexpectedAssaultTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
}
