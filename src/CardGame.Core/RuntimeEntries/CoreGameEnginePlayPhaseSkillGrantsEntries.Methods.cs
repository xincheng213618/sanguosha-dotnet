#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayPhaseSkillGrantsEntries
{
    internal void CloseCurrentPlayPhaseSkillGrants(string reason) => _resolve().CloseCurrentPlayPhaseSkillGrants(reason);
    internal void ClosePlayPhaseSkillGrants(global::CardGame.Core.SkillGrantPhaseExpiry expiry, string reason) => _resolve().ClosePlayPhaseSkillGrants(expiry, reason);
    internal void ExpireAllPlayPhaseSkillGrants(string reason) => _resolve().ExpireAllPlayPhaseSkillGrants(reason);
    internal void ExpirePlayPhaseSkillGrantsAfterGameEnded(global::CardGame.Core.GameEndedEvent ended) => _resolve().ExpirePlayPhaseSkillGrantsAfterGameEnded(ended);
    internal void ExpirePlayPhaseSkillGrantsAfterTurnEnded(global::CardGame.Core.TurnEndedEvent ended) => _resolve().ExpirePlayPhaseSkillGrantsAfterTurnEnded(ended);
    internal void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => _resolve().ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
    internal void GrantProgramPlayPhaseSkills(global::CardGame.Core.ProgramSkillFrame supplied, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => _resolve().GrantProgramPlayPhaseSkills(supplied, skillIds);
    internal void IssueProgramPhaseSkillGrants(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, global::System.Collections.Generic.IReadOnlyList<string> skillIds, global::CardGame.Core.SkillGrantPhaseExpiry expiry) => _resolve().IssueProgramPhaseSkillGrants(frame, recipientSeat, skillIds, expiry);
    internal global::CardGame.Core.SkillGrantPhaseExpiry[] PhaseSkillGrantExpiries() => _resolve().PhaseSkillGrantExpiries();
    internal bool HasPlayPhaseSkillGrantCapability { get => _resolve().HasPlayPhaseSkillGrantCapability; }
}
