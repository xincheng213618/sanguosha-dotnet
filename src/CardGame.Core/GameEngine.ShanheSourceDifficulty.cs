namespace CardGame.Core;

public sealed record ShanheSourceArmorRestoredEvent(int RoundNumber, int PlayerSeat, int Amount,
    int ArmorBefore, int ArmorAfter, int RecoveryCap) : IGameEvent;
