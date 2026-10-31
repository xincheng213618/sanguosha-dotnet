#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramSkillChangesEntries
{
    internal void ExpireCurrentTurnSkillStates(int turnNumber, int turnSeat) => _resolve().ExpireCurrentTurnSkillStates(turnNumber, turnSeat);
    internal void ExpireCurrentRoundSkillStates(int roundNumber) => _resolve().ExpireCurrentRoundSkillStates(roundNumber);
    internal void ApplyProgramSkillState(ProgramSkillFrame frame, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, string? resultBind = null) => _resolve().ApplyProgramSkillState(frame, seat, spec, skills, resultBind);
    internal void IssueSkillState(SkillStateOrigin origin, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, int? endSeat = null) => _resolve().IssueSkillState(origin, seat, spec, skills, endSeat);
    internal IReadOnlyList<string> AcquireProgramSkillChange(GameEngine.ProgramSkillChangeContext context, IReadOnlyList<string> skillIds, bool announceAwakening) => _resolve().AcquireProgramSkillChange(context, skillIds, announceAwakening);
    internal bool CommitConfiguredConversionTier(CharacterState owner, string state, int tier) => _resolve().CommitConfiguredConversionTier(owner, state, tier);
    internal SkillGrant CommitProgramExplicitSkillSource(ProgramSkillFrame frame, SkillGrant grant, IReadOnlyList<string>? revokedGrantIds = default) => _resolve().CommitProgramExplicitSkillSource(frame, grant, revokedGrantIds);
    internal void CommitProgramParticipantSkillGrants(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, string sourceId, SkillGrantTurnExpiry? turnExpiry, SkillGrantNextActualTurnStartExpiry? nextStartExpiry) => _resolve().CommitProgramParticipantSkillGrants(frame, recipientSeat, skillIds, sourceId, turnExpiry, nextStartExpiry);
    internal void CommitProgramSkillChangeFact(ProgramSkillFrame frame, IGameEvent fact) => _resolve().CommitProgramSkillChangeFact(frame, fact);
    internal void GrantIssuedProgramTurnSkills(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, SkillGrantTurnExpiry expiry) => _resolve().GrantIssuedProgramTurnSkills(frame, recipientSeat, skillIds, expiry);
    internal SkillGrant GrantOwnedProgramSkillSource(ProgramSkillFrame frame, string sourceId, string skillId) => _resolve().GrantOwnedProgramSkillSource(frame, sourceId, skillId);
    internal void GrantProgramSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _resolve().GrantProgramSkills(frame, skillIds);
    internal void GrantProgramSkills2(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds) => _resolve().GrantProgramSkills(frame, recipientSeat, skillIds);
    internal GameEngine.ProgramSkillChangeContext PrepareProgramSkillChange(ProgramSkillFrame supplied) => _resolve().PrepareProgramSkillChange(supplied);
    internal void RemoveProgramExplicitSkillSources(ProgramSkillFrame frame, IReadOnlyList<string> grantIds) => _resolve().RemoveProgramExplicitSkillSources(frame, grantIds);
    internal SkillProgramStepOutcome ReplaceProgramOwnerSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ReplaceProgramOwnerSkills(frame, effect);
    internal void ReplaceProgramSkills(ProgramSkillFrame frame, IReadOnlyList<string> lost, IReadOnlyList<string> acquired, ProgramSkillSourceRemoval removal, bool acquireFirst, bool announceAwakening = false) => _resolve().ReplaceProgramSkills(frame, lost, acquired, removal, acquireFirst, announceAwakening);
    internal IReadOnlyList<SkillGrant> RevokeOwnedProgramSkillSources(ProgramSkillFrame frame, IReadOnlyList<string> grantIds) => _resolve().RevokeOwnedProgramSkillSources(frame, grantIds);
    internal void SetProgramBooleanState(ProgramSkillFrame frame, string stateId, bool value) => _resolve().SetProgramBooleanState(frame, stateId, value);
    internal void SetProgramBooleanState2(ProgramSkillFrame source, string skillId, string instanceId, string stateId, bool value) => _resolve().SetProgramBooleanState(source, skillId, instanceId, stateId, value);
    internal bool TryUpgradeConfiguredConversionTier(ProgramSkillFrame frame, string state, int? expectedTier = default) => _resolve().TryUpgradeConfiguredConversionTier(frame, state, expectedTier);
    internal void UpgradeConfiguredConversionTier(ProgramSkillFrame frame, string state) => _resolve().UpgradeConfiguredConversionTier(frame, state);
    internal ProgramSkillChangeRuntime ProgramSkillChanges { get => _resolve().ProgramSkillChanges; }
}
