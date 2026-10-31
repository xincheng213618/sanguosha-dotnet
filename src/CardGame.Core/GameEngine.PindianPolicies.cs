namespace CardGame.Core;
public sealed record PindianTopSourceChosenEvent(int OwnerSeat,CardConversionSource Source,int CardId) : IGameEvent;
