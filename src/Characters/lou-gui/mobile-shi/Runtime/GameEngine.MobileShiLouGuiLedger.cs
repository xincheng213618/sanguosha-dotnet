namespace CardGame.Core;

public sealed partial class GameEngine
{
    // 灌沙's hand-limit bonus counts distinct gained basic names during the
    // current turn only; the committed event history is the only state store.
    private int ShiLgGuanshaHandLimitBonus(int seat) => !HasShiLgCapability ? 0 :
        ProgramEventHistory<ShiLgGuanshaAppliedEvent>()
            .Where(entry => entry.OwnerSeat == seat && entry.ActualTurn == _turnNumber)
            .Sum(entry => entry.DistinctNames);
}
