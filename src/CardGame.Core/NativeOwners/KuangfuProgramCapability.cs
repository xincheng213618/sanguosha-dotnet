#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class KuangfuProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineKuangfuEntries> getCoreGameEngineKuangfuRuntime) : IKuangfuProgramHost
{
    public SkillProgramStepOutcome DiscardEquipmentThenSlashAndOwnershipOutcome(ProgramSkillFrame f) => getCoreGameEngineKuangfuRuntime().BeginKuangfu(f);
}
