#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SoleTargetProtectionCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSoleTargetProtectionRuntime>> getCoreGameEngineSoleTargetProtectionRuntime) : ISoleTargetProtectionHost
{
    public SkillProgramStepOutcome ProtectOtherSoleCardTarget(ProgramSkillFrame frame) => getCoreGameEngineSoleTargetProtectionRuntime().Value.BeginSoleTargetProtection(frame);
}
