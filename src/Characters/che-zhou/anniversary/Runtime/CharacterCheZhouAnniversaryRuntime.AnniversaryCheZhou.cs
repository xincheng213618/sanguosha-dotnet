#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCheZhouAnniversaryRuntime
{
    internal const string CheZhouAction = "anniversary-che-zhou", CheZhouObtainReason = "skill-program.anniversary:pigua.obtain";
    SkillProgramStepOutcome IAnniversaryCheZhouHost.Resolve(ProgramSkillFrame f) => BeginAnniversaryCheZhouPigua(f);
    internal static CardConversionSource CheZhouSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46400)]
    internal bool CanOfferAnniversaryCheZhou(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => candidate.SkillId != AnniversaryCheZhouComposition.Pigua || c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } && source == candidate.OwnerSeat && source != target && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > 0 && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } window && window.ParentFrameId == damageId && window.SourceSeat == source && window.TargetSeat == target && window.TriggerWindow == c.Window && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is { } actual && actual.SourceSeat == source && actual.TargetSeat == target && actual.Amount == c.Amount && _coreDamageZoneAcquisitionOperations.Value.DamageAcquisitionHasExactOwningAttack(actual, window);
    internal bool ExactCheZhouDamage(ProgramSkillFrame f)
    {
        if (f.SkillId != AnniversaryCheZhouComposition.Pigua || f.TriggerId != AnniversaryCheZhouComposition.Binding || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } c || source != f.OwnerSeat || target == source || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.ParentFrameId != damageId || window.SourceSeat != source || window.TargetSeat != target || window.TriggerWindow != c.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is not { } actual || actual.SourceSeat != source || actual.TargetSeat != target || actual.Amount != c.Amount || !_coreDamageZoneAcquisitionOperations.Value.DamageAcquisitionHasExactOwningAttack(actual, window))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageRequestedEvent>().Count(e => e.ResolutionId == damageId && !e.SourceLess && e.SourceSeat == source && e.TargetSeat == target && e.Amount == c.Amount && e.Nature == actual.Nature) == 1;
    }

    internal SkillProgramStepOutcome BeginAnniversaryCheZhouPigua(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryCheZhou is not null || !ExactCheZhouDamage(f) || f.WindowContext is not { TargetSeat: { } victim } c || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Pigua requires its original actual damage source, victim and greater-than-one damage.");
        var r = new AnniversaryCheZhouPiguaReceipt(CheZhouSource(f), f.GameplayHash, c.ParentFrameId, c.DamageFrameId!.Value, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, victim, _coreActualRoundProgressRoundNumberAccess._roundNumber, AnniversaryCheZhouStage.Selecting);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCheZhou = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCheZhouPiguaStartedEvent(f.Id, r.Source, r.Hash, r.WindowId, r.DamageId, r.Turn, r.TurnSeat, r.Victim, r.Round));
        PublishCheZhou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CheZhouChoices(ProgramSkillFrame f)
    {
        if (f.AnniversaryCheZhou is not { Stage: AnniversaryCheZhouStage.Selecting } r || !_coreMatchParticipantsPlayersAccess._players[r.Victim].IsAlive)
            return[];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Victim]);
        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Victim, [CardZoneKind.Hand], OwnedCardMoveIntent.Obtain).Where(c => !r.Selected.Contains(hand[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id)).Select(c => new PromptChoice(new($"che-zhou.{(f.Id)}.pick-{(r.Selected.Count)}.slot-{(c.Parameters["slot-index"])}"), c.Description, [], [], new Dictionary<string, string> { ["program-action"] = CheZhouAction, ["verb"] = "card", ["slot-index"] = c.Parameters["slot-index"], ["selected-count"] = r.Selected.Count.ToString(CultureInfo.InvariantCulture) })).ToList();
        if (r.Selected.Count > 0)
            choices.Add(new(new($"che-zhou.{(f.Id)}.finish-{(r.Selected.Count)}"), $"获得已选的 {(r.Selected.Count)} 张手牌", [], [], new Dictionary<string, string> { ["program-action"] = CheZhouAction, ["verb"] = "finish", ["selected-count"] = r.Selected.Count.ToString(CultureInfo.InvariantCulture) }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishCheZhou(ProgramSkillFrame f)
    {
        var r = f.AnniversaryCheZhou!;
        if (r.Selected.Count >= r.Round || CheZhouChoices(f).All(c => c.Parameters["verb"] == "finish"))
        {
            if (r.Selected.Count > 0)
                IssueCheZhouObtain(f);
            else
                FinishCheZhou(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var choices = CheZhouChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"披挂：选择至多 {(r.Round)} 张暗手牌（已选 {(r.Selected.Count)} 张）。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Victim,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryCheZhouPigua)]
    internal PromptChoice CheZhouAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer(CheZhouAction)]
    internal void AnswerCheZhou(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCheZhou: { Stage: AnniversaryCheZhouStage.Selecting } r } f || !ValidCheZhou(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], CheZhouChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Pigua requires its published opaque Hand slot and unpaid original frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        if (answer.Parameters["verb"] == "finish")
        {
            IssueCheZhouObtain(f);
            return;
        }

        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Victim])[int.Parse(answer.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCheZhou = r with { Selected = r.Selected.Append(card.Id).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCheZhouPiguaSelectedEvent(f.Id, f.AnniversaryCheZhou!.Selected.Count));
        PublishCheZhou(f);
    }

    internal void IssueCheZhouObtain(ProgramSkillFrame f)
    {
        var r = f.AnniversaryCheZhou!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCheZhou = r with { Stage = AnniversaryCheZhouStage.MovementChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, CardLocation.Hand(f.OwnerSeat), new(CheZhouObtainReason), (batch, moves) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryCheZhou = active.AnniversaryCheZhou!with { BatchId = batch, Moves = moves, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCheZhouPiguaPaidEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnSeat, r.Victim, moves.Count, batch, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, disclosure: CardMovementDisclosure.FaceDown);
        ContinueCheZhou(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void FinishCheZhou(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCheZhou = f.AnniversaryCheZhou!with { Stage = AnniversaryCheZhouStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCheZhouPiguaCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46400)]
    internal bool ContinueCheZhou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCheZhou: { Stage: not AnniversaryCheZhouStage.Complete } r } f || f.Id != id)
            return false;
        if (!ValidCheZhou(f))
            throw new InvalidOperationException("Pigua lost its native damage, original Hand selection or paid movement receipt.");
        if (r.Stage == AnniversaryCheZhouStage.Selecting)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCheZhou(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        FinishCheZhou(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46400)]
    internal bool ReturnCheZhouMovement(ProgramSkillFrame f) => f.AnniversaryCheZhou is not null && ContinueCheZhou(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46400)]
    internal bool ContinuePaidCheZhou(ProgramSkillFrame f) => f.AnniversaryCheZhou is { Stage: AnniversaryCheZhouStage.MovementChildren } && ValidCheZhou(f);
}
