#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SoleTargetProtectionCapability(Func<RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries> getCoreGameEngineSoleTargetProtectionRuntime) : ISoleTargetProtectionHost
{
    public SkillProgramStepOutcome ProtectOtherSoleCardTarget(ProgramSkillFrame frame) => getCoreGameEngineSoleTargetProtectionRuntime().BeginSoleTargetProtection(frame);
}
