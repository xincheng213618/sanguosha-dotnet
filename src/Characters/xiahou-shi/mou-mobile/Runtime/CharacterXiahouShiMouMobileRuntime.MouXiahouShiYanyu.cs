#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouShiMouMobileRuntime
{
    internal const string MouXiahouShiDiscardReason = "skill-program.mou-mobile:yanyu.discard", MouXiahouShiDrawReason = "skill-program.mou-mobile:yanyu.Draw", MouXiahouShiSupportDrawReason = "skill-program.mou-mobile:yanyu.support.Draw", MouXiahouShiQiaoshiDrawReason = "skill-program.mou-mobile:qiaoshi.Draw";
    internal static CardConversionSource MouXiahouShiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xiahou_shi_mou_mobile_MouXiahouShiYanyu.MouXiahouShiSource(f);
    internal static bool MouXiahouShiSlash(CardKind k) => RuntimeSkills.RuntimeRules_xiahou_shi_mou_mobile_MouXiahouShiYanyu.MouXiahouShiSlash(k);
    internal int MouXiahouShiPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouShiYanyuStartedEvent>().Count(e => !e.Support && e.Source.OwnerSeat == owner && e.Source.SkillId == MouXiahouShiComposition.Yanyu && e.Phase == phase);
    internal int MouXiahouShiTurnDiscards(int owner, MouXiahouShiTurnKey turn) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouShiYanyuDiscardPaidEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MouXiahouShiComposition.Yanyu && e.Turn == turn && ValidMouXiahouShiPaidHistory(e));
    [ProgramPhaseUsedProjection(40)]
    internal int MouXiahouShiProjectedPhaseUses(int owner, string skill, int fallback) => skill == MouXiahouShiComposition.Yanyu && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? MouXiahouShiPhaseUses(owner, phase) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 29800, SkillProgramEffectOp.ResolveMouXiahouShiYanyuDiscard)]
    internal bool CanStartMouXiahouShiYanyu(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && CurrentMouXiahouShiTurn()is not null && MouXiahouShiPhaseUses(owner.Seat, phase) < 2 && new[]
    {
        CardZoneKind.Hand,
        CardZoneKind.Equipment
    }.Any(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, owner.Seat)).Any(c => MouXiahouShiSlash(c.Kind) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, c, new(zone, owner.Seat), OwnedCardMoveIntent.Discard)));
    internal SkillProgramStepOutcome BeginMouXiahouShiDiscard(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouXiahouShiYanyu is not null || f.SkillId != MouXiahouShiComposition.Yanyu || f.ActivationId != "discard-slash" || f.TriggerId is not null || f.InstructionIndex != 1 || f.WindowContext is not null || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats.Count != 0 || !CanStartMouXiahouShiYanyu(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || CurrentMouXiahouShiTurn()is not { } turn || _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: var seat, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from || seat != f.OwnerSeat)
            throw new InvalidOperationException("Yanyu requires one currently owned payable Slash in the exact actual Play.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        if (!MouXiahouShiSlash(card.Kind) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Yanyu cannot recast or discard a forbidden non-Slash payment.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = new MouXiahouShiYanyuReceipt(MouXiahouShiSource(f), f.GameplayHash, turn, phase, false)
        {
            Stage = MouXiahouShiYanyuStage.Discarding,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiYanyuStartedEvent(f.Id, r.Source, r.GameplayHash, turn, phase, false, 0, 0));
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(MouXiahouShiDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == id && m.From == from && m.Reason.Value == MouXiahouShiDiscardReason);
        var paid = new MouXiahouShiDiscardPayment(id, card.Kind, from, m.To, m.Sequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = r with { Stage = MouXiahouShiYanyuStage.DiscardChildren, Payment = paid, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiYanyuDiscardPaidEvent(f.Id, r.Source, turn, phase, id, card.Kind, from, m.To, m.Sequence));
        SealMouXiahouShiMovements(f.Id, before);
        ContinueMouXiahouShi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome BeginMouXiahouShiSupport(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouXiahouShiYanyu is not null || !ExactMouXiahouShiSupportContext(f) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || CurrentMouXiahouShiTurn()is not { } turn || MouXiahouShiTurnDiscards(f.OwnerSeat, turn)is not > 0)
            throw new InvalidOperationException("Yanyu support requires its current own native PlayEnding and real actual-turn paid Slashes.");
        var count = MouXiahouShiTurnDiscards(f.OwnerSeat, turn);
        var r = new MouXiahouShiYanyuReceipt(MouXiahouShiSource(f), f.GameplayHash, turn, phase, true)
        {
            Stage = MouXiahouShiYanyuStage.TargetChoice,
            WindowId = f.WindowContext!.ParentFrameId,
            FrozenDiscardCount = count,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiYanyuStartedEvent(f.Id, r.Source, r.GameplayHash, turn, phase, true, r.WindowId, count));
        PublishMouXiahouShi(f, f.OwnerSeat, MouXiahouShiSupportChoices(f), $"燕语：令一名其他角色摸{(checked(count * 3))}张牌。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouXiahouShiSupportChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => new PromptChoice(new($"mou-xiahou-shi.{(f.Id)}.support.{(p.Seat)}"), $"令{(p.Name)}摸{(checked(f.MouXiahouShiYanyu!.FrozenDiscardCount * 3))}张牌", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mou-xiahou-shi-yanyu-support", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["target-seat"] = p.Seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    [ProgramAnswer("mou-xiahou-shi-yanyu-support")]
    internal void AnswerMouXiahouShiSupport(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouXiahouShiYanyu!;
        if (!ValidMouXiahouShiYanyu(f) || !r.Support || r.Stage != MouXiahouShiYanyuStage.TargetChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXiahouShiSupportChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Yanyu support accepts only its exact living other target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var seat = selected.Targets.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = r with { TargetSeat = seat, Stage = MouXiahouShiYanyuStage.Drawing } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiYanyuTargetAcceptedEvent(f.Id, seat, r.FrozenDiscardCount, checked(r.FrozenDiscardCount * 3)));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, checked(r.FrozenDiscardCount * 3), new(MouXiahouShiSupportDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouXiahouShiYanyuDiscard, SkillProgramEffectOp.ResolveMouXiahouShiYanyuSupport, SkillProgramEffectOp.ResolveMouXiahouShiQiaoshi)]
    internal void ReturnMouXiahouShiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var q = f.MouXiahouShiQiaoshi;
        var y = f.MouXiahouShiYanyu;
        var seat = q is not null ? q.DamageSourceSeat : y!.Support ? y.TargetSeat!.Value : f.OwnerSeat;
        var count = q is not null ? 2 : y!.Support ? checked(y.FrozenDiscardCount * 3) : 1;
        if (!ValidMouXiahouShi(f) || outcome.RecipientSeat != seat || outcome.RequestedCount != count || q is not null && (q.Stage != MouXiahouShiQiaoshiStage.Drawing || q.Draw is not null) || y is not null && (y.Stage != MouXiahouShiYanyuStage.Drawing || y.Draw is not null))
            throw new InvalidOperationException("Mou Xiahou Shi cannot repeat or redirect its issued native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = y is null ? null : y with { Stage = MouXiahouShiYanyuStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, MouXiahouShiQiaoshi = q is null ? null : q with { Stage = MouXiahouShiQiaoshiStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiDrawReturnedEvent(f.Id, seat, count, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        SealMouXiahouShiMovements(f.Id, outcome.SequenceBefore);
        ContinueMouXiahouShi(f.Id);
    }

    internal void PublishMouXiahouShi(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text)
    {
        if (choices.Count == 0)
            throw new InvalidOperationException("Mou Xiahou Shi lost its actual legal choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void FinishMouXiahouShi(ProgramSkillFrame f)
    {
        var accepted = f.MouXiahouShiQiaoshi?.Branch == "recover-and-draw" || f.MouXiahouShiYanyu is { Payment: not null } or { TargetSeat: not null };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiCompletedEvent(f.Id, f.SkillId, accepted));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = null, MouXiahouShiQiaoshi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouXiahouShiYanyuSupport)]
    internal PromptChoice AiMouXiahouShiSupport(PendingDecision pending, ProgramSkillFrame f) => pending.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[pending.PlayerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(pending.PlayerSeat), c.Targets.Single(), new(OwnerDraw: 0, OwnerRecovery: 0, OwnerHpLoss: 0, TargetDraw: checked(f.MouXiahouShiYanyu!.FrozenDiscardCount * 3), TargetRecovery: 0, TargetHpLoss: 0, GivesSelected: false, DiscardsSelected: false))).ThenBy(c => c.Targets.Single()).First();
    SkillProgramStepOutcome IMouXiahouShiHost.Discard(ProgramSkillFrame f) => BeginMouXiahouShiDiscard(f);
    SkillProgramStepOutcome IMouXiahouShiHost.Support(ProgramSkillFrame f) => BeginMouXiahouShiSupport(f);
    SkillProgramStepOutcome IMouXiahouShiHost.Qiaoshi(ProgramSkillFrame f) => BeginMouXiahouShiQiaoshi(f);
}
