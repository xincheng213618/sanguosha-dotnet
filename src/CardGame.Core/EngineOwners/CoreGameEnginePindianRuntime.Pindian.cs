#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePindianRuntime
{
    internal void BeginSharedPindian(long parentId, SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = null, int? sourceCardId = null, string? programResultBind = null, SkillProgramCardSetVisibility programResultVisibility = SkillProgramCardSetVisibility.Public, PindianOwnedPileSource? ownedPileSource = null)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parentId || _resolutionStackOperations._resolutionStack.Any(item => item is PindianFrame) || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[sourceSeat]) || opponentSeat is { } opponent && !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(sourceSeat, opponent))
            throw new InvalidOperationException("Pindian requires one suspended parent and a living source with hand cards.");
        var selectedSourceTopChoice = sourceCardId is { } selectedSource && ownedPileSource is null && _resolutionStackOperations._resolutionStack[^1] is ProgramSkillFrame selectedParent && selectedParent.OwnerSeat == sourceSeat && selectedParent.SelectedCardIds.SequenceEqual([selectedSource]) && _corePindianPoliciesOperations.CanOfferPindianTopChoice(_coreMatchParticipantsPlayersAccess._players[sourceSeat]);
        var frame = new PindianFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentId, presentation.SkillId, presentation, sourceSeat, opponentSeat, sourceCardId, opponentSeat is null ? PindianStep.ChooseParticipants : sourceCardId is null || selectedSourceTopChoice ? PindianStep.ChooseSourceCard : PindianStep.ChooseOpponentCard, ProgramResultBind: programResultBind, ProgramResultVisibility: programResultVisibility, ParentProcessingCardIds: Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(card => card.Id).Order().ToArray()))
        {
            OwnedPileSource = ownedPileSource
        };
        _corePindianOwnedPileSourcesOperations.Value.AssertOwnedPilePindianSource(frame);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        PublishPindianSelection(frame);
    }

    internal void PublishPindianSelection(PindianFrame frame)
    {
        if (_corePindianRandomSelectionsOperations.TryContinuePindianRandomSelection(frame))
            return;
        frame = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        var selectingSource = frame.PindianStep is PindianStep.ChooseParticipants or PindianStep.ChooseSourceCard;
        var chooser = _coreMatchParticipantsPlayersAccess._players[selectingSource ? frame.SourceSeat : frame.OpponentSeat!.Value];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(chooser);
        if (frame.PindianStep == PindianStep.ChooseSourceCard && frame.SourceCardId is { } originallySelected)
            hand = hand.Where(card => card.Id == originallySelected).ToArray();
        var targets = frame.PindianStep == PindianStep.ChooseParticipants ? _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != chooser.Seat && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(player) && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(frame.SourceSeat, player.Seat)).Select(player => player.Seat).ToArray() : [];
        if (!chooser.IsAlive || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(chooser) || frame.PindianStep == PindianStep.ChooseParticipants && targets.Length == 0 || !selectingSource && (chooser.Seat == frame.SourceSeat || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(frame.SourceSeat, chooser.Seat) || !_corePindianPoliciesOperations.HasCurrentPindianSourceCard(frame)))
            throw new InvalidOperationException("Pindian participants no longer have legal hand cards.");
        var choices = new List<PromptChoice>();
        foreach (var card in hand)
        {
            if (frame.PindianStep == PindianStep.ChooseParticipants)
                foreach (var target in targets)
                    choices.Add(new(new ChoiceId($"pindian.{(frame.Id)}.card-{(card.Id)}.target-{(target)}"), $"以【{(card.DisplayName)}】（{(card.Rank)}）与 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 拼点", [card.Id], [target], new Dictionary<string, string> { ["action"] = "pindian-start" }));
            else if (frame.PindianStep == PindianStep.ChooseSourceCard)
                choices.Add(new(new ChoiceId($"pindian.{(frame.Id)}.source-card-{(card.Id)}"), $"以【{(card.DisplayName)}】（{(card.Rank)}）发起拼点", [card.Id], [], new Dictionary<string, string> { ["action"] = "pindian-source-card" }));
            else
                choices.Add(new(new ChoiceId($"pindian.{(frame.Id)}.card-{(card.Id)}"), $"以【{(card.DisplayName)}】（{(card.Rank)}）参与拼点", [card.Id], [], new Dictionary<string, string> { ["action"] = "pindian-card" }));
        }

        if (_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(chooser))
        {
            var maximumTargets = frame.PindianStep == PindianStep.ChooseParticipants ? targets.Select(t => (int? )t) : new int? []
            {
                null
            };
            foreach (var target in maximumTargets)
                choices.Add(new(new($"pindian.{(frame.Id)}.maximum.{(target)}"), "弃置牌后以牌堆最大点数的牌拼点", [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["action"] = "pindian-maximum" }));
        }

        if (_corePindianPoliciesOperations.CanOfferPindianTopChoice(chooser))
        {
            var topTargets = frame.PindianStep == PindianStep.ChooseParticipants ? targets.Select(t => (int? )t) : new int? []
            {
                null
            };
            foreach (var target in topTargets)
                choices.Add(new(new ChoiceId($"pindian.{(frame.Id)}.top.target-{(target)}"), "使用牌堆顶的牌拼点", [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["action"] = "pindian-top" }));
        }

        var prompt = frame.PindianStep == PindianStep.ChooseParticipants ? $"【{(frame.Presentation.Name)}】：选择自己的拼点牌及一名其他角色。" : frame.PindianStep == PindianStep.ChooseSourceCard ? $"【{(frame.Presentation.Name)}】：请选择自己的拼点牌。" : $"{(_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat].Name)} 对你发动【{(frame.Presentation.Name)}】，请选择一张手牌作为拼点牌。";
        SetPindianPrompt(frame, chooser.Seat, DecisionKind.SkillModule, prompt, choices, hand.Select(card => card.Id).ToArray(), targets, frame.Presentation with { Title = $"{(frame.Presentation.Name)} · 拼点", Instructions = prompt });
    }

    internal void SetPindianPrompt(PindianFrame frame, int playerSeat, DecisionKind kind, string prompt, IReadOnlyList<PromptChoice> choices, IReadOnlyList<int> cards, IReadOnlyList<int> targets, SkillPromptPresentation presentation)
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(kind, playerSeat, prompt, cards, targets, SourceSeat: frame.SourceSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = playerSeat,
            IsPrivate = true,
            SkillPrompt = presentation,
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[playerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolvePindianChoice(PromptChoice selected)
    {
        AssertPindianInvariant();
        var frame = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.All(choice => choice.Id != selected.Id))
            throw new InvalidOperationException("The Pindian answer was not published.");
        if (_corePindianRandomSelectionsOperations.TryResolvePindianRandomChoice(frame, selected))
            return;
        if (frame.PindianStep == PindianStep.ClaimResult)
        {
            _corePindianClaimsOperations.ResolvePindianClaimChoice(selected);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var top = selected.Parameters.GetValueOrDefault("action") == "pindian-top";
        var selectingSource = frame.PindianStep is PindianStep.ChooseParticipants or PindianStep.ChooseSourceCard;
        var chooser = _coreMatchParticipantsPlayersAccess._players[selectingSource ? frame.SourceSeat : frame.OpponentSeat!.Value];
        var selectedId = selected.Parameters.GetValueOrDefault("action") == "pindian-maximum" ? 0 : top ? _corePindianPoliciesOperations.ReserveOrReadPindianTop(chooser, selectingSource) : selected.Cards.Single();
        switch (frame.PindianStep)
        {
            case PindianStep.ChooseParticipants:
                frame = frame with
                {
                    OpponentSeat = selected.Targets.Single(),
                    SourceCardId = selectedId,
                    SourceUsesDrawPileTop = top,
                    PindianStep = PindianStep.ChooseOpponentCard
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame);
                PublishPindianSelection(frame);
                return;
            case PindianStep.ChooseSourceCard:
                frame = frame with
                {
                    SourceCardId = selectedId,
                    SourceUsesDrawPileTop = top,
                    PindianStep = PindianStep.ChooseOpponentCard
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame);
                PublishPindianSelection(frame);
                return;
            case PindianStep.ChooseOpponentCard:
                RevealPindian(frame, selectedId, top);
                return;
            default:
                throw new InvalidOperationException("Unknown Pindian step.");
        }
    }

    internal void RevealPindian(PindianFrame frame, int opponentCardId, bool opponentTop = false)
    {
        if (_characterLuKangWumiaoOperations.TryBeginWuLuKangPairBeforeReveal(frame, opponentCardId, opponentTop))
            return;
        if (_characterPangJiOlOperations.TryBeginDouyuPairPreReveal(frame, opponentCardId, opponentTop))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat];
        var opponent = _coreMatchParticipantsPlayersAccess._players[frame.OpponentSeat!.Value];
        _corePindianOwnedPileSourcesOperations.Value.AssertOwnedPilePindianSource(frame);
        var sourceLocation = frame.OwnedPileSource?.Pile.Location ?? (frame.SourceUsesDrawPileTop ? CardLocation.Processing : CardLocation.Hand(source.Seat));
        var sourceInput = _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, source.Seat, frame.SourceCardId!.Value, sourceLocation, 0, frame.SourceUsesDrawPileTop);
        var opponentInput = _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, opponent.Seat, opponentCardId, opponentTop ? CardLocation.DrawPile : CardLocation.Hand(opponent.Seat), 0, opponentTop);
        sourceLocation = sourceInput.From;
        var sourceCard = _cardZonesOperations._cardZones.CardsAt(sourceLocation).Single(card => card.Id == sourceInput.CardId);
        var opponentCard = _cardZonesOperations._cardZones.CardsAt(opponentInput.From).Single(card => card.Id == opponentInput.CardId);
        var sourceIdentityRank = frame.WuLuKangContest is not null ? sourceInput.Rank : _characterGuanYuWumiaoOperations.JuewuForeignHandRank(source, sourceCard)is not null ? _corePindianPoliciesOperations.EffectivePindianRank(source, sourceCard) : _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholPindianRank(frame, source, sourceCard, true, frame.SourceUsesDrawPileTop);
        var opponentIdentityRank = frame.WuLuKangContest is not null ? opponentInput.Rank : _characterGuanYuWumiaoOperations.JuewuForeignHandRank(opponent, opponentCard)is not null ? _corePindianPoliciesOperations.EffectivePindianRank(opponent, opponentCard) : _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholPindianRank(frame, opponent, opponentCard, false, opponentTop);
        if (sourceLocation != CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(sourceCard, sourceLocation, CardLocation.Processing, CardMoveReasons.PindianReveal);
        if (opponentInput.From != CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(opponentCard, opponentInput.From, CardLocation.Processing, CardMoveReasons.PindianReveal);
        _characterLuKangWumiaoOperations.RevealWuLuKangInputs(frame);
        var result = new PindianResult(source.Seat, opponent.Seat, sourceCard.Id, opponentCard.Id, sourceIdentityRank ?? _corePindianPoliciesOperations.EffectivePindianRank(source, sourceCard), opponentIdentityRank ?? _corePindianPoliciesOperations.EffectivePindianRank(opponent, opponentCard));
        frame = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        result = _characterPangJiOlOperations.ApplyDouyuPairResult(frame, result);
        frame = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        if (_corePindianRevealWindowsOperations.TryBeginPairPindianReveal(frame, result))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianResultDeterminedEvent(frame.Id, frame.SkillId, result));
        _coreMatchLogOperations.AddLog("Pindian", $"{(source.Name)} 以 {(result.SourceRank)} 点与 {(opponent.Name)} 的 {(result.OpponentRank)} 点拼点，" + (result.SourceWon ? "发起者获胜。" : "发起者未赢。"), source.Seat, opponent.Seat);
        frame = frame with
        {
            Result = result
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        if (_characterZhangLiaoMobileGodOperations.TryBeginMobileZhitiPindianPrograms(frame))
            return;
        if (!_corePindianClaimsOperations.BeginPindianClaims(frame))
            CompletePindian((PindianFrame)_resolutionStackOperations._resolutionStack[^1]);
    }

    internal int[] AvailablePindianCards(PindianResult result) => new[]
    {
        result.SourceCardId,
        result.OpponentCardId
    }.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
    internal void CompletePindian(PindianFrame frame)
    {
        var foreignOrigin = _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame owner ? _coreForeignTurnContestsOperations.Value.FreezeForeignTurnPindianOrigin(owner, frame) : null;
        foreach (var id in AvailablePindianCards(frame.Result!))
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.PindianFinish);
        }

        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.Pindian);
        if (_resolutionStackOperations._resolutionStack[^1] is ProgramSkillFrame program)
        {
            if (frame.ProgramResultBind is not { } bind || program.PindianResultBindings.Any(item => item.Name == bind))
                throw new InvalidOperationException("Program Pindian lost its unique result binding.");
            var result = frame.Result!;
            _coreRuntimeOperations.ReplaceRuntimeTop(program with { PindianResultBindings = Array.AsReadOnly(program.PindianResultBindings.Append(new ProgramPindianResultBinding(bind, result.SourceSeat, result.OpponentSeat, result.SourceRank, result.OpponentRank, result.SourceWon, frame.ProgramResultVisibility) { ForeignTurnOrigin = foreignOrigin, NativePindianFrameId = frame.Id }).ToArray()) });
            _corePindianHandReturnsOperations.RecordPindianHandContestReturn(_coreProgramLifecycleOperations.GetActiveProgramFrame(program.Id), frame);
            _characterZhuRongBoundaryMobileOperations.CaptureMobileZhuRongPindianReturn(_coreProgramLifecycleOperations.GetActiveProgramFrame(program.Id), frame);
            _coreRuntimeOperations.AdvanceRuntimeProgram(program.Id);
        }
        else
            throw new InvalidOperationException("Pindian lost its parent continuation.");
    }

    internal void ResolvePendingAiPindian()
    {
        var frame = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision!;
        if (_corePindianRandomSelectionsOperations.IsPindianRandomPrompt(frame))
        {
            ResolvePindianChoice(decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("take") == "true"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (frame.PindianStep == PindianStep.ClaimResult)
        {
            ResolvePindianChoice(decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("take") == "true"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var selected = decision.Choices.OrderByDescending(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-maximum" ? 10 : choice.Parameters.GetValueOrDefault("action") == "pindian-top" ? 9 : _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat], _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]).Single(card => card.Id == choice.Cards.Single()))).ThenBy(choice => choice.Cards.FirstOrDefault()).ThenBy(choice => choice.Targets.FirstOrDefault()).First();
        ResolvePindianChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool HasPindianChild(long parentId) => _resolutionStackOperations._resolutionStack.LastOrDefault()is PindianFrame frame && frame.ParentFrameId == parentId;
    internal void AssertPindianInvariant()
    {
        var frames = _resolutionStackOperations._resolutionStack.OfType<PindianFrame>().ToArray();
        foreach (var owned in frames)
            _corePindianOwnedPileSourcesOperations.Value.AssertOwnedPilePindianSource(owned);
        if (frames.Length == 0)
            return;
        if (frames.Length == 1 && _characterJianYongBoundaryMobileOperations.IsBoundaryJianYongZongshiSubtree(frames[0]))
            return;
        if (frames.Length == 1 && _characterZhangLiaoMobileGodOperations.IsMobileZhitiContestSubtree(frames[0]))
            return;
        if (frames.Length == 1 && _characterLuKangWumiaoOperations.IsWuLuKangPairSubtree(frames[0]))
        {
            _characterLuKangWumiaoOperations.AssertWuLuKangWindows();
            _characterLuKangWumiaoOperations.AssertWuLuKangPairProcessing(frames[0]);
            return;
        }

        if (frames.Length == 1 && _characterPangJiOlOperations.IsDouyuPairSubtree(frames[0]))
        {
            _characterPangJiOlOperations.AssertDouyuWindows();
            _characterPangJiOlOperations.AssertDouyuPairSubtreeProcessing(frames[0]);
            return;
        }

        if (frames.Length == 1 && _corePindianRevealWindowsOperations.IsPairPindianRevealSubtree(frames[0]))
        {
            _corePindianRevealWindowsOperations.AssertPindianRevealWindows();
            return;
        }

        if (frames.Length != 1 || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] != frames[0] || _resolutionStackOperations._resolutionStack[^2].Id != frames[0].ParentFrameId || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true, SkillPrompt: not null } decision || decision.Choices.Count == 0)
            throw new InvalidOperationException("Pindian must retain one parent and one private prompt.");
        var frame = frames[0];
        var parentCards = frame.ParentProcessingCardIds ?? [];
        var contestCards = _corePindianPoliciesOperations.CurrentPindianProcessingIds(frame);
        if (_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(card => card.Id).Order().SequenceEqual(parentCards.Concat(contestCards).Order()) == false || parentCards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing))
            throw new InvalidOperationException("Pindian lost its parent card or acquired an unrelated processing card.");
        var paidDouyuClaim = frame.PindianStep == PindianStep.ClaimResult && frame.Result is { } paidResult && (_characterPangJiOlOperations.IsDouyuPairAssignment(frame, paidResult) || _characterLuKangWumiaoOperations.IsWuLuKangAssignedPair(frame, paidResult)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.Result == paidResult) == 1 && (_characterLuKangWumiaoOperations.IsWuLuKangAssignedPair(frame, paidResult) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuResultRecordedEvent>().Count(e => e.ContestId == frame.Id && e.ComparisonIndex == 0 && e.Result == paidResult) == 1);
        if ((!_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat].IsAlive || frame.OpponentSeat is { } opponent && !_coreMatchParticipantsPlayersAccess._players[opponent].IsAlive) && !paidDouyuClaim || frame.OpponentSeat == frame.SourceSeat || !_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsAlive || _coreMatchOutcomeStatusAccess._status != (_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running))
            throw new InvalidOperationException("Pindian prompt owner or participants are inconsistent.");
        if (_corePindianRandomSelectionsOperations.AssertPindianRandomSelection(frame, decision))
            return;
        if (frame.PindianStep == PindianStep.ClaimResult)
        {
            _corePindianMaximumSlashClaimsOperations.Value.AssertPindianPolicyClaims(frame);
            var claimantSeat = frame.PolicyClaims is { } policyClaims ? (int? )policyClaims.Claims[policyClaims.Index].OwnerSeat : frame.ClaimSeats is { } seats && frame.ClaimIndex >= 0 && frame.ClaimIndex < seats.Count ? seats[frame.ClaimIndex] : null;
            if (frame.Result is null || decision.PlayerSeat != claimantSeat || decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") != "pindian-claim" || choice.Cards.Any(id => !contestCards.Contains(id))))
                throw new InvalidOperationException("Pindian claim lost its public card or claimant.");
            return;
        }

        var owner = frame.PindianStep is PindianStep.ChooseParticipants or PindianStep.ChooseSourceCard ? frame.SourceSeat : frame.OpponentSeat!.Value;
        if (decision.PlayerSeat != owner || decision.SkillPrompt.SkillId != frame.SkillId || frame.PindianStep == PindianStep.ChooseSourceCard && frame.SourceCardId is { } selectedSource && (frame.OwnedPileSource is not null || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame sourceProgram || !sourceProgram.SelectedCardIds.SequenceEqual([selectedSource]) || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(card => card.Id == selectedSource) || decision.ValidCardIds.Any(id => id != selectedSource) || decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") != "pindian-top" && !choice.Cards.SequenceEqual([selectedSource]))) || decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-maximum" ? choice.Cards.Count != 0 || !_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[owner]) : choice.Parameters.GetValueOrDefault("action") == "pindian-top" ? choice.Cards.Count != 0 || !_corePindianPoliciesOperations.CanOfferPindianTopChoice(_coreMatchParticipantsPlayersAccess._players[owner]) : choice.Cards.Count != 1 || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(card => card.Id == choice.Cards[0])) || frame.PindianStep == PindianStep.ChooseOpponentCard && !_corePindianPoliciesOperations.HasCurrentPindianSourceCard(frame))
            throw new InvalidOperationException("Pindian selection leaked or lost a private hand card.");
    }
}
