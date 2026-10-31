#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions { get; }
}
