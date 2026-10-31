#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangXiMobileShiRuntime
{
    internal bool HasShiPxCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiPxXuye);

    internal static bool IsShiPxRiderTrigger(SkillProgramTrigger trigger) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShiPxRider);
    internal IEnumerable<ProgramTriggerCandidate> CollectShiPxRiderCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
                if (!IsShiPxRiderTrigger(trigger))
                    continue;
                var swap = ShiPxActiveSwap(candidate.OwnerSeat);
                if (swap is null || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], ShiPangXiComposition.KuangxiangSkill))
                    continue;
                if (ShiPxRiderSide(swap, batch.Movements)is null)
                    continue;
                yield return candidate;
                break;
            }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 100)]
    internal void CollectWindow_CollectShiPxRiderCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectShiPxRiderCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1200)]
    internal bool ExcludeOrdinaryMovement_IsShiPxRiderTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsShiPxRiderTrigger(trigger);
}
