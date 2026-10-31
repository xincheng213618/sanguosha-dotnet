#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramChoiceProofsRuntime
{
    internal bool HasDeclaredProgramChoiceProof(ProgramSkillFrame frame, ProgramExecutionPlan plan, ProgramChoiceResultBinding binding)
    {
        var producers = plan.Instructions.Take(frame.InstructionIndex).Where(effect => effect.ResultBind == binding.Name).Select(effect => (Effect: effect, Descriptor: ProgramOperationCatalog.Default.Resolve(effect.Op))).Where(item => item.Descriptor is IProgramChoiceResultProof).ToArray();
        if (producers.Length == 0)
            return false;
        if (producers is not [var producer] || producer.Descriptor.Resources(producer.Effect).OfType<CreateChoiceResult>().Where(declaration => declaration.Name == binding.Name).ToArray()is not [var result] || !result.Options.Contains(binding.OptionId, StringComparer.Ordinal) || !((IProgramChoiceResultProof)producer.Descriptor).MatchesChoiceResult(producer.Effect, frame, binding, _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost()))
            throw new InvalidOperationException("An active program choice does not match its declared committed producer.");
        return true;
    }
}
