#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeQinglongEntries
{
    internal void BeginQinglongCrescentBladeFollowup(NativeOwners.QinglongCrescentBladeHandle pending, Card slash, CardKind effectiveSlashKind, int physicalOwnerSeat, IReadOnlyList<Card> physicalCards, NativeOwners.FactionCardRequestHandle? jijiang = default, CardConversionSource? conversionSource = default, bool skipInvocation = false) => _resolve().BeginQinglongCrescentBladeFollowup(pending, slash, effectiveSlashKind, physicalOwnerSeat, physicalCards, jijiang, conversionSource, skipInvocation);
    internal bool CanRequestQinglongCrescentBladeFactionSlash(NativeOwners.QinglongCrescentBladeHandle pending) => _resolve().CanRequestQinglongCrescentBladeFactionSlash(pending);
    internal bool CanUseQinglongCrescentBladeTarget(CharacterState source, CharacterState target, CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, Suit? physicalSuit = default, int? effectiveRank = default, IReadOnlyList<int>? physicalCardIds = default) => _resolve().CanUseQinglongCrescentBladeTarget(source, target, effectiveKind, effectiveColor, allowAnyColor, physicalSuit, effectiveRank, physicalCardIds);
    internal IReadOnlyList<Card> GetQinglongCrescentBladeSlashCards(CharacterState source, CharacterState target) => _resolve().GetQinglongCrescentBladeSlashCards(source, target);
    internal void PublishQinglongCrescentBladeChoice(NativeOwners.QinglongCrescentBladeHandle pending, bool includeFactionSlash) => _resolve().PublishQinglongCrescentBladeChoice(pending, includeFactionSlash);
    internal IReadOnlyList<PromptChoice> QinglongPhysicalSlashChoices(NativeOwners.QinglongCrescentBladeHandle pending, IReadOnlyList<Card> slashes) => _resolve().QinglongPhysicalSlashChoices(pending, slashes);
    internal void ResolveQinglongCrescentBladeChoice(PromptChoice selected) => _resolve().ResolveQinglongCrescentBladeChoice(selected);
    internal bool TryBeginQinglongCrescentBladeChoice(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginQinglongCrescentBladeChoice(attack);
}
