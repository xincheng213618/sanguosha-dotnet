#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramExpressionsRuntime
{
    internal int? EvaluateProgramAmountExpression(long frameId, ProgramExpression expression, SkillProgramUnknownAmountBehavior onUnknown = SkillProgramUnknownAmountBehavior.Skip)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var key = ProgramExpressionAmounts.Key(frame.InstructionIndex);
        if (frame.NumberBindings.Any(binding => binding.Name == key))
            throw new InvalidOperationException("An amount expression cannot be issued twice on the same instruction.");
        var amount = ProgramExpressions.EvaluateInt(expression, _kernelCoreGameEngineProgramExpressionsOperations.CreateProgramExpressionWorld(frame));
        if (amount is null)
        {
            if (onUnknown == SkillProgramUnknownAmountBehavior.CancelBinding)
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "体力值未定义，无法计算此效果。");
            return null;
        }

        amount = Math.Max(0, amount.Value);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NumberBindings = frame.NumberBindings.Append(new ProgramSkillNumberBinding(key, amount.Value)).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAmountEvaluatedEvent(frame.Id, frame.SkillId, frame.GameplayHash, frame.InstructionIndex, amount.Value));
        return amount;
    }

    internal const string ExpressionNumberPrefix = "expression-number-";
    internal int? ReadProgramExpressionNumber(ProgramSkillFrame frame, string name) => frame.NumberBindings.Where(binding => binding.Name == ExpressionNumberPrefix + name).ToArray()is [var value] ? value.Value : null;
    internal void CaptureProgramExpressionNumber(long frameId, string name, int value)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.NumberBindings.Any(binding => binding.Name == ExpressionNumberPrefix + name))
            throw new InvalidOperationException("A captured expression number cannot be overwritten.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NumberBindings = Array.AsReadOnly(frame.NumberBindings.Append(new ProgramSkillNumberBinding(ExpressionNumberPrefix + name, value)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramExpressionNumberCapturedEvent(frame.Id, frame.SkillId, frame.GameplayHash, frame.InstructionIndex, name, value));
    }
}
