using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiQuanCuixian = ShiHuangQuanComposition.CuixianSkill,
        ShiQuanGushi = ShiHuangQuanComposition.GushiSkill;

    [ProgramCapability]
    private sealed class ShiHuangQuanCapability(GameEngine engine) : IShiHuangQuanHost
    {
        public SkillProgramStepOutcome ResolveCuixian(ProgramSkillFrame frame) => engine.BeginShiQuanCuixian(frame);
        public SkillProgramStepOutcome ResolveGushi(ProgramSkillFrame frame) => engine.BeginShiQuanGushi(frame);
    }

    // ---- 摧先: when one of the owner's physical damage cards finalizes exactly
    // one target, one card from each hand is parked on the target's general as
    // "摧先" cards; if that use later draws blood the owner reclaims the first
    // half of the holder's pile. At a holder's turn start the owner either takes
    // the pile's non-damage cards or returns its damage cards and duels them.

    private bool HasShiQuan => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHqCuixian);

    private bool IsShiQuanDuelUse(long id) => LifecycleCardUse(id) is
        { ShiQuanDuelOrigin: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };

    private IReadOnlyList<int> ShiQuanOrderedPileIds(int holderSeat) => !HasShiQuan || !IsValidPlayerSeat(holderSeat) ? [] :
        ProgramEventHistory<ShiQuanCuixianPlacedEvent>().SelectMany(entry => entry.CardIds)
            .Where(id => _cardZones.GetLocation(id) == ShiHuangQuanComposition.Pile(holderSeat))
            .Distinct().ToArray();

    private bool ShiQuanGainsAvailable(int ownerSeat, ProgramSkillWindowContext? context) => context is { } window &&
        window.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized &&
        window.CardUse is { EffectiveKind: { } kind, DesignatedTargetSeats: [var holder] } use &&
        holder != ownerSeat && IsValidPlayerSeat(holder) && _players[ownerSeat].IsAlive && _players[holder].IsAlive &&
        ShiHuangQuanComposition.IsDamageKind(kind) &&
        GetHand(_players[ownerSeat]).Count > 0 && GetHand(_players[holder]).Count > 0;

    private bool ShiQuanClaimAvailable(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse.ParentCardUseFrameId: { } useId }) return false;
        var placement = ShiQuanPendingPlacement(useId);
        return placement is { } pending && ShiQuanUseDamaged(useId) &&
            ShiQuanOrderedPileIds(pending.HolderSeat).Count > 0 && _players[ownerSeat].IsAlive;
    }

    private ShiQuanCuixianPlacedEvent? ShiQuanPendingPlacement(long useId) =>
        ProgramEventHistory<ShiQuanCuixianPlacedEvent>().LastOrDefault(entry => entry.UseId == useId &&
            !ProgramEventHistory<ShiQuanCuixianClaimedEvent>().Any(claimed =>
                claimed.FrameId == entry.FrameId && claimed.UseId == useId));

    private bool ShiQuanUseDamaged(long useId) =>
        ProgramEventHistory<DamageAppliedEvent>().Any(entry => entry.SourceCardUseResolutionId == useId && entry.Amount > 0);

    private bool ShiQuanTurnAvailable(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } holder } ||
            !IsValidPlayerSeat(holder) || !_players[ownerSeat].IsAlive || !_players[holder].IsAlive ||
            holder == ownerSeat || ShiQuanOrderedPileIds(holder).Count == 0) return false;
        var pile = ShiQuanOrderedPileIds(holder);
        return pile.Any(id => !ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)) ||
            pile.Any(id => ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)) &&
            CanIssueShiQuanDuel(ownerSeat, holder);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68770, SkillProgramEffectOp.ResolveShiHqCuixian)]
    private bool CanOfferShiQuanCuixian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        candidate.SkillId != ShiQuanCuixian || trigger.Id switch
        {
            ShiHuangQuanComposition.PlaceTrigger => ShiQuanGainsAvailable(candidate.OwnerSeat, context),
            ShiHuangQuanComposition.ClaimTrigger => ShiQuanClaimAvailable(candidate.OwnerSeat, context),
            ShiHuangQuanComposition.TurnTrigger => ShiQuanTurnAvailable(candidate.OwnerSeat, context),
            _ => false,
        };

    private SkillProgramStepOutcome BeginShiQuanCuixian(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiQuan is not null)
            throw new InvalidOperationException("Shi Huang Quan restarted a running operation.");
        if (f.SkillId != ShiQuanCuixian || !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        switch (f.TriggerId)
        {
            case ShiHuangQuanComposition.PlaceTrigger:
                if (!ShiQuanGainsAvailable(f.OwnerSeat, f.WindowContext) ||
                    f.WindowContext!.CardUse is not { DesignatedTargetSeats: [var holder], ParentCardUseFrameId: var useId })
                    return SkillProgramStepOutcome.Continue;
                ReplaceRuntimeTop(f = f with { ShiQuan = new("cuixian", ShiQuanStage.PickOwn)
                { HolderSeat = holder, UseId = useId } });
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return SkillProgramStepOutcome.AwaitChoice;
            case ShiHuangQuanComposition.ClaimTrigger:
                return ResolveShiQuanClaim(f);
            case ShiHuangQuanComposition.TurnTrigger:
                return BeginShiQuanTurn(f);
            default:
                return SkillProgramStepOutcome.Continue;
        }
    }

    private SkillProgramStepOutcome ResolveShiQuanClaim(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { CardUse.ParentCardUseFrameId: { } useId } ||
            ShiQuanPendingPlacement(useId) is not { } placement || !ShiQuanUseDamaged(useId))
            return SkillProgramStepOutcome.Continue;
        var ordered = ShiQuanOrderedPileIds(placement.HolderSeat);
        var taken = ordered.Take((ordered.Count + 1) / 2).ToArray();
        if (taken.Length == 0 || !_players[placement.HolderSeat].IsAlive || !_players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        MoveShiQuanCards(taken, ShiHuangQuanComposition.Pile(placement.HolderSeat), CardLocation.Hand(f.OwnerSeat),
            ShiHuangQuanComposition.ClaimReason);
        AdvanceEventRulesAndQueueFact(new ShiQuanCuixianClaimedEvent(f.Id, f.OwnerSeat, placement.HolderSeat, useId,
            Array.AsReadOnly(taken)));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 的“摧先”造成伤害，获得 {_players[placement.HolderSeat].Name} 武将牌上的 {taken.Length} 张“摧先”牌。",
            f.OwnerSeat, placement.HolderSeat);
        if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return SkillProgramStepOutcome.AwaitChild;
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    private SkillProgramStepOutcome BeginShiQuanTurn(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { SourceSeat: { } holder } ||
            !ShiQuanTurnAvailable(f.OwnerSeat, f.WindowContext))
            return SkillProgramStepOutcome.Continue;
        var pile = ShiQuanOrderedPileIds(holder);
        var nonDamage = pile.Where(id => !ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)).ToArray();
        var damage = pile.Where(id => ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)).ToArray();
        var canTake = nonDamage.Length > 0;
        var canGive = damage.Length > 0 && CanIssueShiQuanDuel(f.OwnerSeat, holder);
        var receipt = new ShiQuanReceipt("cuixian", ShiQuanStage.CuixianChoice)
        { HolderSeat = holder, Placed = damage, Chosen = nonDamage };
        if (canTake && canGive)
        {
            ReplaceRuntimeTop(f = f with { ShiQuan = receipt });
            PublishShiQuan(GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChoice;
        }
        ReplaceRuntimeTop(f = f with { ShiQuan = receipt });
        ContinueShiQuanCuixianChoice(GetActiveProgramFrame(f.Id), canTake ? "take-nondamage" : "give-damage");
        return _pendingDecision is null ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    private bool CanIssueShiQuanDuel(int ownerSeat, int holderSeat) =>
        _winner == Winner.None && ownerSeat != holderSeat &&
        _players[ownerSeat].IsAlive && _players[holderSeat].IsAlive &&
        !IsCardUseForbidden(ownerSeat, CardKind.Duel, CardActionType.Use) &&
        TargetRules.AllowsNativeUseMaterial(new(ownerSeat, CardKind.Duel, [])) &&
        !HasTurnCardTargetRestriction(ownerSeat, SkillProgramCardTargetRestriction.SelfOnly) &&
        !IsCardTargetProhibited(_players[holderSeat], CardKind.Duel, Suit.None) &&
        !IsDirectedCardTargetProhibited(ownerSeat, holderSeat, CardKind.Duel);

    // ---- prompts --------------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiQuanChoices(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var Choice = (string branch, string label, IReadOnlyList<int>? cards = null) => new PromptChoice(
            new ChoiceId($"shi-huang-quan.{f.Id}.{branch}"), label, cards ?? [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiHuangQuanComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });
        switch (r.Operation, r.Stage)
        {
            case ("cuixian", ShiQuanStage.PickOwn):
                return GetHand(_players[f.OwnerSeat]).Select(card => Choice($"own-{card.Id}",
                    $"将【{card.DisplayName}】置于其武将牌上", [card.Id])).ToArray();
            case ("cuixian", ShiQuanStage.PickHolder):
            {
                var holder = _players[r.HolderSeat];
                var seen = 0;
                return GetHand(holder).Select(card =>
                {
                    var slot = ++seen;
                    return new PromptChoice(
                        new ChoiceId($"shi-huang-quan.{f.Id}.theirs-{card.Id}"), $"选择其第 {slot} 张手牌", [], [],
                        new Dictionary<string, string>
                        {
                            ["program-action"] = ShiHuangQuanComposition.ProgramAction,
                            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                            ["branch"] = $"theirs-{card.Id}",
                        });
                }).ToArray();
            }
            case ("cuixian", ShiQuanStage.CuixianChoice):
            {
                var choices = new List<PromptChoice>();
                if (r.Chosen.Length > 0)
                    choices.Add(Choice("take-nondamage",
                        $"获得其“摧先”牌中的 {r.Chosen.Length} 张非伤害类卡牌"));
                if (r.Placed.Length > 0)
                    choices.Add(Choice("give-damage",
                        $"令其获得“摧先”牌中的 {r.Placed.Length} 张伤害类卡牌，然后视为对其使用【决斗】"));
                return choices.ToArray();
            }
            case ("gushi", ShiQuanStage.GushiChoice):
            {
                var choices = new List<PromptChoice>();
                if (ShiQuanGushiGainCandidate() is { } gain)
                    choices.Add(Choice("gain", $"获得{(gain.From.Zone == CardZoneKind.DiscardPile ? "弃牌堆" : "牌堆顶")}的一张伤害牌"));
                if (ShiQuanGushiReplaceCandidates(f.OwnerSeat).Length > 0)
                    choices.Add(Choice("replace", "用等量手牌替换其武将牌上的伤害类“摧先”牌"));
                return choices.ToArray();
            }
            case ("gushi", ShiQuanStage.GushiPickHolder):
                return ShiQuanGushiReplaceCandidates(f.OwnerSeat).Select(seat => Choice($"holder-{seat}",
                    $"替换 {_players[seat].Name} 武将牌上的“摧先”牌")).ToArray();
            case ("gushi", ShiQuanStage.GushiPickCards):
            {
                var choices = new List<PromptChoice>();
                foreach (var card in GetHand(_players[f.OwnerSeat]).Where(card => !r.Chosen.Contains(card.Id)))
                    choices.Add(Choice($"pick-{card.Id}", $"选择【{card.DisplayName}】", [card.Id]));
                foreach (var card in GetHand(_players[f.OwnerSeat]).Where(card => r.Chosen.Contains(card.Id)))
                    choices.Add(Choice($"unpick-{card.Id}", $"取消选择【{card.DisplayName}】", [card.Id]));
                choices.Add(Choice("confirm", $"确认替换 {r.Chosen.Length} 张手牌"));
                return choices.ToArray();
            }
            default:
                return [];
        }
    }

    private void PublishShiQuan(ProgramSkillFrame f)
    {
        var choices = ShiQuanChoices(f);
        if (choices.Count == 0 || _winner != Winner.None || !_players[f.OwnerSeat].IsAlive)
        { FinishShiQuanOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            (f.ShiQuan!.Operation, f.ShiQuan.Stage) switch
            {
                ("cuixian", ShiQuanStage.PickOwn) => "【摧先】：请选择你的一张手牌置于其武将牌上。",
                ("cuixian", ShiQuanStage.PickHolder) => "【摧先】：请选择其一张手牌置于其武将牌上。",
                ("cuixian", ShiQuanStage.CuixianChoice) => "【摧先】：请选择一项。",
                ("gushi", ShiQuanStage.GushiChoice) => "【固势】：请选择一项。",
                ("gushi", ShiQuanStage.GushiPickHolder) => "【固势】：请选择一名武将牌上有伤害类“摧先”牌的角色。",
                _ => "【固势】：请选择要替换的手牌，选完后确认。",
            },
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiHuangQuanComposition.ProgramAction)]
    private void AnswerShiQuan(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiQuan: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            pending.PlayerSeat != f.OwnerSeat ||
            !ShiQuanChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Huang Quan choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, r.Stage, branch)
        {
            case ("cuixian", ShiQuanStage.PickOwn, var own) when own.StartsWith("own-", StringComparison.Ordinal):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan = r with
                { Stage = ShiQuanStage.PickHolder, Chosen = [int.Parse(own["own-".Length..], CultureInfo.InvariantCulture)] } });
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return;
            case ("cuixian", ShiQuanStage.PickHolder, var theirs) when theirs.StartsWith("theirs-", StringComparison.Ordinal):
                PlaceShiQuanCards(GetActiveProgramFrame(f.Id),
                    int.Parse(theirs["theirs-".Length..], CultureInfo.InvariantCulture));
                return;
            case ("cuixian", ShiQuanStage.CuixianChoice, "take-nondamage"):
            case ("cuixian", ShiQuanStage.CuixianChoice, "give-damage"):
                ContinueShiQuanCuixianChoice(GetActiveProgramFrame(f.Id), branch);
                return;
            case ("gushi", ShiQuanStage.GushiChoice, "gain"):
                ResolveShiHqGushiGain(GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiChoice, "replace"):
                ContinueShiQuanGushiReplace(GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickHolder, var holder) when holder.StartsWith("holder-", StringComparison.Ordinal):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan = r with
                { HolderSeat = int.Parse(holder["holder-".Length..], CultureInfo.InvariantCulture),
                  Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, var pick) when pick.StartsWith("pick-", StringComparison.Ordinal):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan = r with
                { Chosen = [.. r.Chosen, int.Parse(pick["pick-".Length..], CultureInfo.InvariantCulture)] } });
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, var unpick) when unpick.StartsWith("unpick-", StringComparison.Ordinal):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan = r with
                { Chosen = r.Chosen.Where(id => id != int.Parse(unpick["unpick-".Length..], CultureInfo.InvariantCulture)).ToArray() } });
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return;
            case ("gushi", ShiQuanStage.GushiPickCards, "confirm"):
                ResolveShiHqGushiReplace(GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Huang Quan answered an incompatible branch.");
        }
    }

    private void PlaceShiQuanCards(ProgramSkillFrame f, int theirsId)
    {
        var r = f.ShiQuan!;
        var ownId = r.Chosen.Single();
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        _cardZones.EnsurePrivatePersistentPile(location);
        if (_cardZones.GetLocation(ownId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var ownHolder } || ownHolder != f.OwnerSeat ||
            _cardZones.GetLocation(theirsId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var theirsHolder } || theirsHolder != holder)
            throw new InvalidOperationException("CuiXian placement lost its exact frozen hand entities.");
        ReplaceRuntimeTop(f = f with { ShiQuan = r with { Stage = ShiQuanStage.PlaceChildren, Placed = [ownId, theirsId] } });
        MoveCard(GetAdvancedCard(ownId), CardLocation.Hand(f.OwnerSeat), location,
            new(ShiHuangQuanComposition.PlaceReason), disclosure: CardMovementDisclosure.FaceUp);
        MoveCard(GetAdvancedCard(theirsId), CardLocation.Hand(holder), location,
            new(ShiHuangQuanComposition.PlaceReason), disclosure: CardMovementDisclosure.FaceUp);
        AdvanceEventRulesAndQueueFact(new ShiQuanCuixianPlacedEvent(f.Id, f.OwnerSeat, holder, r.UseId,
            Array.AsReadOnly(new[] { ownId, theirsId })));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 将两张牌置于 {_players[holder].Name} 的武将牌上，称为“摧先”牌。",
            f.OwnerSeat, holder);
        if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return;
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
    }

    private void ContinueShiQuanCuixianChoice(ProgramSkillFrame f, string branch)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        if (!_players[f.OwnerSeat].IsAlive || !_players[holder].IsAlive || _winner != Winner.None ||
            r.Chosen.Concat(r.Placed).Any(id => _cardZones.GetLocation(id) != location))
        { FinishShiQuanOperation(GetActiveProgramFrame(f.Id)); return; }
        if (branch == "take-nondamage")
        {
            var cards = r.Chosen.ToArray();
            MoveShiQuanCards(cards, location, CardLocation.Hand(f.OwnerSeat), ShiHuangQuanComposition.GainReason);
            AdvanceEventRulesAndQueueFact(new ShiQuanCuixianChosenEvent(f.Id, f.OwnerSeat, holder, branch,
                Array.AsReadOnly(cards), null));
            AddLog("SkillTriggered",
                $"{_players[f.OwnerSeat].Name} 获得 {_players[holder].Name} 武将牌上的 {cards.Length} 张非伤害类“摧先”牌。",
                f.OwnerSeat, holder);
            if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return;
            FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
            return;
        }
        MoveShiQuanCards(r.Placed.ToArray(), location, CardLocation.Hand(holder), ShiHuangQuanComposition.GiveReason);
        AddLog("SkillTriggered",
            $"{_players[holder].Name} 获得 {r.Placed.Length} 张伤害类“摧先”牌。",
            f.OwnerSeat, holder);
        IssueShiQuanDuel(GetActiveProgramFrame(f.Id), branch);
    }

    private void IssueShiQuanDuel(ProgramSkillFrame f, string branch)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var useId = _resolutionSequence + 1;
        var origin = new ShiQuanDuelOrigin(f.Id, f.InstructionIndex, useId, f.OwnerSeat, holder,
            f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _turnNumber);
        ReplaceRuntimeTop(f = f with { ShiQuan = r with { Stage = ShiQuanStage.DuelIssued, Duel = origin } });
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var issued = BeginCardUse(card, f.OwnerSeat, [holder], CardKind.Duel, physicalCardIds: [], shiQuanDuelOrigin: origin);
        if (issued != useId)
            throw new InvalidOperationException("Shi Huang Quan lost its reserved zero-entity Duel.");
        AdvanceEventRulesAndQueueFact(new ShiQuanCuixianChosenEvent(f.Id, f.OwnerSeat, holder, branch,
            Array.AsReadOnly(r.Placed), issued));
        BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, [holder], LegalActionKind.Duel,
            playedCardKind: CardKind.Duel);
    }

    [NativeUseReturn(261)]
    private void ReturnShiQuanDuel(CardUseFrame use)
    {
        if (use.ShiQuanDuelOrigin is not { AttackStarted: false } origin) return;
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId ||
            f.ShiQuan is not { Stage: ShiQuanStage.DuelIssued, Duel: { } issued } ||
            (issued with { AttackStarted = origin.AttackStarted }) != origin ||
            use.Id != origin.CardUseFrameId)
            throw new InvalidOperationException("Shi Huang Quan Duel lost its exact no-attack typed return.");
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 61)]
    private bool ReturnShiQuanDuelAttack(AttackCompletionReceipt completion)
    {
        if (completion.ShiQuanDuelReturn is null) return false;
        if (completion.ShiQuanDuelReturn is not { AttackStarted: true } origin ||
            completion.ResolutionId != origin.CardUseFrameId || completion.ProgramFrameId != origin.ParentProgramFrameId ||
            _resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId ||
            f.ShiQuan is not { Stage: ShiQuanStage.DuelIssued, Duel: { } issued } ||
            issued.CardUseFrameId != origin.CardUseFrameId ||
            (issued with { AttackStarted = origin.AttackStarted }) != origin)
            throw new InvalidOperationException("Shi Huang Quan Duel lost its exact completed attack return.");
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
        return true;
    }

    private void MoveShiQuanCards(IReadOnlyList<int> cardIds, CardLocation from, CardLocation to, string reason)
    {
        foreach (var cardId in cardIds)
            MoveCard(GetAdvancedCard(cardId), from, to, new(reason),
                disclosure: to.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    // ---- 固势: once per game per lost hand/equipment/judgment zone the locked
    // skill offers a damage card (discard pile first, then the deck top) that the
    // owner may use without count limits, or an equal-count replacement of a
    // holder's damage "摧先" cards with the owner's own hand cards.

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68780, SkillProgramEffectOp.ResolveShiHqGushi)]
    private bool CanOfferShiQuanGushi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        candidate.SkillId != ShiQuanGushi || ShiQuanLostZone(candidate.OwnerSeat, context) is not null &&
        (ShiQuanGushiGainCandidate() is not null || ShiQuanGushiReplaceCandidates(candidate.OwnerSeat).Length > 0);

    private CardZoneKind? ShiQuanLostZone(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } index } ||
            index < 0 || index >= batch.Movements.Count) return null;
        var from = batch.Movements[index].From;
        if (from.OwnerSeat != ownerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) ||
            _cardZones.CardsAt(new CardLocation(from.Zone, ownerSeat)).Count != 0 ||
            ProgramEventHistory<ShiQuanGushiZoneUsedEvent>().Any(entry => entry.OwnerSeat == ownerSeat && entry.Zone == from.Zone))
            return null;
        return from.Zone;
    }

    private (Card Card, CardLocation From)? ShiQuanGushiGainCandidate()
    {
        var discard = _cardZones.CardsAt(CardLocation.DiscardPile)
            .LastOrDefault(card => ShiHuangQuanComposition.IsDamageKind(card.Kind) && !card.IsGeneralWeapon);
        if (discard is not null) return (discard, CardLocation.DiscardPile);
        var deck = _cardZones.CardsAt(CardLocation.DrawPile)
            .LastOrDefault(card => ShiHuangQuanComposition.IsDamageKind(card.Kind) && !card.IsGeneralWeapon);
        return deck is null ? null : (deck, CardLocation.DrawPile);
    }

    private int[] ShiQuanGushiReplaceCandidates(int ownerSeat)
    {
        var handCount = GetHand(_players[ownerSeat]).Count;
        return _players.Where(player => player.IsAlive &&
                ShiQuanOrderedPileIds(player.Seat).Count(id => ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)) is
                    var needed && needed > 0 && handCount >= needed)
            .Select(player => player.Seat).ToArray();
    }

    private SkillProgramStepOutcome BeginShiQuanGushi(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiQuan is not null)
            throw new InvalidOperationException("Shi Huang Quan restarted a running operation.");
        if (f.SkillId != ShiQuanGushi || f.TriggerId is not (ShiHuangQuanComposition.ZoneHandTrigger or
                ShiHuangQuanComposition.ZoneEquipmentTrigger or ShiHuangQuanComposition.ZoneJudgmentTrigger) ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None ||
            ShiQuanLostZone(f.OwnerSeat, f.WindowContext) is not { } zone)
            return SkillProgramStepOutcome.Continue;
        var singleHolder = ShiQuanGushiReplaceCandidates(f.OwnerSeat);
        var canGain = ShiQuanGushiGainCandidate() is not null;
        if (!canGain && singleHolder.Length == 0)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { ShiQuan = new("gushi", ShiQuanStage.GushiChoice) { Zone = zone } });
        if (canGain && singleHolder.Length == 0)
        { ResolveShiHqGushiGain(GetActiveProgramFrame(f.Id)); return SkillProgramStepOutcome.AwaitChild; }
        if (!canGain && singleHolder.Length == 1)
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan =
                GetActiveProgramFrame(f.Id).ShiQuan! with { HolderSeat = singleHolder[0], Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
            PublishShiQuan(GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChoice;
        }
        PublishShiQuan(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ResolveShiHqGushiGain(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        if (ShiQuanGushiGainCandidate() is not { } gain)
        { FinishShiQuanOperation(GetActiveProgramFrame(f.Id)); return; }
        AdvanceEventRulesAndQueueFact(new ShiQuanGushiZoneUsedEvent(f.Id, f.OwnerSeat, _turnNumber, r.Zone));
        MoveCard(gain.Card, gain.From, CardLocation.Hand(f.OwnerSeat), new(ShiHuangQuanComposition.GushiGainReason),
            disclosure: CardMovementDisclosure.FaceDown);
        AdvanceEventRulesAndQueueFact(new ShiQuanGushiCardClaimedEvent(f.Id, f.OwnerSeat, gain.Card.Id, gain.From.Zone));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 的“固势”发动，获得一张伤害牌（此后其无使用次数限制）。",
            f.OwnerSeat);
        if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return;
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
    }

    private void ContinueShiQuanGushiReplace(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var candidates = ShiQuanGushiReplaceCandidates(f.OwnerSeat);
        if (candidates.Length == 0 || !_players[f.OwnerSeat].IsAlive)
        { FinishShiQuanOperation(GetActiveProgramFrame(f.Id)); return; }
        if (candidates.Length == 1)
            ReplaceRuntimeTop(f = f with { ShiQuan = r with
            { HolderSeat = candidates[0], Stage = ShiQuanStage.GushiPickCards, Chosen = [] } });
        PublishShiQuan(GetActiveProgramFrame(f.Id));
    }

    private void ResolveShiHqGushiReplace(ProgramSkillFrame f)
    {
        var r = f.ShiQuan!;
        var holder = r.HolderSeat;
        var location = ShiHuangQuanComposition.Pile(holder);
        _cardZones.EnsurePrivatePersistentPile(location);
        var damage = ShiQuanOrderedPileIds(holder)
            .Where(id => ShiHuangQuanComposition.IsDamageKind(GetAdvancedCard(id).Kind)).ToArray();
        var given = r.Chosen.ToArray();
        if (damage.Length == 0 || given.Length != damage.Length ||
            given.Any(id => _cardZones.GetLocation(id) is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != f.OwnerSeat) ||
            damage.Any(id => _cardZones.GetLocation(id) != location))
            throw new InvalidOperationException("GuShi replacement lost its exact hand and pile entities.");
        AdvanceEventRulesAndQueueFact(new ShiQuanGushiZoneUsedEvent(f.Id, f.OwnerSeat, _turnNumber, r.Zone));
        foreach (var cardId in given)
            MoveCard(GetAdvancedCard(cardId), CardLocation.Hand(f.OwnerSeat), location,
                new(ShiHuangQuanComposition.GushiReplaceInReason), disclosure: CardMovementDisclosure.FaceUp);
        MoveShiQuanCards(damage, location, CardLocation.Hand(f.OwnerSeat), ShiHuangQuanComposition.GushiReplaceOutReason);
        AdvanceEventRulesAndQueueFact(new ShiQuanGushiReplacedEvent(f.Id, f.OwnerSeat, holder,
            Array.AsReadOnly(given), Array.AsReadOnly(damage)));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 用 {given.Length} 张手牌替换了 {_players[holder].Name} 武将牌上的伤害类“摧先”牌。",
            f.OwnerSeat, holder);
        if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return;
        FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
    }

    private void ContinueShiQuan(ProgramSkillFrame f)
    {
        if (_pendingDecision is not null || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiQuan: { } r } current ||
            current.Id != f.Id) return;
        switch (r.Stage)
        {
            case ShiQuanStage.PickOwn or ShiQuanStage.PickHolder or ShiQuanStage.CuixianChoice or
                ShiQuanStage.GushiChoice or ShiQuanStage.GushiPickHolder or ShiQuanStage.GushiPickCards:
                PublishShiQuan(GetActiveProgramFrame(f.Id));
                return;
            case ShiQuanStage.DuelIssued:
                return;
            case ShiQuanStage.PlaceChildren or ShiQuanStage.GushiChildren:
                if (DrainShiQuan(GetActiveProgramFrame(f.Id))) return;
                FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
                return;
            default:
                FinishShiQuanOperation(GetActiveProgramFrame(f.Id));
                return;
        }
    }

    private void FinishShiQuanOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiQuan = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiQuan(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68770)]
    private bool ResumeShiQuan(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiQuan: { } r } f || f.Id != id) return false;
        if (r.Stage == ShiQuanStage.DuelIssued) return true;
        if (DrainShiQuan(GetActiveProgramFrame(id))) return true;
        ContinueShiQuan(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68770)]
    private bool ReturnShiQuanMovement(ProgramSkillFrame f) => f.ShiQuan is not null && ResumeShiQuan(f.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 68770)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 68770)]
    private bool ContinueShiQuanAfterLoss(ProgramSkillFrame f) => f.ShiQuan is { Stage: ShiQuanStage.DuelIssued };

    // ---- 固势 unlimited-use allowance -----------------------------------------

    private bool ShiQuanGushiUnlimitedCard(CharacterState actor, CardKind kind, IReadOnlyList<int> ids) =>
        HasShiQuan && actor.IsAlive && ShiHuangQuanComposition.IsDamageKind(kind) && ids is [var id] && id > 0 &&
        _cardZones.GetLocation(id) is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && holder == actor.Seat &&
        ProgramEventHistory<ShiQuanGushiCardClaimedEvent>().Any(entry => entry.OwnerSeat == actor.Seat && entry.CardId == id);

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 51940)]
    private bool ShiQuanGushiSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) =>
        ShiQuanGushiUnlimitedCard(query.Actor, query.Kind,
            query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 51941)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 51942)]
    private bool ShiQuanGushiSlashDebitExemption(long useId, int actorSeat) =>
        LifecycleCardUse(useId) is { Action: { Type: CardActionType.Use } action } use &&
        use.SourceSeat == actorSeat && action.ActorSeat == actorSeat &&
        action.PhysicalCards is [var cost] &&
        ShiQuanGushiUnlimitedCard(_players[actorSeat], action.EffectiveKind, [cost.CardId]);

    // ---- holder death cleanup ---------------------------------------------------

    private IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(DeathFrame death)
    {
        if (!HasShiQuan) return [];
        var ids = ShiQuanOrderedPileIds(death.VictimSeat);
        if (ids.Count == 0) return [];
        var location = ShiHuangQuanComposition.Pile(death.VictimSeat);
        MoveShiQuanCards(ids, location, CardLocation.DiscardPile, ShiHuangQuanComposition.DeathReason);
        AddLog("SkillTriggered",
            $"{_players[death.VictimSeat].Name} 阵亡，其武将牌上的 {ids.Count} 张“摧先”牌置入弃牌堆。",
            death.VictimSeat);
        return Array.AsReadOnly(ids.ToArray());
    }

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHqCuixian)]
    private PromptChoice ChooseShiQuanCuixianAi(PendingDecision decision, ProgramSkillFrame f) =>
        f.ShiQuan!.Stage switch
        {
            ShiQuanStage.CuixianChoice => decision.Choices.FirstOrDefault(choice =>
                choice.Parameters.GetValueOrDefault("branch") == "take-nondamage") ?? decision.Choices[0],
            _ => decision.Choices[0],
        };

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHqGushi)]
    private PromptChoice ChooseShiQuanGushiAi(PendingDecision decision, ProgramSkillFrame f) =>
        f.ShiQuan!.Stage switch
        {
            ShiQuanStage.GushiChoice => decision.Choices.FirstOrDefault(choice =>
                choice.Parameters.GetValueOrDefault("branch") == "gain") ?? decision.Choices[0],
            _ => decision.Choices.FirstOrDefault(choice =>
                choice.Parameters.GetValueOrDefault("branch") == "confirm") ?? decision.Choices[0],
        };
}
