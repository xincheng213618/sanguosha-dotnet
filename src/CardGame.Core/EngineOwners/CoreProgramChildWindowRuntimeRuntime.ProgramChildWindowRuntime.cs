#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramChildWindowRuntimeRuntime
{
    bool global::CardGame.Core.IProgramChildWindowEntries.Recovery(long frameId, PostEventContinuation continuation) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, continuation);
    bool global::CardGame.Core.IProgramChildWindowEntries.CharacterState(long frameId) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    bool global::CardGame.Core.IProgramChildWindowEntries.HpChanged(long frameId, PostEventContinuation continuation) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, continuation);
}
