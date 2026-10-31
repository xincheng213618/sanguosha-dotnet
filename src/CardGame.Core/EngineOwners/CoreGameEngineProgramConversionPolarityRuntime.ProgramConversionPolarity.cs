#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramConversionPolarityRuntime
{
    void global::CardGame.Core.IConversionPolarityProgramHost.CommitConversionPolarity(ProgramSkillFrame frame) => CommitProgramConversionPolarity(frame);
    internal SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId) => _skillRuntimeStateOperations._skillRuntimeState.GetConversionState(ownerSeat, skillId);
    internal SkillPolarity CommitProgramConversionPolarity(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.ConversionPreviousPolarity is { } committed)
            return committed;
        var previous = GetProgramConversionPolarity(active.OwnerSeat, active.SkillId);
        var parentId = _resolutionStackOperations._resolutionStack.Count > 1 ? (long? )_resolutionStackOperations._resolutionStack[^2].Id : null;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { ConversionPreviousPolarity = previous });
        var current = _skillRuntimeStateOperations._skillRuntimeState.ToggleConversionState(active.OwnerSeat, active.SkillId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramConversionPolarityCommittedEvent(active.Id, parentId, active.OwnerSeat, active.SkillId, active.SkillInstanceId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), previous, current, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
        return previous;
    }
}
