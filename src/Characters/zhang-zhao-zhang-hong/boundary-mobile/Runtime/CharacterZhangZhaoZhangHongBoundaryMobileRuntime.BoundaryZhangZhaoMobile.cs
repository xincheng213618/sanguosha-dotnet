#nullable enable

namespace CardGame.Core.EngineOwners;
[ProgramRuntimeModule, ProgramCapability]
internal sealed class CharacterZhangZhaoZhangHongBoundaryMobileRuntime(
    IKernelContentRegistryAccess content, IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    IActualDiscardRecoveryPhaseQueries phases,
    RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries cards, RuntimeEntries.CoreGameEngineProgramLifecycleEntries lifecycle,
    RuntimeEntries.CoreGameEngineSkillProgramsEntries programs, INativeLogicalDiscardMovement movement,
    IProgramChildWindowEntries children, IProgramSkillsChangedWindows skillsChanged, IProgramMovementWindows cardsMoved,
    IKernelRuleProgressionOperations rules, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess turn,
    EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess journal,
    EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess outcome, IMatchParticipantStateAccess participants)
    : IBoundaryZhangZhaoMobileHost
{
    internal const string BoundaryZhangZhaoGuzheng = "boundary:guzheng-mobile", BoundaryZhangZhaoZhijian = "boundary:zhijian-mobile";
    private IEnumerable<DiscardRecoveryEntity> RemainingDiscards(BoundaryZhangZhaoPhaseReceipt receipt) =>
        receipt.OriginalEntities.Where(e => e.CardId != receipt.ReturnedCardId && phases.StillAvailable(e));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66600)]
    internal bool QualifyBoundaryZhangZhaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c)
    {
        if (b.SkillId == BoundaryZhangZhaoZhijian)
            return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && turn._phase == TurnPhase.Play && turn._currentSeat == b.OwnerSeat;
        if (b.SkillId != BoundaryZhangZhaoGuzheng)
            return true;
        return c.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && phases.Current() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } p &&
            p.ActorSeat != b.OwnerSeat && phases.CurrentPhaseDiscards(p).Any(e => e.OriginalFrom == CardLocation.Hand(p.ActorSeat));
    }

    SkillProgramStepOutcome IBoundaryZhangZhaoMobileHost.Restore(ProgramSkillFrame f)
    {
        if (f.BoundaryZhangZhaoPhase is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded || phases.Current() is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } p || p.ActorSeat == f.OwnerSeat)
            throw new InvalidOperationException("Mobile Guzheng requires its original other player's completed discard phase.");
        frames.ReplaceTop(f = f with { BoundaryZhangZhaoPhase = new(p, p.ActorSeat, "return", phases.CurrentPhaseDiscards(p)) });
        PublishChoices(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> Choices(ProgramSkillFrame f)
    {
        var r = f.BoundaryZhangZhaoPhase!;
        PromptChoice Choice(string branch, int? card, string label) => new(
            new($"mobile-guzheng.{f.Id}.{branch}.{card}"), label, card is { } id ? [id] : [], [],
            new Dictionary<string, string> { ["program-action"] = "boundary-zhang-zhao-phase", ["branch"] = branch });
        if (r.Stage == "return")
            return r.OriginalEntities
                .Where(e => e.OriginalFrom == CardLocation.Hand(r.PhaseOwnerSeat) && phases.StillAvailable(e))
                .Select(e => Choice("return", e.CardId, $"交还【{cards.GetAdvancedCard(e.CardId).DisplayName}】")).ToArray();
        return new[]
        {
            Choice("claim", null, "获得本阶段其余仍在弃牌堆的弃牌"),
            Choice("decline", null, "不获得其余弃牌")
        };
    }

    private void PublishChoices(ProgramSkillFrame f)
    {
        var choices = Choices(f);
        var skill = content._contentRegistry.GetSkill(f.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryZhangZhaoPhase!.Stage == "return" ? "固政：交还该角色本阶段弃置的一张手牌。" : "固政：是否获得本阶段其余弃牌？", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = prompts.NextId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        }, participants.At(f.OwnerSeat).IsHuman);
        rules.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("boundary-zhang-zhao-phase")]
    internal void AnswerBoundaryZhangZhaoPhase(PromptChoice choice)
    {
        if (frames.Top is not ProgramSkillFrame { BoundaryZhangZhaoPhase: { } r } f || prompts.Current?.PlayerSeat != f.OwnerSeat ||
            !Choices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards)))
            throw new InvalidOperationException("Mobile Guzheng rejects an unrelated phase, chooser or discarded entity.");
        prompts.Clear();
        if (r.Stage == "return")
        {
            if (!participants.At(f.OwnerSeat).IsAlive || !participants.At(r.PhaseOwnerSeat).IsAlive || !lifecycle.CanInvokeProgramSkill(f))
            {
                programs.FinishProgramSkill(f, false);
                return;
            }

            var id = choice.Cards.Single();
            var returnHand = CardLocation.Hand(r.PhaseOwnerSeat);
            ProgramStageRuntime.Issue(frames, f with
            {
                BoundaryZhangZhaoPhase = r with { Stage = "return-children" },
                PendingMovementContinuation = new(f.OwnerSeat, 0, null)
            }, issued => movement.Move(cards.GetAdvancedCard(id), CardLocation.DiscardPile, returnHand,
                new("skill-program.boundary:guzheng-mobile.phase-return"), m =>
                {
                    var current = frames.ActiveProgram(issued.Id);
                    var delivered = m.To == returnHand;
                    frames.ReplaceTop(current with
                    {
                        BoundaryZhangZhaoPhase = current.BoundaryZhangZhaoPhase! with
                        {
                            ReturnedCardId = delivered ? m.CardId : null,
                            ReturnMovementSequence = m.Sequence
                        }
                    });
                    if (delivered)
                        rules.AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoReturnedEvent(issued.Id, r.PhaseOwnerSeat, m.CardId, m.Sequence));
                }));
        }
        else if (r.Stage == "claim")
        {
            var take = choice.Parameters["branch"] == "claim" && participants.At(f.OwnerSeat).IsAlive && outcome._winner == Winner.None;
            var ids = take ? RemainingDiscards(r).Select(e => e.CardId).ToArray() : [];
            var before = journal._movementSequence;
            ProgramStageRuntime.Issue(frames, f with
            {
                BoundaryZhangZhaoPhase = r with { Stage = "claim-children" },
                PendingMovementContinuation = new(f.OwnerSeat, 0, null)
            }, issued =>
            {
                if (ids.Length != 0)
                    movement.MoveCards(ids.Select(cards.GetAdvancedCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(issued.OwnerSeat), new("skill-program.boundary:guzheng-mobile.phase-claim"));
                rules.AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoClaimFinishedEvent(issued.Id, take, ids.Length, before, journal._movementSequence));
            });
        }
        else
            throw new InvalidOperationException("Mobile Guzheng cannot repeat an already completed return or claim.");
        frames.AdvanceProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 66600)]
    internal bool ResumeBoundaryZhangZhaoPhase(long id)
    {
        if (frames.Top is not ProgramSkillFrame { BoundaryZhangZhaoPhase: not null } f || f.Id != id)
            return false;
        ProgramStageRuntime.Run(id, frames, prompts, this, MovementStages.Plan);
        return true;
    }

    private bool AwaitMovementChildren(long id) =>
        children.Recovery(id, PostEventContinuation.AwaitedProgramMovement) || children.CharacterState(id) ||
        skillsChanged.Begin(id) || children.HpChanged(id, PostEventContinuation.AwaitedProgramMovement) || cardsMoved.Begin(id);

    private ProgramStageResult ContinueAfterReturn(ProgramSkillFrame f, BoundaryZhangZhaoPhaseReceipt r)
    {
        if (r.ReturnedCardId is not null && outcome._winner == Winner.None &&
            participants.At(f.OwnerSeat).IsAlive && RemainingDiscards(r).Any())
        {
            frames.ReplaceTop(f = f with { BoundaryZhangZhaoPhase = r with { Stage = "claim" } });
            PublishChoices(f);
        }
        else
            FinishMovement(f);
        return ProgramStageResult.Yield;
    }

    private void FinishMovement(ProgramSkillFrame f)
    {
        frames.ReplaceTop(f with { BoundaryZhangZhaoPhase = null });
        frames.AdvanceProgram(f.Id);
    }

    private enum MovementStage { Return, Claim }

    private static class MovementStages
    {
        internal static readonly ProgramStagePlan<CharacterZhangZhaoZhangHongBoundaryMobileRuntime, BoundaryZhangZhaoPhaseReceipt, MovementStage> Plan = new(
            read: static frame => frame.BoundaryZhangZhaoPhase is { Stage: "return-children" or "claim-children" } receipt ? receipt : null,
            stage: static receipt => receipt.Stage == "return-children" ? MovementStage.Return : MovementStage.Claim,
            nodes:
            [
                ProgramStageNode<CharacterZhangZhaoZhangHongBoundaryMobileRuntime, BoundaryZhangZhaoPhaseReceipt, MovementStage>.AwaitChildren(
                    MovementStage.Return, AwaitChildren, static (runtime, frame, receipt) => runtime.ContinueAfterReturn(frame, receipt), clearMovement: true),
                ProgramStageNode<CharacterZhangZhaoZhangHongBoundaryMobileRuntime, BoundaryZhangZhaoPhaseReceipt, MovementStage>.ChildrenComplete(
                    MovementStage.Claim, clearMovement: true)
            ],
            drain: AwaitChildren,
            complete: static (runtime, frame) => runtime.FinishMovement(frame));

        private static bool AwaitChildren(CharacterZhangZhaoZhangHongBoundaryMobileRuntime runtime,
            ProgramSkillFrame frame, BoundaryZhangZhaoPhaseReceipt _) => runtime.AwaitMovementChildren(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 66600)]
    internal bool ReturnBoundaryZhangZhaoPhase(ProgramSkillFrame f) => f.BoundaryZhangZhaoPhase is not null && ResumeBoundaryZhangZhaoPhase(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 66600)]
    internal bool KeepBoundaryZhangZhaoPaidReturn(ProgramSkillFrame f) =>
        f.BoundaryZhangZhaoPhase is { ReturnMovementSequence: > 0 };

    [ProgramAiChoice(SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards)]
    internal PromptChoice BoundaryZhangZhaoPhaseAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
