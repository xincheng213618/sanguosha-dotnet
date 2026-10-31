#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLouGuiMobileShiRuntime
{
    internal const string ShiLgGuanSha = ShiLouGuiComposition.GuanShaSkill, ShiLgJiYu = ShiLouGuiComposition.JiYuSkill;
    internal bool HasShiLgCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiLgGuanSha);

    SkillProgramStepOutcome global::CardGame.Core.IShiLouGuiHost.ResolveGuanSha(ProgramSkillFrame frame) => BeginShiLgGuanSha(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiLouGuiHost.ResolveJiYu(ProgramSkillFrame frame) => BeginShiLgJiYu(frame);
    internal SkillProgramStepOutcome BeginShiLgGuanSha(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiLouGui is not null)
            throw new InvalidOperationException("Shi Lou Gui restarted a running operation.");
        if (f.SkillId != ShiLgGuanSha || f.TriggerId != ShiLouGuiComposition.GuanShaTrigger || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLouGui = new("guansha", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiLgStage.GuanShaAsk) });
        PublishShiLg(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiLgGuanSha(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var owned = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Select(card => card.Id).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Select(card => card.Id)).ToArray();
        foreach (var cardId in owned)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.DiscardPile, new(ShiLouGuiComposition.GuanShaReplaceReason));
        }

        var gained = new List<int>();
        var names = new HashSet<CardKind>();
        while (gained.Count < owned.Length)
        {
            var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => CardCatalog.Get(card.Kind).CategoryName == "基本牌").ToArray();
            if (candidates.Length == 0)
                break;
            var card = candidates[_randomOperations._random.Next(candidates.Length)];
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(ShiLouGuiComposition.GuanShaGainReason));
            gained.Add(card.Id);
            names.Add(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(card.Kind));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLgGuanshaAppliedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, owned.Length, gained.Count, names.Count));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 的“灌沙”将 {(owned.Length)} 张牌替换为 {(gained.Count)} 张随机基本牌，本回合手牌上限+{(names.Count)}。", f.OwnerSeat);
        FinishShiLgOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal int ShiLgJiYuUsesThisPhase(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLgJiYuCostPaidEvent>().Count(entry => entry.OwnerSeat == seat && entry.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber);
    internal bool ShiLgJiYuGainsAllUsed(int seat)
    {
        var lastActivation = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLgJiYuCostPaidEvent>().Where(entry => entry.OwnerSeat == seat && entry.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber).Select(entry => entry.FrameId).LastOrDefault();
        if (lastActivation == 0)
            return false;
        var gained = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLgJiYuGainedEvent>().Where(entry => entry.FrameId == lastActivation).Select(entry => entry.CardId).Distinct();
        return gained.All(cardId => _cardZonesOperations._cardZones.GetLocation(cardId)is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != seat);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68300, SkillProgramEffectOp.ResolveShiLgJiYu)]
    internal bool CanStartShiLgJiYu(CharacterState owner, string skill)
    {
        if (skill != ShiLgJiYu)
            return true;
        return ShiLgJiYuUsesThisPhase(owner.Seat) switch
        {
            0 => true,
            1 => ShiLgJiYuGainsAllUsed(owner.Seat),
            _ => false,
        };
    }

    internal SkillProgramStepOutcome BeginShiLgJiYu(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiLouGui is not null)
            throw new InvalidOperationException("Shi Lou Gui restarted a running operation.");
        if (f.SkillId != ShiLgJiYu || !CanStartShiLgJiYu(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiLouGui = new("jiyu", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiLgStage.JiYuCost) { PhaseId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId } });
        PublishShiLg(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiLgJiYuCost(ProgramSkillFrame f, int cardId)
    {
        var r = f.ShiLouGui!;
        if (_cardZonesOperations._cardZones.GetLocation(cardId)is not { Zone: CardZoneKind.Hand, OwnerSeat: var seat } || seat != f.OwnerSeat)
            throw new InvalidOperationException("JiYu discarded a card outside its owner's current hand.");
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
        var category = CardCatalog.Get(card.Kind).CategoryName;
        _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.DiscardPile, new(ShiLouGuiComposition.JiYuCostReason));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLgJiYuCostPaidEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.PhaseId, cardId));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var ordinal = 0;
        foreach (var other in new[]
        {
            "基本牌",
            "锦囊牌",
            "装备牌"
        }.Where(name => name != category))
        {
            var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(candidate => CardCatalog.Get(candidate.Kind).CategoryName == other).ToArray();
            if (candidates.Length == 0)
                continue;
            var picked = candidates[_randomOperations._random.Next(candidates.Length)];
            _corePhysicalMovementFacadesOperations.MoveCard(picked, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(ShiLouGuiComposition.JiYuGainReason));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLgJiYuGainedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.PhaseId, picked.Id, ++ordinal));
        }

        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 弃置一张手牌，随机获得了两张其它类别的牌（急御）。", f.OwnerSeat);
        FinishShiLgOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IReadOnlyList<PromptChoice> ShiLgChoices(ProgramSkillFrame f)
    {
        var r = f.ShiLouGui!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("guansha", ShiLgStage.GuanShaAsk):
                return[MakeShiLgChoice(f, "arm", "发动“灌沙”：将你的所有牌替换为等量随机基本牌"), MakeShiLgChoice(f, "pass", "不发动“灌沙”"), ];
            case ("jiyu", ShiLgStage.JiYuCost):
                return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => new PromptChoice(new($"shi-lou-gui.{(f.Id)}.cost.{(card.Id)}"), $"弃置这张手牌（{(DescribeShiLgCard(card))}）", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ShiLouGuiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "cost", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture), })).ToArray();
            default:
                return[];
        }
    }

    internal PromptChoice MakeShiLgChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-lou-gui.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiLouGuiComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal static string DescribeShiLgCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{(card.Suit)}{(card.Rank)}";
    internal void PublishShiLg(ProgramSkillFrame f)
    {
        var choices = ShiLgChoices(f);
        if (choices.Count == 0)
        {
            FinishShiLgOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ShiLouGui!.Operation == "guansha" ? "灌沙：选择是否发动。" : "急御：选择要弃置的手牌。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiLouGuiComposition.ProgramAction)]
    internal void AnswerShiLg(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiLouGui: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiLgChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Lou Gui choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("guansha", "arm"):
                ApplyShiLgGuanSha(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("guansha", "pass"):
                FinishShiLgOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("jiyu", "cost")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ApplyShiLgJiYuCost(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), cardId);
                return;
            default:
                throw new InvalidOperationException("Shi Lou Gui answered an incompatible operation branch.");
        }
    }

    internal void FinishShiLgOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiLouGui = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiLg(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68300)]
    internal bool ResumeShiLg(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiLouGui: not null } f || f.Id != id)
            return false;
        if (DrainShiLg(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        PublishShiLg(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68300)]
    internal bool ReturnShiLgMovement(ProgramSkillFrame f) => f.ShiLouGui is not null && ResumeShiLg(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLgGuanSha)]
    internal PromptChoice ChooseShiLgGuanShaAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLgJiYu)]
    internal PromptChoice ChooseShiLgJiYuAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}
