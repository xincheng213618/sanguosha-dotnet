using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;
public sealed record TousuiRequestReceipt(long OwnerFrameId, int InstructionIndex, SkillProgramEffectOp? RequestOp, int ActorSeat, int TargetSeat, CardConversionSource Source, string GameplayHash, PendingDecision OriginalDecision)
{
    private PendingDecision _original = RequestedDeckBasicFrame.FreezeDecision(OriginalDecision);
    public PendingDecision OriginalDecision { get => _original; init => _original = RequestedDeckBasicFrame.FreezeDecision(value); }

    private IReadOnlyList<int> _selected = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CardConversionSource? RequestSource { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RequestGameplayHash { get; init; }
}

public sealed record TousuiRequestStartedEvent(long OwnerFrameId, int InstructionIndex, SkillProgramEffectOp? RequestOp, int ActorSeat, int TargetSeat, CardConversionSource Source, string GameplayHash, PromptId OriginalPromptId, long OriginalRevision, string OriginalDecisionHash) : IGameEvent;
public sealed record TousuiRequestMaterialSelectedEvent(long OwnerFrameId, PromptId OriginalPromptId, int ActorSeat, int Index, int CardId, CardLocation From) : IGameEvent;
public sealed record TousuiRequestFinishedEvent(long OwnerFrameId, int ActorSeat, bool Accepted, int MaterialCount) : IGameEvent;
public sealed record TousuiKeActionAcceptedEvent(long ProgramFrameId, int InstructionIndex, CardConversionSource RequestSource, string RequestGameplayHash, int ActorSeat, int TargetSeat, CardConversionSource Source, string GameplayHash, PromptId OriginalPromptId, int MaterialCount) : IGameEvent;
