using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ResponseRule
{
    public int Minimum { get; }
    public bool Forbid { get; }
    public int? TargetSeat { get; }
    public int? ActorSeat { get; }
    public int? OpponentSeat { get; }
    public IReadOnlyList<CardKind> IncomingKinds { get; }
    public IReadOnlyList<CardKind> RequiredKinds { get; }
    public ProgramExpression? While { get; }
    public bool MatchUseKind { get; }
    public string? SkillName { get; }
    public int? InstructionIndex { get; internal init; }
    public ResponseRule(int minimum = 1, bool forbid = false, int? targetSeat = null, int? actorSeat = null,
        int? opponentSeat = null, IReadOnlyList<CardKind>? incomingKinds = null, IReadOnlyList<CardKind>? requiredKinds = null,
        ProgramExpression? condition = null, bool matchUseKind = false, string? skillName = null)
    {
        if (minimum < 1 || condition is not null && (condition.Type != ProgramExpressionType.Bool ||
            condition.Nodes().Any(node => node.Op is not ("bool" or "int" or "eventSource" or "eventTarget" or
                "hp" or "maxHp" or "lostHp" or "handCount" or "equipmentCount" or "handPrintedSuitCount" or
                "alive" or "hasHp" or "faceDown" or "chained" or "not" or "and" or "or" or "<" or "<=" or ">" or ">=" or "==" or "!=" or
                "+" or "-" or "min" or "max" or "floorDiv" or "when"))))
            throw new InvalidOperationException("A response rule requires a positive minimum and public incoming-actor/responder conditions.");
        (Minimum, Forbid, TargetSeat, ActorSeat, OpponentSeat, IncomingKinds, RequiredKinds, While, MatchUseKind, SkillName) =
            (minimum, forbid, targetSeat, actorSeat, opponentSeat, Array.AsReadOnly((incomingKinds ?? []).ToArray()),
             Array.AsReadOnly((requiredKinds ?? []).ToArray()), condition, matchUseKind, skillName);
    }
}

public abstract partial record ResolutionFrame
{
    private readonly IReadOnlyList<ResponseRule>? _responseRules;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ResponseRule>? ResponseRules
    {
        get => _responseRules;
        init => _responseRules = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

internal readonly record struct NativeResponseQuery(long IncomingFrameId, int? InstructionIndex, int ActorSeat,
    int OpponentSeat, int ResponderSeat, int ProviderSeat, CardKind IncomingKind, CardKind RequiredKind,
    IReadOnlyList<int> TargetSeats, int CounterspellDepth = 0);
internal sealed record ResponseDecision(int Minimum, bool Forbidden, IReadOnlyList<string> SkillNames)
{
    internal static ResponseDecision Default { get; } = new(1, false, []);
}

internal sealed class ResponseRuleExpressionWorld(NativeResponseQuery query, IReadOnlyList<CharacterState> players,
    CardZoneStore cards, RuntimeEntries.CoreGameEngineHealthQueriesEntries health) : IProgramExpressionWorld
{
    private CharacterState? Player(int seat) => seat >= 0 && seat < players.Count ? players[seat] : null;
    public int? Participant(string name) => name switch { "eventSource" => query.ActorSeat, "eventTarget" => query.ResponderSeat, _ => null };
    public IReadOnlyList<int>? ParticipantSet(string name) => null;
    public int? HandPrintedSuitCount(int seat, Suit suit) => Player(seat) is null ? null : cards.CardsAt(CardLocation.Hand(seat)).Count(card => card.Suit == suit);
    public int? Stat(string stat, int seat) => Player(seat) is not { } player ? null : stat switch
    {
        "hp" => health.UnambiguousHp(player), "maxHp" => player.MaxHp,
        "lostHp" => health.UnambiguousHp(player) is { } hp ? player.MaxHp - hp : null,
        "handCount" => cards.Count(CardLocation.Hand(seat)), "equipmentCount" => cards.Count(CardLocation.Equipment(seat)), _ => null
    };
    public bool? Flag(string flag, int seat) => Player(seat) is not { } player ? null : flag switch
    { "alive" => player.IsAlive, "hasHp" => player.HasHpValue, "faceDown" => player.IsFaceDown, "chained" => player.IsChained, _ => null };
}
