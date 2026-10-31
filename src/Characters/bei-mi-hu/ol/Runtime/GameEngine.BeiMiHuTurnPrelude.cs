using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BeiMiHuTurnPreludeFrame(
    long Id, BeiMiHuBeforeTurnProof Proof, bool RoundAdvanced, int Stage = 0,
    ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] long? WindowId = null)
    : ResolutionFrame(Id, ResolutionFrameKind.BeiMiHuTurnPrelude, Step);

public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BeiMiHuBeforeTurnProof? BeforeTurnPreludeProof { get; init; }
}

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BeiMiHuBeforeTurnProof? BeforeTurnPreludeProof { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QingzhongPlayPhaseKey? QingzhongEndingPhase { get; init; }
}

public sealed partial record PlayPhaseStartingBoundaryFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QingzhongPlayPhaseKey? QingzhongStartingPhase { get; init; }
}
