#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramExpressionsEntries
{
    internal void CaptureProgramExpressionNumber(long frameId, string name, int value) => _resolve().CaptureProgramExpressionNumber(frameId, name, value);
    internal int? EvaluateProgramAmountExpression(long frameId, ProgramExpression expression, SkillProgramUnknownAmountBehavior onUnknown = (SkillProgramUnknownAmountBehavior)0) => _resolve().EvaluateProgramAmountExpression(frameId, expression, onUnknown);
    internal int? ReadProgramExpressionNumber(ProgramSkillFrame frame, string name) => _resolve().ReadProgramExpressionNumber(frame, name);
}
