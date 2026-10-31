#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 40)]
    internal bool ZixiFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasZixiEffectsCapability && ZixiEffectsKind(query.Kind);
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 20)]
    internal bool ZixiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasZixiEffectsCapability;
    internal const string ZixiEffectsDrawReason = "skill-program.le-da-qiao.zixi.draw";
    internal const string ZixiEffectsObtainReason = "skill-program.le-da-qiao.zixi.obtain";
    internal bool HasZixiEffectsCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZixiUniqueTargetJudgmentEffects);

    internal static CardConversionSource ZixiEffectsSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_da_qiao_le_LeDaQiaoZixiEffects.ZixiEffectsSource(f);
    internal static bool ZixiEffectsKind(CardKind kind) => RuntimeSkills.RuntimeRules_da_qiao_le_LeDaQiaoZixiEffects.ZixiEffectsKind(kind);
    internal IReadOnlyList<Card> ZixiJudgmentAreaCards(int target)
    {
        // Native judgment results temporarily share the physical Judgment zone,
        // but are not cards placed in that character's judgment area.
        var results = _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Where(j => j.TargetSeat == target && j.CardId is not null).Select(j => j.CardId!.Value).ToHashSet();
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => !results.Contains(c.Id)).ToArray());
    }

    SkillProgramStepOutcome IZixiEffectsHost.ResolveZixiUniqueTargetJudgmentEffects(ProgramSkillFrame f) => BeginZixiEffects(f);
    internal bool TryGetZixiEffectsUse(int actor, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window, out int target)
    {
        use = null !;
        window = null !;
        target = -1;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != id.ParentCardUseFrameId || w.CompletedResponseReturn is not null || w.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(id.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } u || a.ActionId != id.CardActionId || a.ActorSeat != actor || u.SourceSeat != actor || a.EffectiveKind != u.CardKind || w.Action.ActionId != a.ActionId || w.Action.ActorSeat != actor || !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !a.TargetSeats.SequenceEqual(u.TargetSeats) || !ZixiEffectsKind(u.CardKind))
            return false;
        // Borrowed Sword designates weapon owners; its following seats belong to
        // the separate Slash requests, not to this ordinary trick's target set.
        var actual = u.CardKind == CardKind.BorrowedSword ? u.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : u.TargetSeats.ToArray();
        if (actual is not [var sole] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sole) || u.CardKind == CardKind.BorrowedSword && u.TargetSeats.Count != 2)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != u.Id)
            return false;
        use = u;
        window = w;
        target = sole;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28720)]
    internal bool CanOfferZixiEffects(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveZixiUniqueTargetJudgmentEffects))
            return true;
        return candidate.SkillId == "le:zixi" && candidate.BindingId == "unique-target-judgment-effects" && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && TryGetZixiEffectsUse(candidate.OwnerSeat, context, out var use, out var window, out var target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && ZixiJudgmentAreaCards(target).Count is >= 1 and <= 3 && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZixiEffectsStartedEvent>().Any(e => e.UseId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginZixiEffects(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZixiEffects is not null)
        {
            ContinueZixiEffects(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != "le:zixi" || f.TriggerId != "unique-target-judgment-effects" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !CanOfferZixiEffects(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), context) || !TryGetZixiEffectsUse(f.OwnerSeat, context, out var use, out var window, out var target))
            throw new InvalidOperationException("Zixi needs its accepted true Use, sole actual target and original finalized-target window.");
        var count = ZixiJudgmentAreaCards(target).Count;
        var r = new ZixiEffectsReceipt(ZixiEffectsSource(f), f.GameplayHash, use.Id, use.Action!.ActionId, window.Id, target, count, use.CardKind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Stage = count == 2 ? ZixiEffectsStage.Drawing : ZixiEffectsStage.ObtainChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZixiEffects = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.TargetSeat, count, r.CardKind, r.SequenceBefore));
        if (count == 1)
        {
            IssueZixiExtraEffect(f, use, window);
            FinishZixiEffects(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (count == 2)
        {
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(ZixiEffectsDrawReason));
            return SkillProgramStepOutcome.AwaitChild;
        }

        var entities = ZixiJudgmentAreaCards(target).Select(c => new ZixiEffectsEntity(c.Id, c.Kind, CardLocation.Judgment(target))).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => new ZixiEffectsEntity(c.Id, c.Kind, CardLocation.Equipment(target)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZixiEffects = r with { Entities = entities }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && entities.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new(ZixiEffectsObtainReason), (batch, moves) => CaptureZixiEffectsObtain(f.Id, batch, moves));
        else
            CaptureZixiEffectsObtain(f.Id, 0, []);
        ContinueZixiEffects(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void CaptureZixiEffectsObtain(long id, long batch, IReadOnlyList<CardMovementRecord> moves)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.ZixiEffects!;
        if (r.JudgmentCount != 3 || r.Stage != ZixiEffectsStage.ObtainChildren || r.ActualCount != -1 || batch > 0 && (moves.Count != r.Entities.Count || !moves.Select(m => m.CardId).SequenceEqual(r.Entities.Select(e => e.CardId))))
            throw new InvalidOperationException("Zixi cannot repeat or replace its original whole Judgment/Equipment obtain.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZixiEffects = r with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)) } });
        for (var i = 0; i < moves.Count; i++)
        {
            var m = moves[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsEntityObtainedEvent(id, i, m.CardId, m.CardKind, m.From, m.To, m.Sequence, batch));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsObtainPaidEvent(id, batch, moves.Count, moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)), r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal bool DrainZixiEffects(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueZixiEffects(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZixiEffects: { } r } f || f.Id != id)
            return;
        if (f.NativeDraw is not null || f.AttackAttempt is not null)
            return;
        if (r.Stage == ZixiEffectsStage.Drawing)
            throw new InvalidOperationException("Zixi's draw must return through its original typed producer.");
        if (DrainZixiEffects(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.ZixiEffects!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == ZixiEffectsStage.ObtainChildren && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZixiEffects = r with { Stage = ZixiEffectsStage.DamageChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsDamageIssuedEvent(f.Id, f.OwnerSeat, r.TargetSeat, 3));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 3);
            return;
        }

        FinishZixiEffects(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveZixiUniqueTargetJudgmentEffects)]
    internal void ReturnZixiEffectsDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ZixiEffects!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZixiEffects = r with { Stage = ZixiEffectsStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsDrawReturnedEvent(f.Id, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueZixiEffects(f.Id);
    }

    internal void FinishZixiEffects(ProgramSkillFrame f)
    {
        var r = f.ZixiEffects!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiEffectsCompletedEvent(f.Id, r.UseId, r.JudgmentCount));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZixiEffects = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28720)]
    internal bool ResumeZixiEffects(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZixiEffects: not null } f || f.Id != id)
            return false;
        if (f.NativeDraw is null)
            ContinueZixiEffects(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28720)]
    internal bool ReturnZixiEffectsMovement(ProgramSkillFrame f)
    {
        if (f.ZixiEffects is null || f.PendingMovementContinuation is null)
            return false;
        ContinueZixiEffects(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28720)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28720)]
    internal bool ContinueIssuedZixiEffects(ProgramSkillFrame f) => ValidZixiEffectsStarted(f);
}
