#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnSkillGrantsEntries
{
    internal void ExpireActualTurnSkillGrants(int turnNumber, int turnOwnerSeat) => _resolve().ExpireActualTurnSkillGrants(turnNumber, turnOwnerSeat);
    internal void ExpireActualTurnSkillGrantsAfterTurnEnded(global::CardGame.Core.TurnEndedEvent ended) => _resolve().ExpireActualTurnSkillGrantsAfterTurnEnded(ended);
    internal void GrantProgramTurnSkills(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::System.Collections.Generic.IReadOnlyList<string> skillIds, global::CardGame.Core.SkillProgramTurnSkillExpiry? expiry) => _resolve().GrantProgramTurnSkills(frame, targetSeat, skillIds, expiry);
    internal void ValidateTurnSkillGrantActor(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().ValidateTurnSkillGrantActor(frame, targetSeat);
}
