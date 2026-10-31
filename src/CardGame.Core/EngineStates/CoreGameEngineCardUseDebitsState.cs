#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCardUseDebitsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess
{
    private int _cardUseDebitPhaseInstanceId = default !;
    internal ref int ReferenceToCardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    private global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.CardUseDebitIdentity> _cardUseDebits = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.CardUseDebitIdentity> ReferenceToCardUseDebits => ref _cardUseDebits;

    private global::System.Collections.Generic.HashSet<long> _refundedCardUseDebits = [];
    internal ref readonly global::System.Collections.Generic.HashSet<long> ReferenceToRefundedCardUseDebits => ref _refundedCardUseDebits;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.CardUseDebitIdentity> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits => ref _cardUseDebits;

    ref readonly global::System.Collections.Generic.HashSet<long> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._refundedCardUseDebits => ref _refundedCardUseDebits;
}
