#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXingtianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheXingtianEntries> getCoreGameEngineShanheXingtianRuntime) : IShanheXingtianHost
{
    public void ApplyShanheXingtianSeal(ProgramSkillFrame f) => getCoreGameEngineShanheXingtianRuntime().ApplyShanheXingtianSeal(f);
}
