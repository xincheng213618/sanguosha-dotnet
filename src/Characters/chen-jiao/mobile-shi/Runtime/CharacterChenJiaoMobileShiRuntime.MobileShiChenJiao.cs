#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenJiaoMobileShiRuntime
{
    internal const string QingYanSkill = ChenJiaoComposition.QingYanSkill, CeDuanSkill = ChenJiaoComposition.CeDuanSkill;
    internal static CardConversionSource ChenJiaoSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IChenJiaoHost.ResolveQingYan(ProgramSkillFrame frame) => BeginChenJiao(frame);
    SkillProgramStepOutcome IChenJiaoHost.ResolveCeDuan(ProgramSkillFrame frame) => BeginChenJiao(frame);
    SkillProgramStepOutcome IChenJiaoHost.SettleCeDuan(ProgramSkillFrame frame) => SettleChenJiaoCeDuan(frame);
    // 清严 arms only while it is not失效 and the hand can still pay the escalating
    // show count X = min(times activated this round + 1, 5).
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68100, SkillProgramEffectOp.ResolveChenJiaoQingYan)]
    internal bool CanStartChenJiaoQingYan(CharacterState owner, string skill)
    {
        if (skill != QingYanSkill)
            return true;
        var ordinal = Math.Min(ChenJiaoQingYanUsesThisRound(owner.Seat) + 1, ChenJiaoComposition.MaximumOrdinal);
        return owner.IsAlive && !ChenJiaoQingYanLocked(owner.Seat) && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count >= ordinal;
    }

    internal SkillProgramStepOutcome BeginChenJiao(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.SkillId switch
        {
            QingYanSkill => StartChenJiaoQingYan(f),
            CeDuanSkill => StartChenJiaoCeDuan(f),
            _ => throw new InvalidOperationException("Chen Jiao instruction has an incompatible owning skill."),
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = r });
        ContinueChenJiao(f.Id);
        return r.Operation switch
        {
            "ceduan" when r.Stage == ChenJiaoStage.Issue => SkillProgramStepOutcome.AwaitChild,
            _ => SkillProgramStepOutcome.AwaitChoice,
        };
    }

    internal ChenJiaoReceipt StartChenJiaoQingYan(ProgramSkillFrame f)
    {
        var ordinal = Math.Min(ChenJiaoQingYanUsesThisRound(f.OwnerSeat) + 1, ChenJiaoComposition.MaximumOrdinal);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || ChenJiaoQingYanLocked(f.OwnerSeat) || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count < ordinal)
            throw new InvalidOperationException("QingYan requires its unlocked owner with enough hand cards.");
        return new("qingyan", _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            Ordinal = ordinal,
            Round = _coreActualRoundProgressRoundNumberAccess._roundNumber
        };
    }

    internal ChenJiaoReceipt StartChenJiaoCeDuan(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.SelectedTargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            throw new InvalidOperationException("CeDuan requires its living owner and one living in-range target.");
        var holders = Enumerable.Range(0, _coreMatchParticipantsPlayersAccess._players.Count).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(target, seat) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanStartedEvent(f.Id, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.OwnerSeat, target, holders.Length));
        return new("ceduan", _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            TargetSeat = target,
            Round = _coreActualRoundProgressRoundNumberAccess._roundNumber,
            Holders = Array.AsReadOnly(holders)
        };
    }

    internal IReadOnlyList<PromptChoice> ChenJiaoChoices(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        switch (r.Operation, r.Stage)
        {
            case ("qingyan", ChenJiaoStage.Show):
                return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(card => !r.ShownIds.Contains(card.Id)).Select(card => new PromptChoice(new($"shi-chen-jiao.{(f.Id)}.show.{(card.Id)}"), $"展示这张手牌（{(DescribeChenJiaoCard(card))}）", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ChenJiaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "show", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("qingyan", ChenJiaoStage.ChooseKind):
                return new[]
                {
                    MakeChenJiaoChoice(f, "kind", "dodge", "视为使用一张【闪】"),
                    MakeChenJiaoChoice(f, "kind", "nullification", "视为使用一张【无懈可击】"),
                };
            case ("ceduan", ChenJiaoStage.Show)when r.Cursor < r.Holders.Count:
                var holder = r.Holders[r.Cursor];
                return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Select(card => new PromptChoice(new($"shi-chen-jiao.{(f.Id)}.show.{(holder)}.{(card.Id)}"), $"展示这张手牌（{(DescribeChenJiaoCard(card))}）", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ChenJiaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "show", ["holder"] = holder.ToString(CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("ceduan", ChenJiaoStage.ChooseColor):
                return new[]
                {
                    MakeChenJiaoChoice(f, "color", "red", "选择红色"),
                    MakeChenJiaoChoice(f, "color", "black", "选择黑色"),
                };
            default:
                return[];
        }
    }

    internal PromptChoice MakeChenJiaoChoice(ProgramSkillFrame f, string branch, string value, string label) => new(new($"shi-chen-jiao.{(f.Id)}.{(branch)}.{(value)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ChenJiaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value, });
    internal static string DescribeChenJiaoCard(Card card) => card.Kind switch
    {
        CardKind.Slash => "杀",
        CardKind.Dodge => "闪",
        CardKind.Peach => "桃",
        _ => $"{(card.Kind)}",
    } + $"{(card.Suit)}{(card.Rank)}";
    internal void PublishChenJiao(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var choices = ChenJiaoChoices(f);
        var chooser = r.Operation switch
        {
            "ceduan" when r.Stage == ChenJiaoStage.Show => r.Holders[r.Cursor],
            _ => f.OwnerSeat,
        };
        if (choices.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive)
        {
            FinishChenJiaoOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Operation switch
        {
            "qingyan" when r.Stage == ChenJiaoStage.Show => $"清严：请选择第 {(r.Cursor + 1)}/{(r.Ordinal)} 张要展示的手牌。",
            "qingyan" => "清严：选择视为使用的牌。",
            _ => "策断：请选择要展示的手牌。",
        }, choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ChenJiaoComposition.ProgramAction)]
    internal void AnswerChenJiao(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ChenJiao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ChenJiaoChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Chen Jiao choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("qingyan", "show")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ResolveChenJiaoQingYanShow(f, cardId);
                return;
            case ("qingyan", "kind"):
                ResolveChenJiaoQingYanKind(f, selected.Parameters["value"]);
                return;
            case ("ceduan", "show")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var showId) && int.TryParse(selected.Parameters["holder"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var holder):
                ResolveChenJiaoCeDuanShow(f, holder, showId);
                return;
            case ("ceduan", "color"):
                ResolveChenJiaoCeDuanColor(f, selected.Parameters["value"]);
                return;
            default:
                throw new InvalidOperationException("Chen Jiao answered an incompatible operation branch.");
        }
    }

    internal void ResolveChenJiaoQingYanShow(ProgramSkillFrame f, int cardId)
    {
        var r = f.ChenJiao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoQingYanShownEvent(f.Id, r.Round, f.OwnerSeat, cardId, r.Ordinal, r.Cursor));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 展示了手牌（清严 {(r.Cursor + 1)}/{(r.Ordinal)}）。", f.OwnerSeat);
        SyncChenJiaoMarker(f.OwnerSeat);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao!with { Cursor = r.Cursor + 1, ShownIds = Array.AsReadOnly(r.ShownIds.Concat([cardId]).ToArray()) } });
        ContinueChenJiao(f.Id);
    }

    internal void ResolveChenJiaoQingYanKind(ProgramSkillFrame f, string value)
    {
        var r = f.ChenJiao!;
        var kind = value switch
        {
            "dodge" => CardKind.Dodge,
            "nullification" => CardKind.Nullification,
            _ => throw new InvalidOperationException("QingYan chose an unsupported virtual card kind."),
        };
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, f.OwnerSeat, f.OwnerSeat, null, null, null, kind, [f.OwnerSeat], [], [ChenJiaoSource(f)], effectiveSuit: Suit.None, effectiveRank: 0));
        // Showing is not payment. Keep the real zero-entity Use on its original
        // skill parent so declared-use observers can inspect the actual action.
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = r with { Stage = ChenJiaoStage.Issue, Kind = kind, UseId = useId } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, f.OwnerSeat, 0, kind, [f.OwnerSeat], PhysicalCardIds: []) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, kind, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, f.OwnerSeat, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoQingYanUsedEvent(f.Id, r.Round, f.OwnerSeat, r.Ordinal, kind, useId));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“清严”视为使用了一张【{((kind == CardKind.Dodge ? "闪" : "无懈可击"))}】。", f.OwnerSeat);
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreNativeCardUseCompletionOperations.FinishCardUse(useId, new Card(0, kind, Suit.None, 0), kind);
    }

    [NativeUsePreparation(68100, 68100, "ReturnChenJiaoQingYanUse")]
    internal bool PrepareChenJiaoQingYanUse(CardUseFrame use)
    {
        if (use.CardId != 0 || use.CardKind is not (CardKind.Dodge or CardKind.Nullification) || use.Action?.ConversionChain is not [var source] || source.SkillId != QingYanSkill)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        return true;
    }

    internal void ReturnChenJiaoQingYanUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ChenJiao: { Operation: "qingyan", Stage: ChenJiaoStage.Issue } receipt } parent || receipt.UseId != use.Id || receipt.Kind != use.CardKind || use.Action?.ConversionChain is not [var source] || source != ChenJiaoSource(parent))
            throw new InvalidOperationException("QingYan cannot return another or already-completed native use.");
        FinishChenJiaoOperation(parent);
    }

    internal void ResolveChenJiaoCeDuanShow(ProgramSkillFrame f, int holder, int cardId)
    {
        var r = f.ChenJiao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanShownEvent(f.Id, r.Round, holder, cardId, r.Cursor));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 展示了一张手牌（策断）。", holder);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao!with { Cursor = r.Cursor + 1, ShownIds = Array.AsReadOnly(r.ShownIds.Concat([cardId]).ToArray()) } });
        ContinueChenJiao(f.Id);
    }

    internal void ResolveChenJiaoCeDuanColor(ProgramSkillFrame f, string value)
    {
        var color = value switch
        {
            "red" => Suit.Heart,
            "black" => Suit.Spade,
            _ => throw new InvalidOperationException("CeDuan chose an unsupported color."),
        };
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao!with { Color = color } });
        CollectChenJiaoCeDuanMaterials(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueChenJiao(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ChenJiao: { } r } f || f.Id != id)
            return;
        if (r.Skip)
        {
            FinishChenJiaoOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        switch (r.Operation, r.Stage)
        {
            case ("qingyan", ChenJiaoStage.Show)when r.Cursor < r.Ordinal:
                PublishChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("qingyan", ChenJiaoStage.Show):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { ChenJiao = r with { Stage = ChenJiaoStage.ChooseKind } });
                PublishChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("qingyan", ChenJiaoStage.ChooseKind):
                PublishChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Show)when r.Cursor < r.Holders.Count:
                PublishChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Show):
                ContinueChenJiaoCeDuanColor(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.ChooseColor):
                PublishChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Issue):
                if (DrainChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
                IssueChenJiaoCeDuanSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            default:
                throw new InvalidOperationException("Chen Jiao has an unknown continuation stage.");
        }
    }

    internal void ContinueChenJiaoCeDuanColor(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var shown = ChenJiaoCeDuanShownIdsThisActivation(f.Id).Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id)).ToArray();
        var red = shown.Count(ChenJiaoIsRed);
        var black = shown.Length - red;
        if (red > 0 && red == black)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = r with { Stage = ChenJiaoStage.ChooseColor } });
            ContinueChenJiao(f.Id);
            return;
        }

        var color = red > black ? Suit.Heart : Suit.Spade;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = r with { Color = color } });
        CollectChenJiaoCeDuanMaterials(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void CollectChenJiaoCeDuanMaterials(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var selected = ChenJiaoCeDuanShownIdsThisActivation(f.Id).Where(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id)is var card && (r.Color == Suit.Heart ? ChenJiaoIsRed(card) : !ChenJiaoIsRed(card)) && _cardZonesOperations._cardZones.GetLocation(id).Zone == CardZoneKind.Hand).ToArray();
        var movable = selected.Where(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != f.OwnerSeat).ToArray();
        var damageBefore = ChenJiaoCeDuanDamageCount(f.OwnerSeat, r.TargetSeat);
        if (movable.Length > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(movable, CardLocation.Hand(f.OwnerSeat), new(ChenJiaoComposition.CeDuanTakeReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao!with { Stage = ChenJiaoStage.Issue, MaterialCount = selected.Length, DamageCountBefore = damageBefore } });
        ContinueChenJiao(f.Id);
    }

    internal bool CanIssueChenJiaoSlash(int actor, int target) => _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actor, target) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], ignoresCount: true);
    internal void IssueChenJiaoCeDuanSlash(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.MaterialCount == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !CanIssueChenJiaoSlash(f.OwnerSeat, r.TargetSeat))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanCompletedEvent(f.Id, 0, false, false));
            FinishChenJiaoOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var targetSeat = r.TargetSeat;
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [], Array.AsReadOnly(new[] { ChenJiaoSource(f) }), effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao!with { UseId = useId } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targetSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanUseIssuedEvent(f.Id, useId, actor.Seat, targetSeat, r.MaterialCount, r.DamageCountBefore));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(actor.Name)} 将 {(r.MaterialCount)} 张展示牌当作一张不计入次数的【杀】对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 使用。", actor.Seat, targetSeat);
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal SkillProgramStepOutcome SettleChenJiaoCeDuan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.ChenJiao ?? throw new InvalidOperationException("CeDuan settle lost its original receipt.");
        var damaged = r.UseId != 0 && ChenJiaoCeDuanDamageCount(f.OwnerSeat, r.TargetSeat) > r.DamageCountBefore;
        var drew = false;
        if (damaged && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(ChenJiaoComposition.CeDuanTakeReason));
            drew = true;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanCompletedEvent(f.Id, r.UseId, damaged, drew));
        FinishChenJiaoOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    internal void FinishChenJiaoOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ChenJiao = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainChenJiao(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68100)]
    internal bool ResumeChenJiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ChenJiao: { } r } f || f.Id != id)
            return false;
        switch (r.Operation, r.Stage)
        {
            case ("ceduan", ChenJiaoStage.Issue)when r.UseId != 0:
                // The child attack completion re-runs the program; its next
                // instruction is the typed settle step.
                return false;
            case ("ceduan", ChenJiaoStage.Issue):
                if (DrainChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
                IssueChenJiaoCeDuanSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainChenJiao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return true;
                ContinueChenJiao(id);
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68100)]
    internal bool ReturnChenJiaoMovement(ProgramSkillFrame f) => f.ChenJiao is not null && ResumeChenJiao(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenJiaoQingYan)]
    internal PromptChoice ChooseChenJiaoQingYanAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.Stage == ChenJiaoStage.ChooseKind)
            return decision.Choices[0];
        return decision.Choices[^1];
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenJiaoCeDuan)]
    internal PromptChoice ChooseChenJiaoCeDuanAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.Stage == ChenJiaoStage.ChooseColor)
            return decision.Choices[0];
        return decision.Choices[0];
    }
}
