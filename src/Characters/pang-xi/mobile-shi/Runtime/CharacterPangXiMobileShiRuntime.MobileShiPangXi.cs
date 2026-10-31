#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangXiMobileShiRuntime
{
    internal const string ShiPxXuye = ShiPangXiComposition.XuyeSkill, ShiPxKuangxiang = ShiPangXiComposition.KuangxiangSkill;
    SkillProgramStepOutcome global::CardGame.Core.IShiPangXiHost.ResolveXuye(ProgramSkillFrame frame) => BeginShiPxXuye(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiPangXiHost.ResolveKuangxiang(ProgramSkillFrame frame) => BeginShiPxKuangxiang(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiPangXiHost.ResolveRider(ProgramSkillFrame frame) => BeginShiPxRider(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiPangXiHost.ResolveSweep(ProgramSkillFrame frame) => BeginShiPxSweep(frame);
    internal bool ShiPxIsGlobalMinimumHand(int seat)
    {
        var counts = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count).ToArray();
        return counts.Length > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count == counts.Min();
    }

    internal bool ShiPxIsGlobalMaximumHand(int seat)
    {
        var counts = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count).ToArray();
        return counts.Length > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count == counts.Max();
    }

    internal ShiPxKuangxiangSwappedEvent? ShiPxActiveSwap(int seat)
    {
        ShiPxKuangxiangSwappedEvent? swap = null;
        foreach (var payload in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (payload is ShiPxKuangxiangSwappedEvent { OwnerSeat: var owner } swapped && owner == seat)
                swap = swapped;
            else if (payload is ShiPxKuangxiangClearedEvent { OwnerSeat: var cleared } && cleared == seat || payload is ShiPxKuangxiangRiderEvent { OwnerSeat: var rider } && rider == seat)
                swap = null;
        }

        return swap;
    }

    internal string? ShiPxRiderSide(ShiPxKuangxiangSwappedEvent swap, IReadOnlyList<CardMovementRecord> movements)
    {
        bool Emptied(IReadOnlyList<int> took, int holder) => took.Count > 0 && movements.Any(movement => took.Contains(movement.CardId) && movement.From.Zone == CardZoneKind.Hand && movement.From.OwnerSeat == holder) && took.All(id => _cardZonesOperations._cardZones.GetLocation(id)is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != holder);
        if (Emptied(swap.OwnerTookIds, swap.OwnerSeat))
            return "self";
        if (Emptied(swap.TargetTookIds, swap.TargetSeat))
            return "target";
        return null;
    }

    internal SkillProgramStepOutcome BeginShiPxXuye(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxXuye || f.TriggerId != ShiPangXiComposition.XuyeTrigger || f.WindowContext?.TargetSeat is not { } victim || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !ShiPxIsGlobalMinimumHand(victim) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiPxXuyeDrawEvent>().Any(entry => entry.OwnerSeat == f.OwnerSeat && entry.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangXi = new("xuye", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiPxStage.XuyeAsk) { VictimSeat = victim } });
        PublishShiPx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiPxXuyeDraw(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, r.VictimSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason(ShiPangXiComposition.XuyeDrawReason));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxXuyeDrawEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.VictimSeat, 2));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“蓄业”令 {(_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].Name)} 摸两张牌。", f.OwnerSeat, r.VictimSeat);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && ShiPxIsGlobalMaximumHand(r.VictimSeat))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PangXi = f.PangXi!with { Stage = ShiPxStage.XuyePick } });
            ContinueShiPx(f.Id);
            return;
        }

        FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ApplyShiPxTopPlace(ProgramSkillFrame f, int cardId, int holder)
    {
        var r = f.PangXi!;
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.DrawPile, new CardMoveReason("skill-program.mobile:shi-xuye.top"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxXuyeTopPlacedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, holder, cardId));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 将 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 区域内的一张牌置于牌堆顶（蓄业）。", f.OwnerSeat, holder);
        FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IReadOnlyList<int> ShiPxAreaCardIds(int seat) => global::CardGame.Core.EngineOwners.CharacterPangXiMobileShiDefinitions.ShiPxHolderZones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, seat))).Select(card => card.Id).Distinct().ToArray();
    internal SkillProgramStepOutcome BeginShiPxKuangxiang(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count)
            return SkillProgramStepOutcome.Continue;
        ApplyShiPxKuangxiangSwap(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), target);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ApplyShiPxKuangxiangSwap(ProgramSkillFrame f, int targetSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var ownerHandIds = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Select(card => card.Id).ToArray();
        var targetHandIds = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Select(card => card.Id).ToArray();
        foreach (var cardId in ownerHandIds)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.Hand(targetSeat), new CardMoveReason(ShiPangXiComposition.SwapGiveReason));
        }

        foreach (var cardId in targetHandIds)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.Hand(f.OwnerSeat), new CardMoveReason(ShiPangXiComposition.SwapTakeReason));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangSwappedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, targetSeat, targetHandIds, ownerHandIds));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 与 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 交换了手牌（匡襄）。", f.OwnerSeat, targetSeat);
        FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiPxRider(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        var swap = ShiPxActiveSwap(f.OwnerSeat);
        var side = swap is null || f.WindowContext?.MovementBatch is not { } batch ? null : ShiPxRiderSide(swap, batch.Movements);
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId != ShiPangXiComposition.KuangxiangRiderTrigger || swap is null || side is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangXi = new("kuangxiang", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiPxStage.RiderAsk) { Side = side, VictimSeat = side == "self" ? f.OwnerSeat : swap.TargetSeat } });
        PublishShiPx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiPxRiderArm(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangRiderEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.Side, 2));
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason(ShiPangXiComposition.RiderDrawReason));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 对自己发动“蓄业”，摸两张牌。", f.OwnerSeat);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && ShiPxIsGlobalMaximumHand(f.OwnerSeat))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PangXi = f.PangXi!with { Stage = ShiPxStage.RiderPick } });
            ContinueShiPx(f.Id);
            return;
        }

        FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiPxSweep(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId != ShiPangXiComposition.KuangxiangSweepTrigger || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || ShiPxActiveSwap(f.OwnerSeat)is null)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangClearedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiPxChoices(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("xuye", ShiPxStage.XuyeAsk):
                return[MakeShiPxChoice(f, "arm", $"发动“蓄业”：令 {(_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].Name)} 摸两张牌"), MakeShiPxChoice(f, "pass", "不发动“蓄业”"), ];
            case ("xuye", ShiPxStage.XuyePick):
                return ShiPxAreaCardIds(r.VictimSeat).Select(cardId => new PromptChoice(new($"shi-pang-xi.{(f.Id)}.top.{(cardId)}"), $"将这张牌置于牌堆顶（{(DescribeShiPxCard(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId)))}）", [cardId], [], new Dictionary<string, string> { ["program-action"] = ShiPangXiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "top", ["card-id"] = cardId.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("kuangxiang", ShiPxStage.RiderAsk):
                return[MakeShiPxChoice(f, "arm", "对自己发动“蓄业”：摸两张牌"), MakeShiPxChoice(f, "pass", "不发动"), ];
            case ("kuangxiang", ShiPxStage.RiderPick):
                return ShiPxAreaCardIds(f.OwnerSeat).Select(cardId => new PromptChoice(new($"shi-pang-xi.{(f.Id)}.self-top.{(cardId)}"), $"将你的这张牌置于牌堆顶（{(DescribeShiPxCard(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId)))}）", [cardId], [], new Dictionary<string, string> { ["program-action"] = ShiPangXiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "self-top", ["card-id"] = cardId.ToString(CultureInfo.InvariantCulture), })).ToArray();
            default:
                return[];
        }
    }

    internal PromptChoice MakeShiPxChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-pang-xi.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiPangXiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal static string DescribeShiPxCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{(card.Suit)}{(card.Rank)}";
    internal void PublishShiPx(ProgramSkillFrame f)
    {
        var choices = ShiPxChoices(f);
        if (choices.Count == 0)
        {
            FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.PangXi!.Operation == "xuye" ? "蓄业：请选择。" : "匡襄：请选择。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiPangXiComposition.ProgramAction)]
    internal void AnswerShiPx(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PangXi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiPxChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Pang Xi choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("xuye", "arm"):
                ApplyShiPxXuyeDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("xuye", "pass"):
                FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("xuye", "top")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ApplyShiPxTopPlace(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), cardId, r.VictimSeat);
                return;
            case ("kuangxiang", "arm"):
                ApplyShiPxRiderArm(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("kuangxiang", "pass"):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangClearedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber));
                FinishShiPxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("kuangxiang", "self-top")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var selfCardId):
                ApplyShiPxTopPlace(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), selfCardId, f.OwnerSeat);
                return;
            default:
                throw new InvalidOperationException("Shi Pang Xi answered an incompatible operation branch.");
        }
    }

    internal void ContinueShiPx(long id) => PublishShiPx(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    internal void FinishShiPxOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PangXi = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiPx(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68500)]
    internal bool ResumeShiPx(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PangXi: not null } f || f.Id != id)
            return false;
        if (DrainShiPx(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        PublishShiPx(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68500)]
    internal bool ReturnShiPxMovement(ProgramSkillFrame f) => f.PangXi is not null && ResumeShiPx(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxXuye)]
    internal PromptChoice ChooseShiPxXuyeAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxKuangxiang)]
    internal PromptChoice ChooseShiPxKuangxiangAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxRider)]
    internal PromptChoice ChooseShiPxRiderAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxSweep)]
    internal PromptChoice ChooseShiPxSweepAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}
