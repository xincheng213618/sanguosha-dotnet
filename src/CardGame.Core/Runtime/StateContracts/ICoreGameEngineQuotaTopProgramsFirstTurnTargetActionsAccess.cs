#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions { get; }
}
