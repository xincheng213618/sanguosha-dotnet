#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5ShensuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShensuEntries> getCoreGameEngineShanheS5ShensuRuntime) : IShanheS5ShensuHost
{
    public void PrepareShanheS5Shensu(ProgramSkillFrame f) => getCoreGameEngineShanheS5ShensuRuntime().PrepareShanheS5Shensu(f);
}
