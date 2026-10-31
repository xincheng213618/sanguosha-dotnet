#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5LuoyiProgramCapability(Func<RuntimeEntries.CoreGameEngineShanheS5XuchuEntries> getCoreGameEngineShanheS5XuchuRuntime) : IShanheS5LuoyiProgramHost
{
    public void ResolveShanheS5Luoyi(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineShanheS5XuchuRuntime().ResolveShanheS5Luoyi(frame, effect);
}
