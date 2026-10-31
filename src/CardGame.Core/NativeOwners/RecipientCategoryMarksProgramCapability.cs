#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientCategoryMarksProgramCapability(Func<RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries> getCoreGameEngineRecipientCategoryMarksRuntime) : IRecipientCategoryMarksProgramHost
{
    public SkillProgramStepOutcome ExecuteRecipientCategoryMark(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineRecipientCategoryMarksRuntime().ExecuteRecipientCategoryMark(f, e);
}
