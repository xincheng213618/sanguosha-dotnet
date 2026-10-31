#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingHappyGodEntries
{
    internal SkillProgramStepOutcome BeginHappyPoxi(ProgramSkillFrame supplied, string bind) => _resolve().BeginHappyPoxi(supplied, bind);
    internal void ContinueHappyPoxi(long id) => _resolve().ContinueHappyPoxi(id);
    internal void FinishHappyPoxi(ProgramSkillFrame frame) => _resolve().FinishHappyPoxi(frame);
    internal bool KeepHappyPoxiPaid(ProgramSkillFrame frame) => _resolve().KeepHappyPoxiPaid(frame);
    internal bool ResumeHappyPoxi(long id) => _resolve().ResumeHappyPoxi(id);
    internal void ReturnHappyPoxiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnHappyPoxiDraw(supplied, outcome);
    internal bool ReturnHappyPoxiMovement(ProgramSkillFrame frame) => _resolve().ReturnHappyPoxiMovement(frame);
}
