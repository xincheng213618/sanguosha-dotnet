#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJinMouMobileRuntime
{
    internal bool MouYuJinPaid(ProgramSkillFrame f) => f.MouYuJinXiayuan is not null && ValidMouXiayuan(f) && MouXiayuanPayment(f)is not null || f.MouYuJinJieyue is not null && ValidMouJieyue(f) && MouJieyueArmorGranted(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29300)]
    internal bool KeepPaidMouYuJin(ProgramSkillFrame f) => MouYuJinPaid(f);

    internal bool MouYuJinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouYuJinPaid(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is not CardsMovedTriggerWindowFrame moved || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
            return false;
        if (f.MouYuJinXiayuan is { } cost && f.InstructionIndex == 3 && MouXiayuanPayment(f)is { } paid && f.CardSetBindings.SingleOrDefault(b => b.Name == "xiayuan-cost")is { } bind)
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && bind.CardIds.Contains(m.CardId) && m.Sequence > cost.MovementBefore && m.Sequence <= paid.SequenceAfter && (m.From == CardLocation.Hand(f.OwnerSeat) || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(f.OwnerSeat)) && (m.Reason.Value == MouXiayuanCostReason || m.Reason.Value == MouXiayuanCostReason + "-finished" || m.Reason.Value == MouXiayuanCostReason + "-discard"));
        if (f.MouYuJinJieyue is not { Stage: MouYuJinJieyueStage.GiftChildren, GiftCardId: { } id, GiftFrom: { } from, GiftTo: { } to, GiftMovementSequence: { } sequence } gift)
            return false;
        return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (m.CardId == id && m.Sequence == sequence && m.From == from && m.To == to && m.Reason.Value == MouJieyueGiftReason || from.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.WoodenOx && m.Sequence > sequence && m.From == new CardLocation(CardZoneKind.WoodenOxGrain, gift.RecipientSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
    }

    internal bool MouYuJinObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame parent && _characterYuJinMouMobileOperations.MouYuJinFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? MouYuJinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterYuJinMouMobileOperations.MouYuJinFirstChild(f, child), (index) => _characterYuJinMouMobileOperations.MouYuJinObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 820)]
    internal bool IsMouYuJinDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouYuJinObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 590)]
    internal bool IsMouYuJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!MouYuJinPaid(f) || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouYuJinFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouYuJinObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29300)]
    internal bool AdvanceMouYuJinChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouYuJinObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
