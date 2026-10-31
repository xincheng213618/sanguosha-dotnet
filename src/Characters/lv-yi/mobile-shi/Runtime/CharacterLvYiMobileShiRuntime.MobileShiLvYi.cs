#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvYiMobileShiRuntime
{
    internal const string ShiLyHuilv = ShiLvYiComposition.HuilvSkill, ShiLySongyan = ShiLvYiComposition.SongyanSkill, ShiLyShishi = ShiLvYiComposition.ShishiSkill;
    SkillProgramStepOutcome IShiLvYiHost.ResolveHuilv(ProgramSkillFrame frame) => BeginShiLyHuilv(frame);
    SkillProgramStepOutcome IShiLvYiHost.ResolveHuilvSettle(ProgramSkillFrame frame) => BeginShiLyHuilvSettle(frame);
    SkillProgramStepOutcome IShiLvYiHost.ResolveSongyan(ProgramSkillFrame frame) => BeginShiLySongyan(frame);
    internal ShiLyHuilvGrantedEvent? ShiLyOpenGrant(int ownerSeat, int actorSeat)
    {
        var settled = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvSettledEvent>().Select(entry => entry.GrantFrameId).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().FirstOrDefault(entry => entry.OwnerSeat == ownerSeat && entry.ActorSeat == actorSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && !settled.Contains(entry.FrameId));
    }

    internal SkillProgramStepOutcome BeginShiLyHuilv(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLyHuilv || f.TriggerId != ShiLvYiComposition.HuilvStartTrigger || f.WindowContext?.TargetSeat is not { } actor || actor == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) == 0)
            return SkillProgramStepOutcome.Continue;
        var redFree = !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry => entry.OwnerSeat == f.OwnerSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.Red);
        var blackFree = !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry => entry.OwnerSeat == f.OwnerSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && !entry.Red);
        if (!redFree && !blackFree)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvYi = new("huilv", ShiLyStage.HuilvGrant) { ActorSeat = actor } });
        PublishShiLy(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiLyHuilvGrant(ProgramSkillFrame f, bool red)
    {
        var r = f.LvYi!;
        var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit) == red).ToArray();
        if (pool.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive)
        {
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var card = pool[_randomOperations._random.Next(pool.Length)];
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(r.ActorSeat), new CardMoveReason(ShiLvYiComposition.HuilvGainReason), disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvGrantedEvent(f.Id, f.OwnerSeat, r.ActorSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, red, card.Id));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“隳律”令 {(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Name)} 获得了{((red ? "红色" : "黑色"))}牌 {(DescribeShiLyCard(card))}。", f.OwnerSeat, r.ActorSeat);
        FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiLyHuilvSettle(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLyHuilv || f.TriggerId != ShiLvYiComposition.HuilvEndTrigger || f.WindowContext?.TargetSeat is not { } actor || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || ShiLyOpenGrant(f.OwnerSeat, actor)is not { } grant)
            return SkillProgramStepOutcome.Continue;
        var used = _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(movement => movement.CardId == grant.CardId && movement.Reason == CardMoveReasons.Use && movement.From.OwnerSeat == actor);
        var inDiscard = _cardZonesOperations._cardZones.GetLocation(grant.CardId).Zone == CardZoneKind.DiscardPile;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvYi = new("huilv", ShiLyStage.HuilvSettleDamage) { ActorSeat = actor, Red = grant.Red, CardId = grant.CardId, GrantFrameId = grant.FrameId, Used = used, ThenGive = !used && !inDiscard, } });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, grant.FrameId, f.OwnerSeat, actor, "expired", null));
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (used)
            _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), actor, 1, explicitSourceSeat: f.OwnerSeat);
        else if (!inDiscard)
            _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), f.OwnerSeat, 1, explicitSourceSeat: actor);
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, grant.FrameId, f.OwnerSeat, actor, "discarded", null));
        if (!used && inDiscard)
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void CompleteShiLySettleDamage(ProgramSkillFrame f)
    {
        var r = f.LvYi!;
        if (!r.Used && r.ThenGive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && ShiLyGiveTargets(r.ActorSeat).Count > 0 && _cardZonesOperations._cardZones.GetLocation(r.CardId).Zone is not (CardZoneKind.DiscardPile or CardZoneKind.OutsideGame))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { LvYi = r with { Stage = ShiLyStage.HuilvSettleGive } });
            ContinueShiLy(f.Id);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat, r.ActorSeat, r.Used ? "damage" : "kept", null));
        FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IReadOnlyList<int> ShiLyGiveTargets(int holderSeat) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != holderSeat).Select(player => player.Seat).ToArray();
    internal void ApplyShiLyHuilvGive(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.LvYi!;
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(r.CardId)).Single(item => item.Id == r.CardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(r.CardId), CardLocation.Hand(targetSeat), new CardMoveReason(ShiLvYiComposition.HuilvGiveReason), disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat, r.ActorSeat, "given", targetSeat));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 将“隳律”获得的牌交给了 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。", f.OwnerSeat, targetSeat);
        FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiLySongyan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLySongyan || f.TriggerId is not (ShiLvYiComposition.SongyanPrepTrigger or ShiLvYiComposition.SongyanDamageTrigger) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvYi = new("songyan", ShiLyStage.HuilvGrant) });
        PublishShiLy(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiLySongyanUse(ProgramSkillFrame f)
    {
        // The shared damage-trigger cursor forbids nested resolution windows,
        // so a SongYan armed by taking damage settles its draw directly; only
        // the prep-phase use opens the counterable window.
        if (f.WindowContext?.Window == SkillProgramTriggerWindow.AfterDamageApplied)
        {
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, log: true);
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动“耸言”，视为使用【无中生有】。", f.OwnerSeat);
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var card = new Card(0, CardKind.DrawTwo, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [f.OwnerSeat], CardKind.DrawTwo, physicalCardIds: []);
        var actionId = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!.ActionId;
        var outerProcessing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(card => card.Id).ToArray();
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { ShiLySongyanOrigin = new(f.Id, id, actionId, f.OwnerSeat, outerProcessing) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLySongyanUsedEvent(f.Id, f.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, id));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动“耸言”，视为使用【无中生有】。", f.OwnerSeat);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, LegalActionKind.DrawTwo, playedCardKind: CardKind.DrawTwo);
    }

    internal IReadOnlyList<PromptChoice> ShiLyChoices(ProgramSkillFrame f)
    {
        var r = f.LvYi;
        if (r is null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("huilv", ShiLyStage.HuilvGrant):
                var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
                var choices = new List<PromptChoice>();
                if (deck.Any(card => CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry => entry.OwnerSeat == f.OwnerSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.Red))
                    choices.Add(MakeShiLyChoice(f, "red", $"令 {(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Name)} 从牌堆中获得一张红色牌"));
                if (deck.Any(card => !CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry => entry.OwnerSeat == f.OwnerSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && !entry.Red))
                    choices.Add(MakeShiLyChoice(f, "black", $"令 {(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Name)} 从牌堆中获得一张黑色牌"));
                choices.Add(MakeShiLyChoice(f, "pass", "不发动“隳律”"));
                return choices;
            case ("huilv", ShiLyStage.HuilvSettleGive):
                return ShiLyGiveTargets(r.ActorSeat).Select(seat => MakeShiLyChoice(f, $"give-{(seat)}", $"将该牌交给 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}")).Concat([MakeShiLyChoice(f, "keep", "保留该牌")]).ToArray();
            case ("songyan", _):
                return[MakeShiLyChoice(f, "use", "视为使用【无中生有】"), MakeShiLyChoice(f, "pass", "不发动“耸言”"), ];
            default:
                return[];
        }
    }

    internal PromptChoice MakeShiLyChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-lv-yi.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiLvYiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal static string DescribeShiLyCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{(card.Suit)}{(card.Rank)}";
    internal void PublishShiLy(ProgramSkillFrame f)
    {
        var choices = ShiLyChoices(f);
        if (choices.Count == 0)
        {
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.LvYi!.Operation == "huilv" ? "隳律：请选择。" : "耸言：请选择。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiLvYiComposition.ProgramAction)]
    internal void AnswerShiLy(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvYi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiLyChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Lü Yi choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, branch)
        {
            case ("huilv", "red"):
                ApplyShiLyHuilvGrant(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), red: true);
                return;
            case ("huilv", "black"):
                ApplyShiLyHuilvGrant(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), red: false);
                return;
            case ("huilv", "pass"):
                FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("huilv", "keep"):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat, r.ActorSeat, "kept", null));
                FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("huilv", var give)when give.StartsWith("give-", StringComparison.Ordinal) && int.TryParse(give[5..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var giveSeat) && ShiLyGiveTargets(r.ActorSeat).Contains(giveSeat):
                ApplyShiLyHuilvGive(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), giveSeat);
                return;
            case ("songyan", "use"):
                ApplyShiLySongyanUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("songyan", "pass"):
                FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Lü Yi answered an incompatible operation branch.");
        }
    }

    internal void ContinueShiLy(long id) => PublishShiLy(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    internal void FinishShiLyOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { LvYi = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiLy(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68600)]
    internal bool ResumeShiLy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvYi: { } r } f || f.Id != id)
            return false;
        if (r.Operation == "songyan")
        {
            FinishShiLyOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        if (r.Stage == ShiLyStage.HuilvSettleDamage)
        {
            CompleteShiLySettleDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        if (DrainShiLy(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        PublishShiLy(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68600)]
    internal bool ReturnShiLyMovement(ProgramSkillFrame f) => f.LvYi is { Operation: "huilv" } && ResumeShiLy(f.Id);
    // ---- 耸言 conversion gate --------------------------------------------------
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 68610)]
    internal bool ShiLySongyanConversionForbidden(ProgramViewAsLegalityQuery query)
    {
        if (query.Source.SkillId != ShiLySongyan)
            return false;
        if (!query.ForResponse)
            return true;
        if (_resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is not { } window || window.EffectCardKind != CardKind.DrawTwo || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLySongyanUsedEvent>().Any(entry => entry.UseId == window.ParentFrameId) || !query.Owner.IsAlive || query.CardIds is not [var cardId])
            return true;
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (location.OwnerSeat != query.Owner.Seat || location.Zone != CardZoneKind.Hand)
            return true;
        var red = CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(query.Owner, _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId)));
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyRoundDiscardedEvent>().Any(entry => entry.Seat == query.Owner.Seat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.Red == red);
    }

    // ---- observers --------------------------------------------------------------
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68620)]
    internal void ObserveShiLyRoundDiscard(CardMovedEvent moved)
    {
        if (!HasShiLyCapability || moved.To.Zone != CardZoneKind.DiscardPile || moved.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || moved.From.OwnerSeat is not { } holder || !moved.Reason.Value.Contains("discard", StringComparison.Ordinal) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(holder))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyRoundDiscardedEvent(holder, _coreActualRoundProgressRoundNumberAccess._roundNumber, moved.CardId, CoreNativeCardClassificationsRuntime.IsRedSuit(_coreAdvancedLifecycleOperations.GetAdvancedCard(moved.CardId).Suit)));
    }

    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLyHuilv)]
    internal PromptChoice ChooseShiLyHuilvAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "pass") ?? decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLyHuilvSettle)]
    internal PromptChoice ChooseShiLyHuilvSettleAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLySongyan)]
    internal PromptChoice ChooseShiLySongyanAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}
