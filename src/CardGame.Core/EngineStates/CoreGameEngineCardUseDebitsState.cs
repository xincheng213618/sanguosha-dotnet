#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCardUseDebitsState : Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess
{
    private int _cardUseDebitPhaseInstanceId = default !;
    internal ref int ReferenceToCardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    private Dictionary<long, CardUseDebitIdentity> _cardUseDebits = [];
    internal ref readonly Dictionary<long, CardUseDebitIdentity> ReferenceToCardUseDebits => ref _cardUseDebits;

    private HashSet<long> _refundedCardUseDebits = [];
    internal ref readonly HashSet<long> ReferenceToRefundedCardUseDebits => ref _refundedCardUseDebits;

    ref int Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    ref int Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId => ref _cardUseDebitPhaseInstanceId;

    ref readonly Dictionary<long, CardUseDebitIdentity> Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits => ref _cardUseDebits;

    ref readonly HashSet<long> Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._refundedCardUseDebits => ref _refundedCardUseDebits;
}
