using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MouBingcaiExtraEffectGrant(long FrameId, CardConversionSource Source,
    string GameplayHash, long WindowId, CardKind BasicKind, long RecastInvocationId,
    int MaterialId, CardKind PrintedKind, CardKind EffectiveKind, CardLocation From,
    long BatchId, long Before, long After, long MovementSequence);

public enum MouBingcaiLegacyAlcoholStage { Targets, Committed, Recovery }
public sealed record MouBingcaiLegacyAlcoholReturn(long UseId, long ProgramParentId, long DyingId,
    int InstructionIndex, CardConversionSource ProducerSource, string ProducerHash,
    long ActionId, MouBingcaiLegacyAlcoholStage Stage);

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MouBingcaiLegacyAlcoholReturn? MouBingcaiLegacyAlcohol { get; init; }
}

// Paid recast business facts remain scalar; native execution belongs to shared episodes.
public sealed record MouBingcaiExtraEffectIssuedEvent(long FrameId, long UseId, long ActionId,
    CardConversionSource Source, string GameplayHash, long WindowId, CardKind BasicKind,
    long RecastInvocationId, int MaterialId, CardKind PrintedKind, CardKind EffectiveKind,
    CardLocation From, long BatchId, long Before, long After, long MovementSequence) : IGameEvent;
public sealed record MouBingcaiLegacyAlcoholIssuedEvent(long UseId, long ProgramParentId,
    long DyingId, int InstructionIndex, CardConversionSource ProducerSource,
    string ProducerHash, long ActionId) : IGameEvent;
public sealed record MouBingcaiLegacyAlcoholStageEvent(long UseId, long ActionId, MouBingcaiLegacyAlcoholStage Stage) : IGameEvent;
