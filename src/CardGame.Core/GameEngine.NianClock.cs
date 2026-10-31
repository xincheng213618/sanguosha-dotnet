namespace CardGame.Core;

/// <summary>Trusted host elapsed time, recorded as data so a replay never reads a wall clock.</summary>
public sealed record TickNianClockCommand(int ElapsedMilliseconds, long ExpectedRevision) : GameCommand(-1, ExpectedRevision);
public sealed record NianClockAdvancedEvent(int ElapsedMilliseconds, int TotalMilliseconds) : IGameEvent;
public sealed record NianClockExpiredEvent(int TotalMilliseconds) : IGameEvent;
public sealed record NianResolutionCancelledEvent(long FrameId, ResolutionFrameKind Kind) : IGameEvent;

public sealed partial class GameEngine
{
    public bool IsNianClockRunning =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreNianModeProjectionRuntime>().IsNianClockRunning;
    public int? NianRemainingMilliseconds =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreNianModeProjectionRuntime>().NianRemainingMilliseconds;
}
