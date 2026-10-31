namespace CardGame.Core;

/// <summary>The exact startup program and typed draw request that issued a turn-zero draw.</summary>
public sealed record GameStartingDrawOrigin(long WindowFrameId, long ProgramFrameId, int InstructionIndex,
    string GameplayHash, CardConversionSource Source, NativeDrawAttemptReturnKind ReturnKind, SkillProgramEffectOp? Operation)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public GameStartingOutsideDrawOrigin? OutsideDrawChild { get; init; }
}
