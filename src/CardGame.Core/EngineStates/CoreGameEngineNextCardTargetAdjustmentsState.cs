#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNextCardTargetAdjustmentsState : Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess, Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess
{
    private Dictionary<int, CardConversionSource> _nextCardTargetAdjustmentOwners = [];
    internal ref readonly Dictionary<int, CardConversionSource> ReferenceToNextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustmentOwners;

    private IReadOnlyList<int>? _selectedNextCardTargetSeats = default !;
    internal ref IReadOnlyList<int>? ReferenceToSelectedNextCardTargetSeats => ref _selectedNextCardTargetSeats;

    ref IReadOnlyList<int>? Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats => ref _selectedNextCardTargetSeats;

    ref readonly Dictionary<int, CardConversionSource> Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustmentOwners;
}
