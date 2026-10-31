#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramChildWindowEntries(Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime) : IProgramChildWindowEntries
{
    public bool Recovery(long frameId, PostEventContinuation continuation) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(frameId, continuation);
    public bool CharacterState(long frameId) => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    public bool HpChanged(long frameId, PostEventContinuation continuation) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, continuation);
}
