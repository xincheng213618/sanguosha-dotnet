#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetObtainObserverRuntime
{
    internal ProgramSkillFrame? PaidDamageTargetObtainObserverRoot(long windowId)
    {
        if (_coreDamageTargetRegionObtainOperations.PaidDamageTargetRegionObtainObserverRoot(windowId)is { } region)
            return region;
        for (var i = 1; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame f || f.DamageTargetObtain is not { ObtainMovementSequence: { } sequence } d || !_coreDamageTargetObtainProgramsOperations.IsValidDamageTargetObtain(f) || d.DamageWindowId != windowId || _resolutionStackOperations._resolutionStack[i - 1] is not DamageTriggerWindowFrame w || w.Id != windowId || w.ParentFrameId != d.DamageFrameId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].ToProgramCandidate()is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || _resolutionStackOperations._resolutionStack[i + 1] is not CardsMovedTriggerWindowFrame first || f.PendingMovementContinuation is not { CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || first.Batch.ParentFrameId != f.Id || first.Batch.AwaitingProgramFrameId != f.Id || first.Batch.OriginSkillId != f.SkillId || first.Batch.OriginSkillInstanceId != f.SkillInstanceId || first.Batch.OriginOwnerSeat != f.OwnerSeat || first.Batch.Movements is not [var actual] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(actual))
                continue;
            var exact = d.Stage == ProgramDamageTargetObtainStage.ObtainChildren ? actual.Sequence == sequence && actual.From.OwnerSeat == d.VictimSeat && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Contains(actual.From.Zone) && actual.Reason.Value == "skill-program.damage-target-obtain.obtain" : d.Stage == ProgramDamageTargetObtainStage.DrawChildren && d.Receipt is { IsEquipment: false } && actual.From == CardLocation.DrawPile && actual.To == CardLocation.Hand(d.VictimSeat) && actual.Reason.Value == "skill-program.damage-target-obtain.benefit-draw";
            if (!exact)
                continue;
            var legal = true;
            for (var n = i + 1; n < _resolutionStackOperations._resolutionStack.Count; n++)
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(n))
                {
                    legal = false;
                    break;
                }

            if (legal)
                return f;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 40)]
    internal bool HasPaidDamageTargetObtainObserver(long id) => PaidDamageTargetObtainObserverRoot(id)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 310, group: 10)]
    internal bool IsPaidDamageTargetObtainDying() => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } w && _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } dying && PaidDamageTargetObtainObserverRoot(w.Id)is not null && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(f => f.Id == dying.FrameId && f.ParentFrameId == dying.ParentFrameId);
}
