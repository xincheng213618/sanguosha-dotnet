#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiHappyGodEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyTsc(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappyTsc(supplied, op);
    internal bool CanOfferHappyTaishiCi(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyTaishiCi(g);
    internal global::CardGame.Core.PromptChoice ChooseHappyTaishiCiChongwei(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseHappyTaishiCiChongwei(p, f);
    internal void CountHappyTaishiCiRemovedWei(global::CardGame.Core.PlayerMarkerChangedEvent fact) => _resolve().CountHappyTaishiCiRemovedWei(fact);
    internal bool HappyTscAwakened(int owner) => _resolve().HappyTscAwakened(owner);
    internal int HappyTscFieldWei() => _resolve().HappyTscFieldWei();
    internal bool ProveHappyTscMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation continuation) => _resolve().ProveHappyTscMovement(f, effect, continuation);
    internal global::CardGame.Core.SkillProgramStepOutcome RecoverHappyTsc(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RecoverHappyTsc(supplied);
    internal void RequireHappyTsc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireHappyTsc(f);
    internal bool ResumeHappyTsc(long id) => _resolve().ResumeHappyTsc(id);
    internal bool RetainHappyTsc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappyTsc(f);
    internal void ReturnHappyTscDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyTscDraw(f, outcome);
    internal bool ReturnHappyTscMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyTscMovement(f);
    internal bool ValidHappyTscMaterial(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyTscMaterial(f);
}
