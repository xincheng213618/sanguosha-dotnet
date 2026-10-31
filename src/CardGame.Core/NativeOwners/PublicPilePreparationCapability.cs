#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PublicPilePreparationCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPilePreparationEntries> getCoreGameEnginePublicPilePreparationRuntime) : IPublicPilePreparationHost
{
    public SkillProgramStepOutcome ExecutePublicPilePreparation(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPilePreparationRuntime().BeginPublicPilePreparation(e, f);
}
