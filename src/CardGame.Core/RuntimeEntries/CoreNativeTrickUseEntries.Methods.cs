#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeTrickUseEntries
{
    internal void BeginTargetCardEffect(CharacterState source, CharacterState target, Card effectCard, LegalActionKind actionKind, GameEngine.TargetCardEffect effect, int? targetCardId, CardKind? playedCardKind = default, TargetHandCardPreselection? preselection = default) => _resolve().BeginTargetCardEffect(source, target, effectCard, actionKind, effect, targetCardId, playedCardKind, preselection);
    internal void CompleteAlcoholUse(CharacterState source, Card alcohol, long resolutionId) => _resolve().CompleteAlcoholUse(source, alcohol, resolutionId);
    internal IReadOnlyList<Card> GetCardUsePhysicalCards(long resolutionId) => _resolve().GetCardUsePhysicalCards(resolutionId);
    internal void ResolveAlcohol(CharacterState source, Card alcohol, CardConversionSource? conversionSource = default) => _resolve().ResolveAlcohol(source, alcohol, conversionSource);
    internal void ResolveBorrowedSword(CharacterState source, CharacterState weaponOwner, CharacterState slashTarget, Card borrowedSword) => _resolve().ResolveBorrowedSword(source, weaponOwner, slashTarget, borrowedSword);
    internal void ResolveDelayedCard(CharacterState source, CharacterState target, Card delayedCard, LegalActionKind actionKind, CardKind? playedCardKind = default) => _resolve().ResolveDelayedCard(source, target, delayedCard, actionKind, playedCardKind);
    internal void ResolveDelayedCardEffect(NullificationWindowFrame pending) => _resolve().ResolveDelayedCardEffect(pending);
    internal void ResolveDismantlement(CharacterState source, CharacterState target, Card dismantlement, int? targetCardId, CardKind? playedCardKind = default, TargetHandCardPreselection? preselection = default) => _resolve().ResolveDismantlement(source, target, dismantlement, targetCardId, playedCardKind, preselection);
    internal void ResolveDrawTwo(CharacterState source, Card drawTwo) => _resolve().ResolveDrawTwo(source, drawTwo);
    internal void ResolveFireAttack(CharacterState source, CharacterState target, Card fireAttack, CardKind? playedCardKind = default) => _resolve().ResolveFireAttack(source, target, fireAttack, playedCardKind);
    internal void ResolveFiveGrains(CharacterState source, Card fiveGrains) => _resolve().ResolveFiveGrains(source, fiveGrains);
    internal void ResolveGroupCard(CharacterState source, Card groupCard, LegalActionKind actionKind, CardKind requiredCardKind) => _resolve().ResolveGroupCard(source, groupCard, actionKind, requiredCardKind);
    internal void ResolveIronChain(CharacterState source, Card ironChain, IReadOnlyList<int> targetSeats, CardKind? playedCardKind = default) => _resolve().ResolveIronChain(source, ironChain, targetSeats, playedCardKind);
    internal void ResolvePeachGarden(CharacterState source, Card peachGarden) => _resolve().ResolvePeachGarden(source, peachGarden);
    internal void ResolveSnatch(CharacterState source, CharacterState target, Card snatch, int? targetCardId, CardKind? playedCardKind = default, TargetHandCardPreselection? preselection = default) => _resolve().ResolveSnatch(source, target, snatch, targetCardId, playedCardKind, preselection);
}
