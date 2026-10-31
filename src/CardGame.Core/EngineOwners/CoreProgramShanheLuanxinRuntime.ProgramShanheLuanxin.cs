#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramShanheLuanxinRuntime
{
    internal const string ShanheLuanxinSkillId = "shanhe:companion-luanxin";
    void global::CardGame.Core.IShanheLuanxinProgramHost.QueueNextDraw(ProgramSkillFrame frame) => QueueShanheLuanxinNextDraw(frame);
    internal bool HasShanheLuanxinForeignHandDiscard(int owner, CardMovementBatchContext batch)
    {
        var sequences = batch.Movements.Select(movement => (long)movement.Sequence).ToHashSet();
        var invoices = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(invoice => invoice.ExecutorSeat != owner).Select(invoice => invoice.InvocationId).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(material => invoices.Contains(material.InvocationId) && material.From == CardLocation.Hand(owner) && material.To == CardLocation.DiscardPile && sequences.Contains(material.CompletionSequence));
    }

    internal bool HasShanheLuanxinOtherHandGain(int owner, CardMovementBatchContext batch) => batch.Movements.Any(movement => movement.To == CardLocation.Hand(owner) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(movement, batch)is { Zone: CardZoneKind.Hand, OwnerSeat: { } source } && source != owner);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 81600)]
    internal bool CanOfferShanheLuanxinMovement(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != ShanheLuanxinSkillId)
            return true;
        if (trigger.Id is not ("foreign-hand-discard" or "other-hand-gain"))
            return true;
        if (context.MovementBatch is not { } batch)
            return false;
        return trigger.Id == "foreign-hand-discard" ? HasShanheLuanxinForeignHandDiscard(candidate.OwnerSeat, batch) : HasShanheLuanxinOtherHandGain(candidate.OwnerSeat, batch);
    }

    internal void QueueShanheLuanxinNextDraw(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheLuanxinSkillId || frame.TriggerId != "other-hand-gain" || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch } || !HasShanheLuanxinOtherHandGain(frame.OwnerSeat, batch) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Any(issued => issued.ProgramFrameId == frame.Id && issued.InstructionIndex == frame.InstructionIndex))
            throw new InvalidOperationException("Luanxin requires its original completed other-player hand gain batch.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLuanxinNextDrawIssuedEvent(frame.Id, frame.InstructionIndex, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, batch.Id, frame.OwnerSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 81600)]
    internal void AssignShanheLuanxinNextDraw(ActualDiscardRecoveryPhaseStartedEvent started)
    {
        if (started.Phase.Kind != ActualDiscardRecoveryPhaseKind.Draw || !_coreMatchParticipantsPlayersAccess._players[started.Phase.ActorSeat].IsAlive || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.QueueShanheLuanxinNextDraw))
            return;
        var assigned = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLuanxinNextDrawAssignedEvent>().Select(item => (item.ProgramFrameId, item.InstructionIndex)).ToHashSet();
        foreach (var issued in _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Where(item => item.TargetSeat == started.Phase.ActorSeat && !assigned.Contains((item.ProgramFrameId, item.InstructionIndex))).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLuanxinNextDrawAssignedEvent(issued.ProgramFrameId, issued.InstructionIndex, started.Phase));
    }

    internal int ShanheLuanxinNextDrawBonus(CharacterState player)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.QueueShanheLuanxinNextDraw) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat)
            return 0;
        var assigned = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLuanxinNextDrawAssignedEvent>().Where(item => item.Phase == phase).Select(item => (item.ProgramFrameId, item.InstructionIndex)).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Count(item => item.TargetSeat == player.Seat && assigned.Contains((item.ProgramFrameId, item.InstructionIndex)));
    }
}
