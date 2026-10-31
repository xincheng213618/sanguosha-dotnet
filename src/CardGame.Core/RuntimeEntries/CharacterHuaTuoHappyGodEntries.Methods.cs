#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoHappyGodEntries
{
    internal void AnswerHappyHuaNeedle(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHappyHuaNeedle(choice);
    internal bool CanOfferHappyHuaTuo(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferHappyHuaTuo(gate);
    internal global::CardGame.Core.HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence) => _resolve().CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    internal void ChangeHappyMedicine(global::CardGame.Core.ProgramSkillFrame f, int delta) => _resolve().ChangeHappyMedicine(f, delta);
    internal void CommitHappyHuaNeedle(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> targets, string mode) => _resolve().CommitHappyHuaNeedle(f, targets, mode);
    internal bool ContinuePaidHappyHuaTuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHappyHuaTuo(f);
    internal bool HappyHuaDyingTarget(global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.DyingFrame dying) => _resolve().HappyHuaDyingTarget(context, out dying);
    internal global::CardGame.Core.PromptChoice HappyHuaNeedleAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyHuaNeedleAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyHuaNeedleChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyHuaNeedleChoices(f);
    internal bool HappyHuaOriginalCandidate(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window, long parentId) => _resolve().HappyHuaOriginalCandidate(f, window, parentId);
    internal bool HappyHuaPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardConversionSource source, int before, int after, string purpose) => _resolve().HappyHuaPayment(f, source, before, after, purpose);
    internal int HappyMedicine(int owner) => _resolve().HappyMedicine(owner);
    internal int HappyMedicineRedLossCount(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().HappyMedicineRedLossCount(owner, context);
    internal void PublishHappyHuaNeedle(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyHuaNeedle(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveHappyHuaTuo(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveHappyHuaTuo(supplied, op);
    internal bool ValidHappyHuaNeedle(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyHuaNeedle(f);
    internal bool ValidHappyHuaRescue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyHuaRescue(f);
}
