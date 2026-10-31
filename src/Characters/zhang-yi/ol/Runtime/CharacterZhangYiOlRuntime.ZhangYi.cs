#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYiOlRuntime
{
    internal const string ZhangYiDrawReason = "skill-program.zhang-yi.kangrui.draw";
    internal bool HasZhangYiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveKangruiFirstOwnTurnDamage);

    internal CardConversionSource ZhangYiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IZhangYiProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginZhangYi(f, op);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26400)]
    internal void ObserveZhangYiActualDamage(IGameEvent payload)
    {
        if (!HasZhangYiCapability || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber <= 0 || payload is not DamageAppliedEvent { Amount: > 0 } e || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != e.SourceSeat || damage.TargetSeat != e.TargetSeat || damage.Amount != e.Amount || attack.SourceSeat != e.SourceSeat || attack.TargetSeat != e.TargetSeat || attack.IsSourceLess != e.SourceLess || !attack.DamageWasApplied)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiActualDamageFactEvent>().Any(x => x.DamageFrameId == damage.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiActualDamageFactEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, damage.Id, attack.ResolutionId, e.SourceSeat, e.TargetSeat, e.Amount, e.SourceLess));
        if (!e.SourceLess)
            foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiArmedEvent>().Where(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.RecipientSeat == e.SourceSeat).ToArray())
                if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiHandLimitSetEvent>().Any(x => x.GrantFrameId == grant.FrameId))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiKangruiHandLimitSetEvent(grant.FrameId, damage.Id, e.SourceSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
    }

    internal bool IsFirstZhangYiOwnTurnDamage(ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.AfterDamageApplied || c.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || c.Amount <= 0 || c.DamageFrameId is not { } damageId || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id != c.ParentFrameId)
            return false;
        var first = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiActualDamageFactEvent>().FirstOrDefault(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        return first is not null && first.DamageFrameId == damageId && first.ActualTurnOwnerSeat == c.TargetSeat && first.Amount == c.Amount;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26400)]
    internal bool CanOfferZhangYi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveKangruiFirstOwnTurnDamage))
            return IsFirstZhangYiOwnTurnDamage(context);
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveDianjunActualTurnEnd))
            return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && context.TargetSeat == candidate.OwnerSeat;
        return true;
    }

    internal SkillProgramStepOutcome BeginZhangYi(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var dianjun = op == SkillProgramEffectOp.ResolveDianjunActualTurnEnd;
        if (f.ZhangYi is not null || f.InstructionIndex != 1 || f.WindowContext is not { } c || f.SkillId != (dianjun ? "ol:dianjun" : "ol:kangrui") || !CanOfferZhangYi(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c) || c.TargetSeat is not { } target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            throw new InvalidOperationException("Zhang Yi requires its exact actual end or first own-turn positive damage.");
        var r = new ZhangYiReceipt(ZhangYiSource(f), f.GameplayHash, f.InstructionIndex, c.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, target, dianjun ? ZhangYiStage.DianjunDamage : ZhangYiStage.KangruiDraw, c.DamageFrameId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangYi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiResolutionStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentWindowId, r.ActualTurnNumber, target, r.DamageFrameId));
        if (dianjun)
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 1, sourceLess: true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiKangruiDrawIssuedEvent(f.Id, f.OwnerSeat, target, r.ActualTurnNumber, r.SequenceBefore));
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(ZhangYiDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangYi = f.ZhangYi!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueZhangYi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ZhangYiChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { "recover", "damage" }.Select(branch => new PromptChoice(new($"zhang-yi.{(f.Id)}.{(branch)}"), branch == "recover" ? "令其回复1点体力" : "令其本回合下次造成的伤害+1", [], [f.ZhangYi!.RecipientSeat], new Dictionary<string, string> { ["program-action"] = "zhang-yi-kangrui", ["branch"] = branch })).ToArray());
    internal void PublishZhangYi(ProgramSkillFrame f)
    {
        var r = f.ZhangYi!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "亢锐：请选择对本次受伤角色的效果。", [], [r.RecipientSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.RecipientSeat,
            Choices = ZhangYiChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhang-yi-kangrui")]
    internal void AnswerZhangYi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangYi: { Stage: ZhangYiStage.KangruiChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ZhangYiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Kangrui lost its paid draw, frozen injured actor or exact choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        var recipient = _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat];
        var recovery = branch == "recover" && recipient.IsAlive && recipient.HasHpValue ? Math.Min(1, Math.Max(0, recipient.MaxHp - recipient.Hp)) : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ZhangYi = r with { Choice = branch, RecoveryRequested = recovery, Stage = ZhangYiStage.KangruiRecovery } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiKangruiChoiceEvent(f.Id, r.Source, r.RecipientSeat, r.ActualTurnNumber, branch, recovery));
        if (recovery > 0)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.RecipientSeat, recovery, null, null);
        ContinueZhangYi(f.Id);
    }

    internal bool DrainZhangYiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueZhangYi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhangYi: { } r } f && f.Id == id)
        {
            if (f.AttackAttempt is not null)
            {
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(f.Id));
                return;
            }

            if (DrainZhangYiChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ZhangYi!;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishZhangYi(f);
                return;
            }

            if (r.Stage == ZhangYiStage.DianjunDamage)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishZhangYi(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangYi = r with { Stage = ZhangYiStage.DianjunExtraPlay } });
                if (!_coreProgramLifecycleOperations.TryBeginZhangYiDianjunExtraPlay(f))
                    throw new InvalidOperationException("Dianjun lost its actual end-owned extra Play return.");
                return;
            }

            if (r.Stage == ZhangYiStage.DianjunExtraPlay)
                return;
            if (r.Stage == ZhangYiStage.DianjunReturned)
            {
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangYi = r with { Stage = ZhangYiStage.DianjunCleanupChildren, CleanupBefore = before, CleanupAfter = before } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiDianjunCleanupStartedEvent(f.Id, f.OwnerSeat, r.ActualTurnNumber, before));
                _coreProgramLifecycleOperations.ExpireEndOwnedPlayEffects(f);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangYi = f.ZhangYi!with { CleanupAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiDianjunCleanupIssuedEvent(f.Id, f.OwnerSeat, r.ActualTurnNumber, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                continue;
            }

            if (r.Stage == ZhangYiStage.DianjunCleanupChildren)
            {
                FinishZhangYi(f);
                return;
            }

            if (r.Stage == ZhangYiStage.KangruiDraw)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangYi = r with { Stage = ZhangYiStage.KangruiChoice } });
                continue;
            }

            if (r.Stage == ZhangYiStage.KangruiChoice)
            {
                PublishZhangYi(f);
                return;
            }

            if (r.Stage == ZhangYiStage.KangruiRecovery)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiKangruiArmedEvent(f.Id, r.Source, r.RecipientSeat, r.ActualTurnNumber, r.Choice == "damage"));
                FinishZhangYi(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            throw new InvalidOperationException("Unknown Zhang Yi continuation stage.");
        }
    }

    internal void FinishZhangYi(ProgramSkillFrame f)
    {
        var completed = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && (f.SkillId == "ol:kangrui" || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangYi = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26400)]
    internal bool ResumeZhangYi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangYi: not null } f || f.Id != id)
            return false;
        ContinueZhangYi(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26400)]
    internal bool ContinuePaidZhangYi(ProgramSkillFrame f) => f.ZhangYi is { } r && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiResolutionStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ParentWindowId == r.ParentWindowId && e.ActualTurnNumber == r.ActualTurnNumber && e.RecipientSeat == r.RecipientSeat && e.DamageFrameId == r.DamageFrameId) == 1;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveKangruiFirstOwnTurnDamage)]
    internal PromptChoice AiZhangYi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First(c => c.Parameters["branch"] == (_coreMatchParticipantsPlayersAccess._players[f.ZhangYi!.RecipientSeat].Hp < _coreMatchParticipantsPlayersAccess._players[f.ZhangYi.RecipientSeat].MaxHp ? "recover" : "damage"));
}
