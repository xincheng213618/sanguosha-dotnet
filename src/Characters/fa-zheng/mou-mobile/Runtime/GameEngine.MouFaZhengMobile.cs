using System.Globalization;
namespace CardGame.Core;
public sealed partial class GameEngine
{
}
public sealed record MouFaZhengActivationPhaseEvent(long FrameId, int OwnerSeat, ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
