#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSkillChangeOperations(global::CardGame.Core.EngineStates.CoreGameEngineConfiguredConversionsState gameEngineConfiguredConversionsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.MatchSkillBindingIndex> getMatchSkillBindingIndex, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries> getCoreGameEngineConversionStatesRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries> getCoreGameEngineDyingOwnerSkillReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IProgramSkillChangeOperations
{
    public void WriteConversionTier(int seat, string state, int tier) => gameEngineConfiguredConversionsState.ReferenceToConfiguredConversionTiers[(seat, state)] = tier;
    public void Log(string kind, string message, int seat) => getCoreMatchLogRuntime().AddLog(kind, message, seat);
    public void SetBooleanState(int seat, string skillId, string instanceId, string stateId, bool value) => getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(seat, skillId, instanceId, stateId, value);
    public void Grant(int seat, SkillGrant grant) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grant(grant);
    public void RemoveGrant(int seat, string grantId) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.RemoveGrant(grantId);
    public void SetGrantEnabled(int seat, string grantId, bool enabled) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.SetEnabled(grantId, enabled);
    public void InvalidateBindings(int seat) => getMatchSkillBindingIndex().Invalidate(seat);
    public void RegisterTaggedConversionSkill(int seat, string skillId) => getCoreGameEngineConversionStatesRuntime().RegisterTaggedConversionSkill(matchParticipantsState.ReferenceToPlayers[seat], skillId);
    public void AdvanceFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    public IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineDyingOwnerSkillReplacementRuntime().AcquireDyingOwnerReplacementSkills(frame, skillIds);
    public ProgramReplacedPaidHpContinuation? CaptureDyingReplacementContinuation(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineDyingOwnerSkillReplacementRuntime().CaptureDyingOwnerReplacementContinuation(frame, effect);
    public void IssueReplacedPaidHpContinuation(ProgramReplacedPaidHpContinuation continuation) => getCoreGameEngineDyingOwnerSkillReplacementRuntime().IssueReplacedPaidHpContinuation(continuation);
    public void Finish(ProgramSkillFrame frame) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed: true);
}
