#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PublicPilePreparationCapability(Func<RuntimeEntries.CoreGameEnginePublicPilePreparationEntries> getCoreGameEnginePublicPilePreparationRuntime) : IPublicPilePreparationHost
{
    public SkillProgramStepOutcome ExecutePublicPilePreparation(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPilePreparationRuntime().BeginPublicPilePreparation(e, f);
}
