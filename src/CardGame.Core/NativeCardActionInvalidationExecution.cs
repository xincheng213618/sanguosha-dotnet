namespace CardGame.Core;

internal interface INativeCardActionInvalidationExecution : IProgramRuntimeOperations
{
    bool TryCompleteSimple(long useId, ProgramSimpleCardContinuation continuation,
        NativeSimpleCardCancellationBoundary boundary);
    bool HasIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action,
        ProgramCardContinuation continuation);
    bool ContinueIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action,
        ProgramCardContinuation continuation);
}
