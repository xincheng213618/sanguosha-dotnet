#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramLifeRestorationEntries
{
    internal void PublishProgramRevivalFaceState(CharacterState target, bool wasFaceDown) => _resolve().PublishProgramRevivalFaceState(target, wasFaceDown);
    internal bool RestoreProgramCharacterLife(ProgramSkillFrame frame, CharacterState target, int hp) => _resolve().RestoreProgramCharacterLife(frame, target, hp);
}
