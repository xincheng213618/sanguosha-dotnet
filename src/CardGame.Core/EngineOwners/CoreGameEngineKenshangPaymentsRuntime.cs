#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineKenshangPaymentsRuntime
{
    private readonly ICoreKenshangPaymentsCharacterMaXiuMaTieOlOperations _characterMaXiuMaTieOlOperations;
    private readonly ICoreKenshangPaymentsCardZonesOperations _cardZonesOperations;
    private readonly ICoreKenshangPaymentsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreKenshangPaymentsCoreCardUsePaymentsOperations _coreCardUsePaymentsOperations;
    internal CoreGameEngineKenshangPaymentsRuntime(ICoreKenshangPaymentsCharacterMaXiuMaTieOlOperations characterMaXiuMaTieOlOperations, ICoreKenshangPaymentsCardZonesOperations cardZonesOperations, ICoreKenshangPaymentsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreKenshangPaymentsCoreCardUsePaymentsOperations coreCardUsePaymentsOperations)
    {
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
    }
}
