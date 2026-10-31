#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYiOlRuntime
{
    internal void ResumeZhangYiDianjunExtraPlay(long programFrameId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(programFrameId);
        if (f.ZhangYi is not { Stage: ZhangYiStage.DianjunExtraPlay } r || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Finished)
            throw new InvalidOperationException("Dianjun received a foreign extra-phase return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangYi = r with { Stage = ZhangYiStage.DianjunReturned } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(programFrameId);
    }
}
