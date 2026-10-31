#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuExpandedProgramsEntries
{
    internal void AddShenwuPoison(int seat, int source, long frameId) => _resolve().AddShenwuPoison(seat, source, frameId);
    internal bool AllowsShenwuExpandedTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsShenwuExpandedTrigger(gate);
    internal void AnswerShenwuPoison(PromptChoice choice) => _resolve().AnswerShenwuPoison(choice);
    internal PromptChoice ChooseAiShenwuPoison(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ChooseAiShenwuPoison(decision, frame, effect);
    internal void ContinueShenwuXunyouEquipment() => _resolve().ContinueShenwuXunyouEquipment();
    internal SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteShenwuExpanded(supplied, op);
    internal void ObserveShenwuExpandedFacts(IGameEvent fact) => _resolve().ObserveShenwuExpandedFacts(fact);
    internal void PublishShenwuPoisonChoice(ProgramSkillFrame frame) => _resolve().PublishShenwuPoisonChoice(frame);
    internal SkillProgramStepOutcome ResolveShenwuPoison(ProgramSkillFrame frame) => _resolve().ResolveShenwuPoison(frame);
    internal SkillProgramStepOutcome ResolveShenwuRandomDamage(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().ResolveShenwuRandomDamage(frame, op);
    internal SkillProgramStepOutcome ResolveShenwuXunyou(ProgramSkillFrame frame) => _resolve().ResolveShenwuXunyou(frame);
    internal IReadOnlyList<PromptChoice> ShenwuPoisonChoices(ProgramSkillFrame frame) => _resolve().ShenwuPoisonChoices(frame);
    internal Card[] ShenwuPoisonPayable(ProgramSkillFrame frame) => _resolve().ShenwuPoisonPayable(frame);
}
