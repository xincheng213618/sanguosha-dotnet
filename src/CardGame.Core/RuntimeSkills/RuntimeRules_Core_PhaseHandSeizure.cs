
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_PhaseHandSeizure
{
    internal static string PhaseHandSeizureReason(ProgramSkillFrame frame, string part) =>
        $"skill-program.{frame.SkillId}.{SkillProgramEffectOp.DiscardTurnOverAndTakeHand}.{part}";

    internal static string PhaseHandDebtReturnReason(ProgramSkillFrame frame) =>
        $"skill-program.{frame.SkillId}.{SkillProgramEffectOp.ReturnIssuedPhaseHandDebt}";
}
