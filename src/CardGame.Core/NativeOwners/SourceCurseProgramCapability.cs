#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SourceCurseProgramCapability(Func<RuntimeEntries.CoreGameEngineSourceCursesEntries> getCoreGameEngineSourceCursesRuntime) : ISourceCurseProgramHost
{
    public SkillProgramStepOutcome ExecuteSourceCurse(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineSourceCursesRuntime().ExecuteSourceCurse(effect, frame);
}
