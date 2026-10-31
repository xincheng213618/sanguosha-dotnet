#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceXiechanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime>> getCoreGameEngineShanheSourceLiuYanTrioRuntime) : IShanheSourceXiechanHost
{
    public SkillProgramStepOutcome ResolvePindianDuel(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceLiuYanTrioRuntime().Value.ResolveSourceXiechanDuel(frame);
}
