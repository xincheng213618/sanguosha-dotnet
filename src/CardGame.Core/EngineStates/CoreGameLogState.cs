#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameLogState : Contracts.ICoreGameLogLogLogSequenceAccess, Contracts.ICoreGameLogLogAccess, Contracts.ICoreGameLogLogSequenceAccess
{
    private List<GameLogEntry> _log = [];
    internal ref readonly List<GameLogEntry> ReferenceToLog => ref _log;

    private int _logSequence = default !;
    internal ref int ReferenceToLogSequence => ref _logSequence;

    ref int Contracts.ICoreGameLogLogLogSequenceAccess._logSequence => ref _logSequence;

    ref readonly List<GameLogEntry> Contracts.ICoreGameLogLogLogSequenceAccess._log => ref _log;

    ref readonly List<GameLogEntry> Contracts.ICoreGameLogLogAccess._log => ref _log;

    ref int Contracts.ICoreGameLogLogSequenceAccess._logSequence => ref _logSequence;
}
