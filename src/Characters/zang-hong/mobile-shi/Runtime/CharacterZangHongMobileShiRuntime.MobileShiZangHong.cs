#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZangHongMobileShiRuntime
{
    internal const string ShiZhLiezhi = ShiZangHongComposition.LiezhiSkill, ShiZhJuegu = ShiZangHongComposition.JueguSkill;
    SkillProgramStepOutcome global::CardGame.Core.IShiZangHongHost.ResolveLiezhi(ProgramSkillFrame frame) => BeginShiZhLiezhi(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiZangHongHost.ResolveJuegu(ProgramSkillFrame frame) => BeginShiZhJuegu(frame);
    // ---- 烈志: once per turn the owner pays one maximum HP to virtually use a
    // Peach or Wine. The shared zero-entity virtual-basic machinery issues the
    // use; this skill only narrows the offered options to Peach and Wine.
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68710, SkillProgramEffectOp.ResolveShiZhLiezhi)]
    internal bool CanOfferShiZhLiezhiActivation(CharacterState owner, string skill) => skill != ShiZhLiezhi || ShiZhLiezhiOptions(owner.Seat).Any();
    internal SkillProgramStepOutcome BeginShiZhLiezhi(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ZangHong is not null)
            throw new InvalidOperationException("Shi Zang Hong restarted a running operation.");
        if (f.SkillId != ShiZhLiezhi || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var options = ShiZhLiezhiOptions(f.OwnerSeat).ToArray();
        if (options.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhLiezhiPaidEvent(f.Id, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
        PublishShiZhLiezhi(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), options);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<(CardKind Kind, int Target)> ShiZhLiezhiOptions(int owner) => _coreVirtualBasicOffersOperations.VirtualBasicOptions2(owner, anyRegisteredBasic: true).Where(option => option.Kind is CardKind.Peach or CardKind.Alcohol);
    internal void PublishShiZhLiezhi(ProgramSkillFrame f, (CardKind Kind, int Target)[] options)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = options.Select(option => new PromptChoice(new ChoiceId($"virtual-basic.{(f.Id)}.{(option.Kind)}:{(option.Target)}"), $"视为使用【{(CardCatalog.Get(option.Kind).DisplayName)}】", [], [option.Target], new Dictionary<string, string> { ["program-action"] = "virtual-basic", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["basic-option"] = $"{(option.Kind)}:{(option.Target)}", })).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { VirtualBasicDraft = new(f.InstructionIndex) });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】：请选择视为使用的基本牌。", [], options.Select(option => option.Target).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal SkillProgramStepOutcome BeginShiZhJuegu(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ZangHong is not null)
            throw new InvalidOperationException("Shi Zang Hong restarted a running operation.");
        if (f.SkillId != ShiZhJuegu || f.TriggerId != ShiZangHongComposition.JueguTrigger || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhJueguUsedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        var discardCount = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhJueguUsedEvent>().Count(entry => entry.OwnerSeat == f.OwnerSeat && entry.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, log: true);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ZangHong = new("juegu", ShiZhStage.Discarding) { DiscardCount = discardCount } });
        if (DrainShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return SkillProgramStepOutcome.AwaitChild;
        ContinueShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueShiZh(ProgramSkillFrame f)
    {
        var r = f.ZangHong;
        if (r is null)
            return;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
        var remaining = Math.Min(r.DiscardCount - r.Discarded, hand.Count);
        if (remaining <= 0)
        {
            FinishShiZhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = ShiZhDiscardChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】：请弃置 {(remaining)} 张手牌。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiZangHongComposition.ProgramAction)]
    internal void AnswerShiZh(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZangHong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiZhDiscardChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zang Hong choice lost its current frame or legal card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var cardId = selected.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), from, CardLocation.DiscardPile, new CardMoveReason(ShiZangHongComposition.JueguDiscardReason), disclosure: CardMovementDisclosure.FaceUp);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ZangHong = r with { Discarded = r.Discarded + 1 } });
        if (DrainShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            return;
        ContinueShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IReadOnlyList<PromptChoice> ShiZhDiscardChoices(ProgramSkillFrame f) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => new PromptChoice(new ChoiceId($"shi-zang-hong.{(f.Id)}.discard-{(card.Id)}"), $"弃置【{(card.DisplayName)}】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ShiZangHongComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"discard-{(card.Id)}", })).ToArray();
    internal void FinishShiZhOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ZangHong = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiZh(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68700)]
    internal bool ResumeShiZh(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZangHong: { } } f || f.Id != id)
            return false;
        if (DrainShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        ContinueShiZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68700)]
    internal bool ReturnShiZhMovement(ProgramSkillFrame f) => f.ZangHong is not null && ResumeShiZh(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhLiezhi)]
    internal PromptChoice ChooseShiZhLiezhiAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("basic-option")?.StartsWith("peach:", StringComparison.Ordinal) == true) ?? decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhJuegu)]
    internal PromptChoice ChooseShiZhJueguAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("action") == "activate") ?? decision.Choices[0];
}
