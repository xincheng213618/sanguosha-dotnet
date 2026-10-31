#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayPhaseSkillGrantsEntries
{
    internal void CloseCurrentPlayPhaseSkillGrants(string reason) => _resolve().CloseCurrentPlayPhaseSkillGrants(reason);
    internal void ClosePlayPhaseSkillGrants(SkillGrantPhaseExpiry expiry, string reason) => _resolve().ClosePlayPhaseSkillGrants(expiry, reason);
    internal void ExpireAllPlayPhaseSkillGrants(string reason) => _resolve().ExpireAllPlayPhaseSkillGrants(reason);
    internal void ExpirePlayPhaseSkillGrantsAfterGameEnded(GameEndedEvent ended) => _resolve().ExpirePlayPhaseSkillGrantsAfterGameEnded(ended);
    internal void ExpirePlayPhaseSkillGrantsAfterTurnEnded(TurnEndedEvent ended) => _resolve().ExpirePlayPhaseSkillGrantsAfterTurnEnded(ended);
    internal void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => _resolve().ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
    internal void GrantProgramPlayPhaseSkills(ProgramSkillFrame supplied, IReadOnlyList<string> skillIds) => _resolve().GrantProgramPlayPhaseSkills(supplied, skillIds);
    internal void IssueProgramPhaseSkillGrants(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, SkillGrantPhaseExpiry expiry) => _resolve().IssueProgramPhaseSkillGrants(frame, recipientSeat, skillIds, expiry);
    internal SkillGrantPhaseExpiry[] PhaseSkillGrantExpiries() => _resolve().PhaseSkillGrantExpiries();
    internal bool HasPlayPhaseSkillGrantCapability { get => _resolve().HasPlayPhaseSkillGrantCapability; }
}
