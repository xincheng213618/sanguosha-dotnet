#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchDefinitionState : global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess
{
    private int _drawPerTurn = default !;
    internal ref int ReferenceToDrawPerTurn => ref _drawPerTurn;

    private global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> _generalPool = default !;
    internal ref readonly global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> ReferenceToGeneralPool => ref _generalPool;

    private bool _generalPoolInitialized;
    private bool _modeDefinitionInitialized;
    private bool _optionsInitialized;
    private bool _playerCountInitialized;
    private static T InitializeOnce<T>(ref T field, ref bool initialized, T value, string name)
    {
        if (initialized)
            throw new InvalidOperationException($"Match definition '{name}' is already initialized.");
        initialized = true;
        return field = value;
    }

    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> InitializeGeneralPool(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> value) => InitializeOnce(ref _generalPool, ref _generalPoolInitialized, value, nameof(_generalPool));
    private int _initialHandSize = default !;
    internal ref int ReferenceToInitialHandSize => ref _initialHandSize;

    private global::CardGame.Core.ContentModeDefinition _modeDefinition = default !;
    internal ref readonly global::CardGame.Core.ContentModeDefinition ReferenceToModeDefinition => ref _modeDefinition;

    internal global::CardGame.Core.ContentModeDefinition InitializeModeDefinition(global::CardGame.Core.ContentModeDefinition value) => InitializeOnce(ref _modeDefinition, ref _modeDefinitionInitialized, value, nameof(_modeDefinition));
    private global::CardGame.Core.GameOptions _options = default !;
    internal ref readonly global::CardGame.Core.GameOptions ReferenceToOptions => ref _options;

    internal global::CardGame.Core.GameOptions InitializeOptions(global::CardGame.Core.GameOptions value) => InitializeOnce(ref _options, ref _optionsInitialized, value, nameof(_options));
    private int _playerCount = default !;
    internal ref readonly int ReferenceToPlayerCount => ref _playerCount;

    internal int InitializePlayerCount(int value) => InitializeOnce(ref _playerCount, ref _playerCountInitialized, value, nameof(_playerCount));
    ref readonly global::CardGame.Core.ContentModeDefinition global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition => ref _modeDefinition;

    ref readonly global::CardGame.Core.GameOptions global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess._options => ref _options;

    ref readonly global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess._generalPool => ref _generalPool;

    ref readonly global::CardGame.Core.ContentModeDefinition global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess._modeDefinition => ref _modeDefinition;

    ref readonly int global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly global::CardGame.Core.GameOptions global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess._options => ref _options;

    ref readonly int global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly global::CardGame.Core.GameOptions global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess._options => ref _options;

    ref readonly global::CardGame.Core.ContentModeDefinition global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition => ref _modeDefinition;

    ref readonly global::CardGame.Core.GameOptions global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options => ref _options;

    ref readonly int global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly global::CardGame.Core.ContentModeDefinition global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition => ref _modeDefinition;

    ref readonly int global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly global::CardGame.Core.ContentModeDefinition global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess._modeDefinition => ref _modeDefinition;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess._initialHandSize => ref _initialHandSize;

    ref readonly global::CardGame.Core.GameOptions global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess._options => ref _options;

    ref readonly global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess._generalPool => ref _generalPool;
}
