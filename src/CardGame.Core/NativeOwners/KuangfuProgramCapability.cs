#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class KuangfuProgramCapability(Func<RuntimeEntries.CoreGameEngineKuangfuEntries> getCoreGameEngineKuangfuRuntime) : IKuangfuProgramHost
{
    public SkillProgramStepOutcome DiscardEquipmentThenSlashAndOwnershipOutcome(ProgramSkillFrame f) => getCoreGameEngineKuangfuRuntime().BeginKuangfu(f);
}
