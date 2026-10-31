using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheSchoolHpAdjustedEvent(long FrameId, int OwnerSeat, int Before, int After, long? ChangeId) : IGameEvent;

public sealed partial class GameEngine
{
}
