#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnSkillGrantsEntries
{
    internal void ExpireActualTurnSkillGrants(int turnNumber, int turnOwnerSeat) => _resolve().ExpireActualTurnSkillGrants(turnNumber, turnOwnerSeat);
    internal void ExpireActualTurnSkillGrantsAfterTurnEnded(TurnEndedEvent ended) => _resolve().ExpireActualTurnSkillGrantsAfterTurnEnded(ended);
    internal void GrantProgramTurnSkills(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<string> skillIds, SkillProgramTurnSkillExpiry? expiry) => _resolve().GrantProgramTurnSkills(frame, targetSeat, skillIds, expiry);
    internal void ValidateTurnSkillGrantActor(ProgramSkillFrame frame, int targetSeat) => _resolve().ValidateTurnSkillGrantActor(frame, targetSeat);
}
