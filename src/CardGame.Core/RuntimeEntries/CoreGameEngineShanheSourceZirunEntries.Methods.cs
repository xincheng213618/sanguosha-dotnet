#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceZirunEntries
{
    internal SkillProgramStepOutcome BeginShanheSourceZirun(ProgramSkillFrame supplied) => _resolve().BeginShanheSourceZirun(supplied);
    internal bool ContinueShanheSourceZirun(long id) => _resolve().ContinueShanheSourceZirun(id);
    internal bool ResumeShanheSourceZirun(long id) => _resolve().ResumeShanheSourceZirun(id);
    internal void ReturnShanheSourceZirunDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheSourceZirunDraw(supplied, outcome);
}
