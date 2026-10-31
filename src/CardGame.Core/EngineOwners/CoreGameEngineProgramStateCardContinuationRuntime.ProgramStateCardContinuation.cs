#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramStateCardContinuationRuntime
{
    internal bool HasCharacterStateCardUseContinuation()
    {
        var windows = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Where(frame => frame.CharacterStateContinuation is CharacterStateContinuation.SoleIncomingRangeDraw or CharacterStateContinuation.CardUse or CharacterStateContinuation.VirtualBasicCardUse or CharacterStateContinuation.NativeCardEffectDraw or CharacterStateContinuation.WenheCardEffect or CharacterStateContinuation.WumiaoFlood or CharacterStateContinuation.DragonBoatCardEffect or CharacterStateContinuation.GuanduCardEffect).ToArray();
        foreach (var window in windows)
        {
            var index = _resolutionStackOperations._resolutionStack.IndexOf(window);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame parent || parent.Id != window.ResumeProgramFrameId || (window.CharacterStateContinuation == CharacterStateContinuation.SoleIncomingRangeDraw ? !_coreSoleIncomingRangeDrawOperations.IsSoleIncomingRangeDrawStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.GuanduCardEffect ? !_coreGuanduCardsOperations.IsGuanduCardEffectStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.WumiaoFlood ? !_characterGuanYuWumiaoOperations.IsWumiaoFloodStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.DragonBoatCardEffect ? !_coreDragonBoatCardsOperations.IsDragonBoatCardEffectStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.WenheCardEffect ? !_coreWenheCardsOperations.IsWenheCardEffectStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.NativeCardEffectDraw ? !_coreNativeCardEffectDrawsOperations.IsNativeCardEffectDrawStateReturn(parent, window) : window.CharacterStateContinuation == CharacterStateContinuation.VirtualBasicCardUse ? parent.CardId != 0 || parent.VirtualBasicReturn is null || parent.VirtualBasicEffectApplied != true || window.ResumeCardId is not null || window.ResumeCardKind != parent.CardKind : parent.CardId != window.ResumeCardId) || window.Continuation != ProgramLifecycleContinuation.ResumeCharacterStateChange || window.Window is not (SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or SkillProgramTriggerWindow.CharacterEnteredChain) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
                throw new InvalidOperationException("A character-state card continuation lost its exact parent or candidate cursor.");
        }

        return windows.Length != 0;
    }
}
