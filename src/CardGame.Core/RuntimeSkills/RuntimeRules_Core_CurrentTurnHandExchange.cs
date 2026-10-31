
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_CurrentTurnHandExchange
{
    internal static string CurrentTurnHandExchangeReason(ProgramSkillFrame frame, string part) =>
        $"skill-program.{frame.SkillId}.{SkillProgramEffectOp.OfferCurrentTurnHandExchange}.{part}";

    internal static CardConversionSource CurrentTurnHandExchangeSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
}
