#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceZirunEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheSourceZirun(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheSourceZirun(supplied);
    internal bool ContinueShanheSourceZirun(long id) => _resolve().ContinueShanheSourceZirun(id);
    internal bool ResumeShanheSourceZirun(long id) => _resolve().ResumeShanheSourceZirun(id);
    internal void ReturnShanheSourceZirunDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheSourceZirunDraw(supplied, outcome);
}
