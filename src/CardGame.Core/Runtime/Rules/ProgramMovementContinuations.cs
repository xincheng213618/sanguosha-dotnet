namespace CardGame.Core;

// These capabilities are implemented by the shared window/completion runtimes.
// Callers keep their pending state on the original frame while the existing
// movement scheduler owns child windows and their typed returns.
internal interface IProgramMovementWindows : IProgramRuntimeOperations
{
    bool Begin(long? instructionFrameId = null, int? subjectSeat = null);
}

internal interface IProgramMovementReturns : IProgramRuntimeOperations
{
    void ReturnMovement(long frameId);
}
