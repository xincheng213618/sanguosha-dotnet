namespace CardGame.Core;

internal interface IProgramSkillCompletion : IProgramRuntimeOperations
{
    void Finish(ProgramSkillFrame frame, bool completed);
}

/// <summary>
/// Completes a trigger binding and returns paid movement children to their
/// original frame. Continuation catalogs retain each owning rule's proofs.
/// </summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramCompletionRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramCompletionFacts facts, IProgramCompletionProtocols protocols,
    INativeLogicalDiscardOperations logicalDiscards, IProgramBoundCardCleanup cleanup,
    IProgramRuntimePrompts prompts) : IProgramMovementReturns, IProgramSkillCompletion
{
    public void Finish(ProgramSkillFrame frame, bool completed)
    {
        if (protocols.CompleteKeming(frame, completed)) return;
        frame = logicalDiscards.CompleteFrame(frame);
        cleanup.Cleanup(frame, completed);
        if (protocols.CompleteMantianGift(frame, completed)) return;
        if (protocols.CompleteShenwuTransplant(frame, completed)) return;
        if (protocols.CompleteHellSoul(frame, completed)) return;
        if (protocols.CompleteWenheChaos(frame, completed)) return;
        if (facts.Winner != Winner.None && facts.Status != EngineStatus.Completed &&
            prompts.Current is null && frames.Top is null)
            protocols.CompleteGame();
        if (frame.TriggerId is not null)
        {
            CompleteBinding(frame, completed);
            return;
        }
        events.AdvanceFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, completed));
        frames.Pop(frame.Id, ResolutionFrameKind.ProgramSkill);
    }

    internal void CompleteBinding(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext ??
            throw new InvalidOperationException("A trigger program frame lost its window context.");
        protocols.FinishPaidOwnTarget(frame);
        protocols.FinishOwnedDeathBenefit(frame, completed);
        frames.Pop(frame.Id, ResolutionFrameKind.ProgramSkill);
        events.AdvanceFact(new ProgramBindingResolvedEvent(
            frame.Id, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId,
            frame.OwnerSeat, context.Window, Activated: true, Completed: completed));
        if (protocols.CompleteDeferredTurnEnd(frame, context)) return;
        if (protocols.ReturnCompletedBinding(frame, completed)) return;
        if (frames.Top is FirstSlashDuelLegacyWindowFrame)
        {
            protocols.CompleteFirstLegacyCandidate(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId,
                frame.GameplayHash, facts.TriggerPriority(frame)), context);
            return;
        }
        protocols.ReturnWindow(frame, completed);
    }

    void IProgramMovementReturns.ReturnMovement(long frameId) => ReturnMovement(frameId);

    internal void ReturnMovement(long frameId)
    {
        var frame = frames.ActiveProgram(frameId);
        // A declined movement handler can replace a frame. This protocol keeps
        // the captured caller frame until an existing explicit reload boundary.
        if (protocols.TryReturnMovement(frame, afterPayment: false)) return;
        if (frame.SelectedCardPayment is { } payment && frame.SelectedCardPaymentResult is null)
        {
            if (frame.PendingMovementContinuation is not null || !payment.MovementCommitted ||
                payment.ActiveChildFrameId is not null)
                throw new InvalidOperationException("A selected-card payment cannot return before its movement children finish.");
            var valid = (payment.Operation != SkillProgramEffectOp.SelectAndMoveOwnedCard || payment.Movement is { Delivered: true }) &&
                facts.Alive(frame.OwnerSeat) &&
                (facts.HasSkillInstance(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId) ||
                 facts.CanContinueEquipmentSuit(frame) || facts.CanContinueHpConditional(frame) || facts.CanContinuePhaseAlcohol(frame) ||
                 facts.CanContinueSelectedCardPayment(frame));
            frames.ReplaceTop(frame with
            {
                SelectedCardPaymentResult = new ProgramSelectedCardPaymentResult(
                    payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, valid)
            });
            if (!valid)
            {
                protocols.CancelBindingAndCleanup(frames.ActiveProgram(frameId),
                    "移动响应后技能持有人、装备持有人或技能实例已失效。");
                return;
            }
            frames.AdvanceProgram(frameId);
            return;
        }
        if (frame.DamageTargetObtain is not null)
        {
            if (!facts.ValidDamageTargetObtain(frame)) throw new InvalidOperationException("The obtain movement lost its actual producer.");
            frames.ReplaceTop(frame with { PendingMovementContinuation = null });
            frames.AdvanceProgram(frameId);
            return;
        }
        if (frame.DamageTargetMount is { Receipt: not null })
        {
            if (!facts.ValidDamageTargetMount(frame)) throw new InvalidOperationException("The mount movement lost its actual payment.");
            frames.ReplaceTop(frame with { PendingMovementContinuation = null });
            frames.AdvanceProgram(frameId);
            return;
        }
        if (protocols.TryReturnMovement(frame, afterPayment: true)) return;
        var pending = frame.PendingMovementContinuation ??
            throw new InvalidOperationException("The movement continuation is missing.");
        frames.ReplaceTop(frame with { PendingMovementContinuation = null });
        if (!facts.Alive(frame.OwnerSeat) || !facts.Alive(pending.SubjectSeat) ||
            !facts.HasSkillInstance(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId) &&
            !facts.CanContinueEquipmentSuit(frame) && !facts.CanContinueHpConditional(frame) && !facts.CanContinuePhaseAlcohol(frame))
        {
            protocols.CancelBindingAndCleanup(frames.ActiveProgram(frameId),
                "移动响应后技能持有人、装备持有人或技能实例已失效。");
            return;
        }
        if (pending.CoverageResultBind is { } bind)
            protocols.SetAttackRangeCoverage(frameId, bind, pending.SubjectSeat, pending.BeforeCount,
                facts.LivingInAttackRange(pending.SubjectSeat));
        if (frame.RepeatedJudgment is { LastMatched: not null })
            protocols.ContinueRepeatedJudgment(frameId);
        else frames.AdvanceProgram(frameId);
    }
}

