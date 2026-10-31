#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOriginalHandAwakeningRuntime
{
    internal SkillProgramStepOutcome BeginOriginalHandAwakening(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        if (f.OriginalHandAwakening is not null || f.InstructionIndex != 1 || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || trigger.Effects is not [var configured] || configured != effect || !_coreOriginalHandEntitiesOperations.CanOfferOriginalHandEntityProgram(f.OwnerSeat, f.SkillId, trigger, f.WindowContext!, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            throw new InvalidOperationException("Original-hand awakening lost its actual preparation candidate or exhausted original entities.");
        var state = _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess._originalHandEntityStates[new(f.OwnerSeat, effect.SourceBind!, effect.StateId!)];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = owner.MaxHp;
        var after = Math.Max(1, before - 1);
        var r = new ProgramOriginalHandAwakeningReceipt
        {
            InstructionIndex = 1,
            Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            SourceSkillId = effect.SourceBind!,
            StateId = effect.StateId!,
            InitializationFrameId = state.InitializationFrameId,
            OriginalCount = state.Entities.Count,
            LifecycleFrameId = f.WindowContext!.ParentFrameId,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            MaximumBefore = before,
            MaximumAfter = after,
            HpBefore = owner.Hp,
            HpAfter = Math.Min(owner.Hp, after),
            SkillIds = effect.SkillIds,
            Stage = OriginalHandAwakeningStage.MaximumPaid
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OriginalHandAwakening = r });
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalHandAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.SourceSkillId, r.StateId, r.InitializationFrameId, r.OriginalCount, r.LifecycleFrameId, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ResumeOriginalHandAwakening(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.OriginalHandAwakening is not { } r)
            return false;
        _coreOriginalHandEntityObserversOperations.AssertOriginalHandEntityProgram(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.OriginalHandAwakening!;
        if (r.Stage == OriginalHandAwakeningStage.MaximumPaid && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            // The native max-HP operation has already committed. Its exact receipt
            // owns the grant debt even if a health child disables the source.
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OriginalHandAwakening = r with { Stage = OriginalHandAwakeningStage.GrantsIssued } });
            _coreProgramSkillChangesOperations.GrantProgramSkills(f, r.SkillIds);
            foreach (var skill in r.SkillIds)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalHandAwakeningGrantIssuedEvent(id, f.OwnerSeat, f.SkillId, skill));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == OriginalHandAwakeningStage.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OriginalHandAwakening = r with { Stage = OriginalHandAwakeningStage.Complete } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalHandAwakeningCompletedEvent(id, r.Stage == OriginalHandAwakeningStage.GrantsIssued, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    internal bool ValidOriginalHandAwakeningReceipt(ProgramSkillFrame f)
    {
        if (f.OriginalHandAwakening is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != 1 || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.LifecycleFrameId != f.WindowContext!.ParentFrameId || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != f.OwnerSeat || !Enum.IsDefined(r.Stage) || r.MaximumBefore < 1 || r.MaximumAfter != Math.Max(1, r.MaximumBefore - 1) || r.HpAfter != Math.Min(r.HpBefore, r.MaximumAfter) || f.PendingMovementContinuation is not null || !_coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess._originalHandEntityStates.TryGetValue(new(f.OwnerSeat, r.SourceSkillId, r.StateId), out var state) || state.InitializationFrameId != r.InitializationFrameId || state.Entities.Count != r.OriginalCount || state.Entities.Any(e => e.FirstLossSequence is null) && !_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningReceipt(f) || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.AwakenWhenOriginalHandEmpty } effect] || effect.SourceBind != r.SourceSkillId || effect.StateId != r.StateId || !effect.SkillIds.SequenceEqual(r.SkillIds) || r.SkillIds.Count == 0 || r.SkillIds.Distinct(StringComparer.Ordinal).Count() != r.SkillIds.Count)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (!_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || history.OfType<OriginalHandAwakeningMaximumPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new OriginalHandAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.SourceSkillId, r.StateId, r.InitializationFrameId, r.OriginalCount, r.LifecycleFrameId, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter) || !history.OfType<MaximumHpChangedEvent>().Any(e => e.PlayerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.Delta == r.MaximumAfter - r.MaximumBefore && e.MaximumHp == r.MaximumAfter))
            return false;
        var grants = history.OfType<OriginalHandAwakeningGrantIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (grants.Length != 0 && (grants.Any(e => e.OwnerSeat != f.OwnerSeat || e.SourceSkillId != f.SkillId) || !grants.Select(e => e.GrantedSkillId).SequenceEqual(r.SkillIds)) || r.Stage == OriginalHandAwakeningStage.MaximumPaid && grants.Length != 0 || r.Stage == OriginalHandAwakeningStage.GrantsIssued && grants.Length != r.SkillIds.Count)
            return false;
        var completed = history.OfType<OriginalHandAwakeningCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return r.Stage == OriginalHandAwakeningStage.Complete ? completed is [var completion] && completion.GrantsIssued == (grants.Length == r.SkillIds.Count) : completed.Length == 0;
    }

    internal bool OriginalHandAwakeningFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidOriginalHandAwakeningReceipt(f))
            return false;
        var r = f.OriginalHandAwakening!;
        if (r.Stage == OriginalHandAwakeningStage.MaximumPaid && child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.SourceSeat is null && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == r.MaximumBefore - r.MaximumAfter && hp.Change.HpBefore == r.HpAfter && hp.Change.HpAfter == r.HpAfter;
        return r.Stage == OriginalHandAwakeningStage.GrantsIssued && child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
    }
}
