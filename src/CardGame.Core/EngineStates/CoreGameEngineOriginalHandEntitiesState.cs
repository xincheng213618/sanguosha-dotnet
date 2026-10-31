#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineOriginalHandEntitiesState : Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess, Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess
{
    // Durable gameplay state, not pending resolution. A departed physical
    // entity never becomes original again on return or source reacquisition.
    private Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> _originalHandEntityStates = new();
    internal ref readonly Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> ReferenceToOriginalHandEntityStates => ref _originalHandEntityStates;

    private Dictionary<long, OriginalHandPermanentBonus> _originalHandPermanentBonuses = new();
    internal ref readonly Dictionary<long, OriginalHandPermanentBonus> ReferenceToOriginalHandPermanentBonuses => ref _originalHandPermanentBonuses;

    ref readonly Dictionary<long, OriginalHandPermanentBonus> Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses => ref _originalHandPermanentBonuses;

    ref readonly Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates => ref _originalHandEntityStates;

    ref readonly Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess._originalHandEntityStates => ref _originalHandEntityStates;
}
