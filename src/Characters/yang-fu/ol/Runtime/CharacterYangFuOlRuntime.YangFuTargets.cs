#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    internal IReadOnlyList<IReadOnlyList<int>> BingzhongLegalTargets(CardUseFrame use) => _yangFuOperations.Get().BingzhongLegalTargets(use);
    internal bool HasBingzhongTargetTail(CardUseFrame use) => _yangFuOperations.Get().HasBingzhongTargetTail(use);
    internal bool HasBingzhongTargetTail(CardUseFrame use, CardActionContext action) => _yangFuOperations.Get().HasBingzhongTargetTail(use, action);
    internal void RecordBingzhongRedirect(ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat) => _yangFuOperations.Get().RecordBingzhongRedirect(frame, useId, previousSeat, nextSeat);
    bool global::CardGame.Core.IYangFuTargetFacts.TryGetDesignatedExtraTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window) => _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(owner, context, out use, out window);
    bool global::CardGame.Core.IYangFuTargetFacts.DesignatedExtraTargetCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window) => _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window);
    int? global::CardGame.Core.IYangFuTargetFacts.CapturedWholeActualTurnActorUseOrdinal(CardActionContext action) => _coreWholeActualTurnUsesOperations.CapturedWholeActualTurnActorUseOrdinal(action);
    bool global::CardGame.Core.IYangFuTargetFacts.IsKenshangRemovedMaterial(long useId, int cardId) => _coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(useId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.IsWuJingHejiRemovedMaterial(long useId, int cardId) => _characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(useId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => _coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.IsZhangYanSujiRemovedMaterial(long useId, int cardId) => _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(useId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.IsCurrentUsePhysicalCardClaim(long useId, int cardId) => _coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(useId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.IsExchangedCardClaim(long actionId, int cardId) => _coreResponseEntityExchangeOperations.IsExchangedCardClaim(actionId, cardId);
    bool global::CardGame.Core.IYangFuTargetFacts.CanUseVirtualSlash(int owner, int target) => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    bool global::CardGame.Core.IYangFuTargetFacts.DirectedTargetProhibited(int owner, int target, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, kind);
    bool global::CardGame.Core.IYangFuTargetFacts.TargetProhibited(int target, CardKind kind, Suit? suit, bool? isRed) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, suit, isRed);
    bool global::CardGame.Core.IYangFuTargetFacts.BeneficiarySuitShield(int owner, int target, Suit? suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner, target, suit);
    bool global::CardGame.Core.IYangFuTargetFacts.SelfTargetForbidden(int owner, CardKind kind, IReadOnlyList<int> targets) => _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[owner], kind, targets);
    bool global::CardGame.Core.IYangFuTargetFacts.CanBeDistanceFreeDesignatedTarget(CardUseFrame use, int target) => _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, _coreMatchParticipantsPlayersAccess._players[target]);
    bool global::CardGame.Core.IYangFuTargetFacts.CanUsePhysicalSlash(int owner, int target, Card card, CardConversionSource? source, CardKind kind, long useId, int? rank, IReadOnlyList<int> physicalIds) => _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], card, source, kind, existingUseFrameId: useId, specificEffectiveRank: rank, physicalCardIds: physicalIds);
    int global::CardGame.Core.IYangFuTargetFacts.CombatDistance(int owner, int target) => _coreNativeRuleQueriesOperations.GetCombatDistance(owner, target);
    bool global::CardGame.Core.IYangFuTargetFacts.CardDistanceExemption(int owner, int target, CardKind kind, long useId) => _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], kind, useId);
    bool global::CardGame.Core.IYangFuTargetFacts.CardPolicy(int owner, SkillProgramCardPolicyKind policy, CardKind kind) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[owner], policy, kind);
    bool global::CardGame.Core.IYangFuTargetFacts.HasIssuedFuRongDistance(long useId, int actor) => _characterFuRongOlOperations.HasIssuedFuRongDistance(useId, actor);
    bool global::CardGame.Core.IYangFuTargetFacts.HasIssuedProvenanceUseDistance(long useId, int actor) => _coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(useId, actor);
    bool global::CardGame.Core.IYangFuTargetFacts.HasIssuedGrantedPhaseEntityDistance(long useId, int actor) => _coreGrantedEntityPhasesOperations.HasIssuedGrantedPhaseEntityDistance(useId, actor);
    bool global::CardGame.Core.IYangFuTargetFacts.HasPhaseSuitAllowance(int owner, Suit? suit) => _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(owner, suit);
    bool global::CardGame.Core.IYangFuTargetFacts.FirstRedAvailable(int owner, Suit? suit) => _characterGuanYuRuntimeOperations.GuanFirstRedAvailable(_coreMatchParticipantsPlayersAccess._players[owner], suit);
    bool global::CardGame.Core.IYangFuTargetFacts.BorrowedSwordTarget(int owner, int target) => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
}
