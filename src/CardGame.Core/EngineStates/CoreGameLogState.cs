#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameLogState : global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogSequenceAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> _log = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> ReferenceToLog => ref _log;

    private int _logSequence = default !;
    internal ref int ReferenceToLogSequence => ref _logSequence;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess._logSequence => ref _logSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess._log => ref _log;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogAccess._log => ref _log;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogSequenceAccess._logSequence => ref _logSequence;
}
