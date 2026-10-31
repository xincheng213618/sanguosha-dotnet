#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGameActivationAwakeningRuntime
{
    internal long[] GameProgramActivationFrames(int owner, string skill, string activation, long beforeFrame = long.MaxValue) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillStartedEvent>().Where(e => e.OwnerSeat == owner && e.SkillId == skill && e.ActivationId == activation && e.FrameId < beforeFrame).Select(e => e.FrameId).Distinct().Order().ToArray();
    internal int GameProgramActivationCount(int ownerSeat, string skillId, string activationId) => GameProgramActivationFrames(ownerSeat, skillId, activationId).Length;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 950)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 10)]
    internal bool CanOfferGameActivationAwakening(int ownerSeat, string skillId, SkillProgramTrigger trigger) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.AwakenAfterGameActivations) || trigger.Effects is [{ Op: SkillProgramEffectOp.AwakenAfterGameActivations } effect] && (_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(ownerSeat, skillId, trigger.Id) || GameProgramActivationCount(ownerSeat, effect.SourceBind!, effect.StateId!) >= effect.MinimumValue);
    internal bool ExactGameActivationAwakeningParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Continuation: ProgramLifecycleContinuation.NormalTurnStart } parent || parent.OwnerSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id;
    }

    internal SkillProgramStepOutcome BeginGameActivationAwakening(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        if (f.GameActivationAwakening is not null || f.InstructionIndex != 1 || !ExactGameActivationAwakeningParent(f) || trigger.Effects is not [var configured] || configured != effect || !CanOfferGameActivationAwakening(f.OwnerSeat, f.SkillId, trigger) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            throw new InvalidOperationException("Game-activation awakening lost its exact preparation candidate or actual activation threshold.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = owner.MaxHp;
        var after = Math.Max(1, before - 1);
        var r = new ProgramGameActivationAwakeningReceipt(1, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, effect.SourceBind!, effect.StateId!, effect.MinimumValue, GameProgramActivationFrames(f.OwnerSeat, effect.SourceBind!, effect.StateId!, f.Id), effect.SkillIds, before, after, owner.Hp, Math.Min(owner.Hp, after), GameActivationAwakeningStage.MaximumPaid);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameActivationAwakening = r });
        // The existing primitive owns max-HP clamping and native MaximumHp health
        // observations. This family adds a typed continuation, not a Loss event.
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameActivationAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.LifecycleFrameId, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.CountedSkillId, r.CountedActivationId, r.MinimumValue, r.ActivationFrameIds.Count, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 220)]
    internal bool ResumeGameActivationAwakening(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.GameActivationAwakening is not { } r)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.GameActivationAwakening!;
        if (r.Stage == GameActivationAwakeningStage.MaximumPaid && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameActivationAwakening = r with { Stage = GameActivationAwakeningStage.GrantsIssued } });
            _coreProgramSkillChangesOperations.GrantProgramSkills(f, r.SkillIds);
            foreach (var skill in r.SkillIds)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameActivationAwakeningGrantIssuedEvent(id, f.OwnerSeat, f.SkillId, skill));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == GameActivationAwakeningStage.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameActivationAwakening = r with { Stage = GameActivationAwakeningStage.Complete } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameActivationAwakeningCompletedEvent(id, r.Stage == GameActivationAwakeningStage.GrantsIssued, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    internal bool ValidGameActivationAwakeningReceipt(ProgramSkillFrame f)
    {
        if (f.GameActivationAwakening is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != 1 || !ExactGameActivationAwakeningParent(f) || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.LifecycleFrameId != f.WindowContext!.ParentFrameId || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || r.ActualTurnOwnerSeat != f.OwnerSeat || !Enum.IsDefined(r.Stage) || r.MaximumBefore < 1 || r.MaximumAfter != Math.Max(1, r.MaximumBefore - 1) || r.HpAfter != Math.Min(r.HpBefore, r.MaximumAfter) || r.MinimumValue < 1 || r.ActivationFrameIds.Count < r.MinimumValue && !_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningReceipt(f) || !r.ActivationFrameIds.SequenceEqual(GameProgramActivationFrames(f.OwnerSeat, r.CountedSkillId, r.CountedActivationId, f.Id)) || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.AwakenAfterGameActivations } e] || e.SourceBind != r.CountedSkillId || e.StateId != r.CountedActivationId || e.MinimumValue != r.MinimumValue || !e.SkillIds.SequenceEqual(r.SkillIds) || r.SkillIds.Count == 0 || r.SkillIds.Distinct(StringComparer.Ordinal).Count() != r.SkillIds.Count || f.PendingMovementContinuation is not null)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramBindingStartedEvent>().Count(x => x.FrameId == f.Id && x.OwnerSeat == f.OwnerSeat && x.SkillId == f.SkillId && x.BindingId == f.TriggerId && x.SkillInstanceId == f.SkillInstanceId && x.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) != 1 || history.OfType<GameActivationAwakeningMaximumPaidEvent>().Count(x => x.FrameId == f.Id) != 1 || history.OfType<GameActivationAwakeningMaximumPaidEvent>().Single(x => x.FrameId == f.Id) != new GameActivationAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.LifecycleFrameId, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.CountedSkillId, r.CountedActivationId, r.MinimumValue, r.ActivationFrameIds.Count, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter) || !history.OfType<MaximumHpChangedEvent>().Any(x => x.PlayerSeat == f.OwnerSeat && x.SkillId == f.SkillId && x.Delta == r.MaximumAfter - r.MaximumBefore && x.MaximumHp == r.MaximumAfter))
            return false;
        var grants = history.OfType<GameActivationAwakeningGrantIssuedEvent>().Where(x => x.FrameId == f.Id).ToArray();
        if (grants.Length != 0 && (grants.Any(x => x.OwnerSeat != f.OwnerSeat || x.SourceSkillId != f.SkillId) || !grants.Select(x => x.GrantedSkillId).SequenceEqual(r.SkillIds)) || r.Stage == GameActivationAwakeningStage.MaximumPaid && grants.Length != 0 || r.Stage == GameActivationAwakeningStage.GrantsIssued && grants.Length != r.SkillIds.Count)
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 670)]
    internal bool CanContinueGameActivationAwakening(ProgramSkillFrame f) => f.GameActivationAwakening is not null && ValidGameActivationAwakeningReceipt(f);
    internal bool GameActivationAwakeningFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        var r = f.GameActivationAwakening!;
        if (r.Stage == GameActivationAwakeningStage.MaximumPaid && child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.SourceSeat is null && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == r.MaximumBefore - r.MaximumAfter && hp.Change.HpBefore == r.HpAfter && hp.Change.HpAfter == r.HpAfter;
        return r.Stage == GameActivationAwakeningStage.GrantsIssued && child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 160)]
    internal bool IsGameActivationAwakeningProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && _coreDrawDiscardCategoryRefundOperations.CategoryActivationObserverRoot(true)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    SkillProgramStepOutcome IGameActivationAwakeningProgramHost.BeginGameActivationAwakening(ProgramSkillFrame f, SkillProgramEffect e) => BeginGameActivationAwakening(f, e);
}
