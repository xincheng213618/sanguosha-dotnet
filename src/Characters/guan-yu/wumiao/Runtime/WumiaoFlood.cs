using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum WumiaoFloodStage { AwaitingDamage, AfterDamage, ChoosingDiscard, DiscardChildren, Drawing, DrawingChildren, FinishingAttack }
public sealed record WumiaoFloodGrainMaterial(int CardId, CardKind PrintedKind, CardLocation From);
public sealed record WumiaoFloodDiscardMovement(int CardId, CardKind PrintedKind, CardLocation From,
    CardLocation To, CardMoveReason Reason, int MovementSequence);
public sealed record WumiaoFloodDiscardBatch(long BatchId, long? ParentBatchId, int MovementCount);

// The anniversary card is distinct from the national-war card with the same name.
public sealed record WumiaoFloodReceipt(long ActionId, long NullificationFrameId, int TargetSeat,
    int TargetIndex, int ExtraEffectOrdinal, WumiaoFloodStage Stage)
{
    public long? AwaitingProgramFrameId { get; init; }
    public int? DiscardCardId { get; init; }
    public CardLocation? DiscardFrom { get; init; }
    public CardLocation? DiscardTo { get; init; }
    public CardKind? DiscardPrintedKind { get; init; }
    public int DiscardSequence { get; init; }
    public int DiscardSequenceBefore { get; init; }
    public int DiscardSequenceAfter { get; init; }
    public bool DiscardInvoiceSealed { get; init; }
    public bool DiscardNativeDestruction { get; init; }
    private readonly IReadOnlyList<WumiaoFloodGrainMaterial> _grainMaterials = Array.AsReadOnly(Array.Empty<WumiaoFloodGrainMaterial>());
    public IReadOnlyList<WumiaoFloodGrainMaterial> GrainMaterials
    { get => _grainMaterials; init => _grainMaterials = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<WumiaoFloodDiscardMovement> _discardMovements = Array.AsReadOnly(Array.Empty<WumiaoFloodDiscardMovement>());
    public IReadOnlyList<WumiaoFloodDiscardMovement> DiscardMovements
    { get => _discardMovements; init => _discardMovements = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<WumiaoFloodDiscardBatch> _discardBatches = Array.AsReadOnly(Array.Empty<WumiaoFloodDiscardBatch>());
    public IReadOnlyList<WumiaoFloodDiscardBatch> DiscardBatches
    { get => _discardBatches; init => _discardBatches = Array.AsReadOnly(value.ToArray()); }
    public NativeDrawOutcome? DrawOutcome { get; init; }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WumiaoFloodReceipt? WumiaoFlood { get; init; }
}

public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeWumiaoFloodFrameId { get; init; }
}

public sealed record WumiaoFloodEffectStartedEvent(long UseId, long ActionId, long NullificationId,
    int TargetSeat, int TargetIndex, int ExtraEffectOrdinal) : IGameEvent;
public sealed record WumiaoFloodDamageReturnedEvent(long UseId, long NullificationId, int TargetSeat,
    int TargetIndex) : IGameEvent;
public sealed record WumiaoFloodDiscardPaidEvent(long UseId, long NullificationId, int TargetSeat,
    int CardId, CardLocation From, int MovementSequence) : IGameEvent;
public sealed record WumiaoFloodDiscardInvoiceSealedEvent(long UseId, long NullificationId, int PrimarySequence,
    int SequenceBefore, int SequenceAfter, int GrainCount, int MovementCount, int BatchCount) : IGameEvent;
public sealed record WumiaoFloodDiscardMaterialPaidEvent(long UseId, long NullificationId, int Index,
    int CardId, CardKind PrintedKind, CardLocation From, CardLocation To, CardMoveReason Reason, int MovementSequence) : IGameEvent;
public sealed record WumiaoFloodDiscardBatchIssuedEvent(long UseId, long NullificationId, long BatchId,
    long? ParentBatchId, int MovementCount) : IGameEvent;
public sealed record WumiaoFloodDrawReturnedEvent(long UseId, long NullificationId, int RecipientSeat,
    int ActualCount, long SequenceBefore, long SequenceAfter) : IGameEvent;
