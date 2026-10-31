#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNextCardTargetAdjustmentsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardConversionSource> _nextCardTargetAdjustmentOwners = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardConversionSource> ReferenceToNextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustmentOwners;

    private global::System.Collections.Generic.IReadOnlyList<int>? _selectedNextCardTargetSeats = default !;
    internal ref global::System.Collections.Generic.IReadOnlyList<int>? ReferenceToSelectedNextCardTargetSeats => ref _selectedNextCardTargetSeats;

    ref global::System.Collections.Generic.IReadOnlyList<int>? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats => ref _selectedNextCardTargetSeats;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardConversionSource> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustmentOwners;
}
