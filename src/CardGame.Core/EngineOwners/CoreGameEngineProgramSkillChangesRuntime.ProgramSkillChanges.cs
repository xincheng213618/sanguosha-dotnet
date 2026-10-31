#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSkillChangesRuntime
{
    internal ProgramSkillChangeRuntime ProgramSkillChanges => _programSkillChangeOperations.Get();

    internal GameEngine.ProgramSkillChangeContext PrepareProgramSkillChange(ProgramSkillFrame supplied)
    {
        var active = ProgramSkillChanges.Prepare(supplied);
        return new(active, _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat]);
    }

    internal void UpgradeConfiguredConversionTier(ProgramSkillFrame frame, string state) => ProgramSkillChanges.TryUpgradeConversionTier(frame, state, null);
    internal bool TryUpgradeConfiguredConversionTier(ProgramSkillFrame frame, string state, int? expectedTier = null) => ProgramSkillChanges.TryUpgradeConversionTier(frame, state, expectedTier);
    internal bool CommitConfiguredConversionTier(CharacterState owner, string state, int tier) => ProgramSkillChanges.CommitConversionTier(owner.Seat, state, tier);
    internal void SetProgramBooleanState(ProgramSkillFrame frame, string stateId, bool value) => ProgramSkillChanges.SetBooleanState(frame, frame.SkillId, frame.SkillInstanceId, stateId, value);
    internal void SetProgramBooleanState(ProgramSkillFrame source, string skillId, string instanceId, string stateId, bool value) => ProgramSkillChanges.SetBooleanState(source, skillId, instanceId, stateId, value);
    internal void GrantProgramSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => ProgramSkillChanges.Grant(frame, frame.OwnerSeat, skillIds);
    internal void GrantProgramSkills(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds) => ProgramSkillChanges.Grant(frame, recipientSeat, skillIds);
    internal SkillGrant GrantOwnedProgramSkillSource(ProgramSkillFrame frame, string sourceId, string skillId) => ProgramSkillChanges.GrantOwnedSource(frame, sourceId, skillId);
    internal IReadOnlyList<SkillGrant> RevokeOwnedProgramSkillSources(ProgramSkillFrame frame, IReadOnlyList<string> grantIds) => ProgramSkillChanges.RevokeOwnedSources(frame, grantIds);
    internal void GrantIssuedProgramTurnSkills(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, SkillGrantTurnExpiry expiry) => ProgramSkillChanges.GrantIssuedTurnSkills(frame, recipientSeat, skillIds, expiry);
    internal void CommitProgramParticipantSkillGrants(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, string sourceId, SkillGrantTurnExpiry? turnExpiry, SkillGrantNextActualTurnStartExpiry? nextStartExpiry) => ProgramSkillChanges.GrantParticipantSkills(frame, recipientSeat, skillIds, sourceId, turnExpiry, nextStartExpiry);
    internal IReadOnlyList<string> AcquireProgramSkillChange(GameEngine.ProgramSkillChangeContext context, IReadOnlyList<string> skillIds, bool announceAwakening) => ProgramSkillChanges.Acquire(context.Frame, context.Owner.Seat, skillIds, announceAwakening);
    internal void ReplaceProgramSkills(ProgramSkillFrame frame, IReadOnlyList<string> lost, IReadOnlyList<string> acquired, ProgramSkillSourceRemoval removal, bool acquireFirst, bool announceAwakening = false) => ProgramSkillChanges.Replace(frame, lost, acquired, removal, acquireFirst, announceAwakening);
    internal SkillProgramStepOutcome ReplaceProgramOwnerSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => ProgramSkillChanges.ReplaceOwnerSkills(frame, effect);
    internal void CommitProgramSkillChangeFact(ProgramSkillFrame frame, IGameEvent fact) => ProgramSkillChanges.CommitFact(frame, fact);
    internal SkillGrant CommitProgramExplicitSkillSource(ProgramSkillFrame frame, SkillGrant grant, IReadOnlyList<string>? revokedGrantIds = null) => ProgramSkillChanges.CommitExplicitSource(frame, grant, revokedGrantIds);
    internal void RemoveProgramExplicitSkillSources(ProgramSkillFrame frame, IReadOnlyList<string> grantIds) => ProgramSkillChanges.RemoveExplicitSources(frame, grantIds);
    internal int IssueSkillState(SkillStateOrigin origin, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, int? endSeat = null) => ProgramSkillChanges.IssueSkillState(origin, seat, spec, skills, endSeat);
    internal void ExpireCurrentTurnSkillStates(int turnNumber, int turnSeat) => ProgramSkillChanges.ExpireCurrentTurnStates(turnNumber, turnSeat);
    ProgramSkillFrame IProgramSkillChangeFacts.ActiveFrame(long id) => _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
    bool IProgramSkillChangeFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramSkillChangeFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    string IProgramSkillChangeFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string IProgramSkillChangeFacts.SkillName(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Name;
    SkillTag IProgramSkillChangeFacts.SkillTags(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Tags;
    int IProgramSkillChangeFacts.MaximumHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int IProgramSkillChangeFacts.ConversionTier(int seat, string state) => _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers.GetValueOrDefault((seat, state));
    void IProgramSkillChangeFacts.RequireSkill(string skillId) => _ = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    bool IProgramSkillChangeFacts.IsAwakening(string skillId) => (_contentRegistryOperations._contentRegistry.GetSkill(skillId).Tags & SkillTag.Awakening) != 0;
    bool IProgramSkillChangeFacts.IsDyingOwnerReplacement(ProgramSkillFrame frame) => _coreDyingOwnerSkillReplacementOperations.Value.IsDyingOwnerReplacementBinding(frame);
    IReadOnlyList<string> IProgramSkillChangeFacts.EnabledSkillIds(int seat) => _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<SkillGrant> IProgramSkillGrantQueries.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants;
    IReadOnlyList<SkillGrant> IProgramSkillGrantQueries.ActiveGrants(int seat) => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ActiveGrants;
    IReadOnlyList<SkillGrant> IProgramSkillGrantQueries.OwnedGrants(int seat) => _coreNationalSkillsOperations.GetOwnedSkillGrants(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramSkillChangeOperations.WriteConversionTier(int seat, string state, int tier) => _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers[(seat, state)] = tier;
    void IProgramSkillChangeOperations.Log(string kind, string message, int seat, int? target) => _coreMatchLogOperations.AddLog(kind, message, seat, target);
    void IProgramSkillChangeOperations.SetBooleanState(int seat, string skillId, string instanceId, string stateId, bool value) => _coreProgramPersistentStateOperations.SetProgramBooleanState(seat, skillId, instanceId, stateId, value);
    void IProgramSkillChangeOperations.Grant(int seat, SkillGrant grant) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grant(grant);
    void IProgramSkillChangeOperations.RemoveGrant(int seat, string grantId) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.RemoveGrant(grantId);
    void IProgramSkillChangeOperations.SetGrantEnabled(int seat, string grantId, bool enabled) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.SetEnabled(grantId, enabled);
    void IProgramSkillChangeOperations.InvalidateBindings(int seat) => _skillBindingIndexOperations._skillBindingIndex.Invalidate(seat);
    void IProgramSkillChangeOperations.RegisterTaggedConversionSkill(int seat, string skillId) => _coreConversionStatesOperations.RegisterTaggedConversionSkill(_coreMatchParticipantsPlayersAccess._players[seat], skillId);
    void IProgramSkillChangeOperations.AdvanceFact(IGameEvent fact) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    IReadOnlyList<string> IProgramSkillChangeOperations.AcquireDyingOwnerReplacementSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _coreDyingOwnerSkillReplacementOperations.Value.AcquireDyingOwnerReplacementSkills(frame, skillIds);
    ProgramReplacedPaidHpContinuation? IProgramSkillChangeOperations.CaptureDyingReplacementContinuation(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreDyingOwnerSkillReplacementOperations.Value.CaptureDyingOwnerReplacementContinuation(frame, effect);
    void IProgramSkillChangeOperations.IssueReplacedPaidHpContinuation(ProgramReplacedPaidHpContinuation continuation) => _coreDyingOwnerSkillReplacementOperations.Value.IssueReplacedPaidHpContinuation(continuation);
    void IProgramSkillChangeOperations.Finish(ProgramSkillFrame frame) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: true);
}
