#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFactionSlashEntries
{
    internal void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false) => _resolve().AdvanceFactionSlashCandidate(keepCurrentRequest);
    internal void BeginBorrowedSwordFactionSlashRequest(NativeOwners.BorrowedSwordHandle borrowedSword) => _resolve().BeginBorrowedSwordFactionSlashRequest(borrowedSword);
    internal void BeginBorrowedSwordFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = default) => _resolve().BeginBorrowedSwordFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
    internal void BeginFactionSlashResponseRequest(NativeOwners.CardAttackHandle attack) => _resolve().BeginFactionSlashResponseRequest(attack);
    internal void BeginProgramFactionCardRequest(long frameId, int ownerSeat, int targetSeat, string providerFactionId, CardKind requiredKind) => _resolve().BeginProgramFactionCardRequest(frameId, ownerSeat, targetSeat, providerFactionId, requiredKind);
    internal void BeginProvidedFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = default) => _resolve().BeginProvidedFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
    internal void BeginQinglongCrescentBladeFactionSlashRequest(NativeOwners.QinglongCrescentBladeHandle qinglong) => _resolve().BeginQinglongCrescentBladeFactionSlashRequest(qinglong);
    internal void BeginQinglongCrescentBladeFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = default) => _resolve().BeginQinglongCrescentBladeFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
    internal void CompleteFactionSlashFailure(NativeOwners.FactionCardRequestHandle pending) => _resolve().CompleteFactionSlashFailure(pending);
    internal void CompleteFactionSlashResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, Card selectedSlash) => _resolve().CompleteFactionSlashResponse(pending, provider, selectedSlash);
    internal void CompleteQinglongCrescentBladeFactionSlash(NativeOwners.FactionCardRequestHandle pending) => _resolve().CompleteQinglongCrescentBladeFactionSlash(pending);
    internal IReadOnlyList<PromptChoice> CreateFactionSlashResponseChoices(CharacterState provider, NativeOwners.FactionCardRequestHandle pending, IReadOnlyList<Card> slashes) => _resolve().CreateFactionSlashResponseChoices(provider, pending, slashes);
    internal string GetFactionRequestDisplayName(NativeOwners.FactionCardRequestHandle pending) => _resolve().GetFactionRequestDisplayName(pending);
    internal string GetFactionRequestSkillId(NativeOwners.FactionCardRequestHandle pending) => _resolve().GetFactionRequestSkillId(pending);
    internal void PublishOwnerSlashDecision(NativeOwners.FactionCardRequestHandle failedRequest) => _resolve().PublishOwnerSlashDecision(failedRequest);
    internal void ResolveFactionSlashCandidateResponse(NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, CardKind? requestedEffectiveKind = default) => _resolve().ResolveFactionSlashCandidateResponse(pending, useSlash, requestedCardId, requestedEffectiveKind);
    internal void ResolveFactionSlashProgramMultiCardCandidateResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, ProgramMultiCardViewAsSelection selection) => _resolve().ResolveFactionSlashProgramMultiCardCandidateResponse(pending, provider, selection);
    internal void ResolveFactionSlashZhangbaCandidateResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> pair) => _resolve().ResolveFactionSlashZhangbaCandidateResponse(pending, provider, pair);
    internal void ResolveFactionSlashZhuqueFanChoice(NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash) => _resolve().ResolveFactionSlashZhuqueFanChoice(pending, convertToFireSlash);
    internal bool TryBeginFactionSlashZhuqueFanChoice(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind baseEffectiveKind) => _resolve().TryBeginFactionSlashZhuqueFanChoice(pending, provider, physicalCards, baseEffectiveKind);
}
