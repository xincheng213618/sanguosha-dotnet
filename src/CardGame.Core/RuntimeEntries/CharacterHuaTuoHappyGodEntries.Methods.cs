#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoHappyGodEntries
{
    internal void AnswerHappyHuaNeedle(PromptChoice choice) => _resolve().AnswerHappyHuaNeedle(choice);
    internal bool CanOfferHappyHuaTuo(ProgramTriggerLegalityContext gate) => _resolve().CanOfferHappyHuaTuo(gate);
    internal HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence) => _resolve().CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    internal void ChangeHappyMedicine(ProgramSkillFrame f, int delta) => _resolve().ChangeHappyMedicine(f, delta);
    internal void CommitHappyHuaNeedle(ProgramSkillFrame f, IReadOnlyList<int> targets, string mode) => _resolve().CommitHappyHuaNeedle(f, targets, mode);
    internal bool ContinuePaidHappyHuaTuo(ProgramSkillFrame f) => _resolve().ContinuePaidHappyHuaTuo(f);
    internal bool HappyHuaDyingTarget(ProgramSkillWindowContext? context, scoped out DyingFrame dying) => _resolve().HappyHuaDyingTarget(context, out dying);
    internal PromptChoice HappyHuaNeedleAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyHuaNeedleAi(p, f);
    internal IReadOnlyList<PromptChoice> HappyHuaNeedleChoices(ProgramSkillFrame f) => _resolve().HappyHuaNeedleChoices(f);
    internal bool HappyHuaOriginalCandidate(ProgramSkillFrame f, SkillProgramTriggerWindow window, long parentId) => _resolve().HappyHuaOriginalCandidate(f, window, parentId);
    internal bool HappyHuaPayment(ProgramSkillFrame f, CardConversionSource source, int before, int after, string purpose) => _resolve().HappyHuaPayment(f, source, before, after, purpose);
    internal int HappyMedicine(int owner) => _resolve().HappyMedicine(owner);
    internal int HappyMedicineRedLossCount(int owner, ProgramSkillWindowContext? context) => _resolve().HappyMedicineRedLossCount(owner, context);
    internal void PublishHappyHuaNeedle(ProgramSkillFrame f) => _resolve().PublishHappyHuaNeedle(f);
    internal SkillProgramStepOutcome ResolveHappyHuaTuo(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveHappyHuaTuo(supplied, op);
    internal bool ValidHappyHuaNeedle(ProgramSkillFrame f) => _resolve().ValidHappyHuaNeedle(f);
    internal bool ValidHappyHuaRescue(ProgramSkillFrame f) => _resolve().ValidHappyHuaRescue(f);
}
