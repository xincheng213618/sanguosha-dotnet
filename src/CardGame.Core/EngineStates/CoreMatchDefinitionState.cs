#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchDefinitionState : Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess, Contracts.ICoreMatchDefinitionGeneralPoolAccess, Contracts.ICoreMatchDefinitionModeDefinitionAccess, Contracts.ICoreMatchDefinitionPlayerCountAccess, Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess, Contracts.ICoreMatchDefinitionOptionsAccess, Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess, Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess, Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess, Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess
{
    private int _drawPerTurn = default !;
    internal ref int ReferenceToDrawPerTurn => ref _drawPerTurn;

    private IReadOnlyList<GeneralDefinition> _generalPool = default !;
    internal ref readonly IReadOnlyList<GeneralDefinition> ReferenceToGeneralPool => ref _generalPool;

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

    internal IReadOnlyList<GeneralDefinition> InitializeGeneralPool(IReadOnlyList<GeneralDefinition> value) => InitializeOnce(ref _generalPool, ref _generalPoolInitialized, value, nameof(_generalPool));
    private int _initialHandSize = default !;
    internal ref int ReferenceToInitialHandSize => ref _initialHandSize;

    private ContentModeDefinition _modeDefinition = default !;
    internal ref readonly ContentModeDefinition ReferenceToModeDefinition => ref _modeDefinition;

    internal ContentModeDefinition InitializeModeDefinition(ContentModeDefinition value) => InitializeOnce(ref _modeDefinition, ref _modeDefinitionInitialized, value, nameof(_modeDefinition));
    private GameOptions _options = default !;
    internal ref readonly GameOptions ReferenceToOptions => ref _options;

    internal GameOptions InitializeOptions(GameOptions value) => InitializeOnce(ref _options, ref _optionsInitialized, value, nameof(_options));
    private int _playerCount = default !;
    internal ref readonly int ReferenceToPlayerCount => ref _playerCount;

    internal int InitializePlayerCount(int value) => InitializeOnce(ref _playerCount, ref _playerCountInitialized, value, nameof(_playerCount));
    ref readonly ContentModeDefinition Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition => ref _modeDefinition;

    ref readonly GameOptions Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess._options => ref _options;

    ref readonly IReadOnlyList<GeneralDefinition> Contracts.ICoreMatchDefinitionGeneralPoolAccess._generalPool => ref _generalPool;

    ref readonly ContentModeDefinition Contracts.ICoreMatchDefinitionModeDefinitionAccess._modeDefinition => ref _modeDefinition;

    ref readonly int Contracts.ICoreMatchDefinitionPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly GameOptions Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess._options => ref _options;

    ref readonly int Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly GameOptions Contracts.ICoreMatchDefinitionOptionsAccess._options => ref _options;

    ref readonly ContentModeDefinition Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition => ref _modeDefinition;

    ref readonly GameOptions Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options => ref _options;

    ref readonly int Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly ContentModeDefinition Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition => ref _modeDefinition;

    ref readonly int Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess._playerCount => ref _playerCount;

    ref readonly ContentModeDefinition Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess._modeDefinition => ref _modeDefinition;

    ref int Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess._initialHandSize => ref _initialHandSize;

    ref readonly GameOptions Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess._options => ref _options;

    ref readonly IReadOnlyList<GeneralDefinition> Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess._generalPool => ref _generalPool;
}
