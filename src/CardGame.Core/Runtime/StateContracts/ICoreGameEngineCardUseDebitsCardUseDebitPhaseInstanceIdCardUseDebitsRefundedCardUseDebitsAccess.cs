#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _cardUseDebitPhaseInstanceId { get; }

    ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.CardUseDebitIdentity> _cardUseDebits { get; }

    ref readonly global::System.Collections.Generic.HashSet<long> _refundedCardUseDebits { get; }
}
