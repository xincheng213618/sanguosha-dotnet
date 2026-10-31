#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal const string MouZuifengSkill = "mou-ol:zuifeng";
    internal const string MouZuifengBinding = "unique-suit-view-and-wine";
    internal const string MouZuifengDiscardReason = "skill-program.mou-zhou-yu.zuifeng.discard";
    internal static CardConversionSource MouZuifengSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_yu_ol_mou_MouZhouYuZuifeng.MouZuifengSource(f);
    internal bool HasMouZuifengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouZhouYuZuifeng);

    SkillProgramStepOutcome global::CardGame.Core.IMouZhouYuZuifengHost.Resolve(ProgramSkillFrame f) => BeginMouZuifeng(f);
    internal ActualDiscardRecoveryPhaseKey? MouZuifengPlay(int seat)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == seat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.ActorSeat == seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    }

    internal int MouZuifengUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengCostPaidEvent>().Count(e => e.Source.OwnerSeat == owner && e.Phase == phase);
    internal IReadOnlyList<Card> MouZuifengCosts(int owner)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]);
        var counts = hand.GroupBy(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c)).ToDictionary(g => g.Key, g => g.Count());
        return Array.AsReadOnly(hand.Where(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c)is Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond && counts[_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c)] == 1 && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard)).ToArray());
    }

    internal int[] MouZuifengTargets(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 && !_characterSunHongOlOperations.HasPrintedGeneralSkillTargetBan2(f.OwnerSeat, f.SkillId, p.Seat, f.SkillInstanceId)).Select(p => p.Seat).ToArray();
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 29020, SkillProgramEffectOp.ResolveMouZhouYuZuifeng)]
    internal bool CanActivateMouZuifeng(ProgramActivationLegalityContext gate) => MouZuifengPlay(gate.OwnerSeat)is { } phase && MouZuifengUses(gate.OwnerSeat, phase) < gate.Owner.MaxHp && MouZuifengCosts(gate.OwnerSeat).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 && !_characterSunHongOlOperations.HasPrintedGeneralSkillTargetBan2(gate.OwnerSeat, gate.SkillId, p.Seat));
    internal SkillProgramStepOutcome BeginMouZuifeng(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouZuifeng is not null)
        {
            ContinueMouZuifeng(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouZuifengSkill || f.ActivationId != MouZuifengBinding || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || MouZuifengPlay(f.OwnerSeat)is not { } phase || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || MouZuifengUses(f.OwnerSeat, phase) >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp || MouZuifengCosts(f.OwnerSeat).Count == 0 || MouZuifengTargets(f).Length == 0)
            throw new InvalidOperationException("Zuifeng requires its current true Play, dynamic MaxHP allowance and one legally discardable unique-suit Hand entity.");
        var r = new MouZuifengReceipt(MouZuifengSource(f), f.GameplayHash, phase, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
        {
            Stage = MouZuifengStage.CostChoice,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner));
        PublishMouZuifeng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouZuifengChoices(ProgramSkillFrame f)
    {
        var r = f.MouZuifeng!;
        PromptChoice C(string branch, string label, int? card = null, int? target = null) => new(new($"mou-zuifeng.{(f.Id)}.{(r.Stage)}.{(branch)}.{(card)}.{(target)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-zuifeng", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        var choices = new List<PromptChoice>();
        if (r.Stage is MouZuifengStage.CostChoice or MouZuifengStage.TargetChoice)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || MouZuifengPlay(f.OwnerSeat) != r.Phase || MouZuifengUses(f.OwnerSeat, r.Phase) >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)
                return[];
            if (r.Stage == MouZuifengStage.CostChoice)
                choices.AddRange(MouZuifengCosts(f.OwnerSeat).Select(c => C("cost", $"弃置【{(c.DisplayName)}】", c.Id)));
            else if (MouZuifengCosts(f.OwnerSeat).Any(c => c.Id == r.CostId && c.Kind == r.CostKind && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c) == r.CostSuit))
                choices.AddRange(MouZuifengTargets(f).Select(seat => C("target", $"观看{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}的手牌", target: seat)));
            choices.Add(C("cancel", "取消醉锋"));
        }
        else if (r.Stage == MouZuifengStage.HandViewChoice && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
            choices.AddRange(r.Viewed.Where(v => _cardZonesOperations._cardZones.GetLocation(v.CardId) == CardLocation.Hand(r.TargetSeat)).Select(v => C("show", $"展示【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(v.CardId).DisplayName)}】", v.CardId)));
        else if (r.Stage == MouZuifengStage.AlcoholChoice)
        {
            if (CanUseMouZuifengAlcohol(f))
                choices.Add(C("alcohol", $"将展示牌当【酒】对{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}使用", r.ShownId));
            choices.Add(C("decline", "不使用酒"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal int MouZuifengChooser(ProgramSkillFrame f) => f.MouZuifeng!.Stage == MouZuifengStage.AlcoholChoice ? f.MouZuifeng.TargetSeat : f.OwnerSeat;
    internal void PublishMouZuifeng(ProgramSkillFrame f)
    {
        var r = f.MouZuifeng!;
        var choices = MouZuifengChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[MouZuifengChooser(f)].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishMouZuifeng(f);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, MouZuifengChooser(f), choices, r.Stage == MouZuifengStage.HandViewChoice ? "醉锋：观看其完整手牌并公开其中一张。" : r.Stage == MouZuifengStage.AlcoholChoice ? "醉锋：你可将刚才展示的原牌当酒对其使用。" : "醉锋：弃置一个花色的唯一手牌，选择一名有手牌的角色。", r.TargetSeat < 0 ? f.OwnerSeat : r.TargetSeat);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 90)]
    internal IEnumerable<CardSnapshot> MouZuifengPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewerSeat && f.MouZuifeng is { Stage: MouZuifengStage.HandViewChoice }).SelectMany(f => f.MouZuifeng!.Viewed.Where(v => _cardZonesOperations._cardZones.GetLocation(v.CardId) == CardLocation.Hand(f.MouZuifeng.TargetSeat)).Select(v => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(v.CardId))));
    [ProgramViewContributor(ProgramViewContribution.PublicCardsAfterResolution, 40)]
    internal IEnumerable<CardSnapshot> MouZuifengPublicCards() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MouZuifeng?.ShownId is not null).Select(f => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(f.MouZuifeng!.ShownId!.Value)));
    [ProgramAnswer("mou-zuifeng")]
    internal void AnswerMouZuifeng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZuifeng: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != MouZuifengChooser(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouZuifengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Zuifeng lost its private actual chooser or exact original published command.");
        AssertMouZuifeng(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "cancel")
        {
            FinishMouZuifeng(f);
            return;
        }

        if (branch == "cost")
        {
            var card = MouZuifengCosts(f.OwnerSeat).Single(c => c.Id == choice.Cards.Single());
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r with { Stage = MouZuifengStage.TargetChoice, CostId = card.Id, CostKind = card.Kind, CostSuit = suit } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengCostChosenEvent(f.Id, card.Id, card.Kind, suit));
            PublishMouZuifeng(f);
            return;
        }

        if (branch == "target")
        {
            PayMouZuifengCost(f, choice.Targets.Single());
            return;
        }

        if (branch == "show")
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Single(c => c.Id == choice.Cards.Single());
            if (!_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Select(c => c.Id).SequenceEqual(r.Viewed.Select(v => v.CardId)))
                throw new InvalidOperationException("Zuifeng must publicly show one of the exact complete privately viewed Hand entities.");
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card);
            var same = suit == r.CostSuit;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r with { Stage = MouZuifengStage.DamageChildren, ShownId = card.Id, ShownKind = card.Kind, ShownSuit = suit, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengShownEvent(f.Id, f.OwnerSeat, r.TargetSeat, card.Id, card.Kind, suit, same));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "zuifeng-selected-hand"))
                return;
            ContinueZuifengAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (branch is "alcohol" or "decline")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengAlcoholChosenEvent(f.Id, r.TargetSeat, f.OwnerSeat, r.ShownId!.Value, branch == "alcohol"));
            if (branch == "decline")
            {
                FinishMouZuifeng(f);
                return;
            }

            DispatchMouZuifengAlcohol(f);
            return;
        }

        throw new InvalidOperationException("Zuifeng cannot fabricate a branch outside its original private stage.");
    }

    internal void PayMouZuifengCost(ProgramSkillFrame f, int target)
    {
        var r = f.MouZuifeng!;
        var card = MouZuifengCosts(f.OwnerSeat).Single(c => c.Id == r.CostId && c.Kind == r.CostKind && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c) == r.CostSuit);
        if (r.Stage != MouZuifengStage.TargetChoice || r.Paid || !MouZuifengTargets(f).Contains(target) || MouZuifengPlay(f.OwnerSeat) != r.Phase || MouZuifengUses(f.OwnerSeat, r.Phase) >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)
            throw new InvalidOperationException("Zuifeng cannot repeat a quota or pay a changed unique-suit original Hand card.");
        var max = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
        var ordinal = MouZuifengUses(f.OwnerSeat, r.Phase) + 1;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r with { Stage = MouZuifengStage.CostChildren, TargetSeat = target, Paid = true, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(MouZuifengDiscardReason), (batch, moves) =>
        {
            var move = moves.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var now = current.MouZuifeng!;
            if (move.CardId != card.Id || move.From != CardLocation.Hand(f.OwnerSeat) || move.Sequence <= before || move.Reason.Value != MouZuifengDiscardReason)
                throw new InvalidOperationException("Zuifeng requires one actual logical Hand discard payment.");
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouZuifeng = now with { CostBatchId = batch, CostSequence = move.Sequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengCostPaidEvent(f.Id, r.Source, r.GameplayHash, r.Phase, target, card.Id, card.Kind, r.CostSuit, batch, move.From, move.To, move.Sequence, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, max, ordinal));
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZuifeng = f.MouZuifeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouZuifeng(f.Id);
    }

    internal bool DrainMouZuifeng(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouZuifeng(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZuifeng: { } r } f || f.Id != id || f.AttackAttempt is not null)
            return;
        AssertMouZuifeng(f);
        if (r.Stage is MouZuifengStage.CostChoice or MouZuifengStage.TargetChoice or MouZuifengStage.HandViewChoice or MouZuifengStage.AlcoholChoice)
        {
            PublishMouZuifeng(f);
            return;
        }

        if (r.Stage == MouZuifengStage.NativeUse || r.Stage == MouZuifengStage.NativeIssuing)
            return;
        if (r.Stage == MouZuifengStage.DamageChildren && r.FireIssued && !r.FireReturned)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r = r with { FireReturned = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengFireReturnedEvent(id, r.TargetSeat));
        }

        if (DrainMouZuifeng(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.MouZuifeng!;
        if (r.Stage == MouZuifengStage.CostChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Count == 0)
            {
                FinishMouZuifeng(f);
                return;
            }

            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Select((c, index) => new MouZuifengViewedEntity(index, c.Id, c.Kind)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r with { Stage = MouZuifengStage.HandViewChoice, Viewed = hand } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengHandViewedEvent(id, f.OwnerSeat, r.TargetSeat, hand.Length, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            foreach (var entity in hand)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengViewedEntityEvent(id, entity.Index, entity.CardId, entity.PrintedKind));
            PublishMouZuifeng(f);
            return;
        }

        if (r.Stage == MouZuifengStage.DamageChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || r.ShownId is not { } shown || _cardZonesOperations._cardZones.GetLocation(shown) != CardLocation.Hand(r.TargetSeat))
            {
                FinishMouZuifeng(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = r with { Stage = MouZuifengStage.AlcoholChoice } });
            PublishMouZuifeng(f);
            return;
        }

        FinishMouZuifeng(f);
    }

    internal void FinishMouZuifeng(ProgramSkillFrame f)
    {
        var r = f.MouZuifeng!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengCompletedEvent(f.Id, r.Paid, r.ShownId, r.UseId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29020)]
    internal bool ResumeMouZuifeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZuifeng: not null } f || f.Id != id)
            return false;
        ContinueMouZuifeng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29020)]
    internal bool ReturnMouZuifengMovement(ProgramSkillFrame f)
    {
        if (f.MouZuifeng is null)
            return false;
        ContinueMouZuifeng(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29020)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29020)]
    internal bool ContinuePaidMouZuifeng(ProgramSkillFrame f) => IsPaidMouZuifeng(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouZhouYuZuifeng)]
    internal PromptChoice AiMouZuifeng(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouZuifeng!.Stage == MouZuifengStage.AlcoholChoice)
        {
            var alcohol = p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "alcohol");
            var decline = p.Choices.Single(c => c.Parameters["branch"] == "decline");
            var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
            var beneficiary = snapshot.Players.Single(player => player.Seat == f.OwnerSeat);
            // Use the same viewer-safe support estimate as other native aid
            // choices; raw hidden identities must not decide this private offer.
            // An existing ordinary Alcohol effect does not gain another bonus.
            if (alcohol is null || beneficiary.HasAlcoholEffect || !_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(p.PlayerSeat, out var brain))
                return decline;
            var benefit = brain.ScoreProgramTarget(snapshot, beneficiary.Seat, new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false) { TargetValueAdjustment = 1 });
            return benefit > 0 ? alcohol : decline;
        }

        return p.Choices.First(c => c.Parameters["branch"] != "cancel");
    }

    [PublicHandRevealReturn("zuifeng-selected-hand")]
    internal void ContinueZuifengAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.MouZuifeng ?? throw new InvalidOperationException("Zuifeng lost its paid cost and shown entity.");
        var same = r.ShownSuit == r.CostSuit;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.ShownId!.Value);
        if (same && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZuifeng = f.MouZuifeng!with { FireIssued = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZuifengFireIssuedEvent(f.Id, f.OwnerSeat, r.TargetSeat, card.Id));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1, nature: DamageNature.Fire);
            return;
        }

        ContinueMouZuifeng(f.Id);
        return;
    }
}
