#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterAHuiNanOlRuntime
{
    internal const string AHuiNanDrawReason = "skill-program.a-hui-nan.jueman.draw";
    internal static bool AHuiNanBasicKind(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_a_hui_nan_ol_AHuiNan.AHuiNanBasicKind(kind);
    internal bool HasAHuiNanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd);

    internal static CardConversionSource AHuiNanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_a_hui_nan_ol_AHuiNan.AHuiNanSource(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27200)]
    internal void ObserveAHuiNanBasicUses(IGameEvent payload)
    {
        if (!HasAHuiNanCapability || !_coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn)
            return;
        var recorded = _coreProgramEventHistoryOperations.ProgramEventHistory<AHuiNanBasicUseRecordedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat).ToArray();
        if (recorded.Length >= 3)
            return;
        if (payload is CardUseDeclaredEvent d)
        {
            if (_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == d.ResolutionId)is not { } use || use.SourceSeat != d.SourceSeat || use.CardId != d.CardId || use.CardKind != d.CardKind || !AHuiNanBasicKind(use.CardKind) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.SourceSeat))
                return;
            if (use.Action is { } a)
            {
                if (a.Type != CardActionType.Use || a.ActorSeat != use.SourceSeat || a.EffectiveKind != use.CardKind)
                    return;
            }
            else if (!AHuiNanLegacyBasicUse(use))
                return;
            if (recorded.Any(e => e.OriginFrameId == use.Id && e.ActionId == use.Action?.ActionId))
                return;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanBasicUseRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.Id, use.Action?.ActionId, use.SourceSeat, use.CardKind, recorded.Length + 1));
            return;
        }

        if (payload is not CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge } response } || response.ActorSeat != response.ProviderSeat || response.ResponderSeat != response.ActorSeat || response.RequesterSeat is not null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(response.ActorSeat) || recorded.Any(e => e.ActionId == response.ActionId))
            return;
        var defending = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == response.ParentActionId && u.CardAttack?.TargetSeat == response.ActorSeat);
        if (defending?.CardAttack is not { EffectiveCardKind: { } incoming } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) || response.OpponentSeat != attack.SourceSeat || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[response.ActorSeat], CardKind.Dodge))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanBasicUseRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, defending.Id, response.ActionId, response.ActorSeat, CardKind.Dodge, recorded.Length + 1));
    }

    internal bool AHuiNanLegacyBasicUse(CardUseFrame use)
    {
        if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 })
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (i <= 0 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame f || f.OwnerSeat != use.SourceSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash)
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(f, p).GetPausedInstruction(f.InstructionIndex).Effect;
        if (use.CardKind == CardKind.Slash)
            return f.SelectedTargetSeats.Count == 1 && f.SelectedTargetSeats.SequenceEqual(use.TargetSeats) && effect is { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash, UseCardActionWindows: false };
        if (use.CardKind != CardKind.Alcohol || effect.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || f.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.TargetSeat != use.SourceSeat || !use.TargetSeats.SequenceEqual([use.SourceSeat]))
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(d => d.Id == context.ParentFrameId)is { } dying && dying.VictimSeat == use.SourceSeat && dying.ResponderIndex >= 0 && dying.ResponderIndex < dying.ResponderSeats.Count && dying.ResponderSeats[dying.ResponderIndex] == use.SourceSeat;
    }

    internal AHuiNanBasicUseSnapshot[] AHuiNanHistory(int turn) => _coreProgramEventHistoryOperations.ProgramEventHistory<AHuiNanBasicUseRecordedEvent>().Where(e => e.ActualTurn == turn).OrderBy(e => e.Index).Select(e => new AHuiNanBasicUseSnapshot(e, _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCurrentSlashFireChangedEvent>().LastOrDefault(c => c.CardUseFrameId == e.OriginFrameId && c.ActionId == e.ActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(e.Kind))?.FinalKind ?? e.Kind)).ToArray();
    internal bool AHuiNanEndingParent(ProgramTriggerCandidate c, ProgramSkillWindowContext context, out DeferredTurnEndFrame? parent)
    {
        parent = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId);
        return HasAHuiNanCapability && c.SkillId == "ol:jueman" && c.BindingId is ("actual-end-own" or "actual-end-foreign") && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.OwnerSeat == c.OwnerSeat && parent is not null && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) == c && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == c.OccurrenceIndex && context.Facts == parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts && c.BindingId == (c.OwnerSeat == parent.OwnerSeat ? "actual-end-own" : "actual-end-foreign") && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash;
    }

    internal bool IsAHuiNanJuemanEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && AHuiNanEndingParent(c, context, out _) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId);
    internal bool AHuiNanStartedBinding(ProgramSkillFrame f)
    {
        if (f.SkillId != "ol:jueman" || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context)
            return false;
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        return AHuiNanEndingParent(c, context, out var parent) && parent!.AfterTurnEnded?.CurrentChild?.FrameId == f.Id && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == SkillProgramTriggerWindow.AfterTurnEnded) == 1;
    }

    SkillProgramStepOutcome global::CardGame.Core.IAHuiNanHost.Resolve(ProgramSkillFrame f) => BeginAHuiNan(f);
    internal SkillProgramStepOutcome BeginAHuiNan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.AHuiNanJueman is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !AHuiNanStartedBinding(f))
            throw new InvalidOperationException("Jueman requires its genuine started actual-turn-end binding.");
        var history = AHuiNanHistory(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        var own = history.Take(2).Count(e => e.Declaration.ActorSeat == f.OwnerSeat);
        var stage = history.Length >= 2 && own == 1 ? AHuiNanJuemanStage.DrawChildren : history.Length == 3 && own == 0 ? AHuiNanJuemanStage.UseChoice : AHuiNanJuemanStage.NoEffect;
        var r = new AHuiNanJuemanReceipt(AHuiNanSource(f), f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, stage)
        {
            History = history,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AHuiNanJueman = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanJuemanStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, history.Length, own, history.Length == 3 ? history[2].Kind : null));
        if (stage == AHuiNanJuemanStage.DrawChildren)
        {
            var count = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(AHuiNanDrawReason)).Count : 0;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AHuiNanJueman = f.AHuiNanJueman!with { DrawCount = count, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanJuemanDrawnEvent(f.Id, f.OwnerSeat, count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        ContinueAHuiNan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> AHuiNanChoices(ProgramSkillFrame f)
    {
        var options = AHuiNanUseTargets(f);
        return Array.AsReadOnly(options.Select((targets, i) => new PromptChoice(new($"jueman.{(f.Id)}.{(i)}"), $"蟨蛮：视为对{(string.Join('、', targets.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)))}使用【{(CardCatalog.Get(f.AHuiNanJueman!.History[2].Kind).DisplayName)}】", [], targets, new Dictionary<string, string> { ["program-action"] = "a-hui-nan-jueman", ["option-index"] = i.ToString(CultureInfo.InvariantCulture), ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishAHuiNan(ProgramSkillFrame f)
    {
        var choices = AHuiNanChoices(f);
        if (choices.Count == 0)
        {
            FinishAHuiNan(f, "no-legal-use");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "蟨蛮：必须选择第三张基本牌的当前合法用法。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("a-hui-nan-jueman")]
    internal void AnswerAHuiNan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AHuiNanJueman: { Stage: AHuiNanJuemanStage.UseChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !AHuiNanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jueman changed its genuine private chooser or current legal basic use.");
        AssertAHuiNan(f);
        var targets = AHuiNanUseTargets(f)[int.Parse(choice.Parameters["option-index"], CultureInfo.InvariantCulture)];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var o = new AHuiNanJuemanUseOrigin(f.Id, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, r.Source, r.GameplayHash, r.ActualTurn, f.OwnerSeat, r.History[2].Kind, string.Join(',', targets));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, AHuiNanJueman = r with { Stage = AHuiNanJuemanStage.NativeUse, Use = o, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanJuemanUseIssuedEvent(o));
        BeginAHuiNanJuemanUse(f, o, targets);
    }

    internal bool DrainAHuiNan(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    internal void ContinueAHuiNan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AHuiNanJueman: { } r } f || f.Id != id)
            return;
        AssertAHuiNan(f);
        if (r.Stage is AHuiNanJuemanStage.DrawChildren or AHuiNanJuemanStage.NativeChildren)
        {
            if (DrainAHuiNan(id))
                return;
            FinishAHuiNan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), r.Stage == AHuiNanJuemanStage.DrawChildren ? "draw" : "used");
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage == AHuiNanJuemanStage.NoEffect)
        {
            FinishAHuiNan(f, "no-effect");
            return;
        }

        if (r.Stage == AHuiNanJuemanStage.UseChoice)
            PublishAHuiNan(f);
    }

    internal void FinishAHuiNan(ProgramSkillFrame f, string outcome)
    {
        var r = f.AHuiNanJueman!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanJuemanCompletedEvent(f.Id, r.DrawCount, r.Use is not null, outcome));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AHuiNanJueman = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27200)]
    internal bool ResumeAHuiNan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AHuiNanJueman: not null } f || f.Id != id)
            return false;
        ContinueAHuiNan(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27200)]
    internal bool ContinuePaidAHuiNan(ProgramSkillFrame f) => f.AHuiNanJueman is not null ? ValidAHuiNan(f) : f.InstructionIndex is 0 or 1 && AHuiNanStartedBinding(f);
}
