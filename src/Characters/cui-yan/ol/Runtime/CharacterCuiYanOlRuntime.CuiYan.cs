#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    internal const string CuiYanDrawReason = "skill-program.cui-yan.shutong.draw";
    internal static CardConversionSource CuiYanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cui_yan_ol_CuiYan.CuiYanSource(f);
    SkillProgramStepOutcome ICuiYanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCuiYan(f, op);
    internal ProgramCardTriggerWindowFrame? CurrentCuiYanUseWindow(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || context.OwnerSeat != c.OwnerSeat || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex]) != c || window.ParentFrameId != identity.ParentCardUseFrameId || window.Action.ActionId != identity.CardActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActorSeat != c.OwnerSeat || use.SourceSeat != c.OwnerSeat || a.ActionId != window.Action.ActionId)
            return null;
        return window;
    }

    internal bool CurrentCuiYanEnd(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterTurnEnded || context.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } p || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(p) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p) != c || p.TurnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || p.OwnerSeat != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || context.SourceSeat != p.OwnerSeat || context.TargetSeat != p.OwnerSeat)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanEndQualifiedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == p.OwnerSeat && e.OwnerSeat == c.OwnerSeat && e.UsedOrPlayed && e.WasTargeted);
    }

    internal IReadOnlyList<int> CuiYanDamageTargets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && (_characterSunYuanOlOperations.HasSayingRoundRange(owner, p.Seat) || _coreNativeRuleQueriesOperations.GetCombatDistance(owner, p.Seat) <= _coreNativeRuleQueriesOperations.GetAttackRange(owner))).Select(p => p.Seat).ToArray());
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27470)]
    internal bool CanOfferCuiYan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e] || !CuiYanComposition.Handles(e.Op))
            return true;
        return e.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse ? CurrentCuiYanUseWindow(c, context)is not null : CurrentCuiYanEnd(c, context) && CuiYanDamageTargets(c.OwnerSeat).Count > 0;
    }

    internal SkillProgramStepOutcome BeginCuiYan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext ?? throw new InvalidOperationException("Cui Yan requires its exact native trigger.");
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (f.CuiYan is not null || f.InstructionIndex != 1 || f.TriggerId is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !CanOfferCuiYan(c, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Cui Yan lost its unpaid current source or actual boundary.");
        var r = new CuiYanReceipt(CuiYanSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, op == SkillProgramEffectOp.ResolveShutongFinalizedUse ? CuiYanStage.DrawChildren : CuiYanStage.DamageChoice);
        if (op == SkillProgramEffectOp.ResolveShutongFinalizedUse)
        {
            var w = CurrentCuiYanUseWindow(c, context)!;
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)!;
            var a = use.Action!;
            var targets = a.EffectiveDesignatedTargetSeats.Distinct().ToArray();
            if (targets.Length == 0 && !use.TargetsAdjusted && (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Equipment || use.CardKind is CardKind.DrawTwo or CardKind.Peach or CardKind.Alcohol))
                targets = [use.SourceSeat];
            var same = targets.Length > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue && targets.All(s => _coreMatchParticipantsPlayersAccess._players[s].HasHpValue && _coreMatchParticipantsPlayersAccess._players[s].Hp == _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            r = r with
            {
                UseId = use.Id,
                ActionId = a.ActionId,
                Suit = CuiYanTrueUseSuit(a.PhysicalCards.Select(x => x.CardId).ToArray(), a.EffectiveSuit),
                Targets = targets,
                DrawQualified = same
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CuiYan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner, r.UseId, r.ActionId, r.Suit, r.DrawQualified));
        for (var i = 0; i < r.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanFrozenTargetEvent(f.Id, i, r.Targets[i]));
        if (op == SkillProgramEffectOp.ResolveZhijieActualTurnEnd)
        {
            PublishCuiYanDamage(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (r.Suit != Suit.None)
            foreach (var seat in r.Targets.Append(f.OwnerSeat).Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShutongNextUseGrantedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.ActualTurn, r.TurnOwner, seat, r.Suit));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CuiYan = r with { Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var count = r.DrawQualified && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(CuiYanDrawReason)).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CuiYan = f.CuiYan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualDraw = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanDrawIssuedEvent(f.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinueCuiYan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> CuiYanDamageChoices(ProgramSkillFrame f) => Array.AsReadOnly(CuiYanDamageTargets(f.OwnerSeat).Select(s => new PromptChoice(new($"cui-yan.{(f.Id)}.damage.{(s)}"), "对" + _coreMatchParticipantsPlayersAccess._players[s].Name + "造成1点伤害", [], [s], new Dictionary<string, string> { { "program-action", "cui-yan-damage" }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    internal void PublishCuiYanDamage(ProgramSkillFrame f)
    {
        var choices = CuiYanDamageChoices(f);
        if (choices.Count == 0)
        {
            FinishCuiYan(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "直节：选择攻击范围内的一名其他角色造成1点伤害。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("cui-yan-damage")]
    internal void AnswerCuiYanDamage(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CuiYan: { Stage: CuiYanStage.DamageChoice } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CuiYanDamageChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zhijie lost its actual native range/target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = choice.Targets.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, CuiYan = f.CuiYan!with { Stage = CuiYanStage.DamageChildren, DamageTarget = target } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanDamageIssuedEvent(f.Id, f.OwnerSeat, target));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
    }

    internal void ContinueCuiYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CuiYan: { } r } f || f.Id != id)
            return;
        if (r.Stage == CuiYanStage.DamageChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCuiYanDamage(f);
            return;
        }

        if (r.Stage == CuiYanStage.DamageChildren && f.AttackAttempt is not null)
            return;
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishCuiYan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishCuiYan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CuiYan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27470)]
    internal bool ResumeCuiYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CuiYan: not null } f || f.Id != id)
            return false;
        ContinueCuiYan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27470)]
    internal bool ReturnCuiYanMovement(ProgramSkillFrame f)
    {
        if (f.CuiYan is null)
            return false;
        ContinueCuiYan(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27470)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27470)]
    internal bool ContinuePaidCuiYan(ProgramSkillFrame f)
    {
        if (f.CuiYan is not { Stage: not CuiYanStage.DamageChoice })
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveZhijieActualTurnEnd)]
    internal PromptChoice AiCuiYanDamage(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])) ?? p.Choices.First();
}
