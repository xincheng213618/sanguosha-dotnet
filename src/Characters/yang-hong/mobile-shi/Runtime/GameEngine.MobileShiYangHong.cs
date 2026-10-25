using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiYhJijian = ShiYangHongComposition.JijianSkill,
        ShiYhYuanmo = ShiYangHongComposition.YuanmoSkill;

    [ProgramCapability]
    private sealed class ShiYangHongCapability(GameEngine engine) : IShiYangHongHost
    {
        public SkillProgramStepOutcome ResolveJijian(ProgramSkillFrame frame) => engine.BeginShiYhJijian(frame);
        public SkillProgramStepOutcome ResolveYuanmo(ProgramSkillFrame frame) => engine.BeginShiYhYuanmo(frame);
    }

    // ---- 间计: once per play phase the owner secretly holds one hand card and
    // sends two other characters to a Pindian; each may secretly swap the hidden
    // card in as their Pindian card, the winner slashes the loser without
    // distance limit, and a swapped-in Slash has the owner retaliate its users.

    private SkillProgramStepOutcome BeginShiYhJijian(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.YangHong is not null)
            throw new InvalidOperationException("Shi Yang Hong restarted a running operation.");
        if (f.SkillId != ShiYhJijian || f.TriggerId is not null || f.SelectedCardIds is not [var hidden] ||
            f.SelectedTargetSeats is not [var first, var second] || first == second ||
            !_players[f.OwnerSeat].IsAlive || !_players[first].IsAlive || !_players[second].IsAlive ||
            _winner != Winner.None || GetHand(_players[first]).Count == 0 || GetHand(_players[second]).Count == 0 ||
            _cardZones.GetLocation(hidden) is not { Zone: CardZoneKind.Hand, OwnerSeat: var holder } ||
            holder != f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { YangHong = new("jijian", ShiYhStage.SwapFirst)
        { CardId = hidden, FirstSeat = first, SecondSeat = second } });
        PublishShiYh(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void LaunchShiYhContest(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        var skill = _contentRegistry.GetSkill(f.SkillId);
        AdvanceEventRulesAndQueueFact(new ShiYhJijianStartedEvent(f.Id, f.OwnerSeat, r.CardId,
            r.FirstSeat, r.SecondSeat, r.FirstSwap, r.SecondSwap));
        if (r.FirstSwap && r.SecondSwap)
        {
            // Both chose the same hidden card, so the ranks tie and nobody wins.
            AdvanceEventRulesAndQueueFact(new ShiYhJijianResolvedEvent(f.Id, f.OwnerSeat, "tied", null, null));
            FinishShiYhOperation(GetActiveProgramFrame(f.Id));
            return;
        }
        if (r.FirstSwap || r.SecondSwap)
        {
            var swapper = r.FirstSwap ? r.FirstSeat : r.SecondSeat;
            var other = r.FirstSwap ? r.SecondSeat : r.FirstSeat;
            if (swapper != f.OwnerSeat)
                MoveCard(GetAdvancedCard(r.CardId), CardLocation.Hand(f.OwnerSeat), CardLocation.Hand(swapper),
                    new CardMoveReason(ShiYangHongComposition.JijianLendReason));
            BeginSharedPindian(f.Id, new SkillPromptPresentation(f.SkillId, skill.Name,
                $"{skill.Name} · 拼点", skill.Description), swapper, other,
                sourceCardId: r.CardId, programResultBind: ShiYangHongComposition.PindianBind,
                programResultVisibility: SkillProgramCardSetVisibility.Public);
            return;
        }
        BeginSharedPindian(f.Id, new SkillPromptPresentation(f.SkillId, skill.Name,
            $"{skill.Name} · 拼点", skill.Description), r.FirstSeat, r.SecondSeat,
            programResultBind: ShiYangHongComposition.PindianBind,
            programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    private void ContinueShiYhContest(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        var binding = f.PindianResultBindings.Single(item => item.Name == ShiYangHongComposition.PindianBind);
        int? winner = binding.SourceRank > binding.OpponentRank ? binding.SourceSeat :
            binding.OpponentRank > binding.SourceRank ? binding.OpponentSeat : null;
        int? loser = winner == binding.SourceSeat ? binding.OpponentSeat :
            winner == binding.OpponentSeat ? binding.SourceSeat : null;
        AdvanceEventRulesAndQueueFact(new ShiYhJijianResolvedEvent(f.Id, f.OwnerSeat,
            winner is null ? "tied" : "slash", winner, loser));
        if (winner is not { } winnerSeat || loser is not { } loserSeat)
        {
            BeginShiYhRetaliation(GetActiveProgramFrame(f.Id));
            return;
        }
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { Stage = ShiYhStage.Slash } });
        BeginShiYhSlash(GetActiveProgramFrame(f.Id), winnerSeat, loserSeat);
    }

    private void BeginShiYhSlash(ProgramSkillFrame f, int winnerSeat, int loserSeat)
    {
        var id = BeginCardUse(new Card(0, CardKind.Slash, Suit.None, 0), winnerSeat, [loserSeat],
            CardKind.Slash, physicalCardIds: []);
        var targets = LifecycleCardUse(id)!.TargetSeats;
        var attack = new CardAttackHandle(this, id, winnerSeat, targets[0], card: null,
            damageAmount: 1 + _players[winnerSeat].AlcoholDamageBonus, playedCardKind: CardKind.Slash,
            programSkillCardUseFrameId: f.Id);
        ActiveCardAttack = attack;
        CaptureProgramAdjustedSlashBaseDamage(attack);
        AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, winnerSeat, targets[0]));
        NotifyAiOfSlash(_players[winnerSeat], _players[targets[0]]);
        if (!TryMarkProgramUseCommitted(id) || !TryBeginProgramCardWindow(attack, LifecycleCardUse(id)!.Action!,
                SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            BeginSlashTargetResolution(attack);
    }

    private void BeginShiYhRetaliation(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        if (GetAdvancedCard(r.CardId).Kind != CardKind.Slash)
        { FinishShiYhOperation(GetActiveProgramFrame(f.Id)); return; }
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { Stage = ShiYhStage.Retaliate } });
        AdvanceShiYhRetaliation(GetActiveProgramFrame(f.Id));
    }

    private void AdvanceShiYhRetaliation(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        if (r.FirstSwap && _players[r.FirstSeat].IsAlive)
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { FirstSwap = false } });
            BeginProgramSkillDamage(GetActiveProgramFrame(f.Id), r.FirstSeat, 1, explicitSourceSeat: f.OwnerSeat);
            return;
        }
        if (r.SecondSwap && _players[r.SecondSeat].IsAlive)
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { SecondSwap = false } });
            BeginProgramSkillDamage(GetActiveProgramFrame(f.Id), r.SecondSeat, 1, explicitSourceSeat: f.OwnerSeat);
            return;
        }
        FinishShiYhOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- 远谟: twice per turn, at prep or after taking damage, move one card
    // between field areas; then optionally have the giver draw once for every
    // character that left the giver's attack range because of the move.

    private SkillProgramStepOutcome BeginShiYhYuanmo(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.YangHong is not null)
            throw new InvalidOperationException("Shi Yang Hong restarted a running operation.");
        if (f.SkillId != ShiYhYuanmo ||
            f.TriggerId is not (ShiYangHongComposition.YuanmoPrepTrigger or ShiYangHongComposition.YuanmoDamageTrigger) ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None ||
            ProgramEventHistory<ShiYhYuanmoUsedEvent>().Count(entry =>
                entry.OwnerSeat == f.OwnerSeat && entry.Turn == _turnNumber) >= 2 ||
            !ShiYhFieldMovesExist())
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { YangHong = new("yuanmo", ShiYhStage.MoveChoice) });
        PublishShiYh(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private bool ShiYhFieldMovesExist()
    {
        foreach (var _ in ShiYhFieldMoveChoices(null)) return true;
        return false;
    }

    private IEnumerable<PromptChoice> ShiYhFieldMoveChoices(ProgramSkillFrame? f)
    {
        foreach (var source in _players.Where(player => player.IsAlive))
        foreach (var zone in new[] { CardZoneKind.Equipment, CardZoneKind.Judgment })
        foreach (var card in _cardZones.CardsAt(new CardLocation(zone, source.Seat)))
        foreach (var target in _players.Where(player => player.IsAlive && player.Seat != source.Seat))
        {
            var legal = zone == CardZoneKind.Equipment
                ? !card.IsGeneralWeapon && CanEnterEquipmentSlot(target.Seat, card)
                : CanMoveProgramCardToCorrespondingZone(card, zone, target.Seat);
            if (!legal) continue;
            if (f is null)
            {
                yield return new PromptChoice(new("shi-yang-hong.field-probe"), "移动场上牌", [card.Id], [target.Seat],
                    new Dictionary<string, string>());
                yield break;
            }
            yield return MakeShiYhChoice(f, $"move-{card.Id}-{target.Seat}",
                $"将 {source.Name}{(zone == CardZoneKind.Equipment ? "装备区" : "判定区")} 的一张牌移至 {target.Name}",
                [card.Id], [target.Seat]);
        }
    }

    private void ApplyShiYhMove(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.YangHong!;
        var cardId = choice.Cards.Single();
        var targetSeat = choice.Targets.Single();
        var from = _cardZones.GetLocation(cardId);
        var moverSeat = from.OwnerSeat ?? throw new InvalidOperationException("YuanMo moved an unowned card.");
        var rangeBefore = ShiYhRangeCoverage(moverSeat);
        MoveCard(GetAdvancedCard(cardId), from, new CardLocation(from.Zone, targetSeat),
            new CardMoveReason(ShiYangHongComposition.YuanmoMoveReason), disclosure: CardMovementDisclosure.FaceUp);
        var rangeAfter = ShiYhRangeCoverage(moverSeat);
        var drawn = Math.Min(5, rangeBefore.Count(covered => !rangeAfter.Contains(covered)));
        AdvanceEventRulesAndQueueFact(new ShiYhYuanmoMovedEvent(f.Id, f.OwnerSeat, cardId, from,
            new CardLocation(from.Zone, targetSeat), 0));
        AdvanceEventRulesAndQueueFact(new ShiYhYuanmoUsedEvent(f.Id, f.OwnerSeat, _turnNumber));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 的“远谟”移动了场上的一张牌。", f.OwnerSeat, targetSeat);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with
        { Stage = ShiYhStage.DrawChoice, MoverSeat = moverSeat, Drawn = drawn } });
        ContinueShiYh(f.Id);
    }

    // Characters inside the giver's attack range before and after the move.
    private HashSet<int> ShiYhRangeCoverage(int seat)
    {
        var range = GetAttackRange(seat);
        return _players.Where(player => player.IsAlive && player.Seat != seat &&
                GetCombatDistance(seat, player.Seat) <= range)
            .Select(player => player.Seat).ToHashSet();
    }

    // ---- prompts --------------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiYhChoices(ProgramSkillFrame f)
    {
        var r = f.YangHong;
        if (r is null || _winner != Winner.None) return [];
        switch (r.Operation, r.Stage)
        {
            case ("jijian", ShiYhStage.SwapFirst):
            case ("jijian", ShiYhStage.SwapSecond):
                return
                [
                    MakeShiYhChoice(f, "swap", "改为用此牌进行拼点"),
                    MakeShiYhChoice(f, "keep", "以自己的手牌拼点"),
                ];
            case ("yuanmo", ShiYhStage.MoveChoice):
                return ShiYhFieldMoveChoices(f)
                    .Append(MakeShiYhChoice(f, "pass", "不移动场上牌"))
                    .ToArray();
            case ("yuanmo", ShiYhStage.DrawChoice):
            {
                var choices = new List<PromptChoice>();
                if (r.Drawn > 0 && _players[r.MoverSeat].IsAlive)
                    choices.Add(MakeShiYhChoice(f, "draw",
                        $"令 {_players[r.MoverSeat].Name} 摸 {r.Drawn} 张牌"));
                choices.Add(MakeShiYhChoice(f, "pass", "不发动摸牌"));
                return choices;
            }
            default:
                return [];
        }
    }

    private PromptChoice MakeShiYhChoice(ProgramSkillFrame f, string branch, string label,
        IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) =>
        new(new($"shi-yang-hong.{f.Id}.{branch}"), label, cards ?? [], targets ?? [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiYangHongComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private void PublishShiYh(ProgramSkillFrame f)
    {
        var choices = ShiYhChoices(f);
        if (choices.Count == 0)
        { FinishShiYhOperation(GetActiveProgramFrame(f.Id)); return; }
        var r = f.YangHong!;
        var seat = r.Operation == "jijian" && r.Stage == ShiYhStage.SwapFirst ? r.FirstSeat :
            r.Operation == "jijian" && r.Stage == ShiYhStage.SwapSecond ? r.SecondSeat : f.OwnerSeat;
        if (!_players[seat].IsAlive)
        { FinishShiYhOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, seat,
            r.Operation == "jijian" ? "间计：请选择。" : "远谟：请选择。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = seat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiYangHongComposition.ProgramAction)]
    private void AnswerShiYh(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { YangHong: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiYhChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Yang Hong choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, r.Stage, branch)
        {
            case ("jijian", ShiYhStage.SwapFirst, "swap"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with
                { Stage = ShiYhStage.SwapSecond, FirstSwap = true } });
                PublishShiYh(GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapFirst, "keep"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { Stage = ShiYhStage.SwapSecond } });
                PublishShiYh(GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapSecond, "swap"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with
                { Stage = ShiYhStage.Contest, SecondSwap = true } });
                LaunchShiYhContest(GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapSecond, "keep"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = r with { Stage = ShiYhStage.Contest } });
                LaunchShiYhContest(GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.MoveChoice, var move) when move.StartsWith("move-", StringComparison.Ordinal):
                ApplyShiYhMove(GetActiveProgramFrame(f.Id), selected);
                return;
            case ("yuanmo", ShiYhStage.MoveChoice, "pass"):
                FinishShiYhOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.DrawChoice, "draw"):
                DrawCards(_players[r.MoverSeat], r.Drawn, log: true);
                FinishShiYhOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.DrawChoice, "pass"):
                FinishShiYhOperation(GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Yang Hong answered an incompatible operation branch.");
        }
    }

    private void ContinueShiYh(long id) => PublishShiYh(GetActiveProgramFrame(id));

    private void FinishShiYhOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { YangHong = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiYh(ProgramSkillFrame f) => TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
        TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id) ||
        TryBeginAdvancedSkillsChanged(f.Id);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68640)]
    private bool ResumeShiYh(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { YangHong: { } r } f || f.Id != id) return false;
        switch (r.Operation, r.Stage)
        {
            case ("jijian", ShiYhStage.Contest):
                ContinueShiYhContest(GetActiveProgramFrame(id));
                return true;
            case ("jijian", ShiYhStage.Slash):
                BeginShiYhRetaliation(GetActiveProgramFrame(id));
                return true;
            case ("jijian", ShiYhStage.Retaliate):
                AdvanceShiYhRetaliation(GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiYh(GetActiveProgramFrame(id))) return true;
                PublishShiYh(GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68640)]
    private bool ReturnShiYhMovement(ProgramSkillFrame f) => f.YangHong is not null && ResumeShiYh(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiYhJijian)]
    private PromptChoice ChooseShiYhJijianAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "keep") ??
        decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiYhYuanmo)]
    private PromptChoice ChooseShiYhYuanmoAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "pass") ??
        decision.Choices.LastOrDefault() ?? decision.Choices[0];
}
