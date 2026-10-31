#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SignedDamagePaymentCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries> getCoreGameEngineSignedDamagePaymentsRuntime) : ISignedDamagePaymentHost
{
    public SkillProgramStepOutcome DiscardOwnedCardToAdjustCurrentDamage(ProgramSkillFrame f) => getCoreGameEngineSignedDamagePaymentsRuntime().BeginSignedDamagePayment(f);
}
