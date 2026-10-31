namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardConversionSource> _nextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustments.ReferenceToNextCardTargetAdjustmentOwners;
    private ref global::System.Collections.Generic.IReadOnlyList<int>? _selectedNextCardTargetSeats => ref _nextCardTargetAdjustments.ReferenceToSelectedNextCardTargetSeats;
}
