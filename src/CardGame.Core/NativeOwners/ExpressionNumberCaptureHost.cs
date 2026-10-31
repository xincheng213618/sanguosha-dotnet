#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ExpressionNumberCaptureHost(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, ProgramExpressionWorldFactory createProgramExpressionWorld, Func<RuntimeEntries.CoreGameEngineProgramExpressionsEntries> getCoreGameEngineProgramExpressionsRuntime) : IProgramExpressionCaptureHost
{
    public void Capture(ProgramSkillFrame frame, string name, ProgramExpression expression)
    {
        var active = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id);
        if (ProgramExpressions.EvaluateInt(expression, createProgramExpressionWorld(active))is not { } value)
        {
            getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(active, "冻结表达式无法计算，技能剩余步骤取消。");
            return;
        }

        getCoreGameEngineProgramExpressionsRuntime().CaptureProgramExpressionNumber(active.Id, name, value);
    }
}
