#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramChildWindowRuntimeRuntime
{
    bool IProgramChildWindowEntries.Recovery(long frameId, PostEventContinuation continuation) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, continuation);
    bool IProgramChildWindowEntries.CharacterState(long frameId) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    bool IProgramChildWindowEntries.HpChanged(long frameId, PostEventContinuation continuation) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, continuation);
}
