#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramCardUsePaymentsRuntime
{
    internal ProgramCardUsePayments CardUsePayments => _cardUsePaymentsOperations._cardUsePayments ??= new(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsDefinitions.CardUsePaymentMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType));

    internal bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.LeadingTail, use, continuation) || continuation.Kind == RecoveryPaidCardUseKind.CommittedSlash || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.TrailingTail, use, continuation);
    internal bool ObservesPaidCardUseChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => continuation.Kind == RecoveryPaidCardUseKind.CommittedSlash && global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime.OwnsGenericSlashPaymentTail(use) || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.LeadingTail, use, continuation) || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.TrailingTail, use, continuation);
}
