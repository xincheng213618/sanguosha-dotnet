#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheFengxianRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27981)]
    internal bool CanOfferShanheFengxianXiuluo(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId != "shanhe:fengxian-xiuluo" || _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
}
