namespace CardGame.Core;

public sealed partial class GameEngine
{
internal readonly record struct ProgramSkillChangeContext(ProgramSkillFrame Frame, CharacterState Owner);
}
