#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXingtianCapability(Func<RuntimeEntries.CoreGameEngineShanheXingtianEntries> getCoreGameEngineShanheXingtianRuntime) : IShanheXingtianHost
{
    public void ApplyShanheXingtianSeal(ProgramSkillFrame f) => getCoreGameEngineShanheXingtianRuntime().ApplyShanheXingtianSeal(f);
}
