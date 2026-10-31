#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class KuangfuProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineKuangfuRuntime>> getCoreGameEngineKuangfuRuntime) : IKuangfuProgramHost
{
    public SkillProgramStepOutcome DiscardEquipmentThenSlashAndOwnershipOutcome(ProgramSkillFrame f) => getCoreGameEngineKuangfuRuntime().Value.BeginKuangfu(f);
}
