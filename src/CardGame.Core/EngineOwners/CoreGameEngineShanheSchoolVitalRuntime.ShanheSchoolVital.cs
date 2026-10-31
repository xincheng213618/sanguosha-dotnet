#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolVitalRuntime
{
    void global::CardGame.Core.IShanheSchoolVitalHost.SetSchoolHpToMaximum(ProgramSkillFrame frame) => SetShanheSchoolHpToMaximum(frame);
    internal void SetShanheSchoolHpToMaximum(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheSchoolEffects.FullHpSkillId || frame.TriggerId != "preparation-hp-adjustment" || frame.InstructionIndex != 1 || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatAccess._currentSeat || !_coreShanheSchoolsOperations.HasShanheSchoolEffect(frame.OwnerSeat, 5) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Continuation: ProgramLifecycleContinuation.NormalTurnStart } parent || parent.OwnerSeat != frame.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != parent.Id || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSchoolHpAdjustedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("School HP adjustment requires its completed owner's original preparation instruction.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue || owner.Hp == owner.MaxHp)
            return;
        var before = owner.Hp;
        owner.Hp = owner.MaxHp;
        // Adjustment emits health-change rules only. It is not a recovery attempt,
        // so recovery replacement and after-recovery rewards cannot change it.
        var change = _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, frame.OwnerSeat, frame.OwnerSeat, before, owner.Hp, HpChangeKind.Setting);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSchoolHpAdjustedEvent(frame.Id, frame.OwnerSeat, before, owner.Hp, change?.Id));
    }
}
