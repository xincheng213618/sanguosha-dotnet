#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenBoundaryMobileRuntime
{
    internal const string BoundarySidiSkill = "boundary:sidi-mobile", BoundarySidiAction = "boundary-sidi";
    internal static bool BoundarySidiNonDelayed(CardActionContext action) => action.EffectiveKind is not (CardKind.Lightning or CardKind.Indulgence or CardKind.SupplyShortage);
    internal IReadOnlyList<BoundarySidiPrediction> BoundarySidiRecords(int owner, string instance) => Array.AsReadOnly((_coreMatchParticipantsPlayersAccess._players[owner].BoundarySidiPredictions ?? []).Where(record => record.OwnerSeat == owner && record.SkillInstanceId == instance && record.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(BoundarySidiSkill).Program!.GameplayHash && _coreMatchParticipantsPlayersAccess._players[record.HolderSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[record.TargetSeat].IsAlive).ToArray());
    internal IReadOnlyList<BoundarySidiPredictionView>? GetBoundarySidiPredictionViews(int viewer)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(viewer) || !_coreMatchParticipantsPlayersAccess._players[viewer].IsAlive)
            return null;
        var values = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[viewer]).ProgramInstances.Where(instance => instance.SkillId == BoundarySidiSkill && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[viewer], instance.SkillId, instance.SkillInstanceId)).SelectMany(instance => BoundarySidiRecords(viewer, instance.SkillInstanceId)).OrderBy(record => record.HolderSeat).Select(record => new BoundarySidiPredictionView(record.HolderSeat, record.TargetSeat)).ToArray();
        return values.Length == 0 ? null : Array.AsReadOnly(values);
    }

    internal int[] BoundarySidiEligibleHolders(int owner, string instance) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner && !BoundarySidiRecords(owner, instance).Any(record => record.HolderSeat == player.Seat)).Select(player => player.Seat).ToArray();
    internal IReadOnlyList<int> BoundarySidiActualTargets(ProgramCardTriggerWindowFrame window)
    {
        var action = window.Action;
        var targets = action.EffectiveDesignatedTargetSeats;
        if (targets.Count != 0 || action.Type != CardActionType.Use || action.EffectiveKind is not (CardKind.DrawTwo or CardKind.Alcohol) && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Equipment || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } original } use || original.ActionId != action.ActionId || use.SourceSeat != action.ActorSeat || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(declared => declared.ResolutionId == use.Id && declared.SourceSeat == action.ActorSeat && declared.CardKind == action.EffectiveKind))
            return targets;
        // These native producers encode a self target as empty command targets.
        // Name the real effect recipient locally, retaining the accepted action.
        return use.TargetSeats.Count == 0 ? [action.ActorSeat] : use.TargetSeats;
    }

    internal bool TryBoundarySidiWindow(ProgramSkillWindowContext? context, out ProgramCardTriggerWindowFrame window)
    {
        window = null !;
        if (context?.CardUse is not { } identity || context.Window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseCompleted) || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame current || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(current) != context.Window || current.Action.ActionId != identity.CardActionId || !_coreFriendTrioOperations.IsFriendActualCardUse(current.Action) || !BoundarySidiNonDelayed(current.Action))
            return false;
        if (current.Action.Type == CardActionType.Use)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(current.ParentFrameId)is not { Action: { } actual } use || actual.ActionId != current.Action.ActionId || context.Window == SkillProgramTriggerWindow.CardUseCompleted && use.Step != ResolutionFrameStep.Completed)
                return false;
        }
        else if (current.CompletedResponseReturn is not { } typed || typed.IsCommitted != (context.Window == SkillProgramTriggerWindow.CardUseCommitted) || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(current.Action, typed.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(current))
            return false;
        window = current;
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 53400)]
    internal void CollectBoundarySidiUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseCompleted) || !_coreFriendTrioOperations.IsFriendActualCardUse(action) || !BoundarySidiNonDelayed(action))
            return;
        var binding = window == SkillProgramTriggerWindow.CardUseCompleted ? "prepare-forecast" : "consume-first-use";
        result.RemoveAll(candidate => candidate.SkillId == BoundarySidiSkill && candidate.TriggerId == binding);
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, window).Where(candidate => candidate.SkillId == BoundarySidiSkill && candidate.BindingId == binding))
            {
                if (window == SkillProgramTriggerWindow.CardUseCompleted ? action.ActorSeat != owner.Seat || BoundarySidiEligibleHolders(owner.Seat, candidate.SkillInstanceId).Length == 0 : !BoundarySidiRecords(owner.Seat, candidate.SkillInstanceId).Any(record => record.HolderSeat == action.ActorSeat))
                    continue;
                var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, owner.Seat, owner.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(owner, action));
                result.Add(new(owner.Seat, owner.Seat, candidate.SkillId, candidate.BindingId, candidate.GameplayHash, candidate.SkillInstanceId, candidate.Priority, context));
            }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ObservesResponseUseCompletion, 53400)]
    internal bool ObservesBoundarySidiResponse(CardActionContext action) => _coreFriendTrioOperations.IsFriendActualCardUse(action) && _coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(instance => instance.SkillId == BoundarySidiSkill && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, instance.SkillId, instance.SkillInstanceId) && (owner.Seat == action.ActorSeat || BoundarySidiRecords(owner.Seat, instance.SkillInstanceId).Any(record => record.HolderSeat == action.ActorSeat))));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53400)]
    internal bool QualifyBoundarySidi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundarySidiSkill)
            return true;
        if (!TryBoundarySidiWindow(context, out var window))
            return false;
        return trigger.Id == "prepare-forecast" ? window.Action.ActorSeat == candidate.OwnerSeat && BoundarySidiEligibleHolders(candidate.OwnerSeat, candidate.SkillInstanceId).Length > 0 : BoundarySidiRecords(candidate.OwnerSeat, candidate.SkillInstanceId).Any(record => record.HolderSeat == window.Action.ActorSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.OwnerSeat == candidate.OwnerSeat && frame.SkillInstanceId == candidate.SkillInstanceId && frame.BoundarySidi?.WindowId == window.Id);
    }

    SkillProgramStepOutcome global::CardGame.Core.IBoundaryCaoZhenHost.Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginBoundarySidi(frame, effect);
    internal SkillProgramStepOutcome BeginBoundarySidi(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.BoundarySidi is not null || !TryBoundarySidiWindow(frame.WindowContext, out var window))
            throw new InvalidOperationException("Sidi requires its original true-use window and owning frame.");
        if (effect.Op == SkillProgramEffectOp.PrepareBoundaryCaoZhenSidi)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySidi = new(frame.InstructionIndex, BoundarySidiStage.Holder, window.Id, window.Action.ActionId) });
            PublishBoundarySidi(frame);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var record = BoundarySidiRecords(frame.OwnerSeat, frame.SkillInstanceId).Single(item => item.HolderSeat == window.Action.ActorSeat);
        _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].BoundarySidiPredictions = Array.AsReadOnly((_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].BoundarySidiPredictions ?? []).Where(item => item.Id != record.Id).ToArray());
        if (BoundarySidiActualTargets(window)is not [var target] || target != record.TargetSeat)
        {
            CommitBoundarySidiResult(frame, effect.ResultBind!, window.Action.ActionId, "none");
            return SkillProgramStepOutcome.Continue;
        }

        if (target == frame.OwnerSeat)
        {
            CommitBoundarySidiResult(frame, effect.ResultBind!, window.Action.ActionId, "self");
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySidi = new(frame.InstructionIndex, BoundarySidiStage.Benefit, window.Id, window.Action.ActionId, record.HolderSeat, record.TargetSeat, effect.ResultBind!) });
        PublishBoundarySidi(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundarySidiChoices(ProgramSkillFrame frame)
    {
        var draft = frame.BoundarySidi!;
        PromptChoice Choice(string branch, string label, int? target = null) => new(new($"boundary-sidi.{(frame.Id)}.{(draft.Stage)}.{(branch)}.{(target?.ToString() ?? "none")}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = BoundarySidiAction, ["branch"] = branch });
        return draft.Stage switch
        {
            BoundarySidiStage.Holder => Array.AsReadOnly(BoundarySidiEligibleHolders(frame.OwnerSeat, frame.SkillInstanceId).Select(seat => Choice("holder", "为 " + _coreMatchParticipantsPlayersAccess._players[seat].Name + " 秘密预测目标", seat)).ToArray()),
            BoundarySidiStage.Target => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => Choice("target", "秘密指定 " + player.Name + " 为司敌目标", player.Seat)).ToArray()),
            _ => Array.AsReadOnly(new[] { Choice("cancel", "取消该牌；若无人濒死，对使用者造成1点伤害"), Choice("draw", "摸两张牌") })};
    }

    internal void PublishBoundarySidi(ProgramSkillFrame frame)
    {
        var choices = BoundarySidiChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.BoundarySidi!.Stage == BoundarySidiStage.Holder ? "司敌：选择尚未预测的其他角色。" : frame.BoundarySidi.Stage == BoundarySidiStage.Target ? "司敌：秘密指定其下一张非延时牌的唯一目标。" : "司敌预测命中：取消该牌或摸两张牌。", [], Array.AsReadOnly(choices.SelectMany(choice => choice.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundarySidiAction)]
    internal void AnswerBoundarySidi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundarySidi: { } draft } frame || !TryBoundarySidiWindow(frame.WindowContext, out var window) || window.Id != draft.WindowId || window.Action.ActionId != draft.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], BoundarySidiChoices(frame).Where(current => current.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Sidi lost its original private forecast chooser or current native use.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (draft.Stage == BoundarySidiStage.Holder)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySidi = draft with { Stage = BoundarySidiStage.Target, HolderSeat = choice.Targets.Single() } });
            PublishBoundarySidi(frame);
            return;
        }

        if (draft.Stage == BoundarySidiStage.Target)
        {
            var record = new BoundarySidiPrediction(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash, draft.HolderSeat, choice.Targets.Single());
            if (!BoundarySidiEligibleHolders(frame.OwnerSeat, frame.SkillInstanceId).Contains(record.HolderSeat))
                throw new InvalidOperationException("Sidi cannot replace another unconsumed forecast.");
            _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].BoundarySidiPredictions = Array.AsReadOnly((_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].BoundarySidiPredictions ?? []).Append(record).ToArray());
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundarySidi = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySidiForecastRecordedEvent(frame.Id, frame.OwnerSeat));
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var outcome = "draw";
        if (choice.Parameters["branch"] == "cancel")
        {
            if (window.Action.Type != CardActionType.Use || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { } use || use.Action?.ActionId != draft.ActionId)
                throw new InvalidOperationException("A targeted Sidi cancellation must retain its original native Use.");
            var proof = new BoundarySidiCancelledEvent(frame.Id, use.Id, draft.ActionId, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { BoundarySidiCancellation = proof });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(proof);
            outcome = _coreDyingCursorRuntimeOperations.ActiveDying is null && !_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.HasHpValue && player.Hp <= 0) ? "cancel-damage" : "cancel-only";
        }

        CommitBoundarySidiResult(frame, draft.ResultBind!, draft.ActionId, outcome);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CommitBoundarySidiResult(ProgramSkillFrame supplied, string bind, long action, string outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundarySidi = null, ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(bind, outcome, frame.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySidiResultEvent(frame.Id, frame.OwnerSeat, action, bind, outcome));
    }

    internal bool IsBoundarySidiChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => frame.SkillId == BoundarySidiSkill && binding.ChooserSeat == frame.OwnerSeat && frame.WindowContext?.CardUse is { } use && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySidiResultEvent>().Count(record => record.FrameId == frame.Id && record.OwnerSeat == frame.OwnerSeat && record.ActionId == use.CardActionId && record.ResultBind == binding.Name && record.Outcome == binding.OptionId) == 1 && ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Take(frame.InstructionIndex).Count(effect => effect.Op == SkillProgramEffectOp.ResolveBoundaryCaoZhenSidi && effect.ResultBind == binding.Name) == 1;
    internal bool IsBoundarySidiUseIneffective(CardUseFrame use)
    {
        if (use.BoundarySidiCancellation is not { } proof)
            return false;
        if (use.Action?.ActionId != proof.ActionId || use.Id != proof.UseFrameId || proof.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(BoundarySidiSkill).Program?.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySidiCancelledEvent>().Count(record => record == proof) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySidiResultEvent>().Any(record => record.FrameId == proof.ProgramFrameId && record.ActionId == proof.ActionId && record.Outcome is "cancel-only" or "cancel-damage"))
            throw new InvalidOperationException("Sidi cancellation lost its exact original use and committed result.");
        return true;
    }

    internal bool TryBoundarySidiEquipmentCommitted(long id, Card equipment)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } action } use || action.EffectiveKind != equipment.Kind || use.BoundarySidiCancellation is null && !_coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(instance => instance.SkillId == BoundarySidiSkill && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, instance.SkillId, instance.SkillInstanceId) && BoundarySidiRecords(owner.Seat, instance.SkillInstanceId).Any(record => record.HolderSeat == action.ActorSeat))))
            return false;
        var continuation = new ProgramSimpleCardContinuation(equipment.Id, SimpleCardUseEffect.EquipmentPlacement);
        if (_coreCardUseLifecycleOperations.TryBeginCommittedCardUse(id, ProgramCardContinuation.CommittedSimpleCard, simple: continuation))
            return true;
        return ContinueBoundarySidiIneffectiveSimple(id, continuation);
    }

    internal bool ContinueBoundarySidiIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { BoundarySidiCancellation: not null } use || !IsBoundarySidiUseIneffective(use))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || use.Action is not { Type: CardActionType.Use })
            throw new InvalidOperationException("Sidi lost its original simple-card return.");
        foreach (var card in _coreNativeTrickUseOperations.GetCardUsePhysicalCards(id).Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.boundary-sidi.cancelled"));
        var original = use.CardId == 0 ? new Card(0, use.CardKind, Suit.None, 0) : _coreAdvancedLifecycleOperations.GetAdvancedCard(use.CardId);
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, original, use.CardKind);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 53400)]
    internal void AssertBoundarySidi(ProgramSkillFrame frame)
    {
        if (frame.BoundarySidi is not { } draft)
            return;
        if (draft.InstructionIndex != frame.InstructionIndex || !TryBoundarySidiWindow(frame.WindowContext, out var window) || draft.WindowId != window.Id || draft.ActionId != window.Action.ActionId || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, BoundarySidiChoices(frame)))
            throw new InvalidOperationException("Sidi lost its frame-owned private forecast or current first-use choice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.PrepareBoundaryCaoZhenSidi)]
    internal static PromptChoice BoundarySidiPrepareAi(PendingDecision prompt) => BoundarySidiAi(prompt);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryCaoZhenSidi)]
    internal static PromptChoice BoundarySidiResolveAi(PendingDecision prompt) => BoundarySidiAi(prompt);
    internal static PromptChoice BoundarySidiAi(PendingDecision prompt) => prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "draw") ?? prompt.Choices[0];
}
