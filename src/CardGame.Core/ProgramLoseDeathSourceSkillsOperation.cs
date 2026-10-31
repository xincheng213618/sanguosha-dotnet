namespace CardGame.Core;

public sealed record CharacterSkillsLostEvent(int Seat, int SourceSeat, IReadOnlyList<string> SkillIds) : IGameEvent;
