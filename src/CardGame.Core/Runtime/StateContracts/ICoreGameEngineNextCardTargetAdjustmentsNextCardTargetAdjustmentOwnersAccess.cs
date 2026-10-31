#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, CardConversionSource> _nextCardTargetAdjustmentOwners { get; }
}
