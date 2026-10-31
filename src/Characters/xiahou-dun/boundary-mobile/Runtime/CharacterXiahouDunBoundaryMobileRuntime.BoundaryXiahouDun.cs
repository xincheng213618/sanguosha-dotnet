#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunBoundaryMobileRuntime
{
    internal const string BoundaryXiahouDunQingjian = "boundary:qingjian-mobile", BoundaryXiahouDunAction = "boundary-xiahou-dun-qingjian";
    internal const string BoundaryXiahouDunStoreReason = "skill.boundary-xiahou-dun.qingjian.store", BoundaryXiahouDunGiveReason = "skill.boundary-xiahou-dun.qingjian.give";
    internal static CardConversionSource BoundaryXiahouDunSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryXiahouDunHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginBoundaryXiahouDun(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 60010)]
    internal bool OfferBoundaryXiahouDun(ProgramTriggerLegalityContext gate) => gate.SkillId != BoundaryXiahouDunQingjian || (gate.Trigger.Id == "store-after-gain" ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat]).Count > 0 : BoundaryXiahouDunDueDeposits(gate.OwnerSeat, gate.Context.ParentFrameId).Any());
    internal SkillProgramStepOutcome BeginBoundaryXiahouDun(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.BoundaryXiahouDun is not null || f.InstructionIndex != 1 || f.SkillId != BoundaryXiahouDunQingjian || f.WindowContext is not { } context || (op == SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian ? context.Window != SkillProgramTriggerWindow.CardsGained || f.TriggerId != "store-after-gain" : op != SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian || context.Window != SkillProgramTriggerWindow.TurnEnding || f.TriggerId != "distribute-at-ending"))
            throw new InvalidOperationException("Mobile Qingjian requires its original gain or issued ending frame.");
        var storing = op == SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian;
        var r = new BoundaryXiahouDunReceipt(BoundaryXiahouDunSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            Stage = storing ? BoundaryXiahouDunStage.Selecting : BoundaryXiahouDunStage.Distributing,
            Location = storing ? new(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"boundary-xiahou-dun:{(f.Id)}") : null,
            Deposits = storing ? [] : BoundaryXiahouDunDueDeposits(f.OwnerSeat, context.ParentFrameId).Where(d => d.Source.SkillInstanceId == f.SkillInstanceId && d.Hash == f.GameplayHash).Select(d => d.FrameId).ToArray(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r });
        ContinueBoundaryXiahouDun(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryXiahouDunHandChoices(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = BoundaryXiahouDunAction, ["branch"] = "toggle" } }).ToArray();
    internal Card BoundaryXiahouDunHandCard(ProgramSkillFrame f, PromptChoice c) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.OwnerSeat))[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)];
    internal IEnumerable<Card> BoundaryXiahouDunHeld(ProgramSkillFrame f) => f.BoundaryXiahouDun!.Deposits.SelectMany(id => _cardZonesOperations._cardZones.CardsAt(BoundaryXiahouDunDeposit(id).Location));
    internal PromptChoice BoundaryXiahouDunChoice(ProgramSkillFrame f, string branch, int value, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"boundary-xiahou-dun.{(f.Id)}.{(branch)}.{(value)}.{(targets?.FirstOrDefault() ?? -1)}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = BoundaryXiahouDunAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> BoundaryXiahouDunChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryXiahouDun!;
        if (r.Stage == BoundaryXiahouDunStage.Selecting)
            return BoundaryXiahouDunHandChoices(f).Select(c => c with { Description = (r.Selected.Contains(BoundaryXiahouDunHandCard(f, c).Id) ? "已选：" : "") + c.Description }).Append(BoundaryXiahouDunChoice(f, "store", 0, $"扣置已选的{(r.Selected.Count)}张手牌")).ToArray();
        if (r.Stage == BoundaryXiahouDunStage.Distributing)
            return BoundaryXiahouDunHeld(f).SelectMany(card => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => BoundaryXiahouDunChoice(f, "give", card.Id, $"将【{(card.DisplayName)}】交给 {(p.Name)}", [card.Id], [p.Seat]))).ToArray();
        return[];
    }

    internal void PublishBoundaryXiahouDun(ProgramSkillFrame f)
    {
        var choices = BoundaryXiahouDunChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("Qingjian has no real available distribution recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryXiahouDun!.Stage == BoundaryXiahouDunStage.Selecting ? "清俭：选择任意手牌扣置，最后确认。" : "清俭：将剩余扣置牌全部分配给其他角色。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryXiahouDunAction)]
    internal void AnswerBoundaryXiahouDun(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryXiahouDun: { } r } f || !ValidBoundaryXiahouDun(f) || !BoundaryXiahouDunChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || r.Stage == BoundaryXiahouDunStage.Selecting && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Qingjian's exact private selection or issued distribution changed.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters["branch"] == "toggle")
        {
            var card = BoundaryXiahouDunHandCard(f, selected);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Selected = r.Selected.Contains(card.Id) ? r.Selected.Where(id => id != card.Id).ToArray() : r.Selected.Append(card.Id).ToArray() } });
            PublishBoundaryXiahouDun(f);
            return;
        }

        if (selected.Parameters["branch"] == "store")
        {
            StoreBoundaryXiahouDun(f);
            return;
        }

        if (selected.Parameters["branch"] != "give")
            throw new InvalidOperationException("Unknown Qingjian answer.");
        var entity = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(entity.Id);
        var target = selected.Targets.Single();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.GiveChildren, Target = target, Payment = [new(entity.Id, entity.Kind, from)], Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([entity.Id], CardLocation.Hand(target), new(BoundaryXiahouDunGiveReason), disclosure: CardMovementDisclosure.FaceDown);
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.CardId == entity.Id && m.From == from && m.To == CardLocation.Hand(target) && m.Reason.Value == BoundaryXiahouDunGiveReason);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = f.BoundaryXiahouDun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Given = r.Given + actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunGivenEvent(f.Id, f.OwnerSeat, target, actual, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryXiahouDun(f.Id);
    }

    internal void StoreBoundaryXiahouDun(ProgramSkillFrame f)
    {
        var r = f.BoundaryXiahouDun!;
        if (r.Selected.Count == 0)
        {
            FinishBoundaryXiahouDun(f);
            return;
        }

        var available = BoundaryXiahouDunHandChoices(f).Select(c => BoundaryXiahouDunHandCard(f, c).Id).ToHashSet();
        if (r.Selected.Any(id => !available.Contains(id)))
            throw new InvalidOperationException("Qingjian cannot debit foreign or stale selected hand cards.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var location = r.Location!.Value;
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.StoreChildren, Payment = r.Selected.Select(id => new BoundaryXiahouDunMaterial(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, CardLocation.Hand(f.OwnerSeat))).ToArray(), Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, location, new(BoundaryXiahouDunStoreReason), disclosure: CardMovementDisclosure.FaceDown);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = f.BoundaryXiahouDun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var count = _cardZonesOperations._cardZones.CardsAt(location).Count;
        if (count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunStoredEvent(f.Id, r.Source, r.Hash, location, count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryXiahouDun(f.Id);
    }

    internal bool DrainBoundaryXiahouDun(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueBoundaryXiahouDun(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryXiahouDun: { } r } f || f.Id != id)
            return;
        if (!ValidBoundaryXiahouDun(f))
            throw new InvalidOperationException("Qingjian lost its original receipt or paid movement child.");
        if (r.Stage == BoundaryXiahouDunStage.Drawing)
            return;
        if (r.Stage is BoundaryXiahouDunStage.StoreChildren or BoundaryXiahouDunStage.GiveChildren or BoundaryXiahouDunStage.DrawChildren)
        {
            if (DrainBoundaryXiahouDun(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.BoundaryXiahouDun!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage != BoundaryXiahouDunStage.GiveChildren)
            {
                FinishBoundaryXiahouDun(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r = r with { Stage = BoundaryXiahouDunStage.Distributing, Payment = [], Target = -1 } });
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishBoundaryXiahouDun(f);
            return;
        }

        if (r.Stage == BoundaryXiahouDunStage.Distributing && !BoundaryXiahouDunHeld(f).Any())
        {
            if (r.Given <= 1)
            {
                FinishBoundaryXiahouDun(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill.boundary-xiahou-dun.qingjian.reward"));
            return;
        }

        PublishBoundaryXiahouDun(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian)]
    internal void ReturnBoundaryXiahouDunDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.BoundaryXiahouDun is not { Stage: BoundaryXiahouDunStage.Drawing, Given: > 1, Draw: null } r || !ValidBoundaryXiahouDun(f) || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Qingjian reward must return to its original fully distributed frame.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunRewardEvent(f.Id, f.OwnerSeat, r.Given, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryXiahouDun(f.Id);
    }

    internal void FinishBoundaryXiahouDun(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian, SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian)]
    internal PromptChoice AiBoundaryXiahouDun(PendingDecision prompt, ProgramSkillFrame f) => f.BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.Selecting, Selected.Count: > 0 } ? prompt.Choices.Last() : prompt.Choices.First();
}
