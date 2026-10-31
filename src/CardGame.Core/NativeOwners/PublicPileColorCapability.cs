#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileColorCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileColorsEntries> getCoreGameEnginePublicPileColorsRuntime) : IPublicPileColorHost
{
    public SkillProgramStepOutcome ExecutePublicPileColor(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPileColorsRuntime().ExecutePublicPileColor(e, f);
}
