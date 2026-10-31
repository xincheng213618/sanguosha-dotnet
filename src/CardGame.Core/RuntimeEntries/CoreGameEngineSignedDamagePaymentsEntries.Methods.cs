#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSignedDamagePaymentsEntries
{
    internal SkillProgramStepOutcome BeginSignedDamagePayment(ProgramSkillFrame input) => _resolve().BeginSignedDamagePayment(input);
    internal bool CanOfferSignedDamagePayment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferSignedDamagePayment(candidate, trigger, c);
    internal GameEngine.SignedDamageProfile CaptureSignedDamageProfile(int ownerSeat, int otherSeat, SkillProgramEffect effect) => _resolve().CaptureSignedDamageProfile(ownerSeat, otherSeat, effect);
    internal bool IsValidSignedDamagePayment(ProgramSkillFrame f, bool requirePaid) => _resolve().IsValidSignedDamagePayment(f, requirePaid);
    internal void PublishSignedDamagePayment(ProgramSkillFrame f) => _resolve().PublishSignedDamagePayment(f);
    internal void ResolveSignedDamagePayment(PromptChoice choice) => _resolve().ResolveSignedDamagePayment(choice);
    internal bool ResumeSignedDamagePayment(long id) => _resolve().ResumeSignedDamagePayment(id);
    internal bool ReturnSignedDamageMovement(ProgramSkillFrame f) => _resolve().ReturnSignedDamageMovement(f);
    internal PromptChoice SelectAiSignedDamagePayment(PendingDecision d, ProgramSkillFrame f) => _resolve().SelectAiSignedDamagePayment(d, f);
    internal IReadOnlyList<Card> SignedDamageCosts(int ownerSeat, string skillId, string instance, SkillProgramEffect e, GameEngine.SignedDamageProfile p) => _resolve().SignedDamageCosts(ownerSeat, skillId, instance, e, p);
    internal SkillProgramEffect SignedDamageEffect(ProgramSkillFrame f) => _resolve().SignedDamageEffect(f);
    internal IReadOnlyList<PromptChoice> SignedDamagePaymentChoices(ProgramSkillFrame f) => _resolve().SignedDamagePaymentChoices(f);
    internal bool HasSignedDamagePayments { get => _resolve().HasSignedDamagePayments; }
}
