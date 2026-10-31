#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceNamanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceNamanRuntime>> getCoreGameEngineShanheSourceNamanRuntime) : IShanheSourceNamanHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceNamanRuntime().Value.ClaimShanheSourceNamanResponseCards(frame);
}
