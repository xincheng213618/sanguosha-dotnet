#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess : IProgramRuntimeOperations
{
    ref IReadOnlyList<int>? _selectedNextCardTargetSeats { get; }
}
