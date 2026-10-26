namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheYingxiangId = "shanhe:companion-yingxiang";
    private const string ShanheQingjueDiscardReason = "skill-program.shanhe-yingxiang.qingjue-discard";
    [ProgramCapability]
    private sealed class ShanheYingxiangHost(GameEngine engine) : IShanheYingxiangHost
    { public SkillProgramStepOutcome ResolveYingxiang(ProgramSkillFrame frame, bool used) => engine.BeginShanheYingxiang(frame, used); }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5406)]
    private void RecordShanheYingxiangGift(CardMovedEvent fact)
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheYingxiangMovement) ||
            fact.From.OwnerSeat is not { } owner || fact.To.OwnerSeat is not { } recipient || owner == recipient ||
            fact.To.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_players[owner].IsAlive ||
            !EnabledSkillPrograms(_players[owner]).Any(program => program.Id == ShanheYingxiangId)) return;
        var move = _cardMovements.LastOrDefault(m => m.CardId == fact.CardId && m.From == fact.From && m.To == fact.To && m.Reason == fact.Reason);
        if (move is not null) TagShanheYingxiang(owner, move);
    }

    private void TagShanheYingxiang(int owner, CardMovementRecord move)
    {
        if (!ProgramEventHistory<ShanheYingxiangTaggedEvent>().Any(tag => tag.OwnerSeat == owner && tag.MovementSequence == move.Sequence))
            AdvanceEventRulesAndQueueFact(new ShanheYingxiangTaggedEvent(owner, move.Sequence));
    }

    private bool IsShanheYingxiangBefore(int owner, int cardId, int before)
    {
        var tag = ProgramEventHistory<ShanheYingxiangTaggedEvent>().Where(item => item.OwnerSeat == owner && item.MovementSequence < before)
            .OrderByDescending(item => item.MovementSequence).FirstOrDefault(item =>
                _cardMovements.Any(move => move.Sequence == item.MovementSequence && move.CardId == cardId));
        if (tag is null) return false;
        // A transfer retains the original issuer; a settled return to deck, discard or outside ends this tag.
        return !_cardMovements.Any(move => move.CardId == cardId && move.Sequence > tag.MovementSequence && move.Sequence < before &&
            move.To.Zone is CardZoneKind.DrawPile or CardZoneKind.DiscardPile or CardZoneKind.OutsideGame);
    }

    private bool IsShanheYingxiangTrueUseLoss(CardMovementRecord move) => move.To == CardLocation.Processing &&
        (move.Reason == CardMoveReasons.Use || move.Reason == CardMoveReasons.EquipmentUse || move.Reason == CardMoveReasons.IronChainUse ||
         ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().Any(item => item.EntrySequence == move.Sequence &&
             item.CardId == move.CardId && item.From == move.From));

    private bool HasShanheYingxiangNonUseLoss(int owner, CardMovementBatchContext batch) =>
        !ProgramEventHistory<ShanheYingxiangQingjueStartedEvent>().Any(item => item.OwnerSeat == owner && item.RoundNumber == _roundNumber) &&
        batch.Movements.Any(move => move.From.OwnerSeat is { } holder && holder != owner && move.From != move.To &&
            move.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment &&
            IsShanheYingxiangBefore(owner, move.CardId, move.Sequence) && !IsShanheYingxiangTrueUseLoss(move));

    private bool IsShanheYingxiangMovement(ProgramTriggerCandidate candidate) =>
        candidate.SkillId == ShanheYingxiangId && candidate.BindingId == "marked-non-use-loss";

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 5406)]
    private void CollectShanheYingxiangMovement(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!_contentRegistry.Skills.ContainsKey(ShanheYingxiangId)) return;
        candidates.RemoveAll(IsShanheYingxiangMovement);
        foreach (var player in _players.Where(player => player.IsAlive && HasShanheYingxiangNonUseLoss(player.Seat, batch)))
            candidates.AddRange(CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.CardsMoved).Where(IsShanheYingxiangMovement));
    }

    private ProgramSkillWindowContext CreateShanheYingxiangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) =>
        new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat,
            Facts: CaptureProgramTriggerFacts(_players[candidate.OwnerSeat]), MovementBatch: frame.Batch);

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 5406)]
    private bool IsIssuedShanheYingxiangMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        IsShanheYingxiangMovement(candidate) && context is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } &&
        context.OwnerSeat == candidate.OwnerSeat && context.ParentFrameId == batch.Id &&
        _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault() is { } window && window.Id == batch.Id &&
        window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate &&
        HasRuntimeSkillInstance(_players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) &&
        _contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash == candidate.GameplayHash;

    private CardActionContext? ShanheYingxiangUsedAction(int owner, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } use } ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(window => window.Id == context.ParentFrameId) is not { } parent ||
            parent.Action.ActionId != use.CardActionId || parent.Action.ActorSeat == owner ||
            !(parent.Action.Type == CardActionType.Use || parent.Action.Type == CardActionType.Response &&
                parent.Action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) ||
            ProgramEventHistory<ShanheYingxiangUsePaidEvent>().Any(item => item.OwnerSeat == owner && item.ActionId == use.CardActionId)) return null;
        foreach (var card in parent.Action.PhysicalCards)
        {
            var entry = _cardMovements.LastOrDefault(move => move.CardId == card.CardId && move.From == card.From && move.To == CardLocation.Processing);
            if (entry is not null && IsShanheYingxiangBefore(owner, card.CardId, entry.Sequence)) return parent.Action;
        }
        return null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5406)]
    private bool CanOfferShanheYingxiang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        candidate.SkillId != ShanheYingxiangId || (trigger.Id == "marked-use"
            ? ShanheYingxiangUsedAction(candidate.OwnerSeat, context) is not null
            : context.MovementBatch is { } batch && HasShanheYingxiangNonUseLoss(candidate.OwnerSeat, batch));

    private SkillProgramStepOutcome BeginShanheYingxiang(ProgramSkillFrame supplied, bool used)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheYingxiang is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Yingxiang requires its original unpaid trigger window.");
        var source = new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var action = used ? ShanheYingxiangUsedAction(frame.OwnerSeat, context) : null;
        if (used && action is null || !used && (context.MovementBatch is not { } batch || !HasShanheYingxiangNonUseLoss(frame.OwnerSeat, batch)))
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = new(source, frame.GameplayHash, context.ParentFrameId,
            used ? ShanheYingxiangStage.Draw : ShanheYingxiangStage.Cost, action?.ActorSeat ?? -1, action?.ActionId ?? 0) });
        if (used) AdvanceEventRulesAndQueueFact(new ShanheYingxiangUsePaidEvent(frame.Id, frame.OwnerSeat, action!.ActorSeat, action.ActionId));
        else AdvanceEventRulesAndQueueFact(new ShanheYingxiangQingjueStartedEvent(frame.Id, frame.OwnerSeat, _roundNumber, context.MovementBatch!.Id));
        ContinueShanheYingxiang(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private Card[] ShanheQingjueCards(ProgramSkillFrame frame)
    {
        var hand = GetHand(_players[frame.OwnerSeat]);
        return hand.Where(card => hand.Count(other => other.Suit == card.Suit) != 1 &&
            !frame.ShanheYingxiang!.Cards.Contains(card.Id) &&
            !IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, CardLocation.Hand(frame.OwnerSeat), OwnedCardMoveIntent.Discard)).ToArray();
    }

    private IReadOnlyList<PromptChoice> ShanheYingxiangChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheYingxiang!;
        PromptChoice Choice(string branch, int value, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) =>
            new(new($"yingxiang.{frame.Id}.{receipt.Stage}.{branch}.{value}"), label, cards ?? [], targets ?? [],
                new Dictionary<string, string> { ["program-action"] = "shanhe-yingxiang", ["branch"] = branch, ["value"] = value.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        if (receipt.Stage == ShanheYingxiangStage.Cost)
            return ShanheQingjueCards(frame).Select(card => Choice("card", card.Id, "清绝：选择【" + card.DisplayName + "】", [card.Id]))
                .Append(Choice("pay", 0, receipt.Cards.Count == 0 ? "清绝：不弃牌" : "清绝：弃置所选牌")).ToArray();
        if (receipt.Stage == ShanheYingxiangStage.Option)
            return new[] { 1, 2 }.Where(option => (receipt.OptionsChosen & option) == 0)
                .Select(option => Choice("option", option, option == 1 ? "清绝：将弃置牌交给其他角色" : "清绝：获得手牌缺少花色的牌各一张")).ToArray();
        if (receipt.Stage == ShanheYingxiangStage.Recipient)
            return _players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat)
                .Select(player => Choice("recipient", player.Seat, "交给" + player.Name, targets: [player.Seat])).ToArray();
        return [];
    }

    private void PublishShanheYingxiang(ProgramSkillFrame frame)
    {
        var choices = ShanheYingxiangChoices(frame);
        if (choices.Count == 0) { FinishShanheYingxiang(frame); return; }
        ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "萦香：执行清绝", choices.SelectMany(choice => choice.Cards).ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-yingxiang")]
    private void AnswerShanheYingxiang(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheYingxiang: { } receipt } frame ||
            _pendingDecision?.PlayerSeat != frame.OwnerSeat ||
            !AssistedChoicesEqual([choice], ShanheYingxiangChoices(frame).Where(item => item.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Yingxiang lost its private payer, exact card selection or living recipient.");
        ClearPendingDecision();
        var value = int.Parse(choice.Parameters["value"], System.Globalization.CultureInfo.InvariantCulture);
        ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["branch"])
        {
            case "card":
                receipt = receipt with { Cards = receipt.Cards.Append(value).ToArray() };
                ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt });
                if (receipt.Cards.Count == 2) PayShanheQingjue(frame);
                break;
            case "pay": PayShanheQingjue(frame); break;
            case "option":
                ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { OptionsChosen = receipt.OptionsChosen | value,
                    OptionsRemaining = receipt.OptionsRemaining - 1, Stage = value == 1 ? ShanheYingxiangStage.Recipient : ShanheYingxiangStage.EffectChildren } });
                if (value == 2) GainShanheQingjueMissingSuits(frame);
                break;
            case "recipient":
                ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.EffectChildren } });
                var cards = receipt.Cards.Where(id => _cardZones.GetLocation(id) == CardLocation.DiscardPile &&
                    _cardMovements.LastOrDefault(move => move.CardId == id)?.Reason.Value == ShanheQingjueDiscardReason).ToArray();
                MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(value), new("skill-program.shanhe-yingxiang.qingjue-give"),
                    NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, value, cards, _movementSequence, (_, moves) =>
                    { foreach (var move in moves.Where(move => move.To == CardLocation.Hand(value))) TagShanheYingxiang(frame.OwnerSeat, move); }));
                break;
            default: throw new InvalidOperationException("Unknown Yingxiang choice.");
        }
        ContinueShanheYingxiang(frame.Id);
    }

    private void PayShanheQingjue(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheYingxiang!;
        if (receipt.Cards.Count == 0) { FinishShanheYingxiang(frame); return; }
        // The official wording qualifies each card by its suit's current hand multiplicity.
        // It grants at most two listed options, so select up to two such cards; no Qingjue passive is acquired.
        ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.CostChildren,
            OptionsRemaining = receipt.Cards.Count } });
        MoveProgramLogicalDiscard(frame, frame.OwnerSeat, receipt.Cards, new(ShanheQingjueDiscardReason));
    }

    private void GainShanheQingjueMissingSuits(ProgramSkillFrame frame)
    {
        if (!EnsureDrawPile()) return;
        var held = GetHand(_players[frame.OwnerSeat]).Select(card => card.Suit).ToHashSet();
        var cards = new[] { Suit.Spade, Suit.Heart, Suit.Club, Suit.Diamond }.Where(suit => !held.Contains(suit))
            .Select(suit => _cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(card => card.Suit == suit))
            .OfType<Card>().Select(card => card.Id).ToArray();
        MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(frame.OwnerSeat), new("skill-program.shanhe-yingxiang.qingjue-gain"));
    }

    private void ContinueShanheYingxiang(long frameId)
    {
        while (_resolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheYingxiang: { } receipt } frame && frame.Id == frameId)
        {
            if (receipt.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
                receipt.GameplayHash != frame.GameplayHash || frame.WindowContext?.ParentFrameId != receipt.ParentWindowId ||
                receipt.Cards is not System.Collections.IList { IsReadOnly: true } || receipt.Cards.Count > 2)
                throw new InvalidOperationException("Yingxiang lost its original frame, content identity or frozen paid selection.");
            if (TryBeginCardsMovedProgramWindow(frameId) || TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) ||
                TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program)) return;
            if (!_players[frame.OwnerSeat].IsAlive || _winner != Winner.None) { FinishShanheYingxiang(frame); return; }
            switch (receipt.Stage)
            {
                case ShanheYingxiangStage.Cost:
                case ShanheYingxiangStage.Option:
                case ShanheYingxiangStage.Recipient: PublishShanheYingxiang(frame); return;
                case ShanheYingxiangStage.CostChildren:
                case ShanheYingxiangStage.EffectChildren:
                    if (receipt.OptionsRemaining == 0) { FinishShanheYingxiang(frame); return; }
                    ReplaceRuntimeTop(frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.Option } }); continue;
                case ShanheYingxiangStage.Draw:
                    if (receipt.DrawIndex >= 2) { FinishShanheYingxiang(frame); return; }
                    var seat = receipt.DrawIndex == 0 ? frame.OwnerSeat : receipt.UserSeat;
                    ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { DrawIndex = receipt.DrawIndex + 1,
                        Stage = _players[seat].IsAlive ? ShanheYingxiangStage.DrawWaiting : ShanheYingxiangStage.Draw, DrawOutcome = null } });
                    if (!_players[seat].IsAlive) continue;
                    BeginOperationProgramDraw(frame, seat, 1, new("skill-program.shanhe-yingxiang.draw")); return;
                case ShanheYingxiangStage.DrawChildren:
                    ReplaceRuntimeTop(frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.Draw }, PendingMovementContinuation = null }); continue;
                case ShanheYingxiangStage.DrawWaiting:
                    throw new InvalidOperationException("Yingxiang must await its exact native draw return.");
                default: FinishShanheYingxiang(frame); return;
            }
        }
    }

    private void FinishShanheYingxiang(ProgramSkillFrame frame)
    { ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = null }); FinishProgramSkill(frame, true); }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheYingxiangUse)]
    private void ReturnShanheYingxiangDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheYingxiang is not { Stage: ShanheYingxiangStage.DrawWaiting, DrawOutcome: null } receipt ||
            outcome.RecipientSeat != (receipt.DrawIndex == 1 ? frame.OwnerSeat : receipt.UserSeat) ||
            outcome.RequestedCount != 1 || frame.NativeDraw is not null)
            throw new InvalidOperationException("Yingxiang lost its once-issued source/holder native draw.");
        ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.DrawChildren, DrawOutcome = outcome },
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 5406)]
    private bool IsShanheYingxiangDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) =>
        effect?.Op == SkillProgramEffectOp.ResolveShanheYingxiangUse &&
        frame.ShanheYingxiang is { Stage: ShanheYingxiangStage.DrawChildren, DrawOutcome: { } outcome } &&
        movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null &&
        outcome.RecipientSeat == (frame.ShanheYingxiang.DrawIndex == 1 ? frame.OwnerSeat : frame.ShanheYingxiang.UserSeat);
    [ProgramContinuation(ProgramContinuationKind.Resume, 5406)]
    private bool ResumeShanheYingxiang(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheYingxiang: not null } frame || frame.Id != frameId) return false;
        ContinueShanheYingxiang(frameId); return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5406)]
    private bool ContinuePaidShanheYingxiang(ProgramSkillFrame frame) => frame.ShanheYingxiang is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheYingxiangMovement)]
    private PromptChoice AiShanheYingxiang(PendingDecision prompt, ProgramSkillFrame frame) =>
        prompt.Choices.OrderByDescending(choice => choice.Parameters["branch"] switch
        {
            "card" => 10 - GetKeepValue(GetAdvancedCard(choice.Cards.Single()), _players[frame.OwnerSeat]),
            "pay" => frame.ShanheYingxiang!.Cards.Count == 0 ? -10 : 1,
            "option" => choice.Parameters["value"] == "2" ? 20 : 10,
            "recipient" => OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], _players[choice.Targets.Single()]) ? 20 : 0,
            _ => 0
        }).First();
}
