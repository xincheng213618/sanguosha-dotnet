#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnTurnEnemyHandGainProgramCapability(Func<RuntimeEntries.CoreGameEngineShanheS5ZhangliangEntries> getCoreGameEngineShanheS5ZhangliangRuntime) : IOwnTurnEnemyHandGainProgramHost
{
    public void BindOwnTurnEnemyHandGain(ProgramSkillFrame frame) => getCoreGameEngineShanheS5ZhangliangRuntime().BindOwnTurnEnemyHandGain(frame);
}
