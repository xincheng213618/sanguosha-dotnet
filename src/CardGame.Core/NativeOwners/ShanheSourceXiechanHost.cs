#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceXiechanHost(Func<RuntimeEntries.CoreGameEngineShanheSourceLiuYanTrioEntries> getCoreGameEngineShanheSourceLiuYanTrioRuntime) : IShanheSourceXiechanHost
{
    public SkillProgramStepOutcome ResolvePindianDuel(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceLiuYanTrioRuntime().ResolveSourceXiechanDuel(frame);
}
