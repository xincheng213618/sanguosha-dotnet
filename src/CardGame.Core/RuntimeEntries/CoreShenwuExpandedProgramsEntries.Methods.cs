#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuExpandedProgramsEntries
{
    internal void AddShenwuPoison(int seat, int source, long frameId) => _resolve().AddShenwuPoison(seat, source, frameId);
    internal bool AllowsShenwuExpandedTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsShenwuExpandedTrigger(gate);
    internal void AnswerShenwuPoison(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShenwuPoison(choice);
    internal global::CardGame.Core.PromptChoice ChooseAiShenwuPoison(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ChooseAiShenwuPoison(decision, frame, effect);
    internal void ContinueShenwuXunyouEquipment() => _resolve().ContinueShenwuXunyouEquipment();
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteShenwuExpanded(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteShenwuExpanded(supplied, op);
    internal void ObserveShenwuExpandedFacts(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveShenwuExpandedFacts(fact);
    internal void PublishShenwuPoisonChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShenwuPoisonChoice(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShenwuPoison(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveShenwuPoison(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShenwuRandomDamage(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveShenwuRandomDamage(frame, op);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShenwuXunyou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveShenwuXunyou(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenwuPoisonChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShenwuPoisonChoices(frame);
    internal global::CardGame.Core.Card[] ShenwuPoisonPayable(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShenwuPoisonPayable(frame);
}
