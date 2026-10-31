#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillsEntries
{
    internal void AssertWushuangOriginalSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertWushuangOriginalSkill(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWushuangOriginalSkill(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginWushuangOriginalSkill(supplied, operation);
    internal bool ContinuePaidWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWushuangOriginal(f);
    internal bool DrainWushuangOriginal(long id) => _resolve().DrainWushuangOriginal(id);
    internal void DrawWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next) => _resolve().DrawWushuangOriginal(f, recipient, count, next);
    internal void FinishWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, bool completed = true) => _resolve().FinishWushuangOriginal(f, completed);
    internal void PublishWushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishWushuangOriginalChoice(f, choices);
    internal void ReturnWushuangOriginalDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnWushuangOriginalDraw(f, outcome);
    internal bool ReturnWushuangOriginalMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWushuangOriginalMovement(f);
    internal bool RunWushuangOriginalSkill(long id) => _resolve().RunWushuangOriginalSkill(id);
    internal global::CardGame.Core.ProgramSkillFrame UpdateWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WushuangOriginalSkillReceipt r) => _resolve().UpdateWushuangOriginal(f, r);
    internal global::CardGame.Core.PromptChoice WushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().WushuangOriginalChoice(f, option, label, cards, targets);
    internal bool WushuangOriginalEnabled(global::CardGame.Core.CharacterState owner, string id) => _resolve().WushuangOriginalEnabled(owner, id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> WushuangOriginalPrivateCards(int viewer) => _resolve().WushuangOriginalPrivateCards(viewer);
    internal global::CardGame.Core.ProgramSkillFrame WushuangOriginalStage(global::CardGame.Core.ProgramSkillFrame f, string stage) => _resolve().WushuangOriginalStage(f, stage);
    internal global::CardGame.Core.PromptChoice[] WushuangOtherTargets(global::CardGame.Core.ProgramSkillFrame f, string option, global::System.Func<global::CardGame.Core.CharacterState, bool>? condition = default) => _resolve().WushuangOtherTargets(f, option, condition);
}
