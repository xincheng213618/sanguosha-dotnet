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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 93002)]
    internal void AssertProgramAmountExpressions(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var bindings = frame.NumberBindings.Where(binding => binding.Name.StartsWith(ProgramExpressionAmounts.Prefix, StringComparison.Ordinal)).ToArray();
        if (bindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != bindings.Length)
            throw new InvalidOperationException("An amount expression lost its unique instruction receipt.");
        foreach (var binding in bindings)
        {
            if (!int.TryParse(binding.Name.AsSpan(ProgramExpressionAmounts.Prefix.Length), System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var index) || binding.Name != ProgramExpressionAmounts.Key(index) || index < 1 || index > frame.InstructionIndex || index > plan.Instructions.Count || plan.Instructions[index - 1].AmountExpression is null || binding.Value < 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramAmountEvaluatedEvent>().Count(issued => issued.FrameId == frame.Id && issued.SkillId == frame.SkillId && issued.GameplayHash == frame.GameplayHash && issued.InstructionIndex == index && issued.Amount == binding.Value) != 1)
                throw new InvalidOperationException("An amount expression lost its exact issued quantity.");
        }
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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 93003)]
    internal void AssertCapturedExpressionNumbers(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var bindings = frame.NumberBindings.Where(binding => binding.Name.StartsWith(ExpressionNumberPrefix, StringComparison.Ordinal)).ToArray();
        if (bindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != bindings.Length)
            throw new InvalidOperationException("A captured expression number lost its unique name.");
        foreach (var binding in bindings)
        {
            var name = binding.Name[ExpressionNumberPrefix.Length..];
            var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramExpressionNumberCapturedEvent>().Where(record => record.FrameId == frame.Id && record.Name == name).ToArray();
            if (issued is not [var record] || record.SkillId != frame.SkillId || record.GameplayHash != frame.GameplayHash || record.Value != binding.Value || record.InstructionIndex < 1 || record.InstructionIndex > frame.InstructionIndex || record.InstructionIndex > plan.Instructions.Count || !ProgramOperationCatalog.Default.Resolve(plan.Instructions[record.InstructionIndex - 1].Op).Resources(plan.Instructions[record.InstructionIndex - 1]).OfType<CreateNumberBinding>().Any(produced => produced.Name == name))
                throw new InvalidOperationException("A captured expression number lost its exact producer receipt.");
        }
    }
}
