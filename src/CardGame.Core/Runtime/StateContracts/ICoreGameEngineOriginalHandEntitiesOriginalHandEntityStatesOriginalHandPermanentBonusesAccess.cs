#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.OriginalHandPermanentBonus> _originalHandPermanentBonuses { get; }

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> _originalHandEntityStates { get; }
}
