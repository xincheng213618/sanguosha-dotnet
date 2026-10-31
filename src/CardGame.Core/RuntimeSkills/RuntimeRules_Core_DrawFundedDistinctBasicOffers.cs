
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_DrawFundedDistinctBasicOffers
{
    internal const string DrawFundedBasicChoiceMarker = "draw-funded-basic";

    internal static bool IsDrawFundedDistinctBasicAnswer(PromptChoice choice) => choice.Parameters.ContainsKey(DrawFundedBasicChoiceMarker);

    internal static PendingDecision DrawFundedDistinctBasicNativeDecision(PendingDecision prompt) => RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuNativeDecision(prompt with
    { Choices = Array.AsReadOnly(prompt.Choices.Where(c => !RuntimeRules_Core_DrawFundedDistinctBasicOffers.IsDrawFundedDistinctBasicAnswer(c)).ToArray()) });
}
