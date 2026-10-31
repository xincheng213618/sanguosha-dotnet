namespace CardGame.Core;

internal interface IGameStartingDrawOrigins : IProgramRuntimeOperations
{
    GameStartingDrawOrigin? Capture(int recipient, int requested, CardMoveReason reason,
        CardConversionSource? producer, out GameStartingMovementDrawOrigin? movementOrigin);
    bool IsValidProvenance(NativeDrawInvocationProof proof);
    bool SameMovementDrawOrigin(GameStartingMovementDrawOrigin? left, GameStartingMovementDrawOrigin? right);
}

internal interface IGameStartingDrawOriginQueries : IProgramSelectionQuery
{
    bool IsExactGameStartingDrawProgram(ProgramSkillFrame program);
    bool IsExactGameStartingGainDrawProgram(ProgramSkillFrame program);
}

// Only this metadata write belongs to the existing movement-batch owner.
internal interface IGameStartingMovementBatchOrigins : IProgramRuntimeOperations
{
    void AssignGameStartingMovementOrigin(int pendingIndex, GameStartingNativeMovementOrigin origin);
}

internal interface IGameStartingDrawNativeFacts : IProgramSelectionQuery
{
    bool SetupComplete { get; }
    bool TracksNativeDrawInvocations { get; }
    bool HasOperationReturn(SkillProgramEffectOp operation);
}

// Validation is deferred until a query executes. The invoice runtime consumes
// the startup source, so eagerly resolving its query service would form a cycle.
internal interface IGameStartingDrawNativeValidation : IProgramSelectionQuery
{
    bool IsValidNativeDrawInvocation(NativeDrawInvocationProof proof);
}

// Character domains validate their own paid receipt. Startup origin, child and
// payment proofs are interpreted exclusively by GameStartingDrawOriginRuntime.
internal interface IGameStartingDrawNativeReceipts : IProgramRuntimeOperations
{
    IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
}

public sealed partial class GameEngine
{
}
