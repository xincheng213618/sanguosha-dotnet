using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record MobileShiLuYuShengTrack(ActualDiscardRecoveryPhaseKey? Phase,
        IReadOnlyList<int> GainedSeats, IReadOnlyList<int> MarkedCards, int DebtRequired, int DebtProgress, bool Refunded);

}
