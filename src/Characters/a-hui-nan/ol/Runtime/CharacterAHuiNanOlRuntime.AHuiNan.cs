#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterAHuiNanOlRuntime
{
    internal const string AHuiNanDrawReason = "skill-program.a-hui-nan.jueman.draw";
    internal static bool AHuiNanBasicKind(CardKind kind) => RuntimeSkills.RuntimeRules_a_hui_nan_ol_AHuiNan.AHuiNanBasicKind(kind);
    internal bool HasAHuiNanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd);

    internal static CardConversionSource AHuiNanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_a_hui_nan_ol_AHuiNan.AHuiNanSource(f);
    internal AHuiNanBasicUseSnapshot[] AHuiNanHistory(int turn) => RuntimeSkills.RuntimeRules_Core_CurrentTurnUseKinds.ActualTurnBasicUses(
        _coreAlternatingBenefitsOperations.CompleteProgramEventHistory(), turn, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
        .Take(3).Select(e => new AHuiNanBasicUseSnapshot(e.ActorSeat, e.EffectiveCardKind!.Value)).ToArray();
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
        return AHuiNanEndingParent(c, context, out var parent) && parent!.AfterTurnEnded?.CurrentChild?.FrameId == f.Id && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == SkillProgramTriggerWindow.AfterTurnEnded) == 1;
    }

    SkillProgramStepOutcome IAHuiNanHost.Resolve(ProgramSkillFrame f) => BeginAHuiNan(f);
    internal SkillProgramStepOutcome BeginAHuiNan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.AHuiNanJueman is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !AHuiNanStartedBinding(f))
            throw new InvalidOperationException("Jueman requires its genuine started actual-turn-end binding.");
        var history = AHuiNanHistory(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        var own = history.Take(2).Count(e => e.ActorSeat == f.OwnerSeat);
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
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AHuiNanJueman: { Stage: AHuiNanJuemanStage.UseChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !AHuiNanChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jueman changed its genuine private chooser or current legal basic use.");
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
