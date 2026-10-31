namespace CardGame.Core;

/// <summary>Generic post-movement program host. It contains no skill ids.</summary>
public sealed partial class GameEngine
{

    // Interim native alias to CardMovementBatchStore's original suppression stack.
    private readonly Stack<long> _suppressedCardMovedEventBatchIds;
}
