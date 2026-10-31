#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuRongOlRuntime
{
    internal const string FuRongCostReason = "skill-program.fu-rong.xiaosi.discard";
    internal CardConversionSource FuRongSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool FuRongSourceCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal bool FuRongLegalCost(int seat, Card c, CardLocation from) => !c.IsGeneralWeapon && from.OwnerSeat == seat && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], c), SkillProgramCardCategory.Basic) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, from, OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, from, OwnedCardMoveIntent.Discard);
    internal bool FuRongHasCost(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat)).Any(c => FuRongLegalCost(seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id)));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 26310, SkillProgramEffectOp.DiscardBasicPairAndReuse)]
    internal bool AllowsFuRongActivation(CharacterState actor) => FuRongHasCost(actor.Seat);
    internal SkillProgramStepOutcome BeginFuRongPair(ProgramSkillFrame supplied, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var p = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!;
        FuRongComposition.ValidateProgram(f.SkillId, p);
        var a = ProgramInstructionResolver.Default.Resolve(f, p).Activation;
        if (a is null || f.FuRongPair is not null || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var selected] || selected != target || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 || !FuRongSourceCurrent(f) || !FuRongHasCost(f.OwnerSeat) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, a.UsageGroup)) != 1)
            throw new InvalidOperationException("Xiaosi requires its exact unpaid phase activation, source and living nonempty-hand target.");
        var r = new ProgramFuRongPairReceipt
        {
            Source = FuRongSource(f),
            GameplayHash = f.GameplayHash,
            UsageGroup = a.UsageGroup,
            InstructionIndex = f.InstructionIndex,
            TargetSeat = target,
            TurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            PhaseInstanceId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            Stage = FuRongPairStage.OwnerChoice
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FuRongPair = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuRongPairStartedEvent(f.Id, r.Source, r.GameplayHash, r.UsageGroup, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.PhaseInstanceId));
        PublishFuRongPayment(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Dictionary<string, string> FuRongArgs(ProgramSkillFrame f, string branch) => new()
    {
        ["program-action"] = "fu-rong-pair",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["branch"] = branch
    };
    internal IReadOnlyList<PromptChoice> FuRongPaymentChoices(ProgramSkillFrame f)
    {
        var r = f.FuRongPair!;
        var payer = r.Stage == FuRongPairStage.OwnerChoice ? f.OwnerSeat : r.TargetSeat;
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, payer, payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => FuRongLegalCost(payer, card, new(zone, payer))).Select(c => new PromptChoice(new($"fu-rong-cost.{(f.Id)}.{(payer)}.{(c.Cards.Single())}"), c.Description, c.Cards, [], FuRongArgs(f, "cost"))).ToArray());
    }

    internal void PublishFuRongPayment(ProgramSkillFrame f)
    {
        var r = f.FuRongPair!;
        var payer = r.Stage == FuRongPairStage.OwnerChoice ? f.OwnerSeat : r.TargetSeat;
        var choices = FuRongPaymentChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Xiaosi cannot offer an empty mandatory basic payment.");
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, payer, choices, "选择一张可弃置的基本牌。", payer);
    }

    [ProgramAnswer("fu-rong-pair")]
    internal void AnswerFuRongPair(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Xiaosi lost its choice parent.");
        AssertFuRongPair(f);
        var r = f.FuRongPair!;
        var expected = r.Stage is FuRongPairStage.OwnerChoice or FuRongPairStage.TargetChoice ? FuRongPaymentChoices(f) : r.Stage == FuRongPairStage.UseChoice ? FuRongUseChoices(f) : throw new InvalidOperationException("Xiaosi is not awaiting a choice.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != (r.Stage == FuRongPairStage.TargetChoice ? r.TargetSeat : f.OwnerSeat) || !expected.Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Xiaosi requires the exact published chooser, cards, targets and frame parameters.");
        if (r.Stage == FuRongPairStage.UseChoice)
        {
            AnswerFuRongUse(f, selected);
            return;
        }

        if (r.Stage == FuRongPairStage.OwnerChoice && (!FuRongSourceCurrent(f) || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishFuRongPair(f);
            return;
        }

        var payer = prompt.PlayerSeat;
        var from = selected.Cards is [var selectedId] ? _cardZonesOperations._cardZones.GetLocation(selectedId) : default;
        if (selected.Cards is not [var cardId] || !_coreMatchParticipantsPlayersAccess._players[payer].IsAlive || !FuRongLegalCost(payer, _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), from))
            throw new InvalidOperationException("Xiaosi cannot discard a forbidden or substituted physical card.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var owner = r.Stage == FuRongPairStage.OwnerChoice;
        var paid = new FuRongPayment(payer, cardId, card.Kind, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[payer], card), from, 0);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FuRongPair = r with { Stage = owner ? FuRongPairStage.OwnerChildren : FuRongPairStage.TargetChildren, OwnerPayment = owner ? paid : r.OwnerPayment, TargetPayment = owner ? r.TargetPayment : paid }, PendingMovementContinuation = new(payer, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, payer, card, from, new(FuRongCostReason), beforeFact: move =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = active.FuRongPair!;
            var actual = paid with
            {
                Sequence = move.Sequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { FuRongPair = receipt with { OwnerPayment = owner ? actual : receipt.OwnerPayment, TargetPayment = owner ? receipt.TargetPayment : actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuRongPairPaidEvent(f.Id, owner, actual));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainFuRongChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26310)]
    internal bool ResumeFuRongPair(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.FuRongPair is not { } r)
            return false;
        AssertFuRongPair(f);
        if (r.Stage is FuRongPairStage.OwnerChoice or FuRongPairStage.TargetChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishFuRongPayment(f);
            return true;
        }

        if (r.Stage == FuRongPairStage.NativeUse)
            throw new InvalidOperationException("Xiaosi's issued use must return through its exact native receipt.");
        if (r.Stage is FuRongPairStage.OwnerChildren or FuRongPairStage.TargetChildren or FuRongPairStage.DrawChildren or FuRongPairStage.NativeChildren)
        {
            if (DrainFuRongChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.FuRongPair!;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishFuRongPair(f);
            return true;
        }

        if (r.Stage == FuRongPairStage.OwnerChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && FuRongHasCost(r.TargetSeat))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FuRongPair = r with { Stage = FuRongPairStage.TargetChoice } });
                PublishFuRongPayment(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FuRongPair = r with { TargetUnable = true, DrawIssued = true, Stage = FuRongPairStage.DrawChildren, DrawBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuRongTargetUnableEvent(f.Id, r.TargetSeat));
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new("skill-program.fu-rong.xiaosi.compensation")).Count;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { FuRongPair = active.FuRongPair!with { ActualDraw = count, DrawAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuRongCompensationDrawEvent(f.Id, f.OwnerSeat, count));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (r.Stage is FuRongPairStage.TargetChildren or FuRongPairStage.DrawChildren or FuRongPairStage.NativeChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FuRongPair = r with { Stage = FuRongPairStage.UseChoice, SelectedUseCardId = null, ReservedUseId = null, NativeReturn = null, NativeCompleted = false } });
        }

        if (f.FuRongPair!.Stage == FuRongPairStage.UseChoice)
        {
            if (FuRongUseChoices(f).Count == 1)
            {
                FinishFuRongPair(f);
                return true;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishFuRongUse(f);
            return true;
        }

        throw new InvalidOperationException("Xiaosi reached an unknown parent continuation.");
    }

    internal void FinishFuRongPair(ProgramSkillFrame f)
    {
        var r = f.FuRongPair!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FuRongPair = r with { Stage = FuRongPairStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuRongPairCompletedEvent(f.Id, r.OwnerPayment is not null, r.TargetPayment is not null, r.UsedCardIds.Count));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26310)]
    internal bool ReturnFuRongMovement(ProgramSkillFrame f)
    {
        if (f.FuRongPair is not { Stage: FuRongPairStage.OwnerChildren or FuRongPairStage.TargetChildren or FuRongPairStage.DrawChildren or FuRongPairStage.NativeChildren })
            return false;
        AssertFuRongPair(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26310)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26310)]
    internal bool ContinuePaidFuRongPair(ProgramSkillFrame f) => f.FuRongPair?.OwnerPayment is not null;
    SkillProgramStepOutcome global::CardGame.Core.IFuRongProgramHost.DiscardBasicPairAndReuse(ProgramSkillFrame f, int target) => BeginFuRongPair(f, target);
    [ProgramAiChoice(SkillProgramEffectOp.DiscardBasicPairAndReuse)]
    internal PromptChoice SelectAiFuRongPair(PendingDecision prompt, ProgramSkillFrame f) => f.FuRongPair!.Stage == FuRongPairStage.UseChoice ? prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "use") ?? prompt.Choices.Last() : prompt.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat])).ThenBy(c => c.Cards.Single()).First();
}
