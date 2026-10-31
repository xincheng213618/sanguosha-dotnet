#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundNumberAccess : IProgramRuntimeOperations
{
    ref int _roundNumber { get; }
}
