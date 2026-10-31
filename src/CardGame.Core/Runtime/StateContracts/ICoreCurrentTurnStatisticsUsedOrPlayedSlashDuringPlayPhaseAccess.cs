#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _usedOrPlayedSlashDuringPlayPhase { get; }
}
