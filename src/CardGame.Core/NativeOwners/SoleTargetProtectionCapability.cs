#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SoleTargetProtectionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries> getCoreGameEngineSoleTargetProtectionRuntime) : ISoleTargetProtectionHost
{
    public SkillProgramStepOutcome ProtectOtherSoleCardTarget(ProgramSkillFrame frame) => getCoreGameEngineSoleTargetProtectionRuntime().BeginSoleTargetProtection(frame);
}
