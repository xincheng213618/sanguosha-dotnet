#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeTrickEffectsEntries
{
    internal void ApplyTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone) => _resolve().ApplyTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
    internal void BeginTargetCardSelection(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target, int candidateCount) => _resolve().BeginTargetCardSelection(pending, target, candidateCount);
    internal void CompleteIronChainEffect(global::CardGame.Core.NullificationWindowFrame pending, global::System.Collections.Generic.List<string> changes) => _resolve().CompleteIronChainEffect(pending, changes);
    internal void ResolveBorrowedSwordEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveBorrowedSwordEffect(pending);
    internal void ResolveDrawTwoEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveDrawTwoEffect(pending);
    internal void ResolveDuelEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveDuelEffect(pending);
    internal void ResolveFireAttackEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveFireAttackEffect(pending);
    internal void ResolveFiveGrainsEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveFiveGrainsEffect(pending);
    internal void ResolveGroupCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveGroupCardEffect(pending);
    internal void ResolveIronChainEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveIronChainEffect(pending);
    internal void ResolvePeachGardenEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolvePeachGardenEffect(pending);
    internal void ResolveTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveTargetCardEffect(pending);
    internal void ResolveTargetCardSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot) => _resolve().ResolveTargetCardSelection(pending, slot);
    internal void SkipUnavailableTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardEffectSkipReason reason) => _resolve().SkipUnavailableTargetCardEffect(pending, target, reason);
}
