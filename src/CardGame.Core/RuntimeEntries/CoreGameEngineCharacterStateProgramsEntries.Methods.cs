#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterStateProgramsEntries
{
    internal void CompleteFaceUpSkippedTurn(global::CardGame.Core.CharacterState current) => _resolve().CompleteFaceUpSkippedTurn(current);
    internal void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default) => _resolve().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
    internal void ResumeCharacterStateChange(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().ResumeCharacterStateChange(frame);
    internal bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => _resolve().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
    internal bool TryBeginJudgmentPhaseStartingPrograms(global::CardGame.Core.CharacterState turnOwner) => _resolve().TryBeginJudgmentPhaseStartingPrograms(turnOwner);
}
