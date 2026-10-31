namespace CardGame.Core;

// These interfaces exist only for a synchronous trigger query. They are split by
// responsibility; adapters retain the original participant references and defer every read.
internal interface IProgramTriggerContentQuery : IProgramSelectionQuery
{
    SkillProgram Program(string skillId);
    SkillProgram? FindProgram(string skillId);
    SkillProgram ActivityProgram(string skillId);
    SkillTag SkillTags(string skillId);
}

internal interface IProgramTriggerCollectionQuery : IProgramSelectionQuery
{
    SkillBindingShard GetSkillBindingShard(CharacterState player);
    bool HasProgramPhaseHandDebt(ProgramTriggerCandidate candidate);
    IReadOnlyList<ProgramTriggerCandidate> IncludeYuanJiIssuedReturns(CharacterState owner,SkillProgramTriggerWindow window,IReadOnlyList<ProgramTriggerCandidate> ordinary);
    bool IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate);
    bool IsPhaseNamePredictionResolver(ProgramTriggerCandidate c, SkillProgram p);
    bool IsYuanJiReturnOp(SkillProgramEffectOp op);
    IEnumerable<ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(CharacterState owner, SkillProgramTriggerWindow window, int occurrence);
    IReadOnlyList<ProgramTriggerCandidate> IncludeTriggerCandidate(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates);
    int IssuedPhaseHandDebtCount { get; }
}

internal interface IProgramTriggerProofQuery
{
    bool? DecideTriggerProof<TRequest>(ProgramTriggerProofPhase phase, TRequest request) where TRequest : struct;
}

internal interface IProgramTriggerEligibilityQuery : IProgramSelectionQuery, IProgramTriggerProofQuery
{
    bool AdvancedTriggerPrerequisites(CharacterState owner, string skillId, SkillProgramTrigger trigger);
    bool CanPayProgramMarkerCost(CharacterState owner, SkillProgramMarkerCost? cost);
    SkillProgramTriggerFacts CaptureProgramTriggerFacts(CharacterState owner);
    SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId);
    bool HasRuntimeSkillInstance(CharacterState player, string skillId, string skillInstanceId);
    bool HasSkillRoleQualification(CharacterState owner, string skillId, string? instance, Role role);
    bool IsConfirmedActualStartingSubstitution(ProgramSkillWindowContext context);
    bool IsGainPhaseQualified(SkillProgramTrigger trigger, int ownerSeat, CardMovementTiming? timing);
    bool CanTrigger(ProgramTriggerLegalityPhase phase, ProgramTriggerLegalityContext context);
    bool OverridesAwakeningCondition(ProgramTriggerLegalityContext context);
    bool? DecideIssued(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    IReadOnlyList<CharacterState> Players { get; }
}

internal interface IProgramTriggerCardsQuery : IProgramSelectionQuery
{
    EquipmentSlot FieldEquipmentSlot(Card card);
    IReadOnlyList<Card> GetEquipment(CharacterState player);
    IReadOnlyList<Card> GetHand(CharacterState player);
    IReadOnlyList<Card> GetJudgment(CharacterState player);
    Suit GetProgramEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation = null);
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, Card card);
    bool IsSelfHandCategoryDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent);
    bool MatchesProgramCardCategory(CardKind kind, IReadOnlyList<SkillProgramCardCategory> categories);
    IReadOnlyList<Card> CardsAt(CardLocation location);
    CardLocation CardLocation(int cardId);
}

internal interface IProgramTriggerSelectionQuery : IProgramSelectionQuery
{
    ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat);
    PlayerSkillContext CreateSkillContext(CharacterState player, bool includeHandLimit = false);
    IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, SkillProgramTargetKind targetKind, ProgramSkillWindowContext? windowContext = null, PlayerMarkerKind? marker = null, ProgramParticipantReference? actorReference = null);
    int GetTurnDrawCount(CharacterState current);
    IReadOnlyList<int> WuLuKangProgramSelectionSeats(int owner, SkillProgramEffect selection, IReadOnlyList<SkillProgramEffect> instructions, ProgramSkillWindowContext? context = null);
}

internal interface IProgramTriggerWindowQuery : IProgramSelectionQuery, IProgramTriggerProofQuery
{
    bool BeforeDamageHasNoSource(BeforeDamageProgramWindowFrame frame);
    bool CanRunAfterTurnEndedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger);
    bool CanRunProgramJudgmentTrigger(ProgramJudgmentTriggerCandidate candidate, SkillProgramTrigger trigger, JudgmentFinalizedContext judgment);
    JudgmentTriggerCandidate? CurrentJudgmentCandidate(JudgmentFrame pending);
    IReadOnlyList<SkillProgram> EnabledSkillPrograms(CharacterState player);
    FirstGameDomainCrossingEvent? FirstDomainFact(ProgramSkillWindowContext context);
    Card? GetJudgmentCard(JudgmentFrame frame);
    bool HasXuRongDyingQualification(ProgramTriggerCandidate candidate,ProgramSkillWindowContext context);
    bool IsActualSkippedDrawContext(ProgramSkillWindowContext context);
    bool IsExactRoundEndingCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    bool IsIssuedZengdaoCandidate(ProgramTriggerCandidate c,ProgramSkillWindowContext w);
    bool IsProgramTargetContextValid(ProgramSkillWindowContext context);
    bool IsValidPlayerSeat(int seat);
    bool MatchesEarnedActualEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool begun);
    bool MatchesFixedDistanceDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun);
    bool MatchesFrozenOwnOrPreviousEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    bool MatchesPrepDiscardEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun);
    IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window);
    int[] MatchingDiscardPileIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger);
    int CurrentSeat { get; }
    TurnPhase Phase { get; }
    int RoundNumber { get; }
    Winner Winner { get; }
    IReadOnlyList<ResolutionFrame> Frames { get; }
    DyingFrame? ActiveDying { get; }
    DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
    JudgmentFrame? ActiveJudgment { get; }
    bool DamageRedirected { get; }
    int? ActiveSlashTargetSeat { get; }
}

internal interface IProgramTriggerUsageQuery : IProgramRuntimeOperations
{
    int AlivePlayerCount { get; }
    int GetUsage(int ownerSeat, string skillId, string bindingId, SkillUsageScope scope);
    bool TryConsumeUsage(int ownerSeat, string skillId, string bindingId, SkillUsageScope scope, int limit);
    void RecordUsageConsumed(SkillUsageConsumedEvent fact);
}

// Each access resolves the same current damage handle at its original evaluation
// point. No mutable damage handle or eagerly frozen bundle crosses the boundary.
internal interface IProgramTriggerDamageQuery : IProgramSelectionQuery
{
    CardConversionSource? DamageConversionSource(DamageTriggerWindowFrame frame);
    bool IsDamageSourceLess(DamageTriggerWindowFrame frame);
    bool IsDamageDelayedJudgment(DamageTriggerWindowFrame frame);
    CardKind? EffectiveDamageCardKind(DamageTriggerWindowFrame frame);
    IReadOnlyList<Card> PhysicalDamageCards(DamageTriggerWindowFrame frame);
}

public sealed partial class GameEngine
{
}
