#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSkillChangeFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.EngineStates.CoreGameEngineConfiguredConversionsState gameEngineConfiguredConversionsState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries> getCoreGameEngineDyingOwnerSkillReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IProgramSkillChangeFacts
{
    public ProgramSkillFrame ActiveFrame(long id) => getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(id);
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int MaximumHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int ConversionTier(int seat, string state) => gameEngineConfiguredConversionsState.ReferenceToConfiguredConversionTiers.GetValueOrDefault((seat, state));
    public void RequireSkill(string skillId) => _ = getContentRegistry().GetSkill(skillId);
    public bool IsAwakening(string skillId) => (getContentRegistry().GetSkill(skillId).Tags & SkillTag.Awakening) != 0;
    public bool IsDyingOwnerReplacement(ProgramSkillFrame frame) => getCoreGameEngineDyingOwnerSkillReplacementRuntime().IsDyingOwnerReplacementBinding(frame);
    public IReadOnlyList<SkillGrant> Grants(int seat) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grants;
    public IReadOnlyList<SkillGrant> ActiveGrants(int seat) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(matchParticipantsState.ReferenceToPlayers[seat]).ActiveGrants;
    public IReadOnlyList<string> EnabledSkillIds(int seat) => getCoreGameEngineNationalSkillsRuntime().EnabledContentSkillIds(matchParticipantsState.ReferenceToPlayers[seat]);
}
