#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    internal IReadOnlyList<IReadOnlyList<int>> BingzhongLegalTargets(CardUseFrame use) => _yangFuOperations.Get().BingzhongLegalTargets(use);
    internal bool HasBingzhongTargetTail(CardUseFrame use) => _yangFuOperations.Get().HasBingzhongTargetTail(use);
    internal bool HasBingzhongTargetTail(CardUseFrame use, CardActionContext action) => _yangFuOperations.Get().HasBingzhongTargetTail(use, action);
    internal void RecordBingzhongRedirect(ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat) => _yangFuOperations.Get().RecordBingzhongRedirect(frame, useId, previousSeat, nextSeat);
    bool IYangFuTargetFacts.TryGetDesignatedExtraTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window) => _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(owner, context, out use, out window);
    bool IYangFuTargetFacts.DesignatedExtraTargetCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window) => _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window);
    int? IYangFuTargetFacts.CapturedWholeActualTurnActorUseOrdinal(CardActionContext action) => _coreWholeActualTurnUsesOperations.CapturedWholeActualTurnActorUseOrdinal(action);
    bool IYangFuTargetFacts.IsKenshangRemovedMaterial(long useId, int cardId) => _coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(useId, cardId);
    bool IYangFuTargetFacts.IsWuJingHejiRemovedMaterial(long useId, int cardId) => _characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(useId, cardId);
    bool IYangFuTargetFacts.IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => _coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
    bool IYangFuTargetFacts.IsZhangYanSujiRemovedMaterial(long useId, int cardId) => _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(useId, cardId);
    bool IYangFuTargetFacts.IsCurrentUsePhysicalCardClaim(long useId, int cardId) => _coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(useId, cardId);
    bool IYangFuTargetFacts.IsExchangedCardClaim(long actionId, int cardId) => _coreResponseEntityExchangeOperations.IsExchangedCardClaim(actionId, cardId);
    bool IYangFuTargetFacts.CanUseVirtualSlash(int owner, int target) => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    bool IYangFuTargetFacts.DirectedTargetProhibited(int owner, int target, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, kind);
    bool IYangFuTargetFacts.TargetProhibited(int target, CardKind kind, Suit? suit, bool? isRed) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, suit, isRed);
    bool IYangFuTargetFacts.BeneficiarySuitShield(int owner, int target, Suit? suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner, target, suit);
    bool IYangFuTargetFacts.SelfTargetForbidden(int owner, CardKind kind, IReadOnlyList<int> targets) => _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[owner], kind, targets);
    bool IYangFuTargetFacts.CanBeDistanceFreeDesignatedTarget(CardUseFrame use, int target) => _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, _coreMatchParticipantsPlayersAccess._players[target]);
    bool IYangFuTargetFacts.CanUsePhysicalSlash(int owner, int target, Card card, CardConversionSource? source, CardKind kind, long useId, int? rank, IReadOnlyList<int> physicalIds) => _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], card, source, kind, existingUseFrameId: useId, specificEffectiveRank: rank, physicalCardIds: physicalIds);
    int IYangFuTargetFacts.CombatDistance(int owner, int target) => _coreNativeRuleQueriesOperations.GetCombatDistance(owner, target);
    bool IYangFuTargetFacts.CardDistanceExemption(int owner, int target, CardKind kind, long useId) => _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], kind, useId);
    bool IYangFuTargetFacts.CardPolicy(int owner, SkillProgramCardPolicyKind policy, CardKind kind) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[owner], policy, kind);
    bool IYangFuTargetFacts.HasIssuedFuRongDistance(long useId, int actor) => _characterFuRongOlOperations.HasIssuedFuRongDistance(useId, actor);
    bool IYangFuTargetFacts.HasIssuedProvenanceUseDistance(long useId, int actor) => _coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(useId, actor);
    bool IYangFuTargetFacts.HasIssuedGrantedPhaseEntityDistance(long useId, int actor) => _coreGrantedEntityPhasesOperations.HasIssuedGrantedPhaseEntityDistance(useId, actor);
    bool IYangFuTargetFacts.HasPhaseSuitAllowance(int owner, Suit? suit) => _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(owner, suit);
    bool IYangFuTargetFacts.FirstRedAvailable(int owner, Suit? suit) => _characterGuanYuRuntimeOperations.GuanFirstRedAvailable(_coreMatchParticipantsPlayersAccess._players[owner], suit);
    bool IYangFuTargetFacts.BorrowedSwordTarget(int owner, int target) => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
}
