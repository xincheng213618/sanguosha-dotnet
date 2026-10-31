namespace CardGame.Core;

/// <summary>A declaration-owned policy, independent of later grant enablement.</summary>
public sealed record NoDamageSkillDisablePolicy(long UseFrameId, long ActionId, CardConversionSource Source,
    string GameplayHash, int TurnNumber, int TurnOwnerSeat);
public sealed record NoDamageSkillDisablePolicyIssuedEvent(NoDamageSkillDisablePolicy Policy) : IGameEvent;
