namespace CardGame.Core;

/// <summary>A frozen replacement owned by the real card's attack state.</summary>
public sealed partial record CardDamageMaximumHpReplacementReceipt(CardUseEffectSource Source, int RoundNumber,
    int SourceSeat, int TargetSeat, CardKind CardKind, int Amount,
    int MaximumHpBefore, int MaximumHpAfter, int? HpBefore, int? HpAfter);

public sealed record CardDamageReplacedWithMaximumHpLossEvent(long ParentResolutionId,
    CardDamageMaximumHpReplacementReceipt Receipt) : IGameEvent;
