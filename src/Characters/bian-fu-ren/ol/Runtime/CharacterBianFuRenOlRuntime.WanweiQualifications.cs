#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBianFuRenOlRuntime
{
    internal static bool IsBianFuRenWanweiTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_bian_fu_ren_ol_Wanwei_Qualifications.IsBianFuRenWanweiTrigger(trigger);
    internal bool HasBianFuRenWanwei(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).GetInstanceTriggers(SkillProgramTriggerWindow.CardsMoved).Any(binding => IsBianFuRenWanweiTrigger(binding.Trigger));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3000)]
    internal bool ExcludeOrdinaryMovement_IsBianFuRenWanweiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsBianFuRenWanweiTrigger(trigger);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 200, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveBianFuRenMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CharacterState owner) => IsBianFuRenWanweiTrigger(trigger) ? BianFuRenLoss(context, owner.Seat)is not null : null;
}
