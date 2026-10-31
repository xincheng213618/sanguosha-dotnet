#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameStartingDrawOriginRuntimePortsRuntime
{
    bool IGameStartingDrawNativeFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    bool IGameStartingDrawNativeFacts.TracksNativeDrawInvocations => _coreOutsidePhaseDrawDiscardOperations.TracksNativeDrawInvocations;

    bool IGameStartingDrawNativeFacts.HasOperationReturn(SkillProgramEffectOp operation) => CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
    bool IGameStartingDrawNativeValidation.IsValidNativeDrawInvocation(NativeDrawInvocationProof proof) => _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof);
    IReadOnlyList<TangTangGain>? IGameStartingDrawNativeReceipts.CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterTangTangOlOperations.Value.CurrentTangTangGains(candidate, context);
}
