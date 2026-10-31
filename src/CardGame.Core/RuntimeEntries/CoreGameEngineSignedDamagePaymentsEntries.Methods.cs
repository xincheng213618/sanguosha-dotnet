#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSignedDamagePaymentsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSignedDamagePayment(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginSignedDamagePayment(input);
    internal bool CanOfferSignedDamagePayment(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferSignedDamagePayment(candidate, trigger, c);
    internal global::CardGame.Core.GameEngine.SignedDamageProfile CaptureSignedDamageProfile(int ownerSeat, int otherSeat, global::CardGame.Core.SkillProgramEffect effect) => _resolve().CaptureSignedDamageProfile(ownerSeat, otherSeat, effect);
    internal bool IsValidSignedDamagePayment(global::CardGame.Core.ProgramSkillFrame f, bool requirePaid) => _resolve().IsValidSignedDamagePayment(f, requirePaid);
    internal void PublishSignedDamagePayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSignedDamagePayment(f);
    internal void ResolveSignedDamagePayment(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSignedDamagePayment(choice);
    internal bool ResumeSignedDamagePayment(long id) => _resolve().ResumeSignedDamagePayment(id);
    internal bool ReturnSignedDamageMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSignedDamageMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiSignedDamagePayment(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSignedDamagePayment(d, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SignedDamageCosts(int ownerSeat, string skillId, string instance, global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.GameEngine.SignedDamageProfile p) => _resolve().SignedDamageCosts(ownerSeat, skillId, instance, e, p);
    internal global::CardGame.Core.SkillProgramEffect SignedDamageEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SignedDamageEffect(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SignedDamagePaymentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SignedDamagePaymentChoices(f);
    internal bool HasSignedDamagePayments { get => _resolve().HasSignedDamagePayments; }
}
