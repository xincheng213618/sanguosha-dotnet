#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _roundNumber { get; }
}
