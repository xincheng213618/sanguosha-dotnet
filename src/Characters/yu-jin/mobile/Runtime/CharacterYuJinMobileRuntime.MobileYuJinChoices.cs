#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJinMobileRuntime
{
    internal bool ExactMobileYuJinParent(ProgramSkillFrame frame, bool ending)
    {
        if (frame.WindowContext is not { } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != context.ParentFrameId)
            return false;
        return ending ? _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame end && end.OwnerSeat == frame.OwnerSeat && end.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && end.ItemIndex >= 0 && end.ItemIndex < end.Items.Count && end.Items[end.ItemIndex].Candidate is { } candidate && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate) : _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame start && start.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && start.Continuation == ProgramLifecycleContinuation.NormalTurnStart && start.OwnerSeat == frame.OwnerSeat && start.CandidateIndex >= 0 && start.CandidateIndex < start.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, start.Candidates[start.CandidateIndex]);
    }

    internal IReadOnlyList<PromptChoice> MobileYuJinStorePayments(ProgramSkillFrame frame, int target) => _programPaidCardOperations.Get().BuildOwnedCardPaymentChoices(frame.Id, target, target, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => !card.IsGeneralWeapon);
    internal SkillProgramStepOutcome BeginMobileYuJinChoice(ProgramSkillFrame frame)
    {
        if (!ExactMobileYuJinParent(frame, true) || frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || frame.ChoiceBindings.Any(b => b.Name == "jieyue-answer") || _coreProgramLifecycleOperations.GetProgramCardSet(frame, "jieyue-cost").CardIds is not [var paid] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == paid && m.From == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.mobile:jieyue.SelectAndMoveOwnedCard" && m.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber))
            throw new InvalidOperationException("Jieyue's other-participant choice requires its original actual paid hand discard and ending parent.");
        var source = new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var choice = new MobileYuJinChoiceDraft(target, MobileYuJinChoiceStage.Branch, source, frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, frame.WindowContext!.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileYuJinChoice = choice });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileYuJinChoiceOpenedEvent(frame.Id, target, source, frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, choice.ParentId));
        PublishMobileYuJinChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MobileYuJinChoices(ProgramSkillFrame frame)
    {
        var draft = frame.MobileYuJinChoice!;
        var choices = new List<PromptChoice>();
        if (draft.Stage == MobileYuJinChoiceStage.Branch)
        {
            void Add(string id, string label) => choices.Add(new(new ChoiceId($"mobile-jieyue.{(frame.Id)}.branch.{(id)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = "mobile-yu-jin-jieyue", ["frame-id"] = frame.Id.ToString(), ["stage"] = "Branch", ["option-id"] = id }));
            if (MobileYuJinStorePayments(frame, draft.TargetSeat).Count > 0)
                Add("store", "将一张手牌或装备牌置为于禁的节钺牌");
            Add("discard", "令于禁弃置你的一张牌");
        }
        else
            foreach (var payment in MobileYuJinStorePayments(frame, draft.TargetSeat))
            {
                var parameters = new Dictionary<string, string>(payment.Parameters)
                {
                    ["program-action"] = "mobile-yu-jin-jieyue",
                    ["stage"] = "Store",
                    ["option-id"] = "card"
                };
                choices.Add(payment with { Id = new ChoiceId($"mobile-jieyue.{(frame.Id)}.store.{(payment.Id.Value)}"), Parameters = parameters });
            }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal bool ValidMobileYuJinChoice(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinChoice is not { } draft || frame.SkillId != MobileYuJinJieyue.Skill || frame.TriggerId != MobileYuJinJieyue.Ending || frame.InstructionIndex != 3 || frame.MobileYuJinTransfer is not null || draft.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || draft.GameplayHash != frame.GameplayHash || draft.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || draft.ParentId != frame.WindowContext?.ParentFrameId || frame.SelectedTargetSeats is not [var target] || target != draft.TargetSeat || target == frame.OwnerSeat || !Enum.IsDefined(draft.Stage) || !ExactMobileYuJinParent(frame, true))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
        if (history.OfType<MobileYuJinChoiceOpenedEvent>().Count(e => e == new MobileYuJinChoiceOpenedEvent(frame.Id, target, draft.Source, draft.GameplayHash, draft.ActualTurn, draft.ParentId)) != 1)
            return false;
        return draft.Stage == MobileYuJinChoiceStage.Branch ? !frame.ChoiceBindings.Any(b => b.Name == "jieyue-answer") && !history.OfType<MobileYuJinBranchChosenEvent>().Any(e => e.FrameId == frame.Id) : frame.ChoiceBindings.SingleOrDefault(b => b.Name == "jieyue-answer")is { OptionId: "store" } branch && branch.ChooserSeat == target && history.OfType<MobileYuJinBranchChosenEvent>().Count(e => e.FrameId == frame.Id && e.TargetSeat == target && e.Store) == 1;
    }

    internal bool MatchesMobileYuJinChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding)
    {
        if (frame.SkillId != MobileYuJinJieyue.Skill || frame.TriggerId != MobileYuJinJieyue.Ending || frame.InstructionIndex is < 3 or > 4 || binding.Name != "jieyue-answer" || binding.OptionId is not ("store" or "discard") || frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || binding.ChooserSeat != target || !ExactMobileYuJinParent(frame, true) || frame.WindowContext is not { } context)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var source = new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        if (history.OfType<MobileYuJinChoiceOpenedEvent>().Count(e => e == new MobileYuJinChoiceOpenedEvent(frame.Id, target, source, frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, context.ParentFrameId)) != 1 || history.OfType<MobileYuJinBranchChosenEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var chosen] || chosen.TargetSeat != target || chosen.Store != (binding.OptionId == "store"))
            return false;
        if (!chosen.Store)
            return frame.MobileYuJinChoice is null && frame.MobileYuJinTransfer is null && !history.OfType<MobileYuJinTransferStartedEvent>().Any(e => e.FrameId == frame.Id);
        if (frame.MobileYuJinChoice is { Stage: MobileYuJinChoiceStage.Store })
            return ValidMobileYuJinChoice(frame);
        if (frame.MobileYuJinTransfer is { Operation: SkillProgramEffectOp.ResolveMobileYuJinJieyue })
            return ValidMobileYuJinTransfer(frame);
        return history.OfType<MobileYuJinTransferStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [{ Operation: SkillProgramEffectOp.ResolveMobileYuJinJieyue, Count: 1 } started] && started.Source == source && started.GameplayHash == frame.GameplayHash && started.ParentId == context.ParentFrameId && history.OfType<MobileYuJinTransferReturnedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [{ Count: 1 }];
    }

    internal void PublishMobileYuJinChoice(ProgramSkillFrame frame)
    {
        if (!ValidMobileYuJinChoice(frame))
            throw new InvalidOperationException("Jieyue lost its original other-participant private choice.");
        var draft = frame.MobileYuJinChoice!;
        var choices = MobileYuJinChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, draft.Stage == MobileYuJinChoiceStage.Branch ? "【节钺】选择存一张牌，或令于禁弃置你的一张牌。" : "【节钺】选择自己的一张手牌或装备牌置于于禁的武将牌上。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.TargetSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-yu-jin-jieyue")]
    internal void AnswerMobileYuJinChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileYuJinChoice: { } draft } frame || !ValidMobileYuJinChoice(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.TargetSeat || !MobileYuJinChoices(frame).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jieyue's answer must match the exact original participant and still-legal transfer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "节钺参与者或原技能实例已失效。");
            return;
        }

        if (draft.Stage == MobileYuJinChoiceStage.Branch)
        {
            var store = choice.Parameters["option-id"] == "store";
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding("jieyue-answer", store ? "store" : "discard", draft.TargetSeat)).ToArray()), MobileYuJinChoice = store ? draft with { Stage = MobileYuJinChoiceStage.Store } : null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileYuJinBranchChosenEvent(frame.Id, draft.TargetSeat, store));
            if (store)
                PublishMobileYuJinChoice(frame);
            else
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var id = choice.Cards.Single();
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        PayMobileYuJinTransfer(frame, SkillProgramEffectOp.ResolveMobileYuJinJieyue, _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, int.MaxValue), [id], [location]);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileYuJinJieyue)]
    internal PromptChoice AiMobileYuJinChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.MobileYuJinChoice!;
        int Keep(PromptChoice c) => c.Cards is [var id] ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id), _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]) : 0;
        if (draft.Stage == MobileYuJinChoiceStage.Store)
            return decision.Choices.OrderBy(Keep).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var canStore = decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "store");
        return canStore is not null && MobileYuJinStorePayments(frame, draft.TargetSeat).Min(Keep) <= 4 ? canStore : decision.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "discard");
    }
}
