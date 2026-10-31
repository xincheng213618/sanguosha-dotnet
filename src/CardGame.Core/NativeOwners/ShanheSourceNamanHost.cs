#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceNamanHost(Func<RuntimeEntries.CoreGameEngineShanheSourceNamanEntries> getCoreGameEngineShanheSourceNamanRuntime) : IShanheSourceNamanHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceNamanRuntime().ClaimShanheSourceNamanResponseCards(frame);
}
