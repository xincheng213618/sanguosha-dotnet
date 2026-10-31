#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBlackGiftContestsRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 20, SkillProgramEffectOp.GiveBlackHandAndResolveRecipientContest)]
    internal bool AllowsGiveBlackHandAndResolveRecipientContestActivation(ProgramActivationLegalityContext gate)
    {
        var owner = gate.Owner;
        var program = gate.Program;
        var features = gate.Features;
        return !(features.HasOperation(SkillProgramEffectOp.GiveBlackHandAndResolveRecipientContest) && _coreRecipientConsequencesReceiptsOperations.BlackGiftGameIssued(owner.Seat, program.Id));
    }

    internal static CardConversionSource RecipientConsequencesSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.RecipientConsequencesSource(f);
    internal static string BlackGiftReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftReason(f);
    internal static string BlackGiftDiscardReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftDiscardReason(f);
    internal bool BlackGiftMaterialLegal(int owner, Card card) => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner) && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card)is Suit.Spade or Suit.Club;
    internal SkillProgramStepOutcome BeginBlackGiftContest(ProgramSkillFrame supplied, int recipient)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.BlackGiftContest is not null || f.InstructionIndex != 1 || f.TriggerId is not null || f.WindowContext is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedTargetSeats is not [var selected] || selected != recipient || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || f.SelectedCardIds is not [var id])
            throw new InvalidOperationException("A black gift contest lost its exact first other recipient/material.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !BlackGiftMaterialLegal(f.OwnerSeat, card))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "赠牌前来源、黑色手牌或接收者失效。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        var receipt = new BlackGiftContestReceipt(1, RecipientConsequencesSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, recipient, id, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BlackGiftContest = receipt, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreHappyWeiYanGainDestinationsOperations.MoveNativeGivenCards(f.Id, f.OwnerSeat, recipient, [id], new(BlackGiftReason(f)), settlement =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = receipt with
            {
                GiftAfter = settlement.Movements.Select(m => (long)m.Sequence).DefaultIfEmpty(before).Max()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BlackGiftContest = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BlackGiftContestGiftPaidEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, paid.TurnOwnerSeat, recipient, before, paid.GiftAfter));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int[] BlackGiftSecondSeats(ProgramSkillFrame f)
    {
        var first = f.BlackGiftContest!.RecipientSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[first].IsAlive || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[first]))
            return[];
        // “另一名角色” may be the original owner; its hand is checked after the gift.
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != first && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(first, p.Seat)).Select(p => p.Seat).ToArray();
    }

    internal IReadOnlyList<PromptChoice> BlackGiftDiscardOptions(ProgramSkillFrame f)
    {
        var winner = BlackGiftWinner(f.BlackGiftContest!.Result!)!.Value;
        return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, winner, winner, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !f.BlackGiftContest.SelectedDiscardIds.Contains(c.Id));
    }

    internal static int? BlackGiftWinner(PindianResult p) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftWinner(p);
    internal static int[] BlackGiftLosers(PindianResult p) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftLosers(p);
    internal IReadOnlyList<PromptChoice> BlackGiftChoices(ProgramSkillFrame f)
    {
        var r = f.BlackGiftContest!;
        return Array.AsReadOnly((r.Stage == BlackGiftContestStage.ChoosingSecond ? BlackGiftSecondSeats(f).Select(seat => new PromptChoice(new($"black-gift.{(f.Id)}.second-{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].Name)} 与 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 拼点", [], [seat], BlackGiftParameters(f, "second"))) : BlackGiftDiscardOptions(f).Select(c => new PromptChoice(new($"black-gift.{(f.Id)}.discard-{(c.Cards.Single())}"), c.Label, c.Cards, [], BlackGiftParameters(f, "discard")))).ToArray());
    }

    internal static Dictionary<string, string> BlackGiftParameters(ProgramSkillFrame f, string stage) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftParameters(f, stage);
    internal void PublishBlackGiftChoice(ProgramSkillFrame f)
    {
        if (_coreRecipientConsequencesReceiptsOperations.RecipientConsequencesGameEnded())
            return;
        var r = f.BlackGiftContest!;
        var choices = BlackGiftChoices(f);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var seat = r.Stage == BlackGiftContestStage.ChoosingSecond ? f.OwnerSeat : BlackGiftWinner(r.Result!)!.Value;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, r.Stage == BlackGiftContestStage.ChoosingSecond ? "选择受赠者的另一名拼点角色。" : "请选择赢家必须实际弃置的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("black-gift-contest")]
    internal void ResolveBlackGiftChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.BlackGiftContest!;
        var seat = r.Stage == BlackGiftContestStage.ChoosingSecond ? f.OwnerSeat : BlackGiftWinner(r.Result!)!.Value;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != seat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [BlackGiftChoices(f).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("A black gift answer lost its exact private owning choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == BlackGiftContestStage.ChoosingSecond)
        {
            var second = choice.Targets.Single();
            if (!BlackGiftSecondSeats(f).Contains(second))
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            var pindian = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.Pindian, SecondSeat = second, PindianFrameId = pindian }, PendingMovementContinuation = new(r.RecipientSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BlackGiftContestStartedEvent(f.Id, r.RecipientSeat, second, pindian));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), r.RecipientSeat, second, programResultBind: BlackGiftResultBind(f), programResultVisibility: SkillProgramCardSetVisibility.Public);
            return;
        }

        if (r.Stage != BlackGiftContestStage.ChoosingDiscard)
            throw new InvalidOperationException("A completed cost cannot be selected twice.");
        var selected = r.SelectedDiscardIds.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r = r with { SelectedDiscardIds = selected } });
        if (selected.Length < r.RequiredDiscards)
        {
            PublishBlackGiftChoice(f);
            return;
        }

        var winner = BlackGiftWinner(r.Result!)!.Value;
        var locations = selected.Select(_cardZonesOperations._cardZones.GetLocation).ToArray();
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        var invoice = new BlackGiftContestDiscard(winner, selected, locations, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.DiscardPaid, SelectedDiscardIds = [], Discard = invoice }, PendingMovementContinuation = new(winner, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, winner, selected, new(BlackGiftDiscardReason(f)), (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = invoice with
            {
                After = records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BlackGiftContest = active.BlackGiftContest!with { Discard = paid } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BlackGiftContestDiscardPaidEvent(f.Id, winner, selected.Length, before, paid.After));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal static string BlackGiftResultBind(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_BlackGiftContests.BlackGiftResultBind(f);
    internal bool BlackGiftResultMatches(ProgramSkillFrame f, out PindianResult result)
    {
        result = null !;
        var r = f.BlackGiftContest!;
        if (r.PindianFrameId is not { } id || r.SecondSeat is not { } second || f.PindianResultBindings.SingleOrDefault(b => b.Name == BlackGiftResultBind(f))is not { } b)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianResultDeterminedEvent>().Where(e => e.FrameId == id && e.SkillId == f.SkillId).ToArray();
        if (facts is not [var fact] || fact.Result.SourceSeat != r.RecipientSeat || fact.Result.OpponentSeat != second || b.SourceSeat != r.RecipientSeat || b.OpponentSeat != second || b.SourceRank != fact.Result.SourceRank || b.OpponentRank != fact.Result.OpponentRank || b.SourceWon != fact.Result.SourceWon || b.Visibility != SkillProgramCardSetVisibility.Public)
            return false;
        result = fact.Result;
        return true;
    }

    internal bool DrainRecipientConsequences(ProgramSkillFrame f, bool movement) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 390)]
    internal bool ResumeBlackGiftContest(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.BlackGiftContest is not { } r)
            return false;
        if (_coreRecipientConsequencesReceiptsOperations.RecipientConsequencesGameEnded())
            return true;
        if (r.Stage is BlackGiftContestStage.ChoosingSecond or BlackGiftContestStage.ChoosingDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBlackGiftChoice(f);
            return true;
        }

        if (DrainRecipientConsequences(f, f.PendingMovementContinuation is not null))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.BlackGiftContest!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == BlackGiftContestStage.GiftPaid)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.ChoosingSecond } });
            PublishBlackGiftChoice(f);
            return true;
        }

        if (r.Stage == BlackGiftContestStage.Pindian)
        {
            if (!BlackGiftResultMatches(f, out var result))
                throw new InvalidOperationException("Black gift requires its completed real Pindian result.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BlackGiftContestResultCapturedEvent(id, r.PindianFrameId!.Value, result));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r = r with { Result = result, Stage = BlackGiftContestStage.LossReady } });
            if (BlackGiftWinner(result)is { } winner && _coreMatchParticipantsPlayersAccess._players[winner].IsAlive)
            {
                var count = Math.Min(2, BlackGiftDiscardOptions(f).Count);
                if (count > 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.ChoosingDiscard, RequiredDiscards = count } });
                    PublishBlackGiftChoice(f);
                    return true;
                }
            }
        }
        else if (r.Stage == BlackGiftContestStage.DiscardPaid)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r = r with { Stage = BlackGiftContestStage.LossReady } });
        else if (r.Stage == BlackGiftContestStage.LossPaid)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r = r with { Stage = BlackGiftContestStage.LossReady, LossIndex = r.LossIndex + 1 } });
        if (r.Stage == BlackGiftContestStage.LossReady)
        {
            var losers = BlackGiftLosers(r.Result!);
            while (r.LossIndex < losers.Length && !_coreMatchParticipantsPlayersAccess._players[losers[r.LossIndex]].IsAlive)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r = r with { LossIndex = r.LossIndex + 1 } });
            if (r.LossIndex < losers.Length)
            {
                var seat = losers[r.LossIndex];
                var before = _coreMatchParticipantsPlayersAccess._players[seat].Hp;
                var loss = new BlackGiftContestLoss(seat, before, Math.Max(0, before - 1));
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.LossPaid, Losses = r.Losses.Append(loss).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BlackGiftContestLossPaidEvent(id, r.LossIndex, loss));
                var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(id, f.SkillId, seat, 1);
                if (outcome == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BlackGiftContest = r with { Stage = BlackGiftContestStage.Complete } });
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    SkillProgramStepOutcome IRecipientContestConsequencesHost.BlackGiftContest(ProgramSkillFrame f, int recipient) => BeginBlackGiftContest(f, recipient);
    SkillProgramStepOutcome IRecipientContestConsequencesHost.PrintedLordBenefit(ProgramSkillFrame f) => _corePrintedLordBenefitsOperations.Value.BeginPrintedLordBenefit(f);
}
