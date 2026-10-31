using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WushuangOriginalSkillReceipt(SkillProgramEffectOp Operation, int InstructionIndex,
    int ActualTurn, long? WindowId, string Stage, int Target = -1, int Card = 0, int Index = 0,
    int Count = 0, int Mask = 0, int Correct = -1, int DrawOrdinal = 0, int DrawRecipient = -1,
    int DrawRequested = 0, string? NextStage = null, NativeDrawOutcome? Draw = null)
{
    private readonly IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<int> _top = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Top { get => _top; init => _top = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangOriginalSkillReceipt? WushuangOriginalSkill { get; init; }
}
public sealed record WushuangOriginalSkillIssuedEvent(long FrameId, string SkillId, int OwnerSeat,
    SkillProgramEffectOp Operation, int InstructionIndex, int ActualTurn, long? WindowId) : IGameEvent;
public sealed record WushuangOriginalSkillFinishedEvent(long FrameId, bool Completed) : IGameEvent;
public sealed record WushuangOriginalSkillDrawEvent(long FrameId, int Ordinal, int Recipient, int Requested,
    int Actual, long Before, long After, long? Attempt, bool Replaced) : IGameEvent;
public sealed record WushuangTairanHealedEvent(long FrameId, int Owner, string Instance, int ActualTurn, int Recovered) : IGameEvent;
public sealed record WushuangTairanDrawnEvent(long FrameId, int Owner, string Instance, int ActualTurn, IReadOnlyList<int> CardIds) : IGameEvent
{
    private readonly IReadOnlyList<int> _ids = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds { get => _ids; init => _ids = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WushuangTairanSettledEvent(long ProducerId, int Owner, long SettlingFrame) : IGameEvent;
public sealed record WushuangYashiSuppressedEvent(long FrameId, int Owner, int Target, int ActualTurn) : IGameEvent;
public sealed record WushuangYashiExpiredEvent(long ProducerId, int Target) : IGameEvent;
public sealed record WushuangLingrenGuessedEvent(long FrameId, int Owner, int Target, int ActualTurn,
    long UseId, long ActionId, int Correct) : IGameEvent;
public sealed record WushuangChoufaRewrittenEvent(long FrameId, int Owner, int Target, int ActualTurn,
    int MovementSequence, CardUseEffectSource Source, IReadOnlyList<int> CardIds) : IGameEvent
{
    private readonly IReadOnlyList<int> _ids = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds { get => _ids; init => _ids = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WushuangChoufaExpiredEvent(long ProducerId, int Target) : IGameEvent;
public sealed record WushuangCiweiNullifiedEvent(long FrameId, long UseId, long WindowId, int Owner,
    long ActionId, int Actor, CardKind Kind, string SkillId, string SkillInstanceId, string GameplayHash,
    int InstructionIndex, int PaidCard, int PaidMovement) : IGameEvent;
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangCiweiNullifiedEvent? WushuangCiweiNullification { get; init; }
}

public sealed partial class GameEngine
{
}
