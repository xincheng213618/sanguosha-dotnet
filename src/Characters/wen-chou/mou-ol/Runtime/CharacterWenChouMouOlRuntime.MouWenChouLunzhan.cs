#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenChouMouOlRuntime
{
    internal const string MouWenChouDrawReason = "skill-program.mou-ol:lunzhan.draw", MouWenChouDiscardReason = "skill-program.mou-ol:juejue.discard";
    internal bool HasMouWenChouOl => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouWenChouJuejue) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.UseMouWenChouLunzhan);

    internal static CardConversionSource MouWenChouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool MouWenChouCountUsed(int owner, int count) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouLunzhanIssuedEvent>().Any(e => e.Origin.Source.OwnerSeat == owner && e.Origin.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Origin.MaterialCount == count);
    internal bool MouWenChouTargetBanned(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouRewardAcceptedEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    SkillProgramStepOutcome global::CardGame.Core.IMouWenChouOlHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.UseMouWenChouLunzhan ? BeginMouWenChouLunzhan(f) : BeginMouWenChouTrigger(f, op);
    bool global::CardGame.Core.IMouWenChouOlSelection.Target(int owner, int target) => !MouWenChouTargetBanned(owner, target);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 33100, SkillProgramEffectOp.UseMouWenChouLunzhan)]
    internal bool CanActivateMouWenChouLunzhan(ProgramActivationLegalityContext c) => _coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && !MouWenChouCountUsed(c.OwnerSeat, c.Activation.MinCards) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat && !MouWenChouTargetBanned(c.OwnerSeat, p.Seat));
    internal bool ValidMouWenChouOl(ProgramSkillFrame f) => f.MouWenChouOl is { } r && r.Source == MouWenChouSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && Enum.IsDefined(r.Stage) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f).Op == r.Operation && _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == f.GameplayHash && e.Operation == r.Operation && e.UseId == (r.Operation == SkillProgramEffectOp.UseMouWenChouLunzhan ? 0 : r.UseId)) == 1 && (r.Operation == SkillProgramEffectOp.UseMouWenChouLunzhan ? f.TriggerId is null && f.ActivationId == r.Source.BindingId && f.WindowContext is null && f.SelectedCardIds.Count is >= 0 and <= 5 && f.SelectedTargetSeats.Count == 1 : f.ActivationId == f.TriggerId && f.WindowContext?.CardUse?.ParentCardUseFrameId == r.UseId);
    internal Card[] MouWenChouMaterials(ProgramSkillFrame f) => f.SelectedCardIds.Select(id =>
    {
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        if (location.OwnerSeat != f.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Lunzhan requires its actual owned HE materials.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == id);
        if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(f.OwnerSeat, [id]))
            throw new InvalidOperationException("Lunzhan cannot bypass a native material restriction.");
        return card;
    }).ToArray();
    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouWenChouOptions(ProgramSkillFrame f)
    {
        var cards = MouWenChouMaterials(f);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var suits = cards.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, c)).Distinct().ToArray();
        var suit = suits.Length == 1 ? suits[0] : Suit.None;
        var colors = cards.Select(c => _coreActionDiscardColorsOperations.CapturePhysicalCardColor(f.OwnerSeat, c)).Distinct().ToArray();
        var color = colors.Length == 1 ? colors[0] : null;
        return Array.AsReadOnly(_coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, CardKind.Duel, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: f.SelectedCardIds, includeNextActualUseAdjustment: true, conversionSource: MouWenChouSource(f)).Where(o => o.TargetSeats.Count > 0 && o.TargetSeats[0] == f.SelectedTargetSeats[0] && o.TargetSeats.All(t => !MouWenChouTargetBanned(f.OwnerSeat, t))).ToArray());
    }

    internal SkillProgramStepOutcome BeginMouWenChouLunzhan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var amount = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f).Amount;
        if (f.MouWenChouOl is not null || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != amount || f.SelectedCardIds.Distinct().Count() != amount || f.SelectedTargetSeats.Count != 1 || !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || MouWenChouCountUsed(f.OwnerSeat, amount))
            throw new InvalidOperationException("Lunzhan requires an unused actual-turn material count and its own live Play activation.");
        var r = new MouWenChouOlReceipt(MouWenChouSource(f), f.GameplayHash, SkillProgramEffectOp.UseMouWenChouLunzhan, MouWenChouOlStage.Targets)
        {
            ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            TargetSeat = f.SelectedTargetSeats[0]
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, 0));
        var options = MouWenChouOptions(f);
        if (options.Count == 0)
        {
            FinishMouWenChouOl(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (options.Count == 1)
        {
            IssueMouWenChouDuel(f, options[0]);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMouWenChouOl(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card[] MouWenChouDiscardable(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> MouWenChouChoices(ProgramSkillFrame f)
    {
        var r = f.MouWenChouOl!;
        PromptChoice Choice(string branch, string label, int? card = null, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption? option = null) => new(new($"mou-wen-chou.{(f.Id)}.{(r.Stage)}.{(branch)}.{(card)}.{(option?.Id.Value)}"), label, card is { } id ? [id] : [], option?.TargetSeats ?? [], new Dictionary<string, string> { ["program-action"] = "mou-wen-chou-ol", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["option-id"] = option?.Id.Value ?? "" });
        if (r.Stage == MouWenChouOlStage.Targets)
            return Array.AsReadOnly(MouWenChouOptions(f).Select(o => Choice("duel", o.Description, option: o)).ToArray());
        if (r.Stage == MouWenChouOlStage.Reward)
            return[Choice("draw", "摸X张牌，本回合不能再轮战此目标"), Choice("decline", "不摸牌")];
        return r.Stage == MouWenChouOlStage.Discard && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? Array.AsReadOnly(MouWenChouDiscardable(r.TargetSeat).Select(c => Choice("discard", $"弃置【{(c.DisplayName)}】", c.Id)).ToArray()) : [];
    }

    internal void PublishMouWenChouOl(ProgramSkillFrame f)
    {
        var r = f.MouWenChouOl!;
        var choices = MouWenChouChoices(f);
        if (choices.Count == 0)
        {
            FinishMouWenChouOl(f);
            return;
        }

        var chooser = r.Stage == MouWenChouOlStage.Discard ? r.TargetSeat : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouWenChouOlStage.Discard ? $"决绝：还需弃置{(r.Amount - r.Discarded)}张牌。" : r.Stage == MouWenChouOlStage.Reward ? "轮战：是否摸牌并停止本回合轮战此目标？" : "轮战：选择本次决斗的原生目标调整。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-wen-chou-ol")]
    internal void AnswerMouWenChouOl(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouWenChouOl: { } r } f || !ValidMouWenChouOl(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouWenChouOlStage.Discard ? r.TargetSeat : f.OwnerSeat) || !MouWenChouChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mou Wen Chou requires its current chooser and exact live native option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishMouWenChouOl(f);
            return;
        }

        if (branch == "duel")
        {
            IssueMouWenChouDuel(f, MouWenChouOptions(f).Single(o => o.Id.Value == choice.Parameters["option-id"]));
            return;
        }

        if (branch == "draw")
        {
            var x = MouWenChouTargetCount(f.OwnerSeat, r.TargetSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { Amount = x, Stage = MouWenChouOlStage.Drawing } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouRewardAcceptedEvent(f.Id, r.UseId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.TargetSeat, x));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, x, new(MouWenChouDrawReason));
            return;
        }

        if (branch != "discard" || r.Stage != MouWenChouOlStage.Discard || !MouWenChouDiscardable(r.TargetSeat).Any(c => c.Id == choice.Cards.Single()))
            throw new InvalidOperationException("Juejue must discard the current recipient's real discardable HE entity.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var card = choice.Cards.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { Stage = MouWenChouOlStage.DiscardChildren, Before = before, After = before, Discarded = r.Discarded + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, [card], new(MouWenChouDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = f.MouWenChouOl!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouDiscardPaidEvent(f.Id, r.TargetSeat, card, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueMouWenChouOl(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.UseMouWenChouLunzhan, SkillProgramEffectOp.RewardMouWenChouLunzhan, SkillProgramEffectOp.ResolveMouWenChouJuejue)]
    internal PromptChoice ChooseMouWenChouOl(PendingDecision pending, ProgramSkillFrame frame)
    {
        var options = pending.Choices!;
        if (options.Any(c => c.Parameters.GetValueOrDefault("branch") == "draw"))
            return options.First(c => c.Parameters["branch"] == "draw");
        return options.OrderBy(c => c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreResponseEntityExchangeOperations.EntityAtCurrentLocation(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[pending.PlayerSeat]) : 0).First();
    }

    internal bool DrainMouWenChouOl(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouWenChouOl(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouWenChouOl: { } r } f || f.Id != id)
            return;
        if (!ValidMouWenChouOl(f))
            throw new InvalidOperationException("Mou Wen Chou lost its owning frame or signed continuation.");
        if (r.Stage is MouWenChouOlStage.Targets or MouWenChouOlStage.Reward or MouWenChouOlStage.Discard)
        {
            PublishMouWenChouOl(f);
            return;
        }

        if (r.Stage is MouWenChouOlStage.Issuing or MouWenChouOlStage.NativeUse or MouWenChouOlStage.Drawing)
            return;
        if (DrainMouWenChouOl(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouWenChouOl!;
        if (r.Stage == MouWenChouOlStage.DiscardChildren && r.Discarded < r.Amount && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && MouWenChouDiscardable(r.TargetSeat).Length > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { Stage = MouWenChouOlStage.Discard }, PendingMovementContinuation = null });
            PublishMouWenChouOl(f);
            return;
        }

        FinishMouWenChouOl(f);
    }

    internal void FinishMouWenChouOl(ProgramSkillFrame f)
    {
        var r = f.MouWenChouOl!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouCompletedEvent(f.Id, r.Operation));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 33100)]
    internal bool ResumeMouWenChouOl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouWenChouOl: not null } f || f.Id != id)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            ContinueMouWenChouOl(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33100)]
    internal bool ReturnMouWenChouMovement(ProgramSkillFrame f) => f.MouWenChouOl is not null && ResumeMouWenChouOl(f.Id);
}
