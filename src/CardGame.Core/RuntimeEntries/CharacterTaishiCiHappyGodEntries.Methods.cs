#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiHappyGodEntries
{
    internal SkillProgramStepOutcome BeginHappyTsc(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappyTsc(supplied, op);
    internal bool CanOfferHappyTaishiCi(ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyTaishiCi(g);
    internal PromptChoice ChooseHappyTaishiCiChongwei(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseHappyTaishiCiChongwei(p, f);
    internal void CountHappyTaishiCiRemovedWei(PlayerMarkerChangedEvent fact) => _resolve().CountHappyTaishiCiRemovedWei(fact);
    internal bool HappyTscAwakened(int owner) => _resolve().HappyTscAwakened(owner);
    internal int HappyTscFieldWei() => _resolve().HappyTscFieldWei();
    internal SkillProgramStepOutcome RecoverHappyTsc(ProgramSkillFrame supplied) => _resolve().RecoverHappyTsc(supplied);
    internal bool ResumeHappyTsc(long id) => _resolve().ResumeHappyTsc(id);
    internal bool RetainHappyTsc(ProgramSkillFrame f) => _resolve().RetainHappyTsc(f);
    internal void ReturnHappyTscDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyTscDraw(f, outcome);
    internal bool ReturnHappyTscMovement(ProgramSkillFrame f) => _resolve().ReturnHappyTscMovement(f);
}
