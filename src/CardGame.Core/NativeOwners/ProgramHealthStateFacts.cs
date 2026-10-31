#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthStateFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries> getCoreGameEngineWushuangTacticRulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineQimeiProgramsEntries> getCoreGameEngineQimeiProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries> getCharacterSunJianMouOlRuntime, global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries> getCoreShenwuNpcProgramsRuntime) : IProgramHealthStateFacts
{
    public int LivingFactionCount => getCoreGameEngineRuleQueriesRuntime().GetLivingFactionCount();
    public int LivingPlayersMinHp => getCoreGameEngineRuleQueriesRuntime().GetLivingPlayersMinHp();

    public bool HasCardPolicy(CharacterState player, SkillProgramCardPolicyKind policy) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(player, policy);
    public string GetRuntimeSkillInstanceId(CharacterState player, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(player, skillId);
    public bool HasRuntimeSkillInstance(CharacterState player, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(player, skillId, instanceId);
    public bool HasWushuangTactic(int seat, int tactic) => getCoreGameEngineWushuangTacticRulesRuntime().HasWushuangTactic(seat, tactic);
    public bool HasQimeiHealthSubject(int seat) => getCoreGameEngineQimeiProgramsRuntime().HasQimeiHealthSubject(seat);
    public bool HasMouSunJianOlHpSubject(int seat) => getCharacterSunJianMouOlRuntime().HasMouSunJianOlHpSubject(seat);
    public bool HealthChangeSubject(int seat) => getProgramPipelineHooks().HealthChangeSubject(seat);
    public bool ShenwuCannotTurnOver(CharacterState player) => getCoreShenwuNpcProgramsRuntime().ShenwuCannotTurnOver(player);
}
