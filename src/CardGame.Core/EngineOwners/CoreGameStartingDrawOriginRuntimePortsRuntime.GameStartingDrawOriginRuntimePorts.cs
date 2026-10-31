#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameStartingDrawOriginRuntimePortsRuntime
{
    bool global::CardGame.Core.IGameStartingDrawNativeFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    bool global::CardGame.Core.IGameStartingDrawNativeFacts.TracksNativeDrawInvocations => _coreOutsidePhaseDrawDiscardOperations.TracksNativeDrawInvocations;

    bool global::CardGame.Core.IGameStartingDrawNativeFacts.HasOperationReturn(SkillProgramEffectOp operation) => global::CardGame.Core.EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
    bool global::CardGame.Core.IGameStartingDrawNativeValidation.IsValidNativeDrawInvocation(NativeDrawInvocationProof proof) => _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof);
    IReadOnlyList<TangTangGain>? global::CardGame.Core.IGameStartingDrawNativeReceipts.CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterTangTangOlOperations.CurrentTangTangGains(candidate, context);
}
