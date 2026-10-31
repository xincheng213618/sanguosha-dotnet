#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramLifeRestorationEntries
{
    internal void PublishProgramRevivalFaceState(global::CardGame.Core.CharacterState target, bool wasFaceDown) => _resolve().PublishProgramRevivalFaceState(target, wasFaceDown);
    internal bool RestoreProgramCharacterLife(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CharacterState target, int hp) => _resolve().RestoreProgramCharacterLife(frame, target, hp);
}
