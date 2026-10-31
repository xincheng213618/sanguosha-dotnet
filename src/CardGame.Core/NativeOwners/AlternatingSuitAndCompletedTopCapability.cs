#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternatingSuitAndCompletedTopCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries> getCoreGameEngineAlternatingSuitAndCompletedTopRuntime) : IAlternatingSuitAndCompletedTopHost
{
    public SkillProgramStepOutcome BeginAlternatingSuitDrawDiscard(ProgramSkillFrame f) => getCoreGameEngineAlternatingSuitAndCompletedTopRuntime().BeginAlternatingSuitDrawDiscard(f);
    public SkillProgramStepOutcome BeginFirstCategoryCompletedTop(ProgramSkillFrame f) => getCoreGameEngineAlternatingSuitAndCompletedTopRuntime().BeginFirstCategoryCompletedTop(f);
}
