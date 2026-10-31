#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreRecoveryReplacementCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ObservesPaidCardUseChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreRecoveryReplacementCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}
