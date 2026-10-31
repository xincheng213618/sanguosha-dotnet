#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal int BeiMiHuActualCards(int owner) => checked(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<BeiMiHuGujuDrawPaidEvent>().Where(e => e.Source.OwnerSeat == owner).Sum(e => e.Actual));
    internal bool CanDrawAfterKuiDamage(int owner, SkillProgramTrigger trigger, int target) => BeiMiHuOperation(trigger) != SkillProgramEffectOp.DrawAfterActualKuiDamage || _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.Kui) > 0;
    internal SkillProgramStepOutcome BeginBeiMiHuGuju(ProgramSkillFrame f)
    {
        var c = f.WindowContext;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (c is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: { } damage } || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame p || p.Id != c.ParentFrameId || p.ParentFrameId != damage || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex].ToProgramCandidate()) || !CanDrawAfterKuiDamage(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), p.TargetSeat) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<BeiMiHuGujuDrawPaidEvent>().Any(e => e.DamageWindowId == p.Id && e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Guju lost one actual applied damage on a public marked recipient.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = NewBeiMiHuReceipt(f, "draw-children")with
        {
            DamageParent = damage,
            DamageTarget = p.TargetSeat,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(BeiMiHuDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BeiMiHuDrawReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualDraw = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeiMiHuGujuDrawPaidEvent(f.Id, r.Source, r.GameplayHash, p.Id, damage, p.TargetSeat, r.Turn, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 170)]
    internal bool IsBeiMiHuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.DrawAfterActualKuiDamage && f.BeiMiHu is { Stage: "draw-children" } && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 23202)]
    internal bool ReturnBeiMiHuMovement(ProgramSkillFrame f)
    {
        if (f.BeiMiHu is null)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsBeiMiHuMovement(f, BeiMiHuPausedEffect(f), p))
            throw new InvalidOperationException("Guju received a foreign movement return.");
        AssertBeiMiHu(f);
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        FinishBeiMiHu(f);
        return true;
    }
}
