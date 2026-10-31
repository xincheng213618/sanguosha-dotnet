#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramExpressionsEntries
{
    internal void AssertCapturedExpressionNumbers(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertCapturedExpressionNumbers(frame, plan);
    internal void AssertProgramAmountExpressions(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertProgramAmountExpressions(frame, plan);
    internal void CaptureProgramExpressionNumber(long frameId, string name, int value) => _resolve().CaptureProgramExpressionNumber(frameId, name, value);
    internal int? EvaluateProgramAmountExpression(long frameId, global::CardGame.Core.ProgramExpression expression, global::CardGame.Core.SkillProgramUnknownAmountBehavior onUnknown = (global::CardGame.Core.SkillProgramUnknownAmountBehavior)0) => _resolve().EvaluateProgramAmountExpression(frameId, expression, onUnknown);
    internal int? ReadProgramExpressionNumber(global::CardGame.Core.ProgramSkillFrame frame, string name) => _resolve().ReadProgramExpressionNumber(frame, name);
}
