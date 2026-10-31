#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoMouMobileRuntime
{
    internal bool HasMouDaQiao => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouDaQiaoGuose);
    internal int MouDaQiaoGuoseLimit => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Identity ? 4 : 2;

    internal static CardConversionSource MouDaQiaoSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_da_qiao_mou_mobile_MouDaQiaoGuose.MouDaQiaoSource(f);
    internal int MouDaQiaoGuoseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouDaQiaoGuoseReservedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Phase == phase);
    [ProgramPhaseUsedProjection(80)]
    internal int MouDaQiaoProjectedPhaseUses(int owner, string skill, int fallback) => skill == MouDaQiaoComposition.Guose && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p ? MouDaQiaoGuoseUses(owner, p) : fallback;
    internal IEnumerable<Card> MouDaQiaoRemovable(int target) => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => !_characterDaQiaoLeOperations.IsZixiQinJudgmentCard(c.Id) && _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(c) == CardKind.Indulgence);
    internal bool MouDaQiaoGuoseTarget(int owner, int? card, int target, string branch) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && (branch == "remove-delayed" ? MouDaQiaoRemovable(target).Any() : card is { } id && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(_coreMatchParticipantsPlayersAccess._players[owner], id)is { } physical && _coreDiamondDelayedProgramsOperations.DiamondUseTargetLegal(owner, physical, target, MouDaQiaoComposition.Guose));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 31300, SkillProgramEffectOp.ResolveMouDaQiaoGuose, SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove)]
    internal bool CanMouDaQiaoGuose(ProgramActivationLegalityContext c) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && MouDaQiaoGuoseUses(c.OwnerSeat, p) < MouDaQiaoGuoseLimit;
    internal SkillProgramStepOutcome BeginMouDaQiaoGuose(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDaQiaoGuose is not null)
        {
            ContinueMouDaQiaoGuose(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouDaQiaoComposition.Guose || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedTargetSeats is not [var target] || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } p || p.ActorSeat != f.OwnerSeat || MouDaQiaoGuoseUses(f.OwnerSeat, p) >= MouDaQiaoGuoseLimit || !MouDaQiaoGuoseTarget(f.OwnerSeat, f.SelectedCardIds.SingleOrDefault(), target, f.ActivationId))
            throw new InvalidOperationException("Guose requires one current legal native branch within its shared actual-Play budget.");
        var source = MouDaQiaoSource(f);
        var r = new MouDaQiaoGuoseReceipt(source, f.GameplayHash, p, f.ActivationId, target, MouDaQiaoGuoseStage.Selecting);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDaQiaoGuoseReservedEvent(f.Id, source, f.GameplayHash, p, r.Branch, target, MouDaQiaoGuoseLimit));
        if (r.Branch == "remove-delayed")
        {
            _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, MouDaQiaoRemovalChoices(f), "选择要弃置的场上【乐不思蜀】。", target);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(f.SelectedCardIds.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var expected = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r with { Stage = MouDaQiaoGuoseStage.Using, CardId = card.Id, From = from, UseId = expected, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [target], CardKind.Indulgence, conversionSource: source);
        if (id != expected)
            throw new InvalidOperationException("Guose lost its exact newly issued native delayed Use.");
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(parent => parent.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouDaQiaoGuose = f.MouDaQiaoGuose!with { Paid = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDaQiaoGuosePaidEvent(f.Id, r.Branch, card.Id, from, CardLocation.Processing, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, [target], LegalActionKind.Indulgence, playedCardKind: CardKind.Indulgence);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> MouDaQiaoRemovalChoices(ProgramSkillFrame f) => Array.AsReadOnly(MouDaQiaoRemovable(f.MouDaQiaoGuose!.TargetSeat).Select(card => new PromptChoice(new($"mou-da-qiao.guose.{(f.Id)}.{(card.Id)}"), "弃置【乐不思蜀】", [card.Id], [f.MouDaQiaoGuose.TargetSeat], new Dictionary<string, string> { ["program-action"] = "mou-da-qiao-guose", ["frame-id"] = f.Id.ToString() })).ToArray());
    [ProgramAnswer("mou-da-qiao-guose")]
    internal void AnswerMouDaQiaoGuose(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDaQiaoGuose: { Stage: MouDaQiaoGuoseStage.Selecting } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouDaQiaoRemovalChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Guose accepts only its current visible native judgment removal.");
        var id = choice.Cards.Single();
        var from = CardLocation.Judgment(r.TargetSeat);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r with { Stage = MouDaQiaoGuoseStage.MovementChildren, CardId = id, From = from, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new("skill-program.mou-mobile:guose.remove"));
        var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == id && m.Sequence > before && m.From == from && m.To.Zone is CardZoneKind.DiscardPile or CardZoneKind.OutsideGame);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = f.MouDaQiaoGuose!with { Paid = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDaQiaoGuosePaidEvent(f.Id, r.Branch, id, from, moved.To, moved.Sequence));
        ContinueMouDaQiaoGuose(f.Id);
    }

    [NativeUseReturn(31300)]
    internal void ReturnMouDaQiaoGuoseUse(long useId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDaQiaoGuose: { Stage: MouDaQiaoGuoseStage.Using } r } f || r.UseId != useId)
            return;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Any(e => e.ResolutionId == useId && e.CardId == r.CardId && e.CardKind == CardKind.Indulgence))
            throw new InvalidOperationException("Guose returned from a different native Use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r with { Stage = MouDaQiaoGuoseStage.MovementChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueMouDaQiaoGuose(f.Id);
    }

    internal bool DrainMouDaQiao(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouDaQiaoGuose(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDaQiaoGuose: { } r } f || f.Id != id)
            return;
        if (r.Stage is MouDaQiaoGuoseStage.Using or MouDaQiaoGuoseStage.Drawing or MouDaQiaoGuoseStage.Selecting)
            return;
        if (DrainMouDaQiao(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouDaQiaoGuose!;
        if (r.Stage == MouDaQiaoGuoseStage.MovementChildren && r.Paid && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r with { Stage = MouDaQiaoGuoseStage.Drawing }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.mou-mobile:guose.draw"));
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDaQiaoGuoseCompletedEvent(f.Id, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouDaQiaoGuose, SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove)]
    internal void ReturnMouDaQiaoGuoseDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouDaQiaoGuose is not { Stage: MouDaQiaoGuoseStage.Drawing } r || o.RequestedCount != 1 || o.RecipientSeat != f.OwnerSeat)
            throw new InvalidOperationException("Guose lost its exact native Draw1 return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDaQiaoGuose = r with { Stage = MouDaQiaoGuoseStage.DrawChildren, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDaQiaoGuoseDrawReturnedEvent(f.Id, o.AttemptFrameId, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count));
        ContinueMouDaQiaoGuose(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouDaQiaoGuose, SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove)]
    internal PromptChoice AiMouDaQiaoGuose(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    SkillProgramStepOutcome global::CardGame.Core.IMouDaQiaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouDaQiaoGuose or SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove => BeginMouDaQiaoGuose(f),
        SkillProgramEffectOp.ResolveMouDaQiaoLiuliHeart => ResolveMouDaQiaoHeart(f),
        _ => BeginMouDaQiaoLiuliPhase(f)};
    int global::CardGame.Core.IMouDaQiaoQuery.Limit() => MouDaQiaoGuoseLimit;
    bool global::CardGame.Core.IMouDaQiaoQuery.Allows(int owner, int? card, int target, string branch) => MouDaQiaoGuoseTarget(owner, card, target, branch);
}
