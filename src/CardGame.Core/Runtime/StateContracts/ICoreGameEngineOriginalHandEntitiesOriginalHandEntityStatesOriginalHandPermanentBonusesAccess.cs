#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<long, OriginalHandPermanentBonus> _originalHandPermanentBonuses { get; }

    ref readonly Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> _originalHandEntityStates { get; }
}