internal interface IProgramCompletionFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    EngineStatus Status { get; }
    int TriggerPriority(ProgramSkillFrame frame);
    bool Alive(int seat);
    bool HasSkillInstance(int seat, string skillId, string instanceId);
    bool CanContinueEquipmentSuit(ProgramSkillFrame frame);
    bool CanContinueHpConditional(ProgramSkillFrame frame);
    bool CanContinuePhaseAlcohol(ProgramSkillFrame frame);
    bool CanContinueSelectedCardPayment(ProgramSkillFrame frame);
    bool ValidDamageTargetObtain(ProgramSkillFrame frame);
    bool ValidDamageTargetMount(ProgramSkillFrame frame);
    int LivingInAttackRange(int seat);
}

internal interface IProgramCompletionProtocols : IProgramRuntimeOperations
{
    bool CompleteKeming(ProgramSkillFrame frame, bool completed);
    bool CompleteMantianGift(ProgramSkillFrame frame, bool completed);
    bool CompleteShenwuTransplant(ProgramSkillFrame frame, bool completed);
    bool CompleteHellSoul(ProgramSkillFrame frame, bool completed);
    bool CompleteWenheChaos(ProgramSkillFrame frame, bool completed);
    void CompleteGame();
    void FinishPaidOwnTarget(ProgramSkillFrame frame);
    void FinishOwnedDeathBenefit(ProgramSkillFrame frame, bool completed);
    bool CompleteDeferredTurnEnd(ProgramSkillFrame frame, ProgramSkillWindowContext context);
    bool ReturnCompletedBinding(ProgramSkillFrame frame, bool completed);
    void CompleteFirstLegacyCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void ReturnWindow(ProgramSkillFrame frame, bool completed);
    bool TryReturnMovement(ProgramSkillFrame frame, bool afterPayment);
    void CancelBindingAndCleanup(ProgramSkillFrame frame, string reason);
    void SetAttackRangeCoverage(long frameId, string bind, int subjectSeat, int before, int after);
    void ContinueRepeatedJudgment(long frameId);
}
