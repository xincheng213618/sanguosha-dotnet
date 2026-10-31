#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeTrickEffectsEntries
{
    internal void ApplyTargetCardEffect(long resolutionId, Card effectCard, CardKind effectiveCardKind, int sourceSeat, int targetSeat, LegalActionKind actionKind, Card targetCard, CardLocation targetCardFromZone) => _resolve().ApplyTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
    internal void BeginTargetCardSelection(NullificationWindowFrame pending, CharacterState target, int candidateCount) => _resolve().BeginTargetCardSelection(pending, target, candidateCount);
    internal void CompleteIronChainEffect(NullificationWindowFrame pending, List<string> changes) => _resolve().CompleteIronChainEffect(pending, changes);
    internal void ResolveBorrowedSwordEffect(NullificationWindowFrame pending) => _resolve().ResolveBorrowedSwordEffect(pending);
    internal void ResolveDrawTwoEffect(NullificationWindowFrame pending) => _resolve().ResolveDrawTwoEffect(pending);
    internal void ResolveDuelEffect(NullificationWindowFrame pending) => _resolve().ResolveDuelEffect(pending);
    internal void ResolveFireAttackEffect(NullificationWindowFrame pending) => _resolve().ResolveFireAttackEffect(pending);
    internal void ResolveFiveGrainsEffect(NullificationWindowFrame pending) => _resolve().ResolveFiveGrainsEffect(pending);
    internal void ResolveGroupCardEffect(NullificationWindowFrame pending) => _resolve().ResolveGroupCardEffect(pending);
    internal void ResolveIronChainEffect(NullificationWindowFrame pending) => _resolve().ResolveIronChainEffect(pending);
    internal void ResolvePeachGardenEffect(NullificationWindowFrame pending) => _resolve().ResolvePeachGardenEffect(pending);
    internal void ResolveTargetCardEffect(NullificationWindowFrame pending) => _resolve().ResolveTargetCardEffect(pending);
    internal void ResolveTargetCardSelection(TargetCardSelectionFrame pending, int slot) => _resolve().ResolveTargetCardSelection(pending, slot);
    internal void SkipUnavailableTargetCardEffect(NullificationWindowFrame pending, CharacterState target, CardEffectSkipReason reason) => _resolve().SkipUnavailableTargetCardEffect(pending, target, reason);
}
