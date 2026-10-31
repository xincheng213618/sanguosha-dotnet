#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWuJingPaymentsRuntime
{
    private readonly ICoreWuJingPaymentsCharacterWuJingOlOperations _characterWuJingOlOperations;
    private readonly ICoreWuJingPaymentsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWuJingPaymentsCoreCardUsePaymentsOperations _coreCardUsePaymentsOperations;
    internal CoreGameEngineWuJingPaymentsRuntime(ICoreWuJingPaymentsCharacterWuJingOlOperations characterWuJingOlOperations, ICoreWuJingPaymentsResolutionStackOperations resolutionStackOperations, ICoreWuJingPaymentsCoreCardUsePaymentsOperations coreCardUsePaymentsOperations)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
    }
}
