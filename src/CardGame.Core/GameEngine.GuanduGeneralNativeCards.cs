using System.Globalization;

namespace CardGame.Core;

public sealed record GuanduDamagingCardRecordedEvent(long DamageFrameId, long UseId, long ActionId,
    int TargetSeat, CardKind Kind) : IGameEvent;
public sealed record GuanduChoulueArmedEvent(long ProgramFrameId, int OwnerSeat, string InstanceId, int ActualTurn,
    long PhaseBoundaryId, int ProviderSeat, int GiftCardId, int Before, int After, CardKind Kind) : IGameEvent;
public sealed record GuanduYuanlveGiftEvent(long ProgramFrameId, CardConversionSource Source, string GameplayHash,
    int ActualTurn, int TurnOwner, long ActualPhase, int OwnerSeat, int RecipientSeat, int CardId, long Before, long After) : IGameEvent;
