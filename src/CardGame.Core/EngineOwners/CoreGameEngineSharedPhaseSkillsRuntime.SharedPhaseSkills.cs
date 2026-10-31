#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSharedPhaseSkillsRuntime
{
    internal SkillGrantPhaseExpiry CurrentSharedSkillPhase => new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);

    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 27180)]
    internal IEnumerable<LegalAction> BuildSharedProgramActivations(CharacterState actor)
    {
        if (!actor.IsAlive || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0)
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat).OrderBy(player => player.Seat))
            foreach (var program in _coreSkillProgramsOperations.EnabledActivationPrograms(owner))
                foreach (var activation in program.Activations.Where(activation => activation.OtherCharactersOnly))
                {
                    if (_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSharedActivationStartedEvent>().Count(item => item.ActorSeat == actor.Seat && item.Source.OwnerSeat == owner.Seat && item.Source.SkillId == program.Id && item.Source.BindingId == activation.Id && item.Phase == CurrentSharedSkillPhase) >= activation.UsesPerPhase || activation.Effects.Any(effect => effect.Op == SkillProgramEffectOp.LoseHp && (!actor.HasHpValue || actor.Hp <= 0)))
                        continue;
                    yield return new(LegalActionKind.UseProgramSkill, null, null, $"发动 {(_coreMatchParticipantsPlayersAccess._players[owner.Seat].General.Name)} 的【{(_contentRegistryOperations._contentRegistry.GetSkill(program.Id).Name)}】")
                    {
                        ProgramSkillId = program.Id,
                        ProgramActivationId = activation.Id,
                        ProgramSkillOwnerSeat = owner.Seat,
                        ProgramAiHint = new(1, 0, activation.Effects.Where(effect => effect.Op == SkillProgramEffectOp.LoseHp).Sum(effect => effect.Amount), 0, 0, 0, true, false)
                        {
                            ValueAdjustment = 6
                        }
                    };
                }
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 27180)]
    internal bool RejectUnboundSharedProgramActivation(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (command.SkillOwnerSeat is not null || !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(command.SkillId, out var skill) || skill.Program?.Activations.SingleOrDefault(activation => activation.Id == command.ActivationId)?.OtherCharactersOnly != true)
            return false;
        result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "A shared skill requires its exact published owner.");
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 27180)]
    internal bool TryExecuteSharedProgramActivation(CharacterState actor, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (action.ProgramSkillOwnerSeat is not { } ownerSeat || action.ProgramSkillId is not { } skillId || _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program?.Activations.SingleOrDefault(item => item.Id == action.ProgramActivationId)is not { OtherCharactersOnly: true } activation)
            return false;
        if (cards.Count != 0 || targets.Count != 0 || !BuildSharedProgramActivations(actor).Any(candidate => candidate.ProgramSkillId == skillId && candidate.ProgramActivationId == activation.Id && candidate.ProgramSkillOwnerSeat == ownerSeat))
            throw new InvalidOperationException("The shared activation no longer matches its actor, owner and exact phase.");
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var program = _coreSkillProgramsOperations.GetEnabledSkillProgram(owner, skillId);
        var frame = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, ownerSeat, skillId, activation.Id, program.GameplayHash, 0, [], [])
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, skillId),
            ActivatingSeat = actor.Seat,
            ActivationPhase = CurrentSharedSkillPhase
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSharedActivationStartedEvent(frame.Id, new(skillId, activation.Id, ownerSeat, frame.SkillInstanceId), actor.Seat, frame.ActivationPhase));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, ownerSeat, skillId, activation.Id));
        _coreMatchLogOperations.AddLog("ActiveSkill", $"{(actor.Name)} 发动 {(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry.GetSkill(skillId).Name)}】。", actor.Seat, ownerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IRandomPhaseSkillHost.Offer(ProgramSkillFrame frame, SkillProgramEffect effect, int recipientSeat) => OfferRandomPhaseSkill(frame, effect, recipientSeat);
    internal bool IsLiveRandomPhaseSkillRecipient(ProgramSkillFrame frame, int recipientSeat, SkillGrantPhaseExpiry phase) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && CurrentSharedSkillPhase == phase && recipientSeat == phase.PhaseActorSeat && _coreMatchParticipantsPlayersAccess._players[recipientSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramPlayPhaseSkillBoundaryClosedEvent>().Any(item => item.Expiry == phase);
    internal SkillProgramStepOutcome OfferRandomPhaseSkill(ProgramSkillFrame supplied, SkillProgramEffect effect, int recipientSeat)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var phase = frame.ActivationPhase ?? CurrentSharedSkillPhase;
        if (!IsLiveRandomPhaseSkillRecipient(frame, recipientSeat, phase))
            return SkillProgramStepOutcome.Continue;
        foreach (var id in effect.SkillIds)
            _ = _contentRegistryOperations._contentRegistry.GetSkill(id);
        if (frame.RandomPhaseSkillOffer is not null)
            throw new InvalidOperationException("A random phase skill offer cannot be sampled twice.");
        var pool = effect.SkillIds.ToList();
        _randomOperations._random.Shuffle(pool);
        var offered = Array.AsReadOnly(pool.Take(effect.Amount).ToArray());
        var receipt = new ProgramRandomPhaseSkillOffer(frame.InstructionIndex, recipientSeat, phase, offered);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RandomPhaseSkillOffer = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomPhaseSkillsOfferedEvent(frame.Id, receipt.InstructionIndex, frame.OwnerSeat, recipientSeat, phase, offered));
        PresentRandomPhaseSkillOffer(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PresentRandomPhaseSkillOffer(ProgramSkillFrame frame)
    {
        var offer = frame.RandomPhaseSkillOffer!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = offer.SkillIds.Select(id => new PromptChoice(new ChoiceId($"phase-skill.frame-{(frame.Id)}.{(id)}"), $"【{(_contentRegistryOperations._contentRegistry.GetSkill(id).Name)}】：{(_contentRegistryOperations._contentRegistry.GetSkill(id).Description)}", [], [], new Dictionary<string, string> { ["program-action"] = "random-phase-skill", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["skill-id"] = id })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(skill.Name)}】为 {(_coreMatchParticipantsPlayersAccess._players[offer.RecipientSeat].Name)} 选择本阶段获得的技能。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = offer.RecipientSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 选择技能", skill.Description),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("random-phase-skill")]
    internal void AnswerRandomPhaseSkillOffer(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The skill offer lost its owning frame.");
        var offer = frame.RandomPhaseSkillOffer ?? throw new InvalidOperationException("The skill offer lost its frozen candidates.");
        var chosen = choice.Parameters.GetValueOrDefault("skill-id");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || choice.Cards.Count != 0 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || chosen is null || !offer.SkillIds.Contains(chosen, StringComparer.Ordinal))
            throw new InvalidOperationException("The skill choice changed its chooser, frame or offered identity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (IsLiveRandomPhaseSkillRecipient(frame, offer.RecipientSeat, offer.Phase))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomPhaseSkillChosenEvent(frame.Id, frame.OwnerSeat, offer.RecipientSeat, chosen, offer.Phase));
            _corePlayPhaseSkillGrantsOperations.IssueProgramPhaseSkillGrants(frame, offer.RecipientSeat, [chosen], offer.Phase);
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[offer.RecipientSeat].Name)} 获得【{(_contentRegistryOperations._contentRegistry.GetSkill(chosen).Name)}】直到本出牌阶段结束。", offer.RecipientSeat);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { RandomPhaseSkillOffer = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferRandomPlayPhaseSkill)]
    internal PromptChoice SelectRandomPhaseSkillForAi(PendingDecision decision) => decision.Choices.First();
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27180)]
    internal void AssertSharedPhaseSkillFrame(ProgramSkillFrame frame)
    {
        if (frame.ActivatingSeat is { } actor && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || actor == frame.OwnerSeat || frame.ActivationPhase is not { } phase || phase.PhaseActorSeat != actor || phase.PhaseInstanceId <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSharedActivationStartedEvent>().Count(item => item.FrameId == frame.Id && item.ActorSeat == actor && item.Phase == phase) != 1))
            throw new InvalidOperationException("The shared activation lost its exact actor and originating phase.");
        if (frame.RandomPhaseSkillOffer is not { } offer)
            return;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.OfferRandomPlayPhaseSkill || offer.InstructionIndex != frame.InstructionIndex || offer.SkillIds.Count != effect.Amount || offer.SkillIds.Distinct(StringComparer.Ordinal).Count() != offer.SkillIds.Count || offer.SkillIds.Any(id => !effect.SkillIds.Contains(id, StringComparer.Ordinal)) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRandomPhaseSkillsOfferedEvent>().Count(item => item.FrameId == frame.Id && item.InstructionIndex == offer.InstructionIndex && item.SkillIds.SequenceEqual(offer.SkillIds) && item.Phase == offer.Phase && item.ChooserSeat == frame.OwnerSeat && item.RecipientSeat == offer.RecipientSeat) != 1)
            throw new InvalidOperationException("The random phase skill offer lost its frozen program and issued candidates.");
    }
}
