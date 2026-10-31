#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramExpressionBindingsRuntime
{
    void global::CardGame.Core.IProgramExpressionCaptureHost.Capture(ProgramSkillFrame frame, string name, ProgramExpression expression)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (ProgramExpressions.EvaluateInt(expression, _kernelCoreGameEngineProgramExpressionsOperations.CreateProgramExpressionWorld(active))is not { } value)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "冻结表达式无法计算，技能剩余步骤取消。");
            return;
        }

        _coreProgramExpressionsOperations.CaptureProgramExpressionNumber(active.Id, name, value);
    }
}
