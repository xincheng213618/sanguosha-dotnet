#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5BaiyinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5SimayiEntries> getCoreGameEngineShanheS5SimayiRuntime) : IShanheS5BaiyinHost
{
    public SkillProgramStepOutcome GrantShanheS5Baiyin(ProgramSkillFrame frame) => getCoreGameEngineShanheS5SimayiRuntime().GrantShanheS5Baiyin(frame);
}
