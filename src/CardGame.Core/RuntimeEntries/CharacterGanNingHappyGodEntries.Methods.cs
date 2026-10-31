#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingHappyGodEntries
{
    internal void AssertHappyPoxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertHappyPoxi(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyPoxi(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().BeginHappyPoxi(supplied, bind);
    internal void ContinueHappyPoxi(long id) => _resolve().ContinueHappyPoxi(id);
    internal void FinishHappyPoxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishHappyPoxi(frame);
    internal bool HappyPoxiMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().HappyPoxiMovement(frame, effect, pending);
    internal bool KeepHappyPoxiPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepHappyPoxiPaid(frame);
    internal bool ResumeHappyPoxi(long id) => _resolve().ResumeHappyPoxi(id);
    internal void ReturnHappyPoxiDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyPoxiDraw(supplied, outcome);
    internal bool ReturnHappyPoxiMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnHappyPoxiMovement(frame);
    internal bool ValidHappyPoxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidHappyPoxi(frame);
}
