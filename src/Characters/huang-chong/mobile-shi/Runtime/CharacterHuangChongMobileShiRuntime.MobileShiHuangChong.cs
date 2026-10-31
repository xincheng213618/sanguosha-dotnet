#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangChongMobileShiRuntime
{
    internal const string ShiHcCujun = ShiHuangChongComposition.CujunSkill, ShiHcSili = ShiHuangChongComposition.SiliSkill;
    internal bool HasShiHcCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHcCujun) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHcSili);

    SkillProgramStepOutcome IShiHuangChongHost.ResolveCujun(ProgramSkillFrame frame) => BeginShiHcCujun(frame);
    SkillProgramStepOutcome IShiHuangChongHost.ResolveSili(ProgramSkillFrame frame) => BeginShiHcSili(frame);
    // ---- 促军: at the owner's discard phase start they may name any other
    // character; every living character inside that target's attack range
    // (excluding the target) simultaneously gives the target one hand card or
    // raises the target's next play-phase Slash limit by one. If the target
    // then holds fewer hand cards than the owner, it claims the damage cards
    // the owner discards from hand during this very discard phase.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68750, SkillProgramEffectOp.ResolveShiHcCujun)]
    internal bool CanOfferShiHcCujun(ProgramTriggerLegalityContext c)
    {
        if (c.Context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting)
            return c.SkillId == ShiHcCujun && c.Owner.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != c.OwnerSeat);
        if (c.Context.Window == SkillProgramTriggerWindow.DiscardPhaseEnded)
            return c.SkillId == ShiHcCujun && c.Owner.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && ShiHcLedger().PendingClaim(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)is not null;
        return false;
    }

    internal SkillProgramStepOutcome BeginShiHcCujun(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.HuangChong is not null)
            throw new InvalidOperationException("Shi Huang Chong restarted a running operation.");
        if (f.SkillId != ShiHcCujun || f.SelectedCardIds is { Count: > 0 } || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        if (f.TriggerId == ShiHuangChongComposition.CujunClaimTrigger)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = new("cujun", ShiHcStage.CujunClaimSettling) });
            SettleShiHcClaim(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HuangChong: not null } settled && settled.Id == f.Id ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
        }

        if (f.TriggerId != ShiHuangChongComposition.CujunDiscardTrigger)
            return SkillProgramStepOutcome.Continue;
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat).Select(player => player.Seat).ToArray();
        if (targets.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = new("cujun", ShiHcStage.CujunTarget) });
        PublishShiHcChoices(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), f.OwnerSeat, targets.Select(seat => MakeShiHcSeatChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "target", seat, $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}")).ToArray(), $"【{(GetShiHcSkillName(ShiHcCujun))}】：选择一名角色。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void AnswerShiHcCujunTarget(ProgramSkillFrame f, ShiHcReceipt r, string branch)
    {
        var target = int.Parse(branch["target-".Length..], CultureInfo.InvariantCulture);
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            throw new InvalidOperationException("Shi Huang Chong named a dead CuJun target.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcCujunStartedEvent(f.Id, f.OwnerSeat, target));
        var choosers = ShiHcRangeSeats(target).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r with { Stage = ShiHcStage.CujunChoosers, TargetSeat = target, Choosers = choosers, Cursor = 0, GiftCards = Enumerable.Repeat(0, choosers.Length).ToArray() } });
        ContinueShiHcChoosers(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IEnumerable<int> ShiHcRangeSeats(int seat)
    {
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(seat);
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != seat && _coreNativeRuleQueriesOperations.GetCombatDistance(seat, player.Seat) <= range).Select(player => player.Seat).ToArray();
    }

    internal void ContinueShiHcChoosers(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        while (r.Answered < r.Choosers.Length)
        {
            var chooser = r.Choosers[r.Answered];
            if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = r with { Answered = r.Answered + 1 } });
                r = f.HuangChong!;
                continue;
            }

            PublishShiHcChoices(f, chooser, ShiHcChooserChoices(f, chooser), $"【{(GetShiHcSkillName(ShiHcCujun))}】：{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 发动促军，请选择一项。");
            return;
        }

        SettleShiHcCujun(f);
    }

    internal IReadOnlyList<PromptChoice> ShiHcChooserChoices(ProgramSkillFrame f, int chooser)
    {
        var r = f.HuangChong!;
        var choices = new List<PromptChoice>();
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[chooser]))
            choices.Add(new PromptChoice(new($"shi-huang-chong.{(f.Id)}.gift-{(r.Answered)}-{(card.Id)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 一张手牌", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ShiHuangChongComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"gift-{(r.Answered)}-{(card.Id)}", }));
        choices.Add(MakeShiHcBranchChoice(f, "slash-plus", $"令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 下个出牌阶段使用【杀】的次数+1"));
        return choices;
    }

    internal void SettleShiHcCujun(ProgramSkillFrame f)
    {
        var r = f.HuangChong!with
        {
            Stage = ShiHcStage.CujunSettling,
            Cursor = 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r });
        while (r.Cursor < r.Choosers.Length)
        {
            var chooser = r.Choosers[r.Cursor];
            var gift = r.GiftCards[r.Cursor];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = r with { Cursor = r.Cursor + 1 } });
            r = f.HuangChong!;
            if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                continue;
            var moved = 0;
            if (gift > 0 && _cardZonesOperations._cardZones.GetLocation(gift)is { Zone: CardZoneKind.Hand, OwnerSeat: var holder } && holder == chooser)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(gift), CardLocation.Hand(chooser), CardLocation.Hand(r.TargetSeat), new CardMoveReason(ShiHuangChongComposition.CujunGiftReason), disclosure: CardMovementDisclosure.FaceUp);
                moved = gift;
            }

            if (moved > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcCujunGiftMovedEvent(f.Id, chooser, r.TargetSeat, moved, _coreCardMovementJournalCardMovementsAccess._cardMovements.Count > 0 ? _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence : 0));
            else
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcCujunSlashGrantedEvent(f.Id, chooser, r.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
            if (DrainShiHc(f.Id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.HuangChong!;
        }

        if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && ShiHcHandCount(r.TargetSeat) < ShiHcHandCount(f.OwnerSeat))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcCujunClaimIssuedEvent(f.Id, f.OwnerSeat, r.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
            if (DrainShiHc(f.Id))
                return;
        }

        FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal int ShiHcHandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    internal void SettleShiHcClaim(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        var claim = ShiHcLedger().PendingClaim(f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber) ?? throw new InvalidOperationException("Shi Huang Chong lost its pending CuJun claim.");
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r with { TargetSeat = claim.TargetSeat } });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.HuangChong!;
        var discardPile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile);
        var damageIds = discardPile.Where(card => ShiHuangChongComposition.IsDamageKind(card.Kind)).Select(card => card.Id).ToHashSet();
        var poolIds = _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess._discardPhaseHandDiscardIds.Where(damageIds.Contains).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcCujunClaimSettledEvent(claim.FrameId, f.Id, f.OwnerSeat, claim.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, Array.AsReadOnly(poolIds.ToArray())));
        foreach (var cardId in poolIds)
        {
            if (_cardZonesOperations._cardZones.GetLocation(cardId)is not { Zone: CardZoneKind.DiscardPile })
                continue;
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), CardLocation.DiscardPile, CardLocation.Hand(claim.TargetSeat), new CardMoveReason(ShiHuangChongComposition.CujunClaimReason), disclosure: CardMovementDisclosure.FaceUp);
            if (DrainShiHc(f.Id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    // ---- 死励: once per turn after the owner's first health change they may
    // take a Slash from the discard pile (or the draw pile topmost Slash) that
    // cannot be answered while they use it, or raise their next draw phase by
    // one card. Either choice beishui-arms one lost HP at the start of their
    // next turn.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68760, SkillProgramEffectOp.ResolveShiHcSili)]
    internal bool CanOfferShiHcSili(ProgramTriggerLegalityContext c)
    {
        if (c.SkillId != ShiHcSili || !c.Owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        return c.Context.Window switch
        {
            SkillProgramTriggerWindow.AfterHealthChanged => true,
            SkillProgramTriggerWindow.DrawPhaseStarting => ShiHcLedger().PendingDrawGrants(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber).Count > 0,
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => ShiHcLedger().PendingBeishui(c.OwnerSeat).Count > 0,
            _ => false,
        };
    }

    internal SkillProgramStepOutcome BeginShiHcSili(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.HuangChong is not null)
            throw new InvalidOperationException("Shi Huang Chong restarted a running operation.");
        if (f.SkillId != ShiHcSili || f.SelectedCardIds is { Count: > 0 } || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        switch (f.WindowContext?.Window)
        {
            case SkillProgramTriggerWindow.DrawPhaseStarting:
                return SettleShiHcSiliDraw(f);
            case SkillProgramTriggerWindow.TurnStartBeforeNormalFlow:
                return SettleShiHcBeishui(f);
            case SkillProgramTriggerWindow.AfterHealthChanged:
                return PublishShiHcSiliBranch(f);
            default:
                return SkillProgramStepOutcome.Continue;
        }
    }

    internal SkillProgramStepOutcome PublishShiHcSiliBranch(ProgramSkillFrame f)
    {
        var choices = new List<PromptChoice>();
        if (FindShiHcSlashToGain()is { } slash)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = new("sili", ShiHcStage.SiliBranch) { SlashCardId = slash.Id } });
            choices.Add(MakeShiHcBranchChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "gain-slash", "从牌堆或弃牌堆中获得一张【杀】，且你使用此【杀】不可被响应"));
        }
        else
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = new("sili", ShiHcStage.SiliBranch) });
        choices.Add(MakeShiHcBranchChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "draw-plus", "下个摸牌阶段摸牌数+1"));
        PublishShiHcChoices(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), f.OwnerSeat, choices.ToArray(), $"【{(GetShiHcSkillName(ShiHcSili))}】：选择一项。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card? FindShiHcSlashToGain()
    {
        var discardSlash = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).LastOrDefault(card => card.Kind == CardKind.Slash);
        if (discardSlash is not null)
            return discardSlash;
        return _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(card => card.Kind == CardKind.Slash);
    }

    internal SkillProgramStepOutcome SettleShiHcSiliDraw(ProgramSkillFrame f)
    {
        var grants = ShiHcLedger().PendingDrawGrants(f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
        if (grants.Count == 0)
            throw new InvalidOperationException("Shi Huang Chong lost its armed draw grant.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = new("sili", ShiHcStage.SiliDrawSettling) });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliDrawConsumedEvent(f.Id, grant.FrameId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        _coreProgramLifecycleOperations.AdjustProgramNormalDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), grants.Count);
        if (DrainShiHc(f.Id))
            return SkillProgramStepOutcome.AwaitChild;
        FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome SettleShiHcBeishui(ProgramSkillFrame f)
    {
        var armed = ShiHcLedger().PendingBeishui(f.OwnerSeat);
        if (armed.Count == 0)
            throw new InvalidOperationException("Shi Huang Chong lost its armed beishui cost.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangChong = new("sili", ShiHcStage.SiliBeishuiSettling) });
        foreach (var entry in armed)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliBeishuiConsumedEvent(f.Id, entry.FrameId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, armed.Count);
        if (DrainShiHc(f.Id))
            return SkillProgramStepOutcome.AwaitChild;
        if (outcome == SkillProgramStepOutcome.AwaitChild)
            return SkillProgramStepOutcome.AwaitChild;
        FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    internal void AnswerShiHcSiliBranch(ProgramSkillFrame f, ShiHcReceipt r, string branch)
    {
        switch (branch)
        {
            case "gain-slash":
                var slash = r.SlashCardId > 0 ? FindShiHcSlashToGain() : null;
                if (slash is null || slash.Id != r.SlashCardId)
                    throw new InvalidOperationException("Shi Huang Chong lost its offered Slash.");
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r with { SlashCardId = 0 } });
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(slash.Id), _cardZonesOperations._cardZones.GetLocation(slash.Id), CardLocation.Hand(f.OwnerSeat), new CardMoveReason(ShiHuangChongComposition.SiliGainReason), disclosure: _cardZonesOperations._cardZones.GetLocation(slash.Id).Zone == CardZoneKind.DrawPile ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliSlashArmedEvent(f.Id, f.OwnerSeat, slash.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliInvokedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, "slash"));
                break;
            case "draw-plus":
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliDrawGrantedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == f.OwnerSeat));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliInvokedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, "draw"));
                break;
            default:
                throw new InvalidOperationException("Shi Huang Chong answered an incompatible SiLi branch.");
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHcSiliBeishuiArmedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        if (DrainShiHc(f.Id))
            return;
        FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void FinishShiHcOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiHc(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal NativeOwners.ShiHcLedgerView ShiHcLedger()
    {
        if (!HasShiHcCapability)
            return NativeOwners.ShiHcLedgerView.Empty;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var head = history.FirstOrDefault();
        var tail = history.LastOrDefault();
        if (_characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache is not null && _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheCount == history.Length && ReferenceEquals(_characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheHead, head) && ReferenceEquals(_characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheTail, tail))
            return _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache;
        var grants = new List<GameEngine.ShiHcLedgerGrant>();
        var playStarts = new List<GameEngine.ShiHcLedgerPlayStart>();
        var armedSlashes = new List<ShiHcSiliSlashArmedEvent>();
        var drawGrants = new List<ShiHcSiliDrawGrantedEvent>();
        var consumedDraws = new List<long>();
        var beishuiArmed = new List<ShiHcSiliBeishuiArmedEvent>();
        var consumedBeishui = new List<long>();
        var claims = new List<ShiHcCujunClaimIssuedEvent>();
        var settledClaims = new List<long>();
        for (var index = 0; index < history.Length; index++)
        {
            switch (history[index])
            {
                case ShiHcCujunSlashGrantedEvent grant:
                    grants.Add(new(grant, index));
                    break;
                case PhaseChangedEvent { Phase: TurnPhase.Play } play:
                    playStarts.Add(new(play.ActorSeat, index));
                    break;
                case ShiHcSiliSlashArmedEvent armed:
                    armedSlashes.Add(armed);
                    break;
                case ShiHcSiliDrawGrantedEvent draw:
                    drawGrants.Add(draw);
                    break;
                case ShiHcSiliDrawConsumedEvent drawConsumed:
                    consumedDraws.Add(drawConsumed.GrantedFrameId);
                    break;
                case ShiHcSiliBeishuiArmedEvent beishui:
                    beishuiArmed.Add(beishui);
                    break;
                case ShiHcSiliBeishuiConsumedEvent beishuiConsumed:
                    consumedBeishui.Add(beishuiConsumed.ArmedFrameId);
                    break;
                case ShiHcCujunClaimIssuedEvent claim:
                    claims.Add(claim);
                    break;
                case ShiHcCujunClaimSettledEvent settled:
                    settledClaims.Add(settled.ClaimFrameId);
                    break;
            }
        }

        _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache = new NativeOwners.ShiHcLedgerView(Array.AsReadOnly(grants.ToArray()), Array.AsReadOnly(playStarts.ToArray()), Array.AsReadOnly(armedSlashes.ToArray()), Array.AsReadOnly(drawGrants.ToArray()), Array.AsReadOnly(consumedDraws.ToArray()), Array.AsReadOnly(beishuiArmed.ToArray()), Array.AsReadOnly(consumedBeishui.ToArray()), Array.AsReadOnly(claims.ToArray()), Array.AsReadOnly(settledClaims.ToArray()));
        _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheCount = history.Length;
        _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheHead = head;
        _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheTail = tail;
        return _characterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache;
    }

    internal int ShiHcCujunSlashBonus(int seat)
    {
        if (!HasShiHcCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != seat)
            return 0;
        var view = ShiHcLedger();
        return view.CujunGrants.Count(grant => grant.Grant.TargetSeat == seat && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > grant.Grant.GrantedPhaseInstanceId && view.PlayStarts.Count(play => play.Seat == seat && play.Index > grant.Index) == 1);
    }

    internal bool IsShiHcSiliSlashUnrespondable(long frameId) => HasShiHcCapability && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { Action: { Type: CardActionType.Use } action, SourceSeat: var seat } && CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && action.PhysicalCards.Count == 1 && ShiHcLedger().IsSlashArmed(seat, action.PhysicalCards[0].CardId);
    internal string GetShiHcSkillName(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Name;
    internal PromptChoice MakeShiHcSeatChoice(ProgramSkillFrame f, string prefix, int seat, string label) => new(new($"shi-huang-chong.{(f.Id)}.{(prefix)}-{(seat)}"), label, [], [seat], new Dictionary<string, string> { ["program-action"] = ShiHuangChongComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"{(prefix)}-{(seat)}", });
    internal PromptChoice MakeShiHcBranchChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-huang-chong.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiHuangChongComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal void PublishShiHcChoices(ProgramSkillFrame f, int answeringSeat, IReadOnlyList<PromptChoice> choices, string prompt)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, answeringSeat, prompt, choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = answeringSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[answeringSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal IReadOnlyList<PromptChoice> ShiHcCurrentChoices(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        return (r.Operation, r.Stage) switch
        {
            ("cujun", ShiHcStage.CujunTarget) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat).Select(player => MakeShiHcSeatChoice(f, "target", player.Seat, $"选择 {(_coreMatchParticipantsPlayersAccess._players[player.Seat].Name)}")).ToArray(),
            ("cujun", ShiHcStage.CujunChoosers)when r.Answered < r.Choosers.Length => ShiHcChooserChoices(f, r.Choosers[r.Answered]),
            ("sili", ShiHcStage.SiliBranch) => ShiHcSiliBranchChoices(f, r),
            _ => [],
        };
    }

    internal IReadOnlyList<PromptChoice> ShiHcSiliBranchChoices(ProgramSkillFrame f, ShiHcReceipt r)
    {
        var choices = new List<PromptChoice>();
        if (r.SlashCardId > 0 && FindShiHcSlashToGain()is { } slash && slash.Id == r.SlashCardId)
            choices.Add(MakeShiHcBranchChoice(f, "gain-slash", "从牌堆或弃牌堆中获得一张【杀】，且你使用此【杀】不可被响应"));
        choices.Add(MakeShiHcBranchChoice(f, "draw-plus", "下个摸牌阶段摸牌数+1"));
        return choices;
    }

    [ProgramAnswer(ShiHuangChongComposition.ProgramAction)]
    internal void AnswerShiHc(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangChong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiHcCurrentChoices(f).Any(choice => choice.Id == selected.Id && choice.Cards.SequenceEqual(selected.Cards) && choice.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Shi Huang Chong choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch.StartsWith("target-", StringComparison.Ordinal))
        {
            AnswerShiHcCujunTarget(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r, branch);
            return;
        }

        if (r.Operation == "cujun" && r.Stage == ShiHcStage.CujunChoosers && branch.StartsWith("gift-", StringComparison.Ordinal))
        {
            var separator = branch.IndexOf('-', "gift-".Length);
            var cardId = int.Parse(branch[(separator + 1)..], CultureInfo.InvariantCulture);
            var gifts = r.GiftCards.ToArray();
            gifts[r.Answered] = cardId;
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r with { Answered = r.Answered + 1, GiftCards = gifts } });
            ContinueShiHcChoosers(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.Operation == "cujun" && r.Stage == ShiHcStage.CujunChoosers && branch == "slash-plus")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HuangChong = r with { Answered = r.Answered + 1 } });
            ContinueShiHcChoosers(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.Operation == "sili" && r.Stage == ShiHcStage.SiliBranch)
        {
            AnswerShiHcSiliBranch(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r, branch);
            return;
        }

        throw new InvalidOperationException("Shi Huang Chong answered an incompatible stage.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 68750)]
    internal bool ResumeShiHcCujun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangChong: { } r } f || f.Id != id || r.Operation != "cujun")
            return false;
        switch (r.Stage)
        {
            case ShiHcStage.CujunSettling:
                SettleShiHcCujun(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ShiHcStage.CujunClaimSettling:
                SettleShiHcClaim(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ShiHcStage.CujunChoosers when r.Answered >= r.Choosers.Length:
                SettleShiHcCujun(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiHc(id))
                    return true;
                ContinueShiHcPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 68760)]
    internal bool ResumeShiHcSili(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangChong: { } r } f || f.Id != id || r.Operation != "sili")
            return false;
        switch (r.Stage)
        {
            case ShiHcStage.SiliDrawSettling or ShiHcStage.SiliBeishuiSettling:
                if (DrainShiHc(id))
                    return true;
                FinishShiHcOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiHc(id))
                    return true;
                ContinueShiHcPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68750)]
    internal bool ReturnShiHcMovement(ProgramSkillFrame f) => f.HuangChong is { } r && (r.Operation != "cujun" ? ResumeShiHcSili(f.Id) : ResumeShiHcCujun(f.Id));
    internal void ContinueShiHcPrompt(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        switch (r.Operation, r.Stage)
        {
            case ("cujun", ShiHcStage.CujunTarget):
                PublishShiHcChoices(f, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat).Select(player => MakeShiHcSeatChoice(f, "target", player.Seat, $"选择 {(_coreMatchParticipantsPlayersAccess._players[player.Seat].Name)}")).ToArray(), $"【{(GetShiHcSkillName(ShiHcCujun))}】：选择一名角色。");
                return;
            case ("cujun", ShiHcStage.CujunChoosers)when r.Answered < r.Choosers.Length:
                PublishShiHcChoices(f, r.Choosers[r.Answered], ShiHcChooserChoices(f, r.Choosers[r.Answered]), $"【{(GetShiHcSkillName(ShiHcCujun))}】：{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 发动促军，请选择一项。");
                return;
            case ("sili", ShiHcStage.SiliBranch):
                PublishShiHcChoices(f, f.OwnerSeat, ShiHcSiliBranchChoices(f, r), $"【{(GetShiHcSkillName(ShiHcSili))}】：选择一项。");
                return;
            default:
                FinishShiHcOperation(f);
                return;
        }
    }

    // ---- AI ----------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHcCujun)]
    internal PromptChoice ChooseShiHcCujunAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        if (r.Stage == ShiHcStage.CujunChoosers)
        {
            // Choosers deterministically raise the target's Slash quota instead
            // of parting with a hand card.
            var slashPlus = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "slash-plus");
            if (slashPlus is not null)
                return slashPlus;
        }

        return decision.Choices[0];
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHcSili)]
    internal PromptChoice ChooseShiHcSiliAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var gain = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "gain-slash");
        if (gain is not null)
            return gain;
        return decision.Choices[0];
    }
}
