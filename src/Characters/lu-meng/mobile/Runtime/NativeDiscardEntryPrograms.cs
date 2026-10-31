using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeDiscardEntryStage { Choosing, Skipping, Entering }

public sealed record NativeDiscardEntryFrame(long Id, CardConversionSource Source, string GameplayHash,
    int TurnNumber, int TurnOwnerSeat, TurnPhase BeforePhase, int PlayPhaseInstanceId, PromptId PromptId,
    NativeDiscardEntryStage Stage = NativeDiscardEntryStage.Choosing)
    : ResolutionFrame(Id, ResolutionFrameKind.DiscardPhaseEntry, ResolutionFrameStep.ResolvingEffect);

[JsonDerivedType(typeof(NativeDiscardEntryFrame), "native-discard-entry")]
public abstract partial record ResolutionFrame;

public sealed record NativeDiscardEntryPolicyChosenEvent(long FrameId, CardConversionSource Source,
    string GameplayHash, int TurnNumber, int TurnOwnerSeat, TurnPhase BeforePhase, int PlayPhaseInstanceId, bool Applied) : IGameEvent;
