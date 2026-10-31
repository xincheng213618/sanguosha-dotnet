#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoHappyGodEntries
{
    internal SkillProgramStepOutcome BeginHappyGuixin(ProgramSkillFrame supplied) => _resolve().BeginHappyGuixin(supplied);
    internal void FinishHappyGuixin(ProgramSkillFrame f, bool cancelled) => _resolve().FinishHappyGuixin(f, cancelled);
    internal bool ResumeHappyGuixin(long id) => _resolve().ResumeHappyGuixin(id);
    internal bool RetainHappyGuixinPaid(ProgramSkillFrame f) => _resolve().RetainHappyGuixinPaid(f);
    internal bool ReturnHappyGuixinMovement(ProgramSkillFrame f) => _resolve().ReturnHappyGuixinMovement(f);
    internal bool ValidHappyGuixin(ProgramSkillFrame f) => _resolve().ValidHappyGuixin(f);
}
