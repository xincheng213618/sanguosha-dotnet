#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillsEntries
{
    internal SkillProgramStepOutcome BeginWushuangOriginalSkill(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginWushuangOriginalSkill(supplied, operation);
    internal bool ContinuePaidWushuangOriginal(ProgramSkillFrame f) => _resolve().ContinuePaidWushuangOriginal(f);
    internal bool DrainWushuangOriginal(long id) => _resolve().DrainWushuangOriginal(id);
    internal void DrawWushuangOriginal(ProgramSkillFrame f, int recipient, int count, string next) => _resolve().DrawWushuangOriginal(f, recipient, count, next);
    internal void FinishWushuangOriginal(ProgramSkillFrame f, bool completed = true) => _resolve().FinishWushuangOriginal(f, completed);
    internal void PublishWushuangOriginalChoice(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => _resolve().PublishWushuangOriginalChoice(f, choices);
    internal void ReturnWushuangOriginalDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnWushuangOriginalDraw(f, outcome);
    internal bool ReturnWushuangOriginalMovement(ProgramSkillFrame f) => _resolve().ReturnWushuangOriginalMovement(f);
    internal bool RunWushuangOriginalSkill(long id) => _resolve().RunWushuangOriginalSkill(id);
    internal ProgramSkillFrame UpdateWushuangOriginal(ProgramSkillFrame f, WushuangOriginalSkillReceipt r) => _resolve().UpdateWushuangOriginal(f, r);
    internal PromptChoice WushuangOriginalChoice(ProgramSkillFrame f, string option, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().WushuangOriginalChoice(f, option, label, cards, targets);
    internal bool WushuangOriginalEnabled(CharacterState owner, string id) => _resolve().WushuangOriginalEnabled(owner, id);
    internal IEnumerable<CardSnapshot> WushuangOriginalPrivateCards(int viewer) => _resolve().WushuangOriginalPrivateCards(viewer);
    internal ProgramSkillFrame WushuangOriginalStage(ProgramSkillFrame f, string stage) => _resolve().WushuangOriginalStage(f, stage);
    internal PromptChoice[] WushuangOtherTargets(ProgramSkillFrame f, string option, Func<CharacterState, bool>? condition = default) => _resolve().WushuangOtherTargets(f, option, condition);
}
