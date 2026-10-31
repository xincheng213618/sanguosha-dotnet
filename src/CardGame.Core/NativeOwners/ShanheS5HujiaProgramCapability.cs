#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5HujiaProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5HujiaRuntime>> getCoreGameEngineShanheS5HujiaRuntime) : IShanheS5HujiaHost, IShanheS5JianxiongHost
{
    public void BindShanheS5HujiaReward(ProgramSkillFrame frame) => getCoreGameEngineShanheS5HujiaRuntime().Value.BindShanheS5HujiaReward(frame);
    public void ClaimShanheS5JianxiongCards(ProgramSkillFrame frame) => getCoreGameEngineShanheS5HujiaRuntime().Value.ClaimShanheS5JianxiongCards(frame);
}
