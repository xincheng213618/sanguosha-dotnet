namespace CardGame.Core;
public sealed record MouFaZhengRosterEvent(long FrameId, int Index, long MarkId) : IGameEvent;
