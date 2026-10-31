#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SignedDamagePaymentCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime>> getCoreGameEngineSignedDamagePaymentsRuntime) : ISignedDamagePaymentHost
{
    public SkillProgramStepOutcome DiscardOwnedCardToAdjustCurrentDamage(ProgramSkillFrame f) => getCoreGameEngineSignedDamagePaymentsRuntime().Value.BeginSignedDamagePayment(f);
}
