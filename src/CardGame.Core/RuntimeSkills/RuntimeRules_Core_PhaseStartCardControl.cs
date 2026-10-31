
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_PhaseStartCardControl
{
    internal static string PhaseStartControlReason(PhaseStartCardControlKind kind) =>
        kind == PhaseStartCardControlKind.OtherPlayRestriction ? "skill-program.other-play-restriction.cost" : "skill-program.starting-equipment.move";
}
