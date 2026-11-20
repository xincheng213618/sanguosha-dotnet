#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangZhaoZhangHongBoundaryMobileRuntime
{
    internal const string BoundaryZhangZhaoGuzheng = "boundary:guzheng-mobile", BoundaryZhangZhaoZhijian = "boundary:zhijian-mobile";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66600)]
    internal bool QualifyBoundaryZhangZhaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c)
    {
        if (b.SkillId == BoundaryZhangZhaoZhijian)
            return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == b.OwnerSeat;
        if (b.SkillId != BoundaryZhangZhaoGuzheng)
            return true;
        return c.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && _discardPhases.Current() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } p && p.ActorSeat != b.OwnerSeat && _discardPhases.CurrentPhaseDiscards(p).Any(e => e.OriginalFrom == CardLocation.Hand(p.ActorSeat));
    }

    SkillProgramStepOutcome IBoundaryZhangZhaoMobileHost.Restore(ProgramSkillFrame f) => BeginBoundaryZhangZhaoPhase(f);
    internal SkillProgramStepOutcome BeginBoundaryZhangZhaoPhase(ProgramSkillFrame f)
    {
        if (f.BoundaryZhangZhaoPhase is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded || _discardPhases.Current() is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } p || p.ActorSeat == f.OwnerSeat)
            throw new InvalidOperationException("Mobile Guzheng requires its original other player's completed discard phase.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryZhangZhaoPhase = new(p, p.ActorSeat, "return", _discardPhases.CurrentPhaseDiscards(p)) });
        PublishBoundaryZhangZhaoPhase(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryZhangZhaoPhaseChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryZhangZhaoPhase!;
        PromptChoice Choice(string branch, int? card, string label) => new(new($"mobile-guzheng.{(f.Id)}.{(branch)}.{(card)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "boundary-zhang-zhao-phase", ["branch"] = branch });
        if (r.Stage == "return")
            return r.OriginalEntities.Where(e => e.OriginalFrom == CardLocation.Hand(r.PhaseOwnerSeat) && _discardPhases.StillAvailable(e)).Select(e => Choice("return", e.CardId, "交还【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).DisplayName + "】")).ToArray();
        return new[]
        {
            Choice("claim", null, "获得本阶段其余仍在弃牌堆的弃牌"),
            Choice("decline", null, "不获得其余弃牌")
        };
    }

    internal void PublishBoundaryZhangZhaoPhase(ProgramSkillFrame f)
    {
        var choices = BoundaryZhangZhaoPhaseChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryZhangZhaoPhase!.Stage == "return" ? "固政：交还该角色本阶段弃置的一张手牌。" : "固政：是否获得本阶段其余弃牌？", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("boundary-zhang-zhao-phase")]
    internal void AnswerBoundaryZhangZhaoPhase(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhangZhaoPhase: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundaryZhangZhaoPhaseChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards)))
            throw new InvalidOperationException("Mobile Guzheng rejects an unrelated phase, chooser or discarded entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == "return")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.PhaseOwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, false);
                return;
            }

            var id = choice.Cards.Single();
            var returnHand = CardLocation.Hand(r.PhaseOwnerSeat);
            ProgramStageRuntime.Issue(frames, f with
            {
                BoundaryZhangZhaoPhase = r with { Stage = "return-children" },
                PendingMovementContinuation = new(f.OwnerSeat, 0, null)
            }, issued => _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DiscardPile, returnHand,
                new("skill-program.boundary:guzheng-mobile.phase-return"), m =>
                {
                    var current = frames.ActiveProgram(issued.Id);
                    var delivered = m.To == returnHand;
                    frames.ReplaceTop(current with
                    {
                        BoundaryZhangZhaoPhase = current.BoundaryZhangZhaoPhase! with
                        {
                            ReturnedCardId = delivered ? id : null,
                            ReturnMovementSequence = m.Sequence
                        }
                    });
                    if (delivered)
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoReturnedEvent(issued.Id, r.PhaseOwnerSeat, id, m.Sequence));
                }));
        }
        else if (r.Stage == "claim")
        {
            var take = choice.Parameters["branch"] == "claim" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
            var ids = take ? r.OriginalEntities.Where(e => e.CardId != r.ReturnedCardId && _discardPhases.StillAvailable(e)).Select(e => e.CardId).ToArray() : [];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            ProgramStageRuntime.Issue(frames, f with
            {
                BoundaryZhangZhaoPhase = r with { Stage = "claim-children" },
                PendingMovementContinuation = new(f.OwnerSeat, 0, null)
            }, issued =>
            {
                if (ids.Length != 0)
                    _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(issued.OwnerSeat), new("skill-program.boundary:guzheng-mobile.phase-claim"));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoClaimFinishedEvent(issued.Id, take, ids.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
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
        _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
        _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
        _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) ||
        _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) ||
        _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);

    private ProgramStageResult ContinueAfterReturn(ProgramSkillFrame f, BoundaryZhangZhaoPhaseReceipt r)
    {
        if (r.ReturnedCardId is not null && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None &&
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.OriginalEntities.Any(e => e.CardId != r.ReturnedCardId && _discardPhases.StillAvailable(e)))
        {
            frames.ReplaceTop(f = f with { BoundaryZhangZhaoPhase = r with { Stage = "claim" } });
            PublishBoundaryZhangZhaoPhase(f);
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
    internal bool KeepBoundaryZhangZhaoPaidReturn(ProgramSkillFrame f) => f.BoundaryZhangZhaoPhase is { ReturnMovementSequence: > 0 };

    [ProgramAiChoice(SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards)]
    internal PromptChoice BoundaryZhangZhaoPhaseAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
