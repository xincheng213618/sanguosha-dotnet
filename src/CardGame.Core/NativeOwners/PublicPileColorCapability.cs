#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileColorCapability(Func<RuntimeEntries.CoreGameEnginePublicPileColorsEntries> getCoreGameEnginePublicPileColorsRuntime) : IPublicPileColorHost
{
    public SkillProgramStepOutcome ExecutePublicPileColor(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPileColorsRuntime().ExecutePublicPileColor(e, f);
}
