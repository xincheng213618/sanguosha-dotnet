#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceNamanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceNamanEntries> getCoreGameEngineShanheSourceNamanRuntime) : IShanheSourceNamanHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceNamanRuntime().ClaimShanheSourceNamanResponseCards(frame);
}
