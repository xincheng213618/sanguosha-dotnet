#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientCategoryMarksProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries> getCoreGameEngineRecipientCategoryMarksRuntime) : IRecipientCategoryMarksProgramHost
{
    public SkillProgramStepOutcome ExecuteRecipientCategoryMark(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineRecipientCategoryMarksRuntime().ExecuteRecipientCategoryMark(f, e);
}
