namespace CardGame.Core;

public sealed record ProgramAmountEvaluatedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, int Amount) : IGameEvent;
public sealed record ProgramExpressionNumberCapturedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, string Name, int Value) : IGameEvent;

internal static class ProgramExpressionAmounts
{
    internal const string Prefix = "amount-expression-";
    internal static string Key(int index) => Prefix + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
    internal static int IssuedAmount(ProgramSkillFrame frame) =>
        frame.NumberBindings.Where(binding => binding.Name == Key(frame.InstructionIndex)).ToArray() is [var issued] && issued.Value >= 0
            ? issued.Value : -1;
}
