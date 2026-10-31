using System.Globalization;
namespace CardGame.Core;
public sealed record MouFaZhengActivationPhaseEvent(long FrameId, int OwnerSeat, ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
