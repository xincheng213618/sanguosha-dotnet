#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnTurnEnemyHandGainProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime>> getCoreGameEngineShanheS5ZhangliangRuntime) : IOwnTurnEnemyHandGainProgramHost
{
    public void BindOwnTurnEnemyHandGain(ProgramSkillFrame frame) => getCoreGameEngineShanheS5ZhangliangRuntime().Value.BindOwnTurnEnemyHandGain(frame);
}
