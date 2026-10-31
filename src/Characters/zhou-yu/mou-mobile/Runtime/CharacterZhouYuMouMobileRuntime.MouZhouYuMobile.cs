#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouMobileRuntime
{
    internal bool HasMouZhouYuMobileCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouZhouYuFanjian) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouZhouYuYingzi);

    internal static CardConversionSource MouZyMobileSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhou_yu_mou_mobile_MouZhouYuMobile.MouZyMobileSource(f);
    internal MouZhouYuMobileTurn? MouZyMobileTurn()
    {
        var h = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return h.LastOrDefault(e => e is TurnStartedEvent or ActualTurnCursorReturnedEvent)is TurnStartedEvent t && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && t.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat ? new(t.TurnNumber, t.ActorSeat, h.OfType<TurnStartedEvent>().Count()) : null;
    }

    internal bool MouZyMobileDisabled(int owner, MouZhouYuMobileTurn turn) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileDisabledEvent>().Any(e => e.OwnerSeat == owner && e.Turn == turn);
    internal bool MouZyMobileSuitSpent(int owner, MouZhouYuMobileTurn turn, Suit suit) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileCoveredEvent>().Any(e => e.ActualSuit == suit && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == owner && s.Turn == turn));
    internal Card[] MouZyMobileCards(int owner) => MouZyMobileTurn()is { } turn ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => !MouZyMobileSuitSpent(owner, turn, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c))).ToArray() : [];
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 31100, SkillProgramEffectOp.ResolveMouZhouYuFanjian)]
    internal bool CanStartMouZyMobile(CharacterState owner) => MouZyMobileTurn()is { } turn && !MouZyMobileDisabled(owner.Seat, turn) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner.Seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && MouZyMobileCards(owner.Seat).Length > 0;
    SkillProgramStepOutcome IMouZhouYuMobileHost.Resolve(ProgramSkillFrame f, bool yingzi) => BeginMouZyMobile(f, yingzi);
    internal SkillProgramStepOutcome BeginMouZyMobile(ProgramSkillFrame supplied, bool yingzi)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.MouZhouYuMobile is not null || MouZyMobileTurn()is not { } turn)
            throw new InvalidOperationException("Mobile Zhou Yu requires one exact actual-turn instruction.");
        if (yingzi)
        {
            if (f.SkillId != MouZhouYuMobileComposition.Yingzi || f.TriggerId != "three-condition-draw" || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting, SourceSeat: { } source } || source != f.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileYingziIssuedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Yingzi requires its native normal-draw window.");
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count >= 2;
            var hp = owner.Hp >= 2;
            var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count > 0;
            var count = (hand ? 1 : 0) + (hp ? 1 : 0) + (equipment ? 1 : 0);
            if (count > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().AdjustNormalDraw(f, count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileYingziIssuedEvent(f.Id, MouZyMobileSource(f), turn, hand, hp, equipment, count));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.SkillId != MouZhouYuMobileComposition.Fanjian || f.ActivationId != "cover-and-declare" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !CanStartMouZyMobile(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase)
            throw new InvalidOperationException("Fanjian requires its owner's actual Play and one real other target.");
        var r = new MouZhouYuMobileReceipt(MouZyMobileSource(f), f.GameplayHash, turn, phase, target)
        {
            Stage = MouZhouYuMobileStage.OwnerChoice,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileStartedEvent(f.Id, r.Source, r.GameplayHash, turn, phase, target));
        PublishMouZyMobile(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static string MouZyMobileSuitName(Suit s) => RuntimeSkills.RuntimeRules_zhou_yu_mou_mobile_MouZhouYuMobile.MouZyMobileSuitName(s);
    internal PromptChoice MouZyMobileChoice(ProgramSkillFrame f, string branch, string text, int? card = null, Suit? declared = null) => new(new($"mou-zhou-yu-mobile.{(f.Id)}.{(branch)}.{(card)}.{(declared)}"), text, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "mou-zhou-yu-fanjian", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["declared-suit"] = declared?.ToString() ?? "" });
    internal IReadOnlyList<PromptChoice> MouZyMobileChoices(ProgramSkillFrame f)
    {
        var r = f.MouZhouYuMobile!;
        var choices = new List<PromptChoice>();
        if (r.Stage == MouZhouYuMobileStage.OwnerChoice)
        {
            foreach (var card in MouZyMobileCards(f.OwnerSeat))
                foreach (var suit in new[]
                {
                    Suit.Spade,
                    Suit.Heart,
                    Suit.Club,
                    Suit.Diamond
                }

                )
                    choices.Add(MouZyMobileChoice(f, "cover", $"扣置【{(CardCatalog.Get(card.Kind).DisplayName)}】，声明{(MouZyMobileSuitName(suit))}", card.Id, suit));
            choices.Add(MouZyMobileChoice(f, "cancel", "取消反间"));
        }
        else if (r.Stage == MouZhouYuMobileStage.TargetChoice)
        {
            choices.Add(MouZyMobileChoice(f, "same", $"猜测：扣置牌是{(MouZyMobileSuitName(r.DeclaredSuit))}"));
            choices.Add(MouZyMobileChoice(f, "different", $"猜测：扣置牌不是{(MouZyMobileSuitName(r.DeclaredSuit))}"));
            choices.Add(MouZyMobileChoice(f, "turn-over", "翻面，使反间本回合失效"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouZyMobile(ProgramSkillFrame f)
    {
        var r = f.MouZhouYuMobile!;
        var ownerChoice = r.Stage == MouZhouYuMobileStage.OwnerChoice;
        var chooser = ownerChoice ? f.OwnerSeat : r.TargetSeat;
        var choices = MouZyMobileChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, ownerChoice ? "反间：选择实际手牌并声明花色。" : $"反间：对方声明{(MouZyMobileSuitName(r.DeclaredSuit))}，请选择猜测或翻面。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = chooser,
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-zhou-yu-fanjian")]
    internal void AnswerMouZyMobile(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouZhouYuMobile;
        if (r is null || !ValidMouZyMobile(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouZhouYuMobileStage.OwnerChoice ? f.OwnerSeat : r.TargetSeat) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouZyMobileChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Fanjian accepts only the exact actual private chooser's published option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        if (r.Stage == MouZhouYuMobileStage.OwnerChoice)
        {
            if (branch == "cancel" || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                FinishMouZyMobile(f);
                return;
            }

            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
            var actual = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
            var declared = Enum.Parse<Suit>(selected.Parameters["declared-suit"]);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { CardId = card.Id, PrintedKind = card.Kind, ActualSuit = actual, DeclaredSuit = declared, Stage = MouZhouYuMobileStage.CoverChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, new("skill-program.mou-mobile:fanjian.cover"), disclosure: CardMovementDisclosure.FaceDown);
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card.Id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Processing);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = f.MouZhouYuMobile!with { PaymentSequence = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileCoveredEvent(f.Id, card.Id, card.Kind, actual, declared, m.From, m.To, m.Sequence));
            SealMouZyMobile(f.Id, before);
            ContinueMouZyMobile(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { Answer = branch, Stage = MouZhouYuMobileStage.ChoiceChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileAnsweredEvent(f.Id, r.TargetSeat, branch));
        if (branch == "turn-over")
        {
            DisableMouZyMobile(f, "turn-over");
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TurnOver(f.Id, f.OwnerSeat, r.TargetSeat);
        }

        ContinueMouZyMobile(f.Id);
    }

    internal void DisableMouZyMobile(ProgramSkillFrame f, string reason)
    {
        var r = f.MouZhouYuMobile!;
        if (!MouZyMobileDisabled(f.OwnerSeat, r.Turn))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileDisabledEvent(f.Id, f.OwnerSeat, r.Turn, reason));
    }

    internal void ContinueMouZyMobile(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhouYuMobile: { } r } f || f.Id != id)
            return;
        if (!ValidMouZyMobile(f))
            throw new InvalidOperationException("Fanjian changed its actual covered material or paid return.");
        if (r.Stage is MouZhouYuMobileStage.OwnerChoice or MouZhouYuMobileStage.TargetChoice)
            return;
        if (DrainMouZyMobile(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouZhouYuMobile!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || r.Stage == MouZhouYuMobileStage.CleanupChildren)
        {
            FinishMouZyMobile(f);
            return;
        }

        if (r.Stage == MouZhouYuMobileStage.CoverChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { Stage = MouZhouYuMobileStage.TargetChoice }, PendingMovementContinuation = null });
            PublishMouZyMobile(f);
            return;
        }

        if (r.Stage == MouZhouYuMobileStage.ChoiceChildren)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { Revealed = true, Stage = MouZhouYuMobileStage.GiftChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.ActivationId, f.OwnerSeat, "covered", Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(r.TargetSeat), new("skill-program.mou-mobile:fanjian.gift"), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileGiftedEvent(f.Id, card.Id, r.TargetSeat, m.From, m.To, m.Sequence)));
            var paid = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileGiftedEvent>().Single(e => e.FrameId == f.Id);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = f.MouZhouYuMobile!with { GiftSequence = paid.Sequence } });
            SealMouZyMobile(f.Id, before);
            ContinueMouZyMobile(id);
            return;
        }

        if (r.Stage == MouZhouYuMobileStage.GiftChildren)
        {
            if (r.Answer == "turn-over")
            {
                FinishMouZyMobile(f);
                return;
            }

            var correct = (r.ActualSuit == r.DeclaredSuit) == (r.Answer == "same");
            if (correct)
            {
                DisableMouZyMobile(f, "correct-guess");
                FinishMouZyMobile(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { Stage = MouZhouYuMobileStage.LossChildren, LossHpBefore = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Hp }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileLossIssuedEvent(f.Id, r.TargetSeat, 1));
            _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, r.TargetSeat, 1, (before, after, _) =>
            {
                var latest = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(latest with { MouZhouYuMobile = latest.MouZhouYuMobile!with { LossPaid = true, LossHpBefore = before, LossHpAfter = after } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileLossPaidEvent(id, r.TargetSeat, before, after));
            });
            ContinueMouZyMobile(id);
            return;
        }

        FinishMouZyMobile(f);
    }

    internal void FinishMouZyMobile(ProgramSkillFrame f)
    {
        var r = f.MouZhouYuMobile!;
        if (r.CardId > 0 && _cardZonesOperations._cardZones.GetLocation(r.CardId) == CardLocation.Processing && r.Stage != MouZhouYuMobileStage.CleanupChildren)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = r with { Stage = MouZhouYuMobileStage.CleanupChildren, Revealed = true, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.mou-mobile:fanjian.cleanup"));
            SealMouZyMobile(f.Id, before);
            ContinueMouZyMobile(f.Id);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuMobileCompletedEvent(f.Id, r.PaymentSequence > 0, r.Answer));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhouYuMobile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 30)]
    [ProgramViewContributor(ProgramViewContribution.PublicProcessing, 20)]
    internal IReadOnlyList<Card> WithoutMouZhouYuMobileCovered(IReadOnlyList<Card> cards) => !HasMouZhouYuMobileCapability ? cards : Array.AsReadOnly(cards.Where(c => !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouZhouYuMobile is { Revealed: false } r && r.CardId == c.Id)).ToArray());
    [ProgramViewContributor(ProgramViewContribution.ProgramPublicCards, 30)]
    internal IReadOnlyList<CardSnapshot> MouZhouYuMobilePublicCards() => !HasMouZhouYuMobileCapability ? [] : Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MouZhouYuMobile is { Revealed: true, CardId: > 0 }).Select(f => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(f.MouZhouYuMobile!.CardId))).DistinctBy(c => c.Id).ToArray());
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouZhouYuFanjian)]
    internal PromptChoice AiMouZyMobile(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouZhouYuMobile!.Stage == MouZhouYuMobileStage.OwnerChoice)
        {
            var cards = p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).ToArray();
            return cards.Length == 0 ? p.Choices.Single(c => c.Parameters["branch"] == "cancel") : cards[_randomOperations._random.Next(Math.Min(4, cards.Length))];
        }

        // The target never inspects the covered entity/suit or the source Hand.
        var guess = _randomOperations._random.Next(2) == 0 ? "same" : "different";
        return p.Choices.Single(c => c.Parameters["branch"] == guess);
    }
}
