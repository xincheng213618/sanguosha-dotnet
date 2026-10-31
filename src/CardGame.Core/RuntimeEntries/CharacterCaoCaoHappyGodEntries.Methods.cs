#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoHappyGodEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyGuixin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHappyGuixin(supplied);
    internal void FinishHappyGuixin(global::CardGame.Core.ProgramSkillFrame f, bool cancelled) => _resolve().FinishHappyGuixin(f, cancelled);
    internal bool HappyGuixinMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().HappyGuixinMovementProof(f, effect, pending);
    internal bool ResumeHappyGuixin(long id) => _resolve().ResumeHappyGuixin(id);
    internal bool RetainHappyGuixinPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappyGuixinPaid(f);
    internal bool ReturnHappyGuixinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyGuixinMovement(f);
    internal bool ValidHappyGuixin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyGuixin(f);
}
