#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess : IProgramRuntimeOperations
{
    ref int _cardUseDebitPhaseInstanceId { get; }
}
