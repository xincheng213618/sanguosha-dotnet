#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HpDamageShieldProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpDamageShieldsEntries> getCoreGameEngineHpDamageShieldsRuntime) : IHpDamageShieldProgramHost
{
    public SkillProgramStepOutcome PayHpToGrantOneUseDamageShield(ProgramSkillFrame frame, PlayerMarkerKind marker) => getCoreGameEngineHpDamageShieldsRuntime().BeginHpDamageShieldPayment(frame, marker);
}
