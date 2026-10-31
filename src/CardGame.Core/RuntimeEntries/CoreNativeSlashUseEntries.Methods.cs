#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSlashUseEntries
{
    internal void BeginNextFangtianHalberdTarget(NativeOwners.FangtianHalberdHandle pending) => _resolve().BeginNextFangtianHalberdTarget(pending);
    internal void BeginSlashTargetResolution(NativeOwners.CardAttackHandle attack) => _resolve().BeginSlashTargetResolution(attack);
    internal void ContinueSlashAfterFinalizedTargets(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterFinalizedTargets(attack);
    internal void ContinueSlashAfterProgramTargetEffects(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterProgramTargetEffects(attack);
    internal void ContinueSlashAfterRedirectPrograms(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterRedirectPrograms(attack);
    internal void ContinueSlashAfterResponsePrograms(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterResponsePrograms(attack);
    internal void ContinueSlashDefense(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashDefense(attack);
    internal void ResolveFangtianHalberdSlash(CharacterState source, IReadOnlyList<CharacterState> targets, Card slash, CardKind playedCardKind, CardConversionSource? conversionSource = default, IReadOnlyList<CardConversionSource>? additionalConversionSources = default) => _resolve().ResolveFangtianHalberdSlash(source, targets, slash, playedCardKind, conversionSource, additionalConversionSources);
    internal void ResolveSlash(CharacterState source, CharacterState target, Card slash, CardKind playedCardKind, CardConversionSource? conversionSource = default, IReadOnlyList<CardConversionSource>? additionalConversionSources = default) => _resolve().ResolveSlash(source, target, slash, playedCardKind, conversionSource, additionalConversionSources);
    internal void ResolveSlashCore(CharacterState source, CharacterState target, Card slash, CardKind playedCardKind, int physicalOwnerSeat, NativeOwners.FactionCardRequestHandle? factionRequest = default, NativeOwners.BorrowedSwordHandle? borrowedSword = default, IReadOnlyList<Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, CardConversionSource? conversionSource = default, IReadOnlyList<CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, CardLocation? physicalSourceLocation = default) => _resolve().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
    internal void ResolveZhangbaSlash(CharacterState source, CharacterState target, IReadOnlyList<Card> physicalCards) => _resolve().ResolveZhangbaSlash(source, target, physicalCards);
}
