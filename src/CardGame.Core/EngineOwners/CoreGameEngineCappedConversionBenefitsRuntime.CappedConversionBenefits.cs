#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCappedConversionBenefitsRuntime
{
    internal static CardConversionSource CappedConversionSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CappedConversionBenefits.CappedConversionSource(f);
    internal bool ExactCappedConversionParent(ProgramSkillFrame f, out DamageTriggerWindowFrame window)
    {
        window = null !;
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } current || current.TriggerWindow != context.Window || current.TargetSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || context.OwnerSeat != f.OwnerSeat || context.DamageFrameId != current.ParentFrameId || current.CandidateIndex < 0 || current.CandidateIndex >= current.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, current.Candidates[current.CandidateIndex].ToProgramCandidate()))
            return false;
        window = current;
        return true;
    }

    internal SkillProgramStepOutcome BeginCappedConversionBenefit(ProgramSkillFrame supplied, string state)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CappedConversionBenefit is not null || f.InstructionIndex != 1 || !ExactCappedConversionParent(f, out var window) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("A capped conversion benefit lost its exact original damage recipient and current candidate.");
        // Opening effects can deal damage before Round 1. The exact damage
        // parent qualifies this benefit even while the current round is zero.
        var tier = _coreProgramTieredRoundConversionOperations.TieredRoundTier(f.OwnerSeat, state);
        if (tier is < 0 or > 2)
            throw new InvalidOperationException("A conversion modification count cannot exceed its two defined versions.");
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        var receipt = new ProgramCappedConversionBenefitReceipt(f.InstructionIndex, CappedConversionSource(f), f.GameplayHash, state, window.Id, window.ParentFrameId, f.WindowContext!.OccurrenceIndex, _coreActualRoundProgressRoundNumberAccess._roundNumber, tier, before, before, 0, tier > 0, tier > 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CappedConversionBenefit = receipt, PendingMovementContinuation = tier > 0 ? new(f.OwnerSeat, 0, null) : null });
        // X=0 is a genuine zero benefit: no draw call, no movement or CardsDrawn
        // fact. Its accepted operation can still issue modification one.
        var count = tier == 0 ? 0 : _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], tier, true, new($"skill-program.{(f.SkillId)}.capped-conversion-draw")).Count;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        receipt = receipt with
        {
            ActualDrawCount = count,
            SequenceAfter = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CappedConversionBenefit = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CappedConversionBenefitIssuedEvent(f.Id, receipt));
        ContinueCappedConversionBenefit(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueCappedConversionBenefit(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.CappedConversionBenefit!;
        if (!ValidCappedConversionBenefit(f))
            throw new InvalidOperationException("An issued conversion benefit changed its exact damage parent, draw invoice or original source.");
        if (r.AwaitingMovement)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, CappedConversionBenefit = r with { AwaitingMovement = false } });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.CappedConversionBenefit!;
        }

        var qualified = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
        var modified = qualified && _coreProgramSkillChangesOperations.TryUpgradeConfiguredConversionTier(f, r.StateId, r.FrozenSuccessfulModifications);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CappedConversionBenefitCompletedEvent(id, r.Source, r.StateId, r.FrozenSuccessfulModifications, r.ActualDrawCount, modified, _coreProgramTieredRoundConversionOperations.TieredRoundTier(f.OwnerSeat, r.StateId)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { CappedConversionBenefit = null, PendingMovementContinuation = null });
        // The one-op plan has no unissued benefit after this exact completion.
        _coreSkillProgramsOperations.FinishProgramSkill(f, qualified);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 430)]
    internal bool ResumeCappedConversionBenefit(long id)
    {
        if (_coreProgramLifecycleOperations.GetActiveProgramFrame(id).CappedConversionBenefit is null)
            return false;
        ContinueCappedConversionBenefit(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 420)]
    internal bool ReturnCappedConversionBenefitMovement(ProgramSkillFrame frame)
    {
        if (frame.CappedConversionBenefit is null)
            return false;
        if (frame.PendingMovementContinuation is not { } pending || !_coreTieredRoundZeroReturnsOperations.IsCappedConversionAwaitedMovement(frame, pending))
            throw new InvalidOperationException("The conversion draw lost its exact awaited movement return.");
        ContinueCappedConversionBenefit(frame.Id);
        return true;
    }

    internal bool ValidCappedConversionBenefit(ProgramSkillFrame f)
    {
        if (f.CappedConversionBenefit is not { } r || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Source != CappedConversionSource(f) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || string.IsNullOrWhiteSpace(r.Source.BindingId) || r.GameplayHash != f.GameplayHash || !ExactCappedConversionParent(f, out var window) || window.Id != r.DamageWindowFrameId || window.ParentFrameId != r.DamageFrameId || f.WindowContext!.OccurrenceIndex != r.OccurrenceIndex || r.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.FrozenSuccessfulModifications is < 0 or > 2 || r.ActualDrawCount < 0 || r.ActualDrawCount > r.FrozenSuccessfulModifications || r.ActualDrawAttempted != (r.FrozenSuccessfulModifications > 0) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || !r.ActualDrawAttempted && (r.SequenceAfter != r.SequenceBefore || r.ActualDrawCount != 0 || r.AwaitingMovement) || r.AwaitingMovement && (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat) || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.DrawBeforeCappedConversionTierUpgrade, StateId: var state }] || state != r.StateId)
            return false;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CappedConversionBenefitIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt == (r with { AwaitingMovement = r.ActualDrawAttempted })) != 1)
            return false;
        return _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.capped-conversion-draw") == r.ActualDrawCount;
    }

    SkillProgramStepOutcome global::CardGame.Core.ICappedConversionBenefitHost.DrawBeforeCappedConversionTierUpgrade(ProgramSkillFrame f, string state) => BeginCappedConversionBenefit(f, state);
}
