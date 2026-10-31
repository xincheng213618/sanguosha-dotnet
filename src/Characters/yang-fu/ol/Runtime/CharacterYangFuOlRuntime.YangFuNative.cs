#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    internal void BindSuyiEquipmentUse(CardUseFrame use) => _yangFuOperations.Get().BindSuyiEquipmentUse(use);
    internal void IssueSuyiSlashUse(long id, ProgramSkillFrame supplied, CardActionContext action) => _yangFuOperations.Get().IssueSuyiSlashUse(id, supplied, action);
    internal bool IsSuyiSlashUse(long id) => _yangFuOperations.Get().IsSuyiSlashUse(id);
    internal bool TryGetSuyiVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> targets) => _yangFuOperations.Get().TryGetSuyiVirtualPrimaryReturn(use, parent, out targets);
    internal bool IsSuyiEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => _yangFuOperations.Get().IsSuyiEquipmentRide(ride, beneath);
    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27310)]
    internal bool ReturnSuyiAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _yangFuOperations.Get().ReturnSuyiAttack(new(completion.ResolutionId, completion.ProgramFrameId));
    bool global::CardGame.Core.IYangFuAdjacencyFacts.IgnoresSlashLimit(int owner, CardConversionSource source) => _coreCardConversionsOperations.IgnoresProgramSlashLimit(_coreMatchParticipantsPlayersAccess._players[owner], source);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasIssuedJudgedRankSlashQuota(long useId, int actor) => _coreJudgedRankSlashPoliciesOperations.HasIssuedJudgedRankSlashQuota(useId, actor);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasIssuedWuLuKangDixianUse(long useId, int actor) => _characterLuKangWumiaoOperations.HasIssuedWuLuKangDixianUse(useId, actor);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasIssuedRoundGainedBasicBonus(long useId, int actor) => _coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(useId, actor);
    void global::CardGame.Core.IYangFuAdjacencyFacts.AssertCurrentSlashFirePolicy(CardUseFrame use) => _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasPijingTargetTail(CardUseFrame use) => _characterLiuPanOlOperations.HasPijingTargetTail(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasWeifuTargetTail(CardUseFrame use) => _characterQianZhaoOlOperations.HasWeifuTargetTail(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasDesignatedExtraTargetTail(CardUseFrame use) => _coreDesignatedExtraTargetsOperations.HasDesignatedExtraTargetTail(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasRecipientCategorySlashTargetTail(CardUseFrame use) => _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.HasZhangYanLangdaoTargetTail(CardUseFrame use) => _characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(use);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate);
    DyingFrame? global::CardGame.Core.IYangFuAdjacencyFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    bool global::CardGame.Core.IYangFuAdjacencyFacts.MovementObserverEdge(long childId) => _coreObserverSubtreesOperations.IsMovementObserverEdge(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == childId));
    bool global::CardGame.Core.IYangFuAdjacencyFacts.DyingExitEdge(long childId) => _characterYanPuOlOperations.IsYanPuDyingExitEdge(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == childId));
    bool global::CardGame.Core.IYangFuAdjacencyFacts.RandomEquipmentRide(ResolutionFrame child, ResolutionFrame parent) => _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.RecoveryReplacementRide(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool global::CardGame.Core.IYangFuAdjacencyFacts.SuspendedOrPaidRescueSuffix(DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id), dying);
}
