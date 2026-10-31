#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanJiOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 230)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 220)]
    internal bool IsYuanJiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.YuanJi is { Stage: YuanJiStage.DrawChildren } r && r.Draws.Count > 0 && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
}
