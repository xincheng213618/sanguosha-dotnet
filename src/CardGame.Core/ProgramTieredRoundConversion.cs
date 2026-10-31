using System.Text.Json.Serialization;

namespace CardGame.Core;

// Optional ViewAs policy. A missing policy preserves the original positive
// input-count rules, configured conversion tiers and instance-scoped quotas.
public sealed record ProgramTieredRoundConversionPolicy(string StateId, string UsageId);

public sealed record TieredRoundConversionUseReceipt(
    CardConversionSource Source, string GameplayHash, string StateId, string UsageId,
    int FrozenTier, int RoundNumber, int ActualTurnNumber, int ActualTurnOwnerSeat,
    int PhaseInstanceId, long CardActionId, long OwnerFrameId, int ActorSeat,
    CardKind EffectiveKind, int MaterialCount, long? DyingFrameId = null);

public sealed record TrueRoundCardNameUsedEvent(int RoundNumber, int ActorSeat,
    long OriginFrameId, long? CardActionId, CardKind EffectiveKind, bool NullificationUse) : IGameEvent;
public sealed record TieredRoundConversionUseIssuedEvent(TieredRoundConversionUseReceipt Receipt) : IGameEvent;
