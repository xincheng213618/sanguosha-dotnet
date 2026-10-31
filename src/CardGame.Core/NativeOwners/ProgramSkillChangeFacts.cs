#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSkillChangeFacts(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreGameEngineConfiguredConversionsState gameEngineConfiguredConversionsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime>> getCoreGameEngineDyingOwnerSkillReplacementRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IProgramSkillChangeFacts
{
    public ProgramSkillFrame ActiveFrame(long id) => getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(id);
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int MaximumHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int ConversionTier(int seat, string state) => gameEngineConfiguredConversionsState.ReferenceToConfiguredConversionTiers.GetValueOrDefault((seat, state));
    public void RequireSkill(string skillId) => _ = getContentRegistry().GetSkill(skillId);
    public bool IsAwakening(string skillId) => (getContentRegistry().GetSkill(skillId).Tags & SkillTag.Awakening) != 0;
    public bool IsDyingOwnerReplacement(ProgramSkillFrame frame) => getCoreGameEngineDyingOwnerSkillReplacementRuntime().Value.IsDyingOwnerReplacementBinding(frame);
    public IReadOnlyList<SkillGrant> Grants(int seat) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grants;
    public IReadOnlyList<SkillGrant> ActiveGrants(int seat) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(matchParticipantsState.ReferenceToPlayers[seat]).ActiveGrants;
    public IReadOnlyList<string> EnabledSkillIds(int seat) => getCoreGameEngineNationalSkillsRuntime().EnabledContentSkillIds(matchParticipantsState.ReferenceToPlayers[seat]);
}
