#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceXiechanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceLiuYanTrioEntries> getCoreGameEngineShanheSourceLiuYanTrioRuntime) : IShanheSourceXiechanHost
{
    public SkillProgramStepOutcome ResolvePindianDuel(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceLiuYanTrioRuntime().ResolveSourceXiechanDuel(frame);
}
