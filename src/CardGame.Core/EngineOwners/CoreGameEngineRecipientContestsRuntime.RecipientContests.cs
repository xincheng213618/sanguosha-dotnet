#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientContestsRuntime
{
    internal static string RecipientContestGiftReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_RecipientContests.RecipientContestGiftReason(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 10, SkillProgramEffectOp.GiveAllHandAndStartRecipientPindian)]
    internal bool CanStartRecipientContest(CharacterState owner) => owner.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0;
    internal SkillProgramStepOutcome BeginRecipientContest(ProgramSkillFrame supplied, int recipient, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.RecipientContest is not null || f.InstructionIndex != 1 || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var selected] || selected != recipient || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            throw new InvalidOperationException("A recipient contest requires its original one-other-target actual Play activation.");
        if (!_coreUniqueHpPeerProgramsOperations.RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !CanStartRecipientContest(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原赠牌来源或受赠者失效，未支付。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var ids = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray();
        var before = _coreUniqueHpPeerProgramsOperations.RecipientContestSequence;
        var r = new RecipientContestReceipt(1, _coreUniqueHpPeerProgramsOperations.RecipientContestSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, recipient, bind, ids, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { RecipientContest = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(RecipientContestGiftReason(f)), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, _coreCardMovementJournalMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = r with
            {
                SequenceAfter = records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { RecipientContest = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientContestGiftPaidEvent(f.Id, paid.Source, paid.GameplayHash, paid.TurnNumber, paid.TurnOwnerSeat, recipient, bind, ids.Length, paid.SequenceBefore, paid.SequenceAfter));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int[] RecipientContestThirds(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Seat != f.RecipientContest!.RecipientSeat && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(f.RecipientContest.RecipientSeat, p.Seat)).Select(p => p.Seat).ToArray();
    internal void PublishRecipientContestThird(ProgramSkillFrame f)
    {
        var r = f.RecipientContest!;
        var targets = RecipientContestThirds(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (targets.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "赠牌保留，已无合法第三位拼点对象。");
            return;
        }

        var choices = Array.AsReadOnly(targets.Select(seat => new PromptChoice(new($"recipient-contest.{(f.Id)}.third-{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].Name)} 与 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 拼点", [], [seat], new Dictionary<string, string> { ["program-action"] = "recipient-contest", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择受赠者的另一位拼点对象。", [], targets, f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("recipient-contest")]
    internal void ResolveRecipientContestChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.RecipientContest!;
        if (r.Stage != RecipientContestStage.ChoosingThird || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || choice.Cards.Count != 0 || choice.Targets is not [var third] || !RecipientContestThirds(f).Contains(third) || choice.Id.Value != $"recipient-contest.{(f.Id)}.third-{(third)}")
            throw new InvalidOperationException("The recipient contest lost its exact published third participant.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreUniqueHpPeerProgramsOperations.RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "保留赠牌，取消尚未发行的拼点。");
            return;
        }

        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { RecipientContest = r with { Stage = RecipientContestStage.Pindian, ThirdSeat = third, PindianFrameId = id } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientContestStartedEvent(f.Id, r.RecipientSeat, third, id));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), r.RecipientSeat, third, programResultBind: r.ResultBind, programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 380)]
    internal bool ResumeRecipientContest(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.RecipientContest is not { } r)
            return false;
        if (r.Stage == RecipientContestStage.GiftChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (!_coreUniqueHpPeerProgramsOperations.RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "已赠实体和孩子保留，未发行拼点取消。");
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientContest = r with { Stage = RecipientContestStage.ChoosingThird } });
            PublishRecipientContestThird(f);
            return true;
        }

        if (r.Stage == RecipientContestStage.ChoosingThird)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishRecipientContestThird(f);
            return true;
        }

        if (r.Stage == RecipientContestStage.Pindian)
        {
            if (!RecipientContestResultMatches(f, out _))
                throw new InvalidOperationException("A real recipient Pindian returned without its original result.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { RecipientContest = r with { Stage = RecipientContestStage.ResultReady } });
            var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
            new SkillProgramExecutor().Run(id, host, host);
            return true;
        }

        if (r.Stage == RecipientContestStage.SlashIssued)
            throw new InvalidOperationException("A contest winner Slash returned without its typed completion.");
        if (r.Stage == RecipientContestStage.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        return false;
    }

    internal bool RecipientContestResultMatches(ProgramSkillFrame f, out PindianResult result)
    {
        result = null !;
        var r = f.RecipientContest!;
        if (r.PindianFrameId is not { } id || r.ThirdSeat is not { } third || f.PindianResultBindings.SingleOrDefault(b => b.Name == r.ResultBind)is not { } binding)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianResultDeterminedEvent>().Where(e => e.FrameId == id && e.SkillId == f.SkillId).ToArray();
        if (facts is not [var fact] || fact.Result.SourceSeat != r.RecipientSeat || fact.Result.OpponentSeat != third || binding.SourceSeat != r.RecipientSeat || binding.OpponentSeat != third || binding.SourceRank != fact.Result.SourceRank || binding.OpponentRank != fact.Result.OpponentRank || binding.SourceWon != fact.Result.SourceWon || binding.Visibility != SkillProgramCardSetVisibility.Public)
            return false;
        result = fact.Result;
        return true;
    }

    internal SkillProgramStepOutcome UseRecipientContestWinner(ProgramSkillFrame supplied, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.RecipientContest;
        if (r is not { Stage: RecipientContestStage.ResultReady } || f.InstructionIndex != 2 || bind != r.ResultBind || !RecipientContestResultMatches(f, out var result))
            throw new InvalidOperationException("Winner use requires the exact preceding revealed recipient contest.");
        if (result.SourceRank == result.OpponentRank)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { RecipientContest = r with { Stage = RecipientContestStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        var actor = result.SourceWon ? result.SourceSeat : result.OpponentSeat;
        var target = result.SourceWon ? result.OpponentSeat : result.SourceSeat;
        if (!_coreUniqueHpPeerProgramsOperations.RecipientContestUnissuedSource(f) || !_coreForeignTurnContestsOperations.CanUseForeignTurnContestSlash(actor, target))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "真实拼点结果保留，尚未发行的赢家杀取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A recipient winner Slash cannot overwrite another attack.");
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var winner = _coreMatchParticipantsPlayersAccess._players[actor];
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor, actor, null, null, null, CardKind.Slash, [target], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var returned = new PindianWinnerSlashReturn(f.Id, 2, r.Source, f.GameplayHash, bind, r.PindianFrameId!.Value, r.RecipientSeat, r.ThirdSeat!.Value, result.SourceRank, result.OpponentRank, actor, target, useId, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { RecipientContest = r with { Stage = RecipientContestStage.SlashIssued, SlashReturn = returned } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor, 0, CardKind.Slash, [target], PhysicalCardIds: []) { Action = action, PindianWinnerSlashReturn = returned });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianWinnerSlashIssuedEvent(returned));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, [target]));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor, target, card: null, damageAmount: 1 + winner.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(winner, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, winner);
        winner.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor, target));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, [target], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 70)]
    internal bool ReturnRegisteredCompletePindianWinnerSlash(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.PindianWinnerSlashReturn is null)
            return false;
        CompletePindianWinnerSlash(completion);
        return true;
    }

    internal void CompletePindianWinnerSlash(GameEngine.AttackCompletionReceipt completion)
    {
        var returned = completion.PindianWinnerSlashReturn ?? throw new InvalidOperationException("Missing recipient winner typed return.");
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(returned.ProgramFrameId);
        var r = f.RecipientContest;
        if (completion.ProgramFrameId != f.Id || completion.ResolutionId != returned.CardUseFrameId || r is not { Stage: RecipientContestStage.SlashIssued } || r.SlashReturn != returned || returned.Source != _coreUniqueHpPeerProgramsOperations.RecipientContestSource(f) || returned.GameplayHash != f.GameplayHash || f.InstructionIndex != 2 || !RecipientContestResultMatches(f, out _) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Id == returned.CardUseFrameId) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == returned.CardUseFrameId) != 1)
            throw new InvalidOperationException("The winner Slash returned outside its original paid gift/result and owning use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientContest = r with { Stage = RecipientContestStage.Complete } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianWinnerSlashReturnedEvent(returned));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome IRecipientContestProgramHost.DiscardDrawPeer(ProgramSkillFrame f) => _coreUniqueHpPeerProgramsOperations.BeginUniqueHpPeer(f);
    SkillProgramStepOutcome IRecipientContestProgramHost.GiveAllHandContest(ProgramSkillFrame f, int seat, string bind) => BeginRecipientContest(f, seat, bind);
    SkillProgramStepOutcome IRecipientContestProgramHost.UseContestWinnerSlash(ProgramSkillFrame f, string bind) => UseRecipientContestWinner(f, bind);
}
