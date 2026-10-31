#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTriggerWindowsRuntime
{
    internal global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost? CurrentDrivenProgramTriggerWindow() => _resolutionStackOperations._resolutionStack.LastOrDefault() switch
    {
        CardsMovedTriggerWindowFrame => _kernelCoreGameEngineProgramTriggerWindowsOperations.CreateCardsMovedTriggerWindowHost(),
        HpChangedTriggerWindowFrame => _kernelCoreGameEngineProgramTriggerWindowsOperations.CreateHpChangedTriggerWindowHost(),
        ProgramCardTriggerWindowFrame => _kernelCoreGameEngineProgramTriggerWindowsOperations.CreateCardActionTriggerWindowHost(),
        WuLuKangPindianFrame => _kernelCharacterLuKangWumiaoOperations.CreateWuLuKangWindowHost(),
        DouyuWindowFrame => _kernelCharacterPangJiOlOperations.CreateDouyuTriggerWindowHost(),
        _ => null
    };
    internal bool TryGetDrivenProgramTriggerCandidate(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramLifecycleTriggerWindowFrame && _programLifecycleWindowOperations.Get().TryReadPending(out var lifecycleEntry))
        {
            candidate = lifecycleEntry.Candidate;
            context = lifecycleEntry.Context;
            return true;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is PlayPhaseStartingBoundaryFrame && _programPhaseBoundaryOperations.Get().TryReadPlayStartingPending(out var startingEntry))
        {
            candidate = startingEntry.Candidate;
            context = startingEntry.Context;
            return true;
        }

        if (CurrentDrivenProgramTriggerWindow()is { } host && host.TryReadPending(out var entry))
        {
            candidate = entry.Candidate;
            context = entry.Context;
            return true;
        }

        candidate = null !;
        context = null !;
        return false;
    }

    internal bool TrySkipDrivenProgramCandidate(ProgramTriggerCandidate candidate) => _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramLifecycleTriggerWindowFrame ? _programLifecycleWindowOperations.Get().TrySkip(candidate) : _resolutionStackOperations._resolutionStack.LastOrDefault() is PlayPhaseStartingBoundaryFrame ? _programPhaseBoundaryOperations.Get().TrySkipPlayStartingCandidate(candidate) : CurrentDrivenProgramTriggerWindow()?.TrySkip(candidate) == true;
    internal bool TryResolveDrivenProgramTriggerGroupChoice(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action) => _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramLifecycleTriggerWindowFrame ? _programLifecycleWindowOperations.Get().TryResolveGroup(candidate, context, selected, action) : _resolutionStackOperations._resolutionStack.LastOrDefault() is PlayPhaseStartingBoundaryFrame ? _programPhaseBoundaryOperations.Get().TryResolvePlayStartingGroup(candidate, context, selected, action) : CurrentDrivenProgramTriggerWindow()?.TryResolveGroup(candidate, context, selected, action) == true;
}
