namespace CardGame.Core;

// Scalar-only, target-owned conclusion. No material IDs or concealed Hand
// identity is added to public history or snapshot.
public sealed record ComparedBlackSlashReceipt(long CardUseFrameId, long ActionId, int ActualTurnNumber,
    int ActualTurnOwnerSeat, int ActorSeat, int ProviderSeat, int TargetSeat, CardKind EffectiveKind,
    Suit EffectiveSuit, bool EffectiveIsRed, int ActorHp, int TargetHp, int ActorHandCount, int TargetHandCount,
    CardConversionSource? TargetPolicySource, string? TargetPolicyHash, CardConversionSource? ActorPolicySource,
    string? ActorPolicyHash, bool Ineffective, bool CannotRespond);
public sealed record ComparedBlackSlashAppliedEvent(ComparedBlackSlashReceipt Receipt) : IGameEvent;
