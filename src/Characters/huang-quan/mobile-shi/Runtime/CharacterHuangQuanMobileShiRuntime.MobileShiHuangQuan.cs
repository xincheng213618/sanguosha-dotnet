#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMobileShiRuntime
{
    internal const string ShiQuanCuixian = ShiHuangQuanComposition.CuixianSkill, ShiQuanGushi = ShiHuangQuanComposition.GushiSkill;
    SkillProgramStepOutcome IShiHuangQuanHost.ResolveCuixian(ProgramSkillFrame frame) => BeginShiQuanCuixian(frame);
    SkillProgramStepOutcome IShiHuangQuanHost.ResolveGushi(ProgramSkillFrame frame) => BeginShiQuanGushi(frame);
    internal bool HasShiQuan => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHqCuixian);

    internal bool IsShiQuanDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ShiQuanDuelOrigin: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal IReadOnlyList<int> ShiQuanOrderedPileIds(int holderSeat) => !HasShiQuan || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(holderSeat) ? [] : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiQuanCuixianPlacedEvent>().SelectMany(entry => entry.CardIds).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == ShiHuangQuanComposition.Pile(holderSeat)).Distinct().ToArray();
    internal bool ShiQuanGainsAvailable(int ownerSeat, ProgramSkillWindowContext? context) => context is { } window && window.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && window.CardUse is { EffectiveKind: { } kind, DesignatedTargetSeats: [var holder] } use && holder != ownerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive && ShiHuangQuanComposition.IsDamageKind(kind) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Count > 0;
    internal bool ShiQuanClaimAvailable(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse.ParentCardUseFrameId: { } useId })
            return false;
        var placement = ShiQuanPendingPlacement(useId);
        return placement is { } pending && ShiQuanUseDamaged(useId) && ShiQuanOrderedPileIds(pending.HolderSeat).Count > 0 && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive;
    }

    internal ShiQuanCuixianPlacedEvent? ShiQuanPendingPlacement(long useId) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiQuanCuixianPlacedEvent>().LastOrDefault(entry => entry.UseId == useId && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiQuanCuixianClaimedEvent>().Any(claimed => claimed.FrameId == entry.FrameId && claimed.UseId == useId));
    internal bool ShiQuanUseDamaged(long useId) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Any(entry => entry.SourceCardUseResolutionId == useId && entry.Amount > 0);
    internal bool ShiQuanTurnAvailable(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } holder } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive || holder == ownerSeat || ShiQuanOrderedPileIds(holder).Count == 0)
            return false;
        var pile = ShiQuanOrderedPileIds(holder);
        return pile.Any(id => !ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind)) || pile.Any(id => ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind)) && CanIssueShiQuanDuel(ownerSeat, holder);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68770, SkillProgramEffectOp.ResolveShiHqCuixian)]
    internal bool CanOfferShiQuanCuixian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId != ShiQuanCuixian || trigger.Id switch
    {
        ShiHuangQuanComposition.PlaceTrigger => ShiQuanGainsAvailable(candidate.OwnerSeat, context),
        ShiHuangQuanComposition.ClaimTrigger => ShiQuanClaimAvailable(candidate.OwnerSeat, context),
        ShiHuangQuanComposition.TurnTrigger => ShiQuanTurnAvailable(candidate.OwnerSeat, context),
        _ => false,
    };
    internal SkillProgramStepOutcome BeginShiQuanCuixian(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiQuan is not null)
            throw new InvalidOperationException("Shi Huang Quan restarted a running operation.");
        if (f.SkillId != ShiQuanCuixian || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        switch (f.TriggerId)
        {
            case ShiHuangQuanComposition.PlaceTrigger:
                if (!ShiQuanGainsAvailable(f.OwnerSeat, f.WindowContext) || f.WindowContext!.CardUse is not { DesignatedTargetSeats: [var holder], ParentCardUseFrameId: var useId })
                    return SkillProgramStepOutcome.Continue;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = new("cuixian", ShiQuanStage.PickOwn) { HolderSeat = holder, UseId = useId } });
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return SkillProgramStepOutcome.AwaitChoice;
            case ShiHuangQuanComposition.ClaimTrigger:
                return ResolveShiQuanClaim(f);
            case ShiHuangQuanComposition.TurnTrigger:
                return BeginShiQuanTurn(f);
            default:
                return SkillProgramStepOutcome.Continue;
        }
    }

    internal SkillProgramStepOutcome ResolveShiQuanClaim(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { CardUse.ParentCardUseFrameId: { } useId } || ShiQuanPendingPlacement(useId)is not { } placement || !ShiQuanUseDamaged(useId))
            return SkillProgramStepOutcome.Continue;
        var ordered = ShiQuanOrderedPileIds(placement.HolderSeat);
        var taken = ordered.Take((ordered.Count + 1) / 2).ToArray();
        if (taken.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[placement.HolderSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        MoveShiQuanCards(taken, ShiHuangQuanComposition.Pile(placement.HolderSeat), CardLocation.Hand(f.OwnerSeat), ShiHuangQuanComposition.ClaimReason);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanCuixianClaimedEvent(f.Id, f.OwnerSeat, placement.HolderSeat, useId, Array.AsReadOnly(taken)));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“摧先”造成伤害，获得 {(_coreMatchParticipantsPlayersAccess._players[placement.HolderSeat].Name)} 武将牌上的 {(taken.Length)} 张“摧先”牌。", f.OwnerSeat, placement.HolderSeat);
        if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return SkillProgramStepOutcome.AwaitChild;
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome BeginShiQuanTurn(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { SourceSeat: { } holder } || !ShiQuanTurnAvailable(f.OwnerSeat, f.WindowContext))
            return SkillProgramStepOutcome.Continue;
        var pile = ShiQuanOrderedPileIds(holder);
        var nonDamage = pile.Where(id => !ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind)).ToArray();
        var damage = pile.Where(id => ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind)).ToArray();
        var canTake = nonDamage.Length > 0;
        var canGive = damage.Length > 0 && CanIssueShiQuanDuel(f.OwnerSeat, holder);
        var receipt = new ShiQuanReceipt("cuixian", ShiQuanStage.CuixianChoice)
        {
            HolderSeat = holder,
            Placed = damage,
            Chosen = nonDamage
        };
        if (canTake && canGive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = receipt });
            PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = receipt });
        ContinueShiQuanCuixianChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), canTake ? "take-nondamage" : "give-damage");
        return _corePendingDecisionPolicyPipelineOperations._pendingDecision is null ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool CanIssueShiQuanDuel(int ownerSeat, int holderSeat) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && ownerSeat != holderSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[holderSeat].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, CardKind.Duel, CardActionType.Use) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(ownerSeat, CardKind.Duel, [])) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(ownerSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[holderSeat], CardKind.Duel, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, holderSeat, CardKind.Duel);
    internal IReadOnlyList<PromptChoice> ShiQuanChoices(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var Choice = (string branch, string label, IReadOnlyList<int>? cards = null) => new PromptChoice(new ChoiceId($"shi-huang-quan.{(f.Id)}.{(branch)}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = ShiHuangQuanComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
        switch (r.Operation, r.Stage)
        {
            case ("cuixian", ShiQuanStage.PickOwn):
                return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => Choice($"own-{(card.Id)}", $"将【{(card.DisplayName)}】置于其武将牌上", [card.Id])).ToArray();
            case ("cuixian", ShiQuanStage.PickHolder):
            {
                var holder = _coreMatchParticipantsPlayersAccess._players[r.HolderSeat];
                var seen = 0;
                return _corePhysicalOwnedCardQueriesOperations.GetHand(holder).Select(card =>
                {
                    var slot = ++seen;
                    return new PromptChoice(new ChoiceId($"shi-huang-quan.{(f.Id)}.theirs-{(card.Id)}"), $"选择其第 {(slot)} 张手牌", [], [], new Dictionary<string, string> { ["program-action"] = ShiHuangQuanComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"theirs-{(card.Id)}", });
                }).ToArray();
            }

            case ("cuixian", ShiQuanStage.CuixianChoice):
            {
                var choices = new List<PromptChoice>();
                if (r.Chosen.Length > 0)
                    choices.Add(Choice("take-nondamage", $"获得其“摧先”牌中的 {(r.Chosen.Length)} 张非伤害类卡牌"));
                if (r.Placed.Length > 0)
                    choices.Add(Choice("give-damage", $"令其获得“摧先”牌中的 {(r.Placed.Length)} 张伤害类卡牌，然后视为对其使用【决斗】"));
                return choices.ToArray();
            }

            case ("gushi", ShiQuanStage.GushiChoice):
            {
                var choices = new List<PromptChoice>();
                if (ShiQuanGushiGainCandidate()is { } gain)
                    choices.Add(Choice("gain", $"获得{((gain.From.Zone == CardZoneKind.DiscardPile ? "弃牌堆" : "牌堆顶"))}的一张伤害牌"));
                if (ShiQuanGushiReplaceCandidates(f.OwnerSeat).Length > 0)
                    choices.Add(Choice("replace", "用等量手牌替换其武将牌上的伤害类“摧先”牌"));
                return choices.ToArray();
            }

            case ("gushi", ShiQuanStage.GushiPickHolder):
                return ShiQuanGushiReplaceCandidates(f.OwnerSeat).Select(seat => Choice($"holder-{(seat)}", $"替换 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 武将牌上的“摧先”牌")).ToArray();
            case ("gushi", ShiQuanStage.GushiPickCards):
            {
                var choices = new List<PromptChoice>();
                foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(card => !r.Chosen.Contains(card.Id)))
                    choices.Add(Choice($"pick-{(card.Id)}", $"选择【{(card.DisplayName)}】", [card.Id]));
                foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(card => r.Chosen.Contains(card.Id)))
                    choices.Add(Choice($"unpick-{(card.Id)}", $"取消选择【{(card.DisplayName)}】", [card.Id]));
                choices.Add(Choice("confirm", $"确认替换 {(r.Chosen.Length)} 张手牌"));
                return choices.ToArray();
            }

            default:
                return[];
        }
    }

    internal void PublishShiQuan(ProgramSkillFrame f)
    {
        var choices = ShiQuanChoices(f);
        if (choices.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, (f.ShiQuan!.Operation, f.ShiQuan.Stage) switch
        {
            ("cuixian", ShiQuanStage.PickOwn) => "【摧先】：请选择你的一张手牌置于其武将牌上。",
            ("cuixian", ShiQuanStage.PickHolder) => "【摧先】：请选择其一张手牌置于其武将牌上。",
            ("cuixian", ShiQuanStage.CuixianChoice) => "【摧先】：请选择一项。",
            ("gushi", ShiQuanStage.GushiChoice) => "【固势】：请选择一项。",
            ("gushi", ShiQuanStage.GushiPickHolder) => "【固势】：请选择一名武将牌上有伤害类“摧先”牌的角色。",
            _ => "【固势】：请选择要替换的手牌，选完后确认。",
        }, choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiHuangQuanComposition.ProgramAction)]
    internal void AnswerShiQuan(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiQuan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || pending.PlayerSeat != f.OwnerSeat || !ShiQuanChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Huang Quan choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, r.Stage, branch)
        {
            case ("cuixian", ShiQuanStage.PickOwn, var own)when own.StartsWith("own-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = r with { Stage = ShiQuanStage.PickHolder, Chosen = [int.Parse(own["own-".Length..], CultureInfo.InvariantCulture)] } });
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("cuixian", ShiQuanStage.PickHolder, var theirs)when theirs.StartsWith("theirs-", StringComparison.Ordinal):
                PlaceShiQuanCards(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), int.Parse(theirs["theirs-".Length..], CultureInfo.InvariantCulture));
                return;
            case ("cuixian", ShiQuanStage.CuixianChoice, "take-nondamage"):
            case ("cuixian", ShiQuanStage.CuixianChoice, "give-damage"):
                ContinueShiQuanCuixianChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), branch);
                return;
            case ("gushi", ShiQuanStage.GushiChoice, "gain"):
                ResolveShiHqGushiGain(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiChoice, "replace"):
                ContinueShiQuanGushiReplace(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickHolder, var holder)when holder.StartsWith("holder-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = r with { HolderSeat = int.Parse(holder["holder-".Length..], CultureInfo.InvariantCulture), Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, var pick)when pick.StartsWith("pick-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = r with { Chosen = [..r.Chosen, int.Parse(pick["pick-".Length..], CultureInfo.InvariantCulture)] } });
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, var unpick)when unpick.StartsWith("unpick-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = r with { Chosen = r.Chosen.Where(id => id != int.Parse(unpick["unpick-".Length..], CultureInfo.InvariantCulture)).ToArray() } });
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, "confirm"):
                ResolveShiHqGushiReplace(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Huang Quan answered an incompatible branch.");
        }
    }

    internal void PlaceShiQuanCards(ProgramSkillFrame f, int theirsId)
    {
        var r = f.ShiQuan!;
        var ownId = r.Chosen.Single();
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        if (_cardZonesOperations._cardZones.GetLocation(ownId)is not { Zone: CardZoneKind.Hand, OwnerSeat: var ownHolder } || ownHolder != f.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(theirsId)is not { Zone: CardZoneKind.Hand, OwnerSeat: var theirsHolder } || theirsHolder != holder)
            throw new InvalidOperationException("CuiXian placement lost its exact frozen hand entities.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = r with { Stage = ShiQuanStage.PlaceChildren, Placed = [ownId, theirsId] } });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(ownId), CardLocation.Hand(f.OwnerSeat), location, new(ShiHuangQuanComposition.PlaceReason), disclosure: CardMovementDisclosure.FaceUp);
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(theirsId), CardLocation.Hand(holder), location, new(ShiHuangQuanComposition.PlaceReason), disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanCuixianPlacedEvent(f.Id, f.OwnerSeat, holder, r.UseId, Array.AsReadOnly(new[] { ownId, theirsId })));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 将两张牌置于 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 的武将牌上，称为“摧先”牌。", f.OwnerSeat, holder);
        if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return;
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiQuanCuixianChoice(ProgramSkillFrame f, string branch)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Chosen.Concat(r.Placed).Any(id => _cardZonesOperations._cardZones.GetLocation(id) != location))
        {
            FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (branch == "take-nondamage")
        {
            var cards = r.Chosen.ToArray();
            MoveShiQuanCards(cards, location, CardLocation.Hand(f.OwnerSeat), ShiHuangQuanComposition.GainReason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanCuixianChosenEvent(f.Id, f.OwnerSeat, holder, branch, Array.AsReadOnly(cards), null));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 获得 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 武将牌上的 {(cards.Length)} 张非伤害类“摧先”牌。", f.OwnerSeat, holder);
            if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                return;
            FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        MoveShiQuanCards(r.Placed.ToArray(), location, CardLocation.Hand(holder), ShiHuangQuanComposition.GiveReason);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 获得 {(r.Placed.Length)} 张伤害类“摧先”牌。", f.OwnerSeat, holder);
        IssueShiQuanDuel(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), branch);
    }

    internal void IssueShiQuanDuel(ProgramSkillFrame f, string branch)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var useId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var origin = new ShiQuanDuelOrigin(f.Id, f.InstructionIndex, useId, f.OwnerSeat, holder, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = r with { Stage = ShiQuanStage.DuelIssued, Duel = origin } });
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var issued = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [holder], CardKind.Duel, physicalCardIds: [], shiQuanDuelOrigin: origin);
        if (issued != useId)
            throw new InvalidOperationException("Shi Huang Quan lost its reserved zero-entity Duel.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanCuixianChosenEvent(f.Id, f.OwnerSeat, holder, branch, Array.AsReadOnly(r.Placed), issued));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, [holder], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    [NativeUseReturn(261)]
    internal void ReturnShiQuanDuel(CardUseFrame use)
    {
        if (use.ShiQuanDuelOrigin is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId || f.ShiQuan is not { Stage: ShiQuanStage.DuelIssued, Duel: { } issued } || (issued with
        {
            AttackStarted = origin.AttackStarted
        }

        ) != origin || use.Id != origin.CardUseFrameId)
            throw new InvalidOperationException("Shi Huang Quan Duel lost its exact no-attack typed return.");
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 61)]
    internal bool ReturnShiQuanDuelAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ShiQuanDuelReturn is null)
            return false;
        if (completion.ShiQuanDuelReturn is not { AttackStarted: true } origin || completion.ResolutionId != origin.CardUseFrameId || completion.ProgramFrameId != origin.ParentProgramFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId || f.ShiQuan is not { Stage: ShiQuanStage.DuelIssued, Duel: { } issued } || issued.CardUseFrameId != origin.CardUseFrameId || (issued with
        {
            AttackStarted = origin.AttackStarted
        }

        ) != origin)
            throw new InvalidOperationException("Shi Huang Quan Duel lost its exact completed attack return.");
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return true;
    }

    internal void MoveShiQuanCards(IReadOnlyList<int> cardIds, CardLocation from, CardLocation to, string reason)
    {
        foreach (var cardId in cardIds)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), from, to, new(reason), disclosure: to.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    // ---- 固势: once per game per lost hand/equipment/judgment zone the locked
    // skill offers a damage card (discard pile first, then the deck top) that the
    // owner may use without count limits, or an equal-count replacement of a
    // holder's damage "摧先" cards with the owner's own hand cards.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68780, SkillProgramEffectOp.ResolveShiHqGushi)]
    internal bool CanOfferShiQuanGushi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId != ShiQuanGushi || ShiQuanLostZone(candidate.OwnerSeat, context)is not null && (ShiQuanGushiGainCandidate()is not null || ShiQuanGushiReplaceCandidates(candidate.OwnerSeat).Length > 0);
    internal CardZoneKind? ShiQuanLostZone(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            return null;
        var from = batch.Movements[index].From;
        if (from.OwnerSeat != ownerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || _cardZonesOperations._cardZones.CardsAt(new CardLocation(from.Zone, ownerSeat)).Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiQuanGushiZoneUsedEvent>().Any(entry => entry.OwnerSeat == ownerSeat && entry.Zone == from.Zone))
            return null;
        return from.Zone;
    }

    internal (Card Card, CardLocation From)? ShiQuanGushiGainCandidate()
    {
        var discard = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).LastOrDefault(card => ShiHuangQuanComposition.IsDamageKind(card.Kind) && !card.IsGeneralWeapon);
        if (discard is not null)
            return (discard, CardLocation.DiscardPile);
        var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(card => ShiHuangQuanComposition.IsDamageKind(card.Kind) && !card.IsGeneralWeapon);
        return deck is null ? null : (deck, CardLocation.DrawPile);
    }

    internal int[] ShiQuanGushiReplaceCandidates(int ownerSeat)
    {
        var handCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count;
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && ShiQuanOrderedPileIds(player.Seat).Count(id => ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind))is var needed && needed > 0 && handCount >= needed).Select(player => player.Seat).ToArray();
    }

    internal SkillProgramStepOutcome BeginShiQuanGushi(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiQuan is not null)
            throw new InvalidOperationException("Shi Huang Quan restarted a running operation.");
        if (f.SkillId != ShiQuanGushi || f.TriggerId is not (ShiHuangQuanComposition.ZoneHandTrigger or ShiHuangQuanComposition.ZoneEquipmentTrigger or ShiHuangQuanComposition.ZoneJudgmentTrigger) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || ShiQuanLostZone(f.OwnerSeat, f.WindowContext)is not { } zone)
            return SkillProgramStepOutcome.Continue;
        var singleHolder = ShiQuanGushiReplaceCandidates(f.OwnerSeat);
        var canGain = ShiQuanGushiGainCandidate()is not null;
        if (!canGain && singleHolder.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = new("gushi", ShiQuanStage.GushiChoice) { Zone = zone } });
        if (canGain && singleHolder.Length == 0)
        {
            ResolveShiHqGushiGain(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!canGain && singleHolder.Length == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShiQuan!with { HolderSeat = singleHolder[0], Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
            PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveShiHqGushiGain(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        if (ShiQuanGushiGainCandidate()is not { } gain)
        {
            FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanGushiZoneUsedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.Zone));
        _corePhysicalMovementFacadesOperations.MoveCard(gain.Card, gain.From, CardLocation.Hand(f.OwnerSeat), new(ShiHuangQuanComposition.GushiGainReason), disclosure: CardMovementDisclosure.FaceDown);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanGushiCardClaimedEvent(f.Id, f.OwnerSeat, gain.Card.Id, gain.From.Zone));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“固势”发动，获得一张伤害牌（此后其无使用次数限制）。", f.OwnerSeat);
        if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return;
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiQuanGushiReplace(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var candidates = ShiQuanGushiReplaceCandidates(f.OwnerSeat);
        if (candidates.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (candidates.Length == 1)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiQuan = r with { HolderSeat = candidates[0], Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
        PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ResolveShiHqGushiReplace(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        var damage = ShiQuanOrderedPileIds(holder).Where(id => ShiHuangQuanComposition.IsDamageKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind)).ToArray();
        var given = r.Chosen.ToArray();
        if (damage.Length == 0 || given.Length != damage.Length || given.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != f.OwnerSeat) || damage.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != location))
            throw new InvalidOperationException("GuShi replacement lost its exact hand and pile entities.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanGushiZoneUsedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.Zone));
        foreach (var cardId in given)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), CardLocation.Hand(f.OwnerSeat), location, new(ShiHuangQuanComposition.GushiReplaceInReason), disclosure: CardMovementDisclosure.FaceUp);
        MoveShiQuanCards(damage, location, CardLocation.Hand(f.OwnerSeat), ShiHuangQuanComposition.GushiReplaceOutReason);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiQuanGushiReplacedEvent(f.Id, f.OwnerSeat, holder, Array.AsReadOnly(given), Array.AsReadOnly(damage)));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 用 {(given.Length)} 张手牌替换了 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 武将牌上的伤害类“摧先”牌。", f.OwnerSeat, holder);
        if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return;
        FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiQuan(ProgramSkillFrame f)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiQuan: { } r } current || current.Id != f.Id)
            return;
        switch (r.Stage)
        {
            case ShiQuanStage.PickOwn or ShiQuanStage.PickHolder or ShiQuanStage.CuixianChoice or ShiQuanStage.GushiChoice or ShiQuanStage.GushiPickHolder or ShiQuanStage.GushiPickCards:
                PublishShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ShiQuanStage.DuelIssued:
                return;
            case ShiQuanStage.PlaceChildren or ShiQuanStage.GushiChildren:
                if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
                FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            default:
                FinishShiQuanOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
        }
    }

    internal void FinishShiQuanOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiQuan = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiQuan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68770)]
    internal bool ResumeShiQuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiQuan: { } r } f || f.Id != id)
            return false;
        if (r.Stage == ShiQuanStage.DuelIssued)
            return true;
        if (DrainShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        ContinueShiQuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68770)]
    internal bool ReturnShiQuanMovement(ProgramSkillFrame f) => f.ShiQuan is not null && ResumeShiQuan(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 68770)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 68770)]
    internal bool ContinueShiQuanAfterLoss(ProgramSkillFrame f) => f.ShiQuan is { Stage: ShiQuanStage.DuelIssued };
    internal bool ShiQuanGushiUnlimitedCard(CharacterState actor, CardKind kind, IReadOnlyList<int> ids) => HasShiQuan && actor.IsAlive && ShiHuangQuanComposition.IsDamageKind(kind) && ids is [var id] && id > 0 && _cardZonesOperations._cardZones.GetLocation(id)is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && holder == actor.Seat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiQuanGushiCardClaimedEvent>().Any(entry => entry.OwnerSeat == actor.Seat && entry.CardId == id);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 51940)]
    internal bool ShiQuanGushiSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => ShiQuanGushiUnlimitedCard(query.Actor, query.Kind, query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 51941)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 51942)]
    internal bool ShiQuanGushiSlashDebitExemption(long useId, int actorSeat) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { Type: CardActionType.Use } action } use && use.SourceSeat == actorSeat && action.ActorSeat == actorSeat && action.PhysicalCards is [var cost] && ShiQuanGushiUnlimitedCard(_coreMatchParticipantsPlayersAccess._players[actorSeat], action.EffectiveKind, [cost.CardId]);
    internal IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(DeathFrame death)
    {
        if (!HasShiQuan)
            return[];
        var ids = ShiQuanOrderedPileIds(death.VictimSeat);
        if (ids.Count == 0)
            return[];
        var location = ShiHuangQuanComposition.Pile(death.VictimSeat);
        MoveShiQuanCards(ids, location, CardLocation.DiscardPile, ShiHuangQuanComposition.DeathReason);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[death.VictimSeat].Name)} 阵亡，其武将牌上的 {(ids.Count)} 张“摧先”牌置入弃牌堆。", death.VictimSeat);
        return Array.AsReadOnly(ids.ToArray());
    }

    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHqCuixian)]
    internal PromptChoice ChooseShiQuanCuixianAi(PendingDecision decision, ProgramSkillFrame f) => f.ShiQuan!.Stage switch
    {
        ShiQuanStage.CuixianChoice => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "take-nondamage") ?? decision.Choices[0],
        _ => decision.Choices[0],
    };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHqGushi)]
    internal PromptChoice ChooseShiQuanGushiAi(PendingDecision decision, ProgramSkillFrame f) => f.ShiQuan!.Stage switch
    {
        ShiQuanStage.GushiChoice => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "gain") ?? decision.Choices[0],
        _ => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "confirm") ?? decision.Choices[0],
    };
}
