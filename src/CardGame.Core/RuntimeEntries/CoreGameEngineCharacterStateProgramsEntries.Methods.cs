#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterStateProgramsEntries
{
    internal void CompleteFaceUpSkippedTurn(CharacterState current) => _resolve().CompleteFaceUpSkippedTurn(current);
    internal void RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window, long? parentFrameId = default, CharacterTurnedOverState? turnedOver = default) => _resolve().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
    internal void ResumeCharacterStateChange(ProgramLifecycleTriggerWindowFrame frame) => _resolve().ResumeCharacterStateChange(frame);
    internal bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, CharacterStateContinuation continuation = (CharacterStateContinuation)0, int? cardId = default, CardKind? cardKind = default) => _resolve().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
    internal bool TryBeginJudgmentPhaseStartingPrograms(CharacterState turnOwner) => _resolve().TryBeginJudgmentPhaseStartingPrograms(turnOwner);
}
