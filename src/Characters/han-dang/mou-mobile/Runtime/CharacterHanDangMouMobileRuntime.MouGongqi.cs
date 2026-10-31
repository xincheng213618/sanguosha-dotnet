#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangMouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30700, SkillProgramEffectOp.ResolveMouHanDangGongqi)]
    internal bool CanOfferMouHanDangGongqi(ProgramTriggerLegalityContext gate) => gate.Context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == gate.Candidate.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && MouHanDangDiscardCards(p.ActorSeat).Length > 0;
    internal bool MouHanDangPlayStartParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } c || c.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId || f.TriggerId != "discard-for-phase-responses")
            return false;
        return _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId) switch
        {
            PlayPhaseStartingBoundaryFrame p when p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } candidate => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate),
            ProgramLifecycleTriggerWindowFrame p when p.Window == c.Window && p.OwnerSeat == f.OwnerSeat && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }
}
