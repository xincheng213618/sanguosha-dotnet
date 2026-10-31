namespace CardGame.Core;

public sealed record ProgramPublicPileSelectedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, string Bind, string SourceSkillId, string? SourceSkillInstanceId,
    CardLocation? SourceLocation, IReadOnlyList<int> CardIds, IReadOnlyList<Suit> PrintedSuits) : IGameEvent;

public sealed partial class GameEngine
{
}
