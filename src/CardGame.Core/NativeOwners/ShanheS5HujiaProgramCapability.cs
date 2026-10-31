#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5HujiaProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5HujiaEntries> getCoreGameEngineShanheS5HujiaRuntime) : IShanheS5HujiaHost, IShanheS5JianxiongHost
{
    public void BindShanheS5HujiaReward(ProgramSkillFrame frame) => getCoreGameEngineShanheS5HujiaRuntime().BindShanheS5HujiaReward(frame);
    public void ClaimShanheS5JianxiongCards(ProgramSkillFrame frame) => getCoreGameEngineShanheS5HujiaRuntime().ClaimShanheS5JianxiongCards(frame);
}
