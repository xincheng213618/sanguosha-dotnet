#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineOriginalHandEntitiesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess
{
    // Durable gameplay state, not pending resolution. A departed physical
    // entity never becomes original again on return or source reacquisition.
    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> _originalHandEntityStates = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> ReferenceToOriginalHandEntityStates => ref _originalHandEntityStates;

    private global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.OriginalHandPermanentBonus> _originalHandPermanentBonuses = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.OriginalHandPermanentBonus> ReferenceToOriginalHandPermanentBonuses => ref _originalHandPermanentBonuses;

    ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.OriginalHandPermanentBonus> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses => ref _originalHandPermanentBonuses;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates => ref _originalHandEntityStates;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess._originalHandEntityStates => ref _originalHandEntityStates;
}
