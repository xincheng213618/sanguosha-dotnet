#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYunOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 360)]
    internal bool CanAwakenWangYun(int owner, SkillProgramTrigger trigger) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.AwakenFromIssuedLianjiSlashDamage) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangYunLianjiDamageEvent>().Any(e => e.PlanOwner == owner && e.Amount > 0) || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(owner, "ol:moucheng", trigger.Id);
    internal SkillProgramStepOutcome AwakenFromIssuedLianjiSlashDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        if (f.WangYunAwakening is not null || f.InstructionIndex != 1 || f.TriggerId is null || c is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !CanAwakenWangYun(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f)) || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is not { } parent || parent.OwnerSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("Moucheng lost its actual issued plan Slash damage and own preparation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunAwakening = new(WangYunSource(f), f.GameplayHash, c.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, 1) });
        _coreProgramSkillChangesOperations.GrantProgramSkills(f, ["ol:jingong"]);
        foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == "ol:lianji").ToArray())
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunAwakenedEvent(f.Id, f.OwnerSeat, "ol:lianji", "ol:jingong"));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
