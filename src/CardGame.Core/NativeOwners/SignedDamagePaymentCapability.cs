#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SignedDamagePaymentCapability(Func<RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries> getCoreGameEngineSignedDamagePaymentsRuntime) : ISignedDamagePaymentHost
{
    public SkillProgramStepOutcome DiscardOwnedCardToAdjustCurrentDamage(ProgramSkillFrame f) => getCoreGameEngineSignedDamagePaymentsRuntime().BeginSignedDamagePayment(f);
}
