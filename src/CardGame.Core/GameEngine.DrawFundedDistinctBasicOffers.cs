using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record DrawFundedBasicNeed(DrawFundedDistinctBasicIntent Intent, long? ParentFrameId,
        long? RequestFrameId, long? ParentActionId, int Cursor, int? FixedTargetSeat);
}
