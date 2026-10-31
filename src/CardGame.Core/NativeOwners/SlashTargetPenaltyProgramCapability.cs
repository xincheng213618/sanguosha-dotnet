#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SlashTargetPenaltyProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyPaymentsEntries> getCoreGameEngineSlashTargetPenaltyPaymentsRuntime) : ISlashTargetPenaltyProgramHost
{
    public SkillProgramStepOutcome RequireTargetDiscardOrEquipmentRecast(ProgramSkillFrame f) => getCoreGameEngineSlashTargetPenaltyPaymentsRuntime().RequireTargetDiscardOrEquipmentRecast(f);
}
