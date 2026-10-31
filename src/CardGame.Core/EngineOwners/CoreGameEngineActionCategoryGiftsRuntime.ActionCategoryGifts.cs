#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActionCategoryGiftsRuntime
{
    [ProgramAiChoice(SkillProgramEffectOp.ChooseDifferentActionCategoryGift)]
    internal PromptChoice SelectAiDifferentActionCategoryGift(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (_coreMatchDefinitionOptionsAccess._options.AiPolicyVersion < AiDecisionPolicy.CooperativeCategoryGiftVersion)
            return decision.Choices[0];
        var choice = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseActionCategoryGift(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), frame.OwnerSeat, decision.Choices);
        return decision.Choices.Single(item => item.Id == choice);
    }

    internal SkillProgramStepOutcome BeginDifferentActionCategoryGift(ProgramSkillFrame frame, int provider, string resultBind, IReadOnlyList<CardZoneKind> zones)
    {
        if (provider == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[provider].IsAlive || frame.WindowContext?.CardUse is not { } use)
            throw new InvalidOperationException("A category gift requires a living other provider and the completed real action.");
        var excluded = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.EffectiveKind);
        var choices = new List<PromptChoice>();
        foreach (var zone in zones)
            foreach (var card in _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, provider)).Where(card => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) != excluded).OrderBy(card => card.Id))
                choices.Add(new PromptChoice(new ChoiceId($"action-category-gift.{(frame.Id)}.card-{(card.Id)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 【{(card.DisplayName)}】（{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}）", [card.Id], [], GiftChoiceParameters(frame, resultBind, "different-action-category-gift")));
        choices.Add(new PromptChoice(new ChoiceId($"action-category-gift.{(frame.Id)}.decline"), "不交牌", [], [], GiftChoiceParameters(frame, resultBind, "different-action-category-decline")));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, provider, $"【{(skill.Name)}】请选择与【{(CardCatalog.Get(use.EffectiveKind).DisplayName)}】类型不同的一张牌交给 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)}。", choices.SelectMany(choice => choice.Cards).ToArray(), [], SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = provider,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 交牌", skill.Description),
            Choices = choices.ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[provider].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static IReadOnlyDictionary<string, string> GiftChoiceParameters(ProgramSkillFrame frame, string resultBind, string action) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActionCategoryGifts.GiftChoiceParameters(frame, resultBind, action);
    [ProgramAnswer("different-action-category-gift", "different-action-category-decline")]
    internal void ResolveDifferentActionCategoryGift(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The category gift lost its frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.ChooseDifferentActionCategoryGift || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString() || choice.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind || frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind) || frame.SelectedTargetSeats.Count != 1 || frame.WindowContext?.CardUse is not { } use)
            throw new InvalidOperationException("The category gift does not match its suspended real action.");
        var provider = frame.SelectedTargetSeats.Single();
        if (provider == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[provider].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("The gift participants or exact skill instance are no longer available.");
        Card? card = null;
        CardLocation? location = null;
        var action = choice.Parameters.GetValueOrDefault("program-action");
        if (action == "different-action-category-gift")
        {
            var id = choice.Cards.Single();
            location = _cardZonesOperations._cardZones.GetLocation(id);
            if (location.Value.OwnerSeat != provider || !effect.Zones.Contains(location.Value.Zone))
                throw new InvalidOperationException("The gift no longer belongs to the provider's allowed zone.");
            card = _cardZonesOperations._cardZones.CardsAt(location.Value).Single(item => item.Id == id);
            if (global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.EffectiveKind))
                throw new InvalidOperationException("The gift matches the used card's category.");
        }
        else if (action != "different-action-category-decline" || choice.Cards.Count != 0)
            throw new InvalidOperationException("The category gift has an invalid branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ChoiceBindings = frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, card is null ? "declined" : "gifted", provider)).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramActionCategoryGiftResolvedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, provider, use.EffectiveKind, card?.Id));
        if (card is not null)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, location!.Value, CardLocation.Hand(frame.OwnerSeat), new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, provider, frame.OwnerSeat, [card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                return;
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void AssertDifferentActionCategoryGift(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (paused.Op != SkillProgramEffectOp.ChooseDifferentActionCategoryGift || frame.ChoiceBindings.Any(binding => binding.Name == paused.ResultBind) || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            return;
        if (frame.SelectedTargetSeats.Count != 1 || frame.SelectedTargetSeats[0] == frame.OwnerSeat || frame.WindowContext?.CardUse is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.SelectedTargetSeats[0] || prompt.Choices.Count == 0 || prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action")is not ("different-action-category-gift" or "different-action-category-decline") || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString() || choice.Parameters.GetValueOrDefault("result-bind") != paused.ResultBind))
            throw new InvalidOperationException("The private category gift lost its source, entity prompt or participants.");
    }
}
