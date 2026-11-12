namespace CardGame.Core.EngineOwners;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramSlashRequestFactsRuntime(
    RuntimeEntries.CoreGameEngineProgramLifecycleEntries lifecycle,
    RuntimeEntries.CoreNativeRuleQueriesEntries distances) : IProgramSlashRequestFacts
{
    public bool CanInvokeProgram(ProgramSkillFrame frame) => lifecycle.CanInvokeProgramSkill(frame);
    public int GetCombatDistance(int actorSeat, int targetSeat) => distances.GetCombatDistance(actorSeat, targetSeat);
}
