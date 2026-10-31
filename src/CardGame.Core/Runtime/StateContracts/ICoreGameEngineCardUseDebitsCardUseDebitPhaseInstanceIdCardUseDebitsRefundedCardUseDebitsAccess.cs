#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess : IProgramRuntimeOperations
{
    ref int _cardUseDebitPhaseInstanceId { get; }

    ref readonly Dictionary<long, CardUseDebitIdentity> _cardUseDebits { get; }

    ref readonly HashSet<long> _refundedCardUseDebits { get; }
}
