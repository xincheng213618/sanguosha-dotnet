#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess : IProgramRuntimeOperations
{
    ref long _currentTurnSkillSuppressionRevision { get; }
}
