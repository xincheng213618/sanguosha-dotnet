#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceBiriRuntime
{
    internal const string ShanheSourceBiriSkill = "shanhe:source-biri-14039";
    internal const string ShanheSourceBiriDrawReason = "skill-program.shanhe-source-biri-14039.draw";
    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceBiriHost.PreventFireRecoverDraw(ProgramSkillFrame frame) => BeginShanheSourceBiri(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62047, SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw)]
    internal bool CanOfferShanheSourceBiri(ProgramTriggerLegalityContext context) => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && context.Owner.HasHpValue && context.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, TargetSeat: { } target } && target == context.Owner.Seat;
    internal BeforeDamageProgramWindowFrame ExactShanheSourceBiriParent(ProgramSkillFrame frame)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || frame.SkillId != ShanheSourceBiriSkill || frame.TriggerId != "prevent-fire" || frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame before || before.Id != context.ParentFrameId || before.TargetSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || context.OwnerSeat != frame.OwnerSeat || before.RedirectedTargetSeat is not null || before.Nature != DamageNature.Fire || before.Amount <= 0 || context.Amount != before.Amount || before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, before.Candidates[before.CandidateIndex].Candidate))
            throw new InvalidOperationException("Source Biri lost its original owner fire-damage candidate.");
        return before;
    }

    internal SkillProgramStepOutcome BeginShanheSourceBiri(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var before = ExactShanheSourceBiriParent(frame);
        if (frame.ShanheSourceBiri is not null || before.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != (before.ContinuationAttackResolutionId ?? before.ParentFrameId) || attack.TargetSeat != frame.OwnerSeat || attack.SourceSeat != before.SourceSeat || frame.WindowContext!.SourceSeat != (attack.IsSourceLess ? null : attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Source Biri cannot prevent or pay a fire-damage attempt twice.");
        var amount = before.Amount;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = new(frame.InstructionIndex, before.Id, before.ParentFrameId, attack.ResolutionId, amount, checked(amount * 2)) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceBiriIssuedEvent(frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.InstructionIndex, before.Id, before.ParentFrameId, attack.ResolutionId, amount, checked(amount * 2)));
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
        // The shared recovery primitive owns modifiers, replacement and the real
        // RecoveryFrame. Its atomic result is captured below before HP children.
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, frame.OwnerSeat, checked(amount * 2), null, null);
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.PendingRecoveryAttempts is { Count: > 0 } attempts)
        {
            if (attempts is not [var request] || request.SourceSeat != frame.OwnerSeat || request.TargetSeat != frame.OwnerSeat || request.Completion != new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, frame.InstructionIndex))
                throw new InvalidOperationException("Source Biri lost its exact queued recovery request.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = frame.ShanheSourceBiri!with { RecoveryAttemptId = request.Id, RecoveryAttemptAmount = request.Amount } });
        }

        ContinueShanheSourceBiri(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // Read the paid native result, not owner HP after descendants have run.
    // A redirected recovery belongs to its other recipient and yields zero here.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62047)]
    internal void CaptureShanheSourceBiriRecovery(RecoveryAppliedEvent applied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RecoveryFrame recovery || applied.Amount <= 0)
            return;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == recovery.ParentFrameId);
        var frame = parent as ProgramSkillFrame;
        if (parent is RecoveryReplacementFrame replacement)
        {
            frame = _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == replacement.ParentFrameId) as ProgramSkillFrame;
            if (frame?.ShanheSourceBiri is not { } queued || queued.RecoveryAttemptId != replacement.Id || replacement.Attempt.Id != replacement.Id || replacement.Attempt.SourceSeat != frame.OwnerSeat || replacement.Attempt.TargetSeat != frame.OwnerSeat || replacement.Attempt.Amount != queued.RecoveryAttemptAmount || replacement.CandidateIndex is not null)
                return;
        }

        if (frame?.ShanheSourceBiri is not { Stage: ShanheSourceBiriStage.RecoveryChildren } receipt || frame.SkillId != ShanheSourceBiriSkill || recovery.SourceSeat != frame.OwnerSeat || recovery.TargetSeat != frame.OwnerSeat)
            return;
        if (receipt.AppliedRecoveryFrameId is not null || applied.SourceSeat != recovery.SourceSeat || applied.TargetSeat != recovery.TargetSeat || applied.Amount > recovery.Amount || applied.RemainingHp != _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp)
            throw new InvalidOperationException("Source Biri recovery lost its once-paid native application.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { ShanheSourceBiri = receipt with { AppliedRecoveryFrameId = recovery.Id, ActualRecovered = applied.Amount } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceBiriRecoveredEvent(frame.Id, recovery.Id, recovery.ParentFrameId, frame.OwnerSeat, applied.Amount));
    }

    internal void ContinueShanheSourceBiri(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheSourceBiri is { } receipt)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            receipt = frame.ShanheSourceBiri!;
            if (receipt.Stage == ShanheSourceBiriStage.DrawChildren || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
                return;
            }

            var count = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive ? receipt.ActualRecovered : 0;
            var sequence = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = receipt with { Stage = ShanheSourceBiriStage.DrawChildren, DrawCount = count, MovementSequenceBefore = sequence, MovementSequenceAfter = sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceBiriDrawIssuedEvent(frame.Id, frame.OwnerSeat, receipt.ActualRecovered, count));
            if (count > 0)
                _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new(ShanheSourceBiriDrawReason));
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = frame.ShanheSourceBiri!with { MovementSequenceAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62047)]
    internal bool ResumeShanheSourceBiri(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceBiri: not null } frame || frame.Id != id)
            return false;
        ContinueShanheSourceBiri(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62047)]
    internal bool ReturnShanheSourceBiriMovement(ProgramSkillFrame frame) => frame.ShanheSourceBiri is not null && ResumeShanheSourceBiri(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62047)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62047)]
    internal bool CanFinishShanheSourceBiri(ProgramSkillFrame frame) => frame.ShanheSourceBiri is not null;
}
