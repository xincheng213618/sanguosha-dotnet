#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileShiRuntime
{
    internal const string ShiLsHaoshi = "mobile:shi-haoshi", ShiLsDimeng = "mobile:shi-dimeng";
    internal static CardConversionSource ShiLsSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason ShiLsDiscardReason => new("skill.mobile-shi-lu-su.dimeng.discard");

    SkillProgramStepOutcome global::CardGame.Core.IShiLuSuHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiLs(f, op);
    internal IEnumerable<ShiLsHandGrantEvent> UnexpiredShiLsGrants() => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsHandGrantEvent>().Where(g => _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Beneficiary) && g.Source.OwnerSeat != g.Beneficiary && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(ShiLsHaoshi, out var skill) && skill.Program?.GameplayHash == g.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsStartedEvent>().Count(e => e.FrameId == g.GrantId && e.Source == g.Source && e.Hash == g.Hash && e.Turn == g.Turn) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsHandGrantExpiredEvent>().Any(e => e.GrantId == g.GrantId));
    internal IEnumerable<ShiLsHandGrantEvent> LiveShiLsGrants() => UnexpiredShiLsGrants().Where(g => _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[g.Beneficiary].IsAlive);
    internal bool HasShiLsHandView(int actor, int provider) => LiveShiLsGrants().Any(g => g.Source.OwnerSeat == provider && g.Beneficiary == actor);
    internal bool HasShiLsHandGrantsToExpire(int owner) => UnexpiredShiLsGrants().Any(g => g.Source.OwnerSeat == owner);
    internal IReadOnlyList<Card> ShiLsAuthorizedHandCards(CharacterState actor) => LiveShiLsGrants().Where(g => g.Beneficiary == actor.Seat).SelectMany(g => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat])).DistinctBy(c => c.Id).ToArray();
    internal IReadOnlyList<Card> AppendShiLsPlayableCards(CharacterState actor, IReadOnlyList<Card> own) => own.Concat(ShiLsAuthorizedHandCards(actor)).DistinctBy(c => c.Id).ToArray();
    internal Card? FindShiLsPlayableCard(CharacterState actor, int id) => ShiLsAuthorizedHandCards(actor).FirstOrDefault(c => c.Id == id);
    internal bool IsShiLsHandAccess(CharacterState actor, int id) => TryShiLsHandLocation(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), out _);
    internal bool IsShiLsHandAccess(int actor, int id) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && IsShiLsHandAccess(_coreMatchParticipantsPlayersAccess._players[actor], id);
    internal bool TryShiLsHandLocation(CharacterState actor, Card card, out CardLocation from)
    {
        from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        return from.Zone == CardZoneKind.Hand && from.OwnerSeat is { } owner && owner != actor.Seat && HasShiLsHandView(actor.Seat, owner);
    }

    internal void ExpireShiLsHandGrants(int upcomingOwnerSeat, long preludeId)
    {
        foreach (var g in UnexpiredShiLsGrants().Where(g => g.Source.OwnerSeat == upcomingOwnerSeat).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsHandGrantExpiredEvent(g.GrantId, preludeId));
    }

    internal CardActionContext NormalizeShiLsResponseAction(CardActionContext a)
    {
        if (a.Type != CardActionType.Response || !a.PhysicalCards.Any(c => c.From.Zone == CardZoneKind.Hand && c.From.OwnerSeat is { } p && p != a.ActorSeat && HasShiLsHandView(a.ActorSeat, p)))
            return a;
        // Each material keeps its actual source. The primary material determines
        // the scalar provider, just as in CaptureCardUseAction.
        return new(a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.PhysicalCards.First().From.OwnerSeat ?? a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, a.PhysicalCards, a.ConversionChain, a.DesignatedTargetSeats, a.EffectiveSuit, a.EffectiveRank, a.EffectiveIsRed, a.FactionOrigin);
    }

    internal bool IsShiLsAutonomousResponse(CardActionContext a) => a.Type == CardActionType.Response && a.ActorSeat == a.ResponderSeat && a.RequesterSeat is null && a.PhysicalCards.Any(c => c.From.OwnerSeat != a.ActorSeat && IsShiLsAuthorizedResponseMaterial(a, c)) && a.PhysicalCards.All(c => c.From.OwnerSeat == a.ActorSeat || IsShiLsAuthorizedResponseMaterial(a, c));
    internal bool IsShiLsAuthorizedResponseMaterial(CardActionContext action, CardActionCost cost) => cost.From.Zone == CardZoneKind.Hand && cost.From.OwnerSeat is { } provider && (HasShiLsHandView(action.ActorSeat, provider) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Action, action)) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.ActionId == action.ActionId && e.Actor == action.ActorSeat && e.CardId == cost.CardId && e.From == cost.From && e.PaidSequence > 0 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.PaidSequence && m.CardId == cost.CardId && m.From == cost.From && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && (m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification))));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 48900)]
    internal void CaptureShiLsAuthorizedAction(IGameEvent fact)
    {
        var a = fact is CardUseDeclaredEvent declared ? _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)?.Action : fact is CardActionAcceptedEvent accepted ? accepted.Action : null;
        if (a is null || a.Type is not (CardActionType.Use or CardActionType.Response))
            return;
        foreach (var cost in a.PhysicalCards.Where(c => c.From.Zone == CardZoneKind.Hand && c.From.OwnerSeat != a.ActorSeat))
        {
            var grant = LiveShiLsGrants().LastOrDefault(g => g.Beneficiary == a.ActorSeat && g.Source.OwnerSeat == cost.From.OwnerSeat);
            if (grant is null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.ActionId == a.ActionId && e.CardId == cost.CardId))
                continue;
            var payment = a.Type == CardActionType.Response ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.From == cost.From && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && (m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification))?.Sequence ?? 0 : 0;
            if (a.Type == CardActionType.Response && payment == 0)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsMaterialAuthorizedEvent(grant.GrantId, a.ActionId, a.ActorSeat, cost.CardId, cost.From, fact is CardUseDeclaredEvent issued ? issued.ResolutionId : 0, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, payment));
        }
    }

    internal ShiLsHandGrantEvent? ShiLsRefillGrant(int owner, string instance, CardMovementBatchContext? b)
    {
        if (b is null || !b.SourceCounts.Any(c => c.Location == CardLocation.Hand(owner) && c.CountBefore > 0 && c.CountAfter == 0))
            return null;
        return LiveShiLsGrants().LastOrDefault(g => g.Source.OwnerSeat == owner && g.Source.SkillInstanceId == instance && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsRefillIssuedEvent>().Count(e => e.GrantId == g.GrantId) < 2 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsRefillIssuedEvent>().Any(e => e.GrantId == g.GrantId && e.BatchId == b.Id) && b.Movements.Any(m => m.From == CardLocation.Hand(owner) && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.GrantId == g.GrantId && e.CardId == m.CardId && e.From == m.From && (e.PaidSequence == m.Sequence && _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(a => a.Action.ActionId == e.ActionId && a.Action.Type == CardActionType.Response && a.Action.PhysicalCards.Any(c => c.CardId == m.CardId && c.From == m.From)) || e.UseId > 0 && b.ParentFrameId == e.UseId && m.Sequence > e.Before && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(a => a.Id == e.UseId && a.Action?.ActionId == e.ActionId && a.Action.PhysicalCards.Any(c => c.CardId == m.CardId && c.From == m.From))))));
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectShiLsIssuedMovementCandidates(CardMovementBatchContext batch)
    {
        return LiveShiLsGrants().Where(g => !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat]).ProgramInstances.Any(i => i.SkillId == ShiLsHaoshi && i.SkillInstanceId == g.Source.SkillInstanceId) && ShiLsRefillGrant(g.Source.OwnerSeat, g.Source.SkillInstanceId, batch)?.GrantId == g.GrantId).Select(g => new ProgramTriggerCandidate(g.Source.OwnerSeat, ShiLsHaoshi, "shared-last-hand", g.Source.SkillInstanceId, g.Hash, 0)).Distinct().ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 48900)]
    internal bool IssuedShiLsRefill(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == ShiLsHaoshi && c.BindingId == "shared-last-hand" && w.Window == SkillProgramTriggerWindow.CardsMoved && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == c.OwnerSeat && w.TargetSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && parent.Batch.Id == w.MovementBatch?.Id && parent.Batch.Movements.SequenceEqual(w.MovementBatch.Movements) && ShiLsRefillGrant(c.OwnerSeat, c.SkillInstanceId, w.MovementBatch)is { } grant && grant.Hash == c.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 48900)]
    internal bool CanOfferShiLs(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => c.SkillId != ShiLsHaoshi || (t.Id == "share-hand" ? _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat) : ShiLsRefillGrant(c.OwnerSeat, c.SkillInstanceId, w.MovementBatch)is not null);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 48900, SkillProgramEffectOp.ResolveMobileShiLuSuDimeng)]
    internal bool CanActivateShiLs(ProgramActivationLegalityContext c) => _coreMatchParticipantsPlayersAccess._players.Any(a => a.IsAlive && _coreMatchParticipantsPlayersAccess._players.Any(b => b.IsAlive && b.Seat != a.Seat && Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetHand(a).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(b).Count) <= 3));
    internal SkillProgramStepOutcome BeginShiLs(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiLuSu is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Shi Lu Su requires its original unstarted instruction.");
        var r = new ShiLsReceipt(ShiLsSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext?.ParentFrameId, op == SkillProgramEffectOp.ResolveMobileShiLuSuDimeng ? ShiLsStage.Pair : ShiLsStage.Target)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (op == SkillProgramEffectOp.ResolveMobileShiLuSuDimeng && (f.SkillId != ShiLsDimeng || f.WindowContext is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play))
            throw new InvalidOperationException("Dimeng requires the actual owner's Play activation.");
        if (op == SkillProgramEffectOp.ResolveMobileShiLuSuHaoshi && (f.SkillId != ShiLsHaoshi || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding))
            throw new InvalidOperationException("Haoshi requires the owner's original ending boundary.");
        if (op == SkillProgramEffectOp.ResolveMobileShiLuSuHaoshiRefill)
        {
            var g = ShiLsRefillGrant(f.OwnerSeat, f.SkillInstanceId, f.WindowContext?.MovementBatch) ?? throw new InvalidOperationException("Haoshi refill lost its actual authorized last-Hand payment.");
            r = r with
            {
                Stage = ShiLsStage.Drawing,
                GrantId = g.GrantId,
                BatchId = f.WindowContext!.MovementBatch!.Id,
                Target = f.OwnerSeat,
                X = Math.Max(0, 3 - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count)
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.WindowId));
        if (op == SkillProgramEffectOp.ResolveMobileShiLuSuHaoshiRefill)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsRefillIssuedEvent(f.Id, r.GrantId, r.BatchId, r.X));
            if (r.X == 0)
            {
                FinishShiLs(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.X, new("skill.mobile-shi-lu-su.haoshi.refill"));
        }

        PublishShiLs(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShiLsChoices(ProgramSkillFrame f)
    {
        var r = f.ShiLuSu!;
        PromptChoice C(string branch, string text, int? seat = null) => new(new($"shi-lu-su.{(f.Id)}.{(r.Stage)}.{(branch)}.{(seat ?? -1)}"), text, [], seat is { } s ? [s] : [], new Dictionary<string, string> { ["program-action"] = "shi-lu-su", ["branch"] = branch });
        if (r.Stage == ShiLsStage.Target)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => C("share", "授权 " + p.Name + " 使用或打出你的手牌", p.Seat)).ToArray();
        if (r.Stage == ShiLsStage.Pair)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !r.Seats.Contains(p.Seat) && (r.Seats.Count == 0 ? _coreMatchParticipantsPlayersAccess._players.Any(b => b.IsAlive && b.Seat != p.Seat && Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(b).Count) <= 3) : Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Seats[0]]).Count) <= 3)).Select(p => C("pair", "选择 " + p.Name + $"（{(_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count)}张手牌）", p.Seat)).Concat(r.Seats.Count == 1 ? [C("reset", "重新选择角色")] : Array.Empty<PromptChoice>()).ToArray();
        if (r.Stage == ShiLsStage.Benefit)
        {
            var choices = new List<PromptChoice>();
            if (r.Lesser >= 0 && _coreMatchParticipantsPlayersAccess._players[r.Lesser].IsAlive)
                choices.Add(C("draw", _coreMatchParticipantsPlayersAccess._players[r.Lesser].Name + $" 摸{(Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp))}张牌", r.Lesser));
            choices.Add(C("discard", $"你弃置{(Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp))}张牌（不足全弃）"));
            return choices;
        }

        if (r.Stage != ShiLsStage.Discard)
            return[];
        var cards = ShiLsDiscardChoices(f).Where(c => r.Cards.Count < r.Required || r.Cards.Contains(c.Cards.Single())).Select(c => c with { Id = new($"shi-lu-su.{(f.Id)}.discard.{(c.Cards.Single())}." + (r.Cards.Contains(c.Cards.Single()) ? "remove" : "add")), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "shi-lu-su", ["branch"] = r.Cards.Contains(c.Cards.Single()) ? "remove" : "add" } });
        return cards.Concat(r.Cards.Count == r.Required ? [C("pay", $"确认弃置{(r.Required)}张牌")] : Array.Empty<PromptChoice>()).Concat(r.Cards.Count > 0 ? [C("reset", "重新选择弃牌")] : Array.Empty<PromptChoice>()).ToArray();
    }

    internal IReadOnlyList<PromptChoice> ShiLsDiscardChoices(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, new(zone, f.OwnerSeat), OwnedCardMoveIntent.Discard));
    internal void PublishShiLs(ProgramSkillFrame f)
    {
        var choices = ShiLsChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiLs(f);
            return;
        }

        var r = f.ShiLuSu!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage switch
        {
            ShiLsStage.Target => "好施：选择另一名角色，授权持续至你的下个回合开始。",
            ShiLsStage.Pair => $"缔盟：选择两名手牌数之差至多3的角色（已选{(r.Seats.Count)}/2，可以包含自己）。",
            ShiLsStage.Discard => $"缔盟：多选{(r.Required)}张手牌或装备牌后确认（已选{(r.Cards.Count)}/{(r.Required)}）。",
            _ => "缔盟：交换完成，选择摸牌或由你弃牌。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shi-lu-su")]
    internal void AnswerShiLs(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiLuSu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiLsChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shi Lu Su rejects an unpublished participant, material or benefit.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "share")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsHandGrantEvent(f.Id, r.Source, r.Hash, r.Turn, choice.Targets.Single()));
            FinishShiLs(f);
            return;
        }

        if (branch == "reset")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r with { Seats = r.Stage == ShiLsStage.Pair ? [] : r.Seats, Cards = [] } });
            PublishShiLs(f);
            return;
        }

        if (branch == "pair")
        {
            var seats = r.Seats.Append(choice.Targets.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r = r with { Seats = seats } });
            if (seats.Length < 2)
            {
                PublishShiLs(f);
                return;
            }

            var first = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seats[0]]).Count;
            var second = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seats[1]]).Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r with { Stage = ShiLsStage.ExchangeChildren, FirstCount = first, SecondCount = second, Lesser = first > second ? seats[0] : second > first ? seats[1] : -1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsExchangeIssuedEvent(f.Id, seats[0], seats[1], first, second, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            if (_coreProgramHandExchangesOperations.ExchangeProgramSelectedTargetHands(f, seats) == SkillProgramStepOutcome.Continue)
                ContinueShiLs(f.Id);
            return;
        }

        if (branch is "add" or "remove")
        {
            var id = choice.Cards.Single();
            if (branch == "add" && r.Cards.Count >= r.Required)
                throw new InvalidOperationException("Dimeng already selected its whole frozen payment.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r with { Cards = branch == "add" ? r.Cards.Append(id).ToArray() : r.Cards.Where(c => c != id).ToArray() } });
            PublishShiLs(f);
            return;
        }

        if (branch is "draw" or "discard")
        {
            var x = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            var drawing = branch == "draw";
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsBenefitBoundEvent(f.Id, drawing ? r.Lesser : f.OwnerSeat, x, drawing));
            r = r with
            {
                X = x,
                Target = drawing ? r.Lesser : f.OwnerSeat,
                Stage = drawing ? ShiLsStage.Drawing : ShiLsStage.Discard,
                Required = drawing ? 0 : Math.Min(x, ShiLsDiscardChoices(f).Count)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r });
            if (x == 0 || !drawing && r.Required == 0)
            {
                FinishShiLs(f);
                return;
            }

            if (drawing)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, x, new("skill.mobile-shi-lu-su.dimeng.draw"));
            else
                PublishShiLs(f);
            return;
        }

        if (branch != "pay" || r.Cards.Count != r.Required)
            throw new InvalidOperationException("Dimeng requires its exact frozen payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r with { Stage = ShiLsStage.DiscardChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Cards, ShiLsDiscardReason);
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { ShiLuSu = current.ShiLuSu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsDiscardPaidEvent(f.Id, r.Cards.Count, _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShiLuSu!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueShiLs(f.Id);
    }

    internal bool DrainShiLs(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 48900)]
    internal bool ContinueShiLs(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiLuSu: { } r } f || f.Id != id)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || r.Stage == ShiLsStage.Drawing)
            return true;
        if (r.Stage is ShiLsStage.ExchangeChildren or ShiLsStage.DiscardChildren or ShiLsStage.DrawChildren)
        {
            if (DrainShiLs(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShiLuSu!;
            if (r.Stage == ShiLsStage.ExchangeChildren)
            {
                if (!r.Paid)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsExchangeCompletedEvent(id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = r with { Stage = ShiLsStage.Benefit, Paid = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = null });
                PublishShiLs(f);
                return true;
            }

            FinishShiLs(f);
            return true;
        }

        PublishShiLs(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48900)]
    internal bool ReturnShiLsMovement(ProgramSkillFrame f) => f.ShiLuSu is not null && ContinueShiLs(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiLuSuHaoshiRefill, SkillProgramEffectOp.ResolveMobileShiLuSuDimeng)]
    internal void ReturnShiLsDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.ShiLuSu!;
        if (r.Stage != ShiLsStage.Drawing || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.X)
            throw new InvalidOperationException("Shi Lu Su draw lost its original recipient and frozen amount.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiLuSu = r with { Stage = ShiLsStage.DrawChildren, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter) }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueShiLs(f.Id);
    }

    internal void FinishShiLs(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLuSu = f.ShiLuSu!with { Stage = ShiLsStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal PendingDecision? ProjectShiLsCardSelection(PendingDecision decision, ProgramSkillFrame f) => f.ShiLuSu is { Stage: ShiLsStage.Discard } r && decision.PlayerSeat == f.OwnerSeat ? _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(decision, f, "shi-lu-su:discard", "branch", ["add"], ["pay"], r.Cards, r.Required - r.Cards.Count, r.Required - r.Cards.Count, resetValues: ["reset"]) : null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiLuSuHaoshi, SkillProgramEffectOp.ResolveMobileShiLuSuDimeng, SkillProgramEffectOp.ResolveMobileShiLuSuHaoshiRefill)]
    internal PromptChoice ChooseShiLs(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch")is "pay" or "draw") ?? p.Choices.Where(c => c.Parameters.GetValueOrDefault("branch")is not ("reset" or "remove")).OrderBy(c => c.Cards is [var id] ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).First();
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4300)]
    internal void CollectWindow_CollectShiLsIssuedMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectShiLsIssuedMovementCandidates(batch));
}
