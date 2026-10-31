#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardUsePaymentsCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }

    bool HasPaidCardUseMovementTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}
