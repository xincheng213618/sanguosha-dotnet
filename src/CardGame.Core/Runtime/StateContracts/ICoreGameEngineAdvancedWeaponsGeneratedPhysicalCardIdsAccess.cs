#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _generatedPhysicalCardIds { get; }
}
