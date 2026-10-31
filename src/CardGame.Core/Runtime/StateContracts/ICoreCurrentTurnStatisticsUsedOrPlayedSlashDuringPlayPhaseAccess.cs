#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess : IProgramRuntimeOperations
{
    ref bool _usedOrPlayedSlashDuringPlayPhase { get; }
}
