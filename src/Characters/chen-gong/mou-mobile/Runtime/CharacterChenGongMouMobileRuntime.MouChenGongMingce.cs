#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    internal const string MouChenGongGiftReason = "skill-program.mou-mobile:mingce.give";
    internal bool HasMouChenGongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouChenGongMingceGift) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouChenGongMingceRelease) || _contentRegistryOperations._contentRegistry.Skills.ContainsKey(MouChenGongComposition.Zhichi);

    internal static CardConversionSource MouChenGongSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_chen_gong_mou_mobile_MouChenGongMingce.MouChenGongSource(f);
    internal bool MouChenGongPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMingceStartedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MouChenGongComposition.Mingce && e.Phase == phase);
    [ProgramPhaseUsedProjection(30)]
    internal int MouChenGongProjectedPhaseUses(int owner, string skill, int fallback) => skill == MouChenGongComposition.Mingce && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMingceStartedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Phase == phase) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 29600, SkillProgramEffectOp.ResolveMouChenGongMingceGift)]
    internal bool CanStartMouChenGongGift(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && !MouChenGongPhaseSpent(owner.Seat, phase) && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Any() && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat);
    internal bool MouChenGongGiftRoot(ProgramSkillFrame f) => f.MouChenGongGift is { } r && f.SkillId == MouChenGongComposition.Mingce && f.TriggerId is null && f.ActivationId == "gift-and-plan" && f.InstructionIndex == 1 && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.SequenceEqual([r.RecipientSeat]) && r.Source == MouChenGongSource(f) && r.GameplayHash == f.GameplayHash && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RecipientSeat) && r.RecipientSeat != f.OwnerSeat && r.Phase.ActorSeat == f.OwnerSeat && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } p && p.GameplayHash == f.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongMingceStartedEvent>().Count(e => e == new MouChenGongMingceStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.RecipientSeat)) == 1;
    internal SkillProgramStepOutcome BeginMouChenGongGift(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouChenGongGift is not null || f.SkillId != MouChenGongComposition.Mingce || f.ActivationId != "gift-and-plan" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var recipient] || recipient == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || MouChenGongPhaseSpent(f.OwnerSeat, phase))
            throw new InvalidOperationException("Mingce requires one fresh exact actual own Play and one living other recipient.");
        var r = new MouChenGongGiftReceipt(MouChenGongSource(f), f.GameplayHash, phase, recipient);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongMingceStartedEvent(f.Id, r.Source, r.GameplayHash, phase, recipient));
        PublishMouChenGong(f, f.OwnerSeat, MouChenGongGiftChoices(f), "明策：将一张手牌或装备牌交给目标。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishMouChenGong(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text)
    {
        if (choices.Count == 0)
            throw new InvalidOperationException("Mingce lost every canonical private choice.");
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

    internal IReadOnlyList<PromptChoice> MouChenGongGiftChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: f.MouChenGongGift!.RecipientSeat).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-chen-gong-gift" } }).ToArray());
    [ProgramAnswer("mou-chen-gong-gift")]
    internal void AnswerMouChenGongGift(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouChenGongGift!;
        if (!MouChenGongGiftRoot(f) || r.Stage != MouChenGongGiftStage.GiftChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouChenGongGiftChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mingce requires the owner's exact current HE payment slot.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.GiftChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(r.RecipientSeat), new(MouChenGongGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, r.RecipientSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, m =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            if (!MouChenGongGiftRoot(current) || current.MouChenGongGift!.Gift is not null || m.CardId != card.Id || m.From != from)
                throw new InvalidOperationException("Mingce can pay only its one original actual gift.");
            var paid = new MouChenGongGiftPayment(m.CardId, m.CardKind, m.From, m.To, m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouChenGongGift = current.MouChenGongGift with { Gift = paid, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongGiftPaidEvent(f.Id, r.RecipientSeat, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
        }), disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        SealMouChenGongMovements(f.Id, before);
        ContinueMouChenGongGift(f.Id);
    }

    internal void ContinueMouChenGongGift(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault() is not ProgramSkillFrame { MouChenGongGift: { Stage: MouChenGongGiftStage.GiftChildren } r } f || f.Id != id || DrainMouChenGongChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = null, PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(id, host, host);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29600)]
    internal void RecordMouChenGongProgramFacts(IGameEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: MouChenGongComposition.Mingce, ActivationId: "gift-and-plan", TriggerId: null } f)
            return;
        if (fact is ProgramSkillResolvedEvent done && done.FrameId == f.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongGiftPaidEvent>().SingleOrDefault(e => e.FrameId == f.Id) is { } paid)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongMingceCompletedEvent(f.Id, paid.RecipientSeat, f.ChoiceBindings.SingleOrDefault(b => b.Name == "mingce-branch")?.OptionId, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongCeChangedEvent>().Any(e => e.FrameId == f.Id && e.After > e.Before)));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 29600)]
    internal void RecordMouChenGongCe(IGameEvent fact)
    {
        if (fact is PlayerMarkerChangedEvent { Marker: PlayerMarkerKind.MouChenGongCe, Delta: > 0, Reason: "program.participant-marker" } marker && _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame { SkillId: MouChenGongComposition.Mingce } f && marker.ResolutionId == f.Id)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongCeChangedEvent(f.Id, f.OwnerSeat, marker.Count - marker.Delta, marker.Count));
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouChenGongMingceGift)]
    internal PromptChoice AiMouChenGongGift(PendingDecision pending, ProgramSkillFrame f) => pending.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();

    [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = "mou-mobile:mingce")]
    internal PromptChoice AiMouChenGongBranch(PendingDecision pending, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(pending.PlayerSeat);
        var friendly = _coreAiBrainsAiBrainsAccess._aiBrains[pending.PlayerSeat].ScoreProgramTarget(view, f.OwnerSeat, new(OwnerDraw: 0, OwnerRecovery: 0, OwnerHpLoss: 0, TargetDraw: 1, TargetRecovery: 0, TargetHpLoss: 0, GivesSelected: false, DiscardsSelected: false)) > 0;
        var branch = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && view.Players[pending.PlayerSeat].Hp >= 4 && friendly ? "lose-hp" : "draw";
        return pending.Choices.Single(c => c.Parameters["option-id"] == branch);
    }

    SkillProgramStepOutcome IMouChenGongHost.Gift(ProgramSkillFrame frame) => BeginMouChenGongGift(frame);
    SkillProgramStepOutcome IMouChenGongHost.Release(ProgramSkillFrame frame) => BeginMouChenGongRelease(frame);
}
