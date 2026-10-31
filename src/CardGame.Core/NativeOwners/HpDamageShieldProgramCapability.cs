#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HpDamageShieldProgramCapability(Func<RuntimeEntries.CoreGameEngineHpDamageShieldsEntries> getCoreGameEngineHpDamageShieldsRuntime) : IHpDamageShieldProgramHost
{
    public SkillProgramStepOutcome PayHpToGrantOneUseDamageShield(ProgramSkillFrame frame, PlayerMarkerKind marker) => getCoreGameEngineHpDamageShieldsRuntime().BeginHpDamageShieldPayment(frame, marker);
}
