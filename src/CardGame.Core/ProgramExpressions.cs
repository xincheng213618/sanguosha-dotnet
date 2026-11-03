using System.Text.Json;

namespace CardGame.Core;

public enum ProgramExpressionType { Bool, Int, Participant, ParticipantSet, Fact, FactSet, Card, CardSet }

/// <summary>
/// Immutable typed expression read by <c>{"kind":"expression","expr":[...]}</c>
/// conditions. Evaluation is pure: it reads current public seat state through
/// <see cref="IProgramExpressionWorld"/> and never writes game state.
/// </summary>
public sealed class ProgramExpression
{
    internal ProgramExpression(string op, ProgramExpressionType type, IReadOnlyList<ProgramExpression> args,
        int literal = 0, string? name = null)
    {
        Op = op; Type = type; Args = Array.AsReadOnly(args.ToArray()); Literal = literal; Name = name;
    }

    public string Op { get; }
    public ProgramExpressionType Type { get; }
    public IReadOnlyList<ProgramExpression> Args { get; }
    public int Literal { get; }
    public string? Name { get; }

    internal bool Reads(string op) => Op == op || Args.Any(arg => arg.Reads(op));
    internal IEnumerable<ProgramExpression> Nodes() => new[] { this }.Concat(Args.SelectMany(arg => arg.Nodes()));
    internal bool StructurallyEquals(ProgramExpression other) => Op == other.Op && Type == other.Type &&
        Literal == other.Literal && Name == other.Name && Args.Count == other.Args.Count &&
        Args.Zip(other.Args).All(pair => pair.First.StructurallyEquals(pair.Second));
}

/// <summary>Read-only seat facts. Null means unknown or undefined.</summary>
internal interface IProgramExpressionWorld
{
    int? Participant(string name);
    IReadOnlyList<int>? ParticipantSet(string name);
    IReadOnlyList<int>? UseResultTargets(string bind) => null;
    int? Stat(string stat, int seat);
    bool? Flag(string flag, int seat);
    int? BindingStat(string stat, string name) => null;
    int? Number(string name) => null;
    int? CardMarkCount(int seat, string mark, CardZoneKind zone) => null;
    int? HandPrintedSuitCount(int seat, Suit suit) => null;
    int? PublicPileCount(int seat, string skillId) => null;
    int? WindowStat(string stat, string participant) => null;
    IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view) => null;
    CardUseHistoryFact? CurrentUse() => null;
    HandGainOccurrence? HandGain(int recipient) => null;
    bool? GainHasSource(int recipient, string moment) => null;
    bool? CardAt(int cardId, int seat, CardZoneKind zone) => null;
    bool? CardCanDiscard(int seat, int cardId) => null;
    bool? CardInBinding(int cardId, string name) => null;
    IReadOnlyList<ProgramExpressionCardFact>? PublicRevealCards(string scope, string purpose) => null;
    IReadOnlyList<ProgramExpressionCardFact>? BoundCards(string name) => null;
    bool? CardStillAtSource(ProgramExpressionCardFact card) => null;
    bool? SameTeam(int first, int second) => null;
    bool? IsFaction(int seat, string factionId) => null;
    int? CombatDistance(int source, int target) => null;
    int? WindowAmount() => null;
    IReadOnlyList<ProgramExpressionCardFact>? GivenCards(int giver, int recipient) => null;
    int? PhaseCount(string moment, int actor, ActualDiscardRecoveryPhaseKind? kind, ActualPhaseHistoryOutcome outcome) => null;
}

internal static class ProgramExpressions
{
    internal static int? ReadWindowStat(SkillProgramTriggerFacts? facts, string stat, string participant, bool hasTarget)
    {
        if (facts is null) return null;
        return (stat, participant) switch
        {
            ("windowHp", "owner") when !facts.OwnerHpUndefined && facts.UnselectedOwnerHp is null => facts.CurrentHp,
            ("windowHp", "eventTarget") when hasTarget && !facts.EventTargetHpUndefined && facts.UnselectedEventTargetHp is null => facts.EventTargetHp,
            ("windowHandCount", "owner") => facts.CurrentHandCount,
            ("windowHandCount", "eventTarget") when hasTarget => facts.EventTargetHandCount,
            _ => null
        };
    }

    public const int MaximumDepth = 12;
    public const string Iterator = "it";
    public const string FactIterator = "use";
    public const string CardIterator = "card";
    public static readonly string[] Participants = ["turnOwner", "owner", "selectedTarget", "selectedFirst", "selectedSecond", "eventSource", "eventTarget"];
    public static readonly string[] Sets = ["living", "livingOthers"];
    private static readonly string[] Stats = ["hp", "maxHp", "lostHp", "handCount", "handLimit", "equipmentCount", "equipmentSuitCount"];
    private static readonly string[] Flags = ["alive", "hasHp", "faceDown", "chained", "ruleWounded", "inIssuedPhase", "inUpcomingTurnPrelude", "isTurnOwner", "inIssuedTurn", "inIssuedDying"];
    private static readonly string[] Comparisons = ["<", "<=", ">", ">=", "==", "!="];

    public static ProgramExpression Parse(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Bool, 0, iteratorInScope: false);

    public static ProgramExpression ParseInt(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Int, 0, iteratorInScope: false);

    public static ProgramExpression ParseParticipantSet(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.ParticipantSet, 0, iteratorInScope: false);

    public static ProgramExpression ParseCardSet(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.CardSet, 0, iteratorInScope: false);

    private static ProgramExpression Parse(JsonElement node, string path, ProgramExpressionType? expected, int depth,
        bool iteratorInScope, bool factInScope = false, IReadOnlySet<string>? cardNames = null)
    {
        if (depth > MaximumDepth) Fail(path, $"expression nesting exceeds {MaximumDepth}");
        var result = node.ValueKind switch
        {
            JsonValueKind.True or JsonValueKind.False =>
                new ProgramExpression("bool", ProgramExpressionType.Bool, [], node.GetBoolean() ? 1 : 0),
            JsonValueKind.Number when node.TryGetInt32(out var value) && value is >= -99 and <= 99 =>
                new ProgramExpression("int", ProgramExpressionType.Int, [], value),
            JsonValueKind.String => ParseName(node.GetString()!, path, iteratorInScope, factInScope, cardNames),
            JsonValueKind.Array => ParseCall(node, path, depth, iteratorInScope, factInScope, cardNames),
            _ => throw Failure(path, "must be a boolean, an integer from -99 to 99, a name or an [operator, ...] array")
        };
        if (expected is { } expectedType && result.Type != expectedType) Fail(path, $"expected {Camel(expectedType)} but found {Camel(result.Type)}");
        return result;
    }

    private static ProgramExpression ParseName(string name, string path, bool iteratorInScope, bool factInScope,
        IReadOnlySet<string>? cardNames)
    {
        if (name == "target") name = "selectedTarget";
        if (name == Iterator)
        {
            if (!iteratorInScope) Fail(path, "'it' is only available inside a participant-set filter, all, any or count");
            return new(name, ProgramExpressionType.Participant, [], name: name);
        }
        if (name == FactIterator && !factInScope) Fail(path, "'use' is only available inside a fact-set filter, all, any or count");
        if (name is FactIterator or "currentUse") return new(name, ProgramExpressionType.Fact, [], name: name);
        if (Participants.Contains(name)) return new(name, ProgramExpressionType.Participant, [], name: name);
        if (Sets.Contains(name)) return new(name, ProgramExpressionType.ParticipantSet, [], name: name);
        if (cardNames?.Contains(name) == true) return new("card", ProgramExpressionType.Card, [], name: name);
        if (name == CardIterator) Fail(path, "'card' is only available inside a card-set filter, all, any or count");
        throw Failure(path, $"unknown name '{name}'");
    }

    private static ProgramExpression ParseCall(JsonElement node, string path, int depth, bool iteratorInScope, bool factInScope,
        IReadOnlySet<string>? cardNames)
    {
        var items = node.EnumerateArray().ToArray();
        if (items.Length == 0 || items[0].ValueKind != JsonValueKind.String) Fail(path, "an array must start with an operator name");
        var op = items[0].GetString()!;
        var args = items.Skip(1).ToArray();
        ProgramExpression Arg(int index, ProgramExpressionType? type, bool iterator = false, bool factIterator = false) =>
            Parse(args[index], $"{path}[{index + 1}]", type, depth + 1, iteratorInScope || iterator, factInScope || factIterator, cardNames);
        void Arity(int minimum, int maximum)
        {
            if (args.Length < minimum || args.Length > maximum)
                Fail(path, minimum == maximum ? $"'{op}' requires {minimum} argument(s)" : $"'{op}' requires {minimum} to {maximum} arguments");
        }
        ProgramExpression[] Each(ProgramExpressionType type) => args.Select((_, index) => Arg(index, type)).ToArray();

        if (op is "and" or "or") { Arity(2, 8); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Bool)); }
        if (op == "not") { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Bool)); }
        if (Comparisons.Contains(op)) { Arity(2, 2); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Int)); }
        if (op == "same") { Arity(2, 2); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (op == "sameTeam") { Arity(2, 2); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (op == "distance") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Participant)); }
        if (op == "isFaction")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128)
                Fail(path, "isFaction requires a faction identifier");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Participant)], name: args[1].GetString());
        }
        if (Flags.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (Stats.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Participant)); }
        if (op == "publicRevealCards")
        {
            Arity(1, 2);
            var scope = args[0].ValueKind == JsonValueKind.String ? args[0].GetString() : null;
            var purpose = args.Length == 1 ? "all" : args[1].ValueKind == JsonValueKind.String ? args[1].GetString() : null;
            if (scope is not ("all" or "actualTurn" or "actualPhase")) Fail(path, "publicRevealCards requires all, actualTurn or actualPhase");
            if (purpose is not ("all" or "explicit")) Fail(path, "publicRevealCards purpose requires all or explicit");
            return new(op, ProgramExpressionType.CardSet, [], name: scope, literal: purpose == "explicit" ? 1 : 0);
        }
        if (op == "useResultTargets")
        {
            Arity(1, 1);
            if (args[0].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[0].GetString()) || args[0].GetString()!.Length > 128)
                Fail(path, "useResultTargets requires a binding identifier");
            return new(op, ProgramExpressionType.ParticipantSet, [], name: args[0].GetString());
        }
        if (op == "windowAmount") { Arity(0, 0); return new(op, ProgramExpressionType.Int, []); }
        if (op == "givenCards") { Arity(2, 2); return new(op, ProgramExpressionType.CardSet, Each(ProgramExpressionType.Participant)); }
        if (op == "boundCards")
        {
            Arity(1, 1);
            if (args[0].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[0].GetString()) || args[0].GetString()!.Length > 128)
                Fail(path, "boundCards requires a binding identifier");
            return new(op, ProgramExpressionType.CardSet, [], name: args[0].GetString());
        }
        if (op == "cardStillAtSource") { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Card)); }
        if (op == "gainCards")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("incoming" or "previousHand"))
                Fail(path, "gainCards requires incoming or previousHand");
            return new(op, ProgramExpressionType.CardSet, [Arg(0, ProgramExpressionType.Participant)], name: args[1].GetString());
        }
        if (op is "gainInitiallyAlive" or "gainFirstOutsideDraw" or "gainRecipientAlive" or "gainActualTurn")
        {
            Arity(1, 1);
            return new(op, op == "gainActualTurn" ? ProgramExpressionType.Int : ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant));
        }
        if (op == "gainHasSource")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("initial" or "completion"))
                Fail(path, "gainHasSource requires initial or completion");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Participant)], name: args[1].GetString());
        }
        if (op == "cardAt")
        {
            Arity(3, 3);
            var zone = ParseFactEnum<CardZoneKind>(args[2], path + "[3]");
            if (zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) Fail(path, "cardAt requires hand or equipment");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Card), Arg(1, ProgramExpressionType.Participant)],
                literal: (int)zone);
        }
        if (op == "cardCanDiscard")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Participant), Arg(1, ProgramExpressionType.Card)]);
        }
        if (op == "cardInBinding")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128)
                Fail(path, "cardInBinding requires a binding identifier");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Card)], name: args[1].GetString());
        }
        if (op == "gainPhaseIs")
        {
            Arity(3, 3);
            return new(op, ProgramExpressionType.Bool,
                [Arg(0, ProgramExpressionType.Participant), Arg(2, ProgramExpressionType.Participant)],
                literal: (int)ParseFactEnum<TurnPhase>(args[1], path + "[2]"));
        }
        if (op == "cardActualGain")
        {
            Arity(1, 1);
            return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Card));
        }
        if (op is "sameCardKind" or "sameCardSuit" or "sameCardName")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Card));
        }
        if (op is "cardKindIs" or "cardSuitIs")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.Array) Fail(path, op + " requires an array of card kinds or suits");
            var values = args[1].EnumerateArray().Select((value, index) => op == "cardKindIs"
                ? (int)ParseFactEnum<CardKind>(value, $"{path}[2][{index}]")
                : (int)ParseFactEnum<Suit>(value, $"{path}[2][{index}]")).ToArray();
            if (values.Distinct().Count() != values.Length) Fail(path, op + " contains duplicate values");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Card),
                .. values.Select(value => new ProgramExpression("int", ProgramExpressionType.Int, [], literal: value))]);
        }
        if (op is "windowHp" or "windowHandCount")
        {
            Arity(1, 1);
            var subject = Arg(0, ProgramExpressionType.Participant);
            if (subject.Name is not ("owner" or "eventTarget"))
                Fail(path, "window stats require the frozen owner or eventTarget");
            return new(op, ProgramExpressionType.Int, [subject]);
        }
        if (op == "publicPileCount")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128)
                Fail(path, "publicPileCount requires a skill identifier");
            try { _ = new ContentId(args[1].GetString()!); }
            catch (ArgumentException) { throw Failure(path, "publicPileCount requires a skill identifier"); }
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)], name: args[1].GetString());
        }
        if (op == "cardMarkCount")
        {
            Arity(3, 3);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128 ||
                args[2].ValueKind != JsonValueKind.String || args[2].GetString() is not ("hand" or "equipment"))
                Fail(path, "cardMarkCount requires a mark identifier and hand or equipment");
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)],
                literal: (int)(args[2].GetString() == "hand" ? CardZoneKind.Hand : CardZoneKind.Equipment), name: args[1].GetString());
        }
        if (op == "handPrintedSuitCount")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("none" or "spade" or "club" or "heart" or "diamond"))
                Fail(path, "handPrintedSuitCount requires a printed suit");
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)],
                literal: (int)Enum.Parse<Suit>(args[1].GetString()!, ignoreCase: true));
        }
        if (op == "mod") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op is "+" or "min" or "max") { Arity(2, 8); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "-") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "floorDiv") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "when")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Bool), Arg(1, ProgramExpressionType.Int)]);
        }
        if (op is "number" or "choiceNumber" or "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank" or "boundIsEquipment")
        {
            Arity(1, 1);
            if (args[0].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[0].GetString()) || args[0].GetString()!.Length > 128)
                Fail(path, "a binding reference requires a nonempty identifier of at most 128 characters");
            return new(op, op == "boundIsEquipment" ? ProgramExpressionType.Bool : ProgramExpressionType.Int, [], name: args[0].GetString());
        }
        if (op == "phaseCount")
        {
            Arity(4, 4);
            if (args[0].ValueKind != JsonValueKind.String || args[0].GetString() is not ("window" or "now"))
                Fail(path, "phaseCount requires window or now");
            var phase = args[2].ValueKind == JsonValueKind.String && args[2].GetString() == "all"
                ? -1 : (int)ParseFactEnum<ActualDiscardRecoveryPhaseKind>(args[2], path + "[3]");
            var outcome = ParseFactEnum<ActualPhaseHistoryOutcome>(args[3], path + "[4]");
            return new(op, ProgramExpressionType.Int,
                [Arg(1, ProgramExpressionType.Participant), new("int", ProgramExpressionType.Int, [], literal: (int)outcome)],
                literal: phase, name: args[0].GetString());
        }
        if (op == "cardUses")
        {
            Arity(2, 2);
            if (args[0].ValueKind != JsonValueKind.String || args[0].GetString() is not ("window" or "now" or "turnEnd") ||
                args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("latest" or "observations" or "confirmations" or "accepted" or "finalizations" or "actualUses"))
                Fail(path, "cardUses requires window, now or turnEnd, and latest, observations, confirmations, accepted, finalizations or actualUses");
            return new(op, ProgramExpressionType.FactSet, [], literal: args[0].GetString() switch
                { "now" => 1, "turnEnd" => 2, _ => 0 }, name: args[1].GetString());
        }
        if (op is "useActor" or "useDeclarer" or "useTargets" or "useConfirmedTargets" or "usePrimaryTargets" or "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse" or "useFinalized" or "useCompleted")
        {
            Arity(1, 1);
            var type = op is "useTargets" or "useConfirmedTargets" or "usePrimaryTargets" ? ProgramExpressionType.ParticipantSet :
                op is "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse" or "useFinalized" or "useCompleted" ? ProgramExpressionType.Bool : ProgramExpressionType.Participant;
            return new(op, type, [Arg(0, ProgramExpressionType.Fact)]);
        }
        if (op is "first" or "last")
        {
            Arity(1, 1);
            return new(op, ProgramExpressionType.Fact, [Arg(0, ProgramExpressionType.FactSet)]);
        }
        if (op is "takeFirst" or "takeLast")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.FactSet,
                [Arg(0, ProgramExpressionType.FactSet), Arg(1, ProgramExpressionType.Int)]);
        }
        if (op == "intersect")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.ParticipantSet, Each(ProgramExpressionType.ParticipantSet));
        }
        if (op == "samePlayPhase")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Fact));
        }
        if (op == "sameUse")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Fact));
        }
        if (op == "useKindIs")
        {
            Arity(2, 3);
            if (args.Length == 3 && (args[2].ValueKind != JsonValueKind.String || args[2].GetString() != "effective"))
                Fail(path, "useKindIs stage must be effective");
            if (args[1].ValueKind != JsonValueKind.Array) Fail(path, "useKindIs requires an array of card kinds");
            var kinds = args[1].EnumerateArray().Select((kind, index) =>
                ParseFactEnum<CardKind>(kind, $"{path}[2][{index}]")).ToArray();
            if (kinds.Distinct().Count() != kinds.Length) Fail(path, "useKindIs contains duplicate card kinds");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact),
                .. kinds.Select(kind => new ProgramExpression("int", ProgramExpressionType.Int, [], literal: (int)kind))],
                name: args.Length == 3 ? "effective" : null);
        }
        if (op == "usePhaseIs")
        {
            Arity(2, 3);
            if (args.Length == 3 && (args[2].ValueKind != JsonValueKind.String || args[2].GetString() != "accepted"))
                Fail(path, "usePhaseIs stage must be accepted");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact)],
                literal: (int)ParseFactEnum<TurnPhase>(args[1], path + "[2]"), name: args.Length == 3 ? "accepted" : null);
        }
        if (op == "atOrBefore")
        {
            Arity(3, 3);
            if (args[2].ValueKind != JsonValueKind.String || args[2].GetString() != "accepted")
                Fail(path, "atOrBefore requires the accepted order");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact), Arg(1, ProgramExpressionType.Fact)], name: "accepted");
        }
        if (op == "targets")
        {
            Arity(1, 1);
            return new(op, ProgramExpressionType.ParticipantSet, [Arg(0, ProgramExpressionType.FactSet)]);
        }
        if (op == "contains")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.ParticipantSet), Arg(1, ProgramExpressionType.Participant)]);
        }
        if (op is "all" or "any" or "count" or "filter")
        {
            Arity(2, 3);
            var source = Arg(0, null);
            if (source.Type is not (ProgramExpressionType.ParticipantSet or ProgramExpressionType.FactSet or ProgramExpressionType.CardSet))
                Fail(path, "all, any, count and filter require a participant set, a fact set or a card set");
            if (source.Type == ProgramExpressionType.CardSet)
            {
                var name = CardIterator;
                if (args.Length == 3)
                {
                    if (args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not { } identifier ||
                        identifier.Length is 0 or > 128 || !(char.IsLetter(identifier[0]) || identifier[0] == '_') ||
                        identifier.Any(character => !(char.IsLetterOrDigit(character) || character is '_' or '-')))
                        Fail(path, "a card iterator requires an identifier of at most 128 characters");
                    name = args[1].GetString()!;
                    if (name is Iterator or FactIterator or CardIterator or "target" or "currentUse" ||
                        Participants.Contains(name) || Sets.Contains(name))
                        Fail(path, $"card iterator '{name}' is reserved");
                }
                if (cardNames?.Contains(name) == true) Fail(path, $"card iterator '{name}' is already in scope");
                var scope = cardNames is null ? new HashSet<string>(StringComparer.Ordinal) : new HashSet<string>(cardNames, StringComparer.Ordinal);
                scope.Add(name);
                return new(op, op == "filter" ? source.Type : op == "count" ? ProgramExpressionType.Int : ProgramExpressionType.Bool,
                    [source, Parse(args[^1], $"{path}[{args.Length}]", ProgramExpressionType.Bool, depth + 1,
                        iteratorInScope, factInScope, scope)], name: name);
            }
            if (args.Length != 2) Fail(path, "named iterators require a card set");
            return new(op, op == "filter" ? source.Type : op == "count" ? ProgramExpressionType.Int : ProgramExpressionType.Bool,
                [source, Arg(1, ProgramExpressionType.Bool, iterator: source.Type == ProgramExpressionType.ParticipantSet,
                    factIterator: source.Type == ProgramExpressionType.FactSet)]);
        }
        throw Failure(path, $"unknown operator '{op}'");
    }

    /// <summary>Three-valued result: null when a read fact is unknown or undefined.</summary>
    public static bool? EvaluateBool(ProgramExpression e, IProgramExpressionWorld world, int? it = null, CardUseHistoryFact? use = null,
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards = null)
    {
        if (e.Op == "boundIsEquipment") return world.BindingStat(e.Op, e.Name!) is { } value ? value != 0 : null;
        switch (e.Op)
        {
            case "bool": return e.Literal != 0;
            case "gainInitiallyAlive" or "gainFirstOutsideDraw" or "gainRecipientAlive":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } recipient &&
                    world.HandGain(recipient) is { } gain
                    ? e.Op == "gainInitiallyAlive" ? gain.InitiallyAlive : e.Op == "gainRecipientAlive" ? gain.RecipientAlive : gain.FirstOutsideDraw : null;
            case "gainHasSource":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } sourceRecipient
                    ? world.GainHasSource(sourceRecipient, e.Name!) : null;
            case "cardAt":
                return EvaluateCard(e.Args[0], cards) is { } located && EvaluateParticipant(e.Args[1], world, it, use, cards) is { } holder
                    ? world.CardAt(located.CardId, holder, (CardZoneKind)e.Literal) : null;
            case "cardCanDiscard":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } discarder && EvaluateCard(e.Args[1], cards) is { } discardCard
                    ? world.CardCanDiscard(discarder, discardCard.CardId) : null;
            case "cardInBinding":
                return EvaluateCard(e.Args[0], cards) is { } boundCard ? world.CardInBinding(boundCard.CardId, e.Name!) : null;
            case "cardStillAtSource":
                return EvaluateCard(e.Args[0], cards) is { } capturedCard ? world.CardStillAtSource(capturedCard) : null;
            case "gainPhaseIs":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } gainRecipient &&
                    world.HandGain(gainRecipient)?.Timing is { } timing &&
                    EvaluateParticipant(e.Args[1], world, it, use, cards) is { } phaseActor
                    ? (int)timing.Phase == e.Literal && timing.PhaseActorSeat == phaseActor : null;
            case "cardActualGain": return EvaluateCard(e.Args[0], cards)?.IsActualGain;
            case "cardKindIs":
                return EvaluateCard(e.Args[0], cards)?.EffectiveKind is { } knownKind
                    ? ProgramCardKindMatch.Contains(e.Args.Skip(1).Select(arg => (CardKind)arg.Literal).ToArray(), knownKind) : null;
            case "cardSuitIs":
                return EvaluateCard(e.Args[0], cards)?.EffectiveSuit is { } knownSuit
                    ? e.Args.Skip(1).Any(arg => (Suit)arg.Literal == knownSuit) : null;
            case "sameCardKind" or "sameCardSuit" or "sameCardName":
            {
                var firstCard = EvaluateCard(e.Args[0], cards);
                var secondCard = EvaluateCard(e.Args[1], cards);
                if (e.Op == "sameCardSuit") return firstCard?.EffectiveSuit is { } firstSuit && secondCard?.EffectiveSuit is { } secondSuit
                    ? firstSuit == secondSuit : null;
                if (firstCard?.EffectiveKind is not { } firstKind || secondCard?.EffectiveKind is not { } secondKind) return null;
                return e.Op == "sameCardKind" ? firstKind == secondKind :
                    RuntimeSkills.RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(firstKind) ==
                    RuntimeSkills.RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(secondKind);
            }
            case "not": return !EvaluateBool(e.Args[0], world, it, use, cards);
            case "and":
            {
                bool? result = true;
                foreach (var arg in e.Args)
                {
                    var value = EvaluateBool(arg, world, it, use, cards);
                    if (value == false) return false;
                    if (value is null) result = null;
                }
                return result;
            }
            case "or":
            {
                bool? result = false;
                foreach (var arg in e.Args)
                {
                    var value = EvaluateBool(arg, world, it, use, cards);
                    if (value == true) return true;
                    if (value is null) result = null;
                }
                return result;
            }
            case "same":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } left && EvaluateParticipant(e.Args[1], world, it, use, cards) is { } right
                    ? left == right : null;
            case "sameTeam":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } firstSeat && EvaluateParticipant(e.Args[1], world, it, use, cards) is { } secondSeat
                    ? world.SameTeam(firstSeat, secondSeat) : null;
            case "isFaction":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } factionSeat
                    ? world.IsFaction(factionSeat, e.Name!) : null;
            case "contains":
                return EvaluateParticipantSet(e.Args[0], world, it, use, cards) is { } members &&
                    EvaluateParticipant(e.Args[1], world, it, use, cards) is { } member ? members.Contains(member) : null;
            case "sameUse":
                if (EvaluateFact(e.Args[0], world, it, use, cards) is not { } first ||
                    EvaluateFact(e.Args[1], world, it, use, cards) is not { } second) return null;
                if (first.NativeUseId is { } firstNative && second.NativeUseId is { } secondNative)
                    return firstNative == secondNative;
                return first.NativeUseId is null && second.NativeUseId is null &&
                    first.ActionId is { } firstAction && second.ActionId is { } secondAction ? firstAction == secondAction : null;
            case "useKindIs":
                var kindFact = EvaluateFact(e.Args[0], world, it, use, cards);
                return (e.Name == "effective" ? kindFact?.EffectiveKind : kindFact?.DeclaredKind) is { } kind
                    ? ProgramCardKindMatch.Contains(e.Args.Skip(1).Select(arg => (CardKind)arg.Literal).ToArray(), kind) : null;
            case "usePhaseIs":
                var phaseFact = EvaluateFact(e.Args[0], world, it, use, cards);
                return (e.Name == "accepted" ? phaseFact?.AcceptedPhase : phaseFact?.Phase) is { } phase ? (int)phase == e.Literal : null;
            case "samePlayPhase":
                if (EvaluateFact(e.Args[0], world, it, use, cards) is not { AcceptedPhase: { } firstPhase } firstPlay ||
                    EvaluateFact(e.Args[1], world, it, use, cards) is not { AcceptedPhase: { } secondPhase } secondPlay) return null;
                if (firstPhase != TurnPhase.Play || secondPhase != TurnPhase.Play) return false;
                return firstPlay.AcceptedActualTurn is { } firstTurn && secondPlay.AcceptedActualTurn is { } secondTurn &&
                    firstPlay.AcceptedTurnOwnerSeat is { } firstOwner && secondPlay.AcceptedTurnOwnerSeat is { } secondOwner &&
                    firstPlay.AcceptedPlayPhaseInstanceId is { } firstInstance && secondPlay.AcceptedPlayPhaseInstanceId is { } secondInstance
                    ? firstTurn == secondTurn && firstOwner == secondOwner && firstInstance == secondInstance : null;
            case "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse" or "useFinalized" or "useCompleted":
                return EvaluateFact(e.Args[0], world, it, use, cards) is { } fact
                    ? e.Op switch { "useEstablished" => fact.Established, "useDeclared" => fact.NativeUseId.HasValue,
                        "useIsSelfNativeResponse" => fact.IsSelfNativeResponseUse, "useFinalized" => fact.Finalized,
                        "useCompleted" => fact.Completed, _ => fact.Type == CardActionType.Response } : null;
            case "atOrBefore":
                return EvaluateFact(e.Args[0], world, it, use, cards) is { } before && EvaluateFact(e.Args[1], world, it, use, cards) is { } after
                    ? before.AcceptedCursor is { } beforeCursor && after.AcceptedCursor is { } afterCursor
                        ? beforeCursor <= afterCursor
                        : before.ActionId is { } beforeAction && after.ActionId is { } afterAction
                        ? beforeAction <= afterAction : before.DeclarationCursor <= after.DeclarationCursor : null;
            case "all" or "any":
            {
                if (EvaluatePredicates(e, world, it, use, cards) is not { } values) return null;
                bool? result = e.Op == "all";
                foreach (var value in values)
                {
                    if (value == (e.Op == "any")) return value;
                    if (value is null) result = null;
                }
                return result;
            }
        }
        if (Comparisons.Contains(e.Op))
        {
            if (EvaluateInt(e.Args[0], world, it, use, cards) is not { } left || EvaluateInt(e.Args[1], world, it, use, cards) is not { } right) return null;
            return e.Op switch
            {
                "<" => left < right, "<=" => left <= right, ">" => left > right,
                ">=" => left >= right, "==" => left == right, _ => left != right
            };
        }
        if (Flags.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } seat ? world.Flag(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not boolean.");
    }

    public static int? EvaluateInt(ProgramExpression e, IProgramExpressionWorld world, int? it = null, CardUseHistoryFact? use = null,
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards = null)
    {
        switch (e.Op)
        {
            case "int": return e.Literal;
            case "when": return EvaluateBool(e.Args[0], world, it, use, cards) == true ? EvaluateInt(e.Args[1], world, it, use, cards) : null;
            case "number": return world.Number(e.Name!);
            case "windowAmount": return world.WindowAmount();
            case "distance":
                return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } distanceSource &&
                    EvaluateParticipant(e.Args[1], world, it, use, cards) is { } distanceTarget
                    ? world.CombatDistance(distanceSource, distanceTarget) : null;
            case "gainActualTurn": return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } gainOwner ? world.HandGain(gainOwner)?.ActualTurn : null;
            case "phaseCount": return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } phaseActor
                ? world.PhaseCount(e.Name!, phaseActor, e.Literal < 0 ? null : (ActualDiscardRecoveryPhaseKind)e.Literal,
                    (ActualPhaseHistoryOutcome)e.Args[1].Literal) : null;
            case "windowHp" or "windowHandCount": return world.WindowStat(e.Op, e.Args[0].Name!);
            case "cardMarkCount": return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } markedSeat
                ? world.CardMarkCount(markedSeat, e.Name!, (CardZoneKind)e.Literal) : null;
            case "handPrintedSuitCount": return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } handSeat
                ? world.HandPrintedSuitCount(handSeat, (Suit)e.Literal) : null;
            case "publicPileCount": return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } pileSeat
                ? world.PublicPileCount(pileSeat, e.Name!) : null;
            case "choiceNumber" or "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank": return world.BindingStat(e.Op, e.Name!);
            case "-":
                return EvaluateInt(e.Args[0], world, it, use, cards) is { } left && EvaluateInt(e.Args[1], world, it, use, cards) is { } right ? left - right : null;
            case "mod":
                return EvaluateInt(e.Args[0], world, it, use, cards) is { } numerator &&
                    EvaluateInt(e.Args[1], world, it, use, cards) is { } denominator && denominator != 0
                    ? (int)(numerator % (long)denominator) : null;
            case "floorDiv":
            {
                if (EvaluateInt(e.Args[0], world, it, use, cards) is not { } dividend ||
                    EvaluateInt(e.Args[1], world, it, use, cards) is not { } divisor || divisor == 0) return null;
                var quotient = dividend / (long)divisor;
                if (dividend % (long)divisor != 0 && (dividend < 0) != (divisor < 0)) quotient--;
                return quotient is >= int.MinValue and <= int.MaxValue ? (int)quotient : null;
            }
            case "+" or "min" or "max":
            {
                var values = e.Args.Select(arg => EvaluateInt(arg, world, it, use, cards)).ToArray();
                if (values.Any(value => value is null)) return null;
                var numbers = values.Select(value => value!.Value);
                return e.Op == "+" ? numbers.Sum() : e.Op == "min" ? numbers.Min() : numbers.Max();
            }
            case "count":
            {
                if (EvaluatePredicates(e, world, it, use, cards) is not { } values) return null;
                var count = 0;
                foreach (var value in values)
                    switch (value)
                    {
                        case null: return null;
                        case true: count++; break;
                    }
                return count;
            }
        }
        if (Stats.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } seat ? world.Stat(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not an integer.");
    }

    public static IReadOnlyList<int>? EvaluateParticipantSet(ProgramExpression e, IProgramExpressionWorld world,
        int? it = null, CardUseHistoryFact? use = null,
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards = null)
    {
        switch (e.Op)
        {
            case "useResultTargets": return world.UseResultTargets(e.Name!) is { } targets
                ? Array.AsReadOnly(targets.Distinct().ToArray()) : null;
            case "useTargets": return EvaluateFact(e.Args[0], world, it, use, cards)?.Targets;
            case "useConfirmedTargets": return EvaluateFact(e.Args[0], world, it, use, cards)?.ConfirmedTargets;
            case "usePrimaryTargets": return EvaluateFact(e.Args[0], world, it, use, cards)?.PrimaryTargets;
            case "targets": return EvaluateFactSet(e.Args[0], world, it, use, cards) is { } facts
                ? Array.AsReadOnly(facts.SelectMany(fact => fact.Targets).Distinct().ToArray()) : null;
            case "intersect": return EvaluateParticipantSet(e.Args[0], world, it, use, cards) is { } left &&
                    EvaluateParticipantSet(e.Args[1], world, it, use, cards) is { } right
                ? Array.AsReadOnly(left.Intersect(right).ToArray()) : null;
            case "filter":
            {
                if (EvaluateParticipantSet(e.Args[0], world, it, use, cards) is not { } seats) return null;
                var result = new List<int>();
                foreach (var seat in seats)
                {
                    var accepted = EvaluateBool(e.Args[1], world, seat, use, cards);
                    if (accepted is null) return null;
                    if (accepted == true) result.Add(seat);
                }
                return Array.AsReadOnly(result.ToArray());
            }
            default: return world.ParticipantSet(e.Name!);
        }
    }

    public static IReadOnlyList<ProgramExpressionCardFact>? EvaluateCardSet(ProgramExpression e, IProgramExpressionWorld world,
        int? it = null, CardUseHistoryFact? use = null,
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards = null)
    {
        if (e.Op == "publicRevealCards") return world.PublicRevealCards(e.Name!, e.Literal == 1 ? "explicit" : "all");
        if (e.Op == "boundCards") return world.BoundCards(e.Name!);
        if (e.Op == "givenCards")
            return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } giver &&
                EvaluateParticipant(e.Args[1], world, it, use, cards) is { } recipient
                    ? world.GivenCards(giver, recipient) : null;
        if (e.Op == "gainCards")
            return EvaluateParticipant(e.Args[0], world, it, use, cards) is { } recipient && world.HandGain(recipient) is { } gain
                ? Array.AsReadOnly((e.Name == "incoming" ? gain.Incoming : gain.PreviousHand)
                    .Select(card => ProgramExpressionCardFact.FromHandGain(card, recipient)).ToArray()) : null;
        if (e.Op != "filter") throw new InvalidOperationException($"Expression operator '{e.Op}' is not a card set.");
        if (EvaluateCardSet(e.Args[0], world, it, use, cards) is not { } source) return null;
        var result = new List<ProgramExpressionCardFact>();
        foreach (var card in source)
        {
            var accepted = EvaluateBool(e.Args[1], world, it, use, BindCard(cards, e.Name!, card));
            if (accepted is null) return null;
            if (accepted == true) result.Add(card);
        }
        return Array.AsReadOnly(result.ToArray());
    }

    private static IReadOnlyList<CardUseHistoryFact>? EvaluateFactSet(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use, IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards)
    {
        if (e.Op == "cardUses") return world.CardUses(e.Literal switch { 1 => "now", 2 => "turnEnd", _ => "window" }, e.Name!);
        if (e.Op is "takeFirst" or "takeLast") return EvaluateFactSet(e.Args[0], world, it, use, cards) is { } source &&
                EvaluateInt(e.Args[1], world, it, use, cards) is { } count
            ? Array.AsReadOnly((e.Op == "takeFirst" ? source.Take(Math.Max(0, count)) : source.TakeLast(Math.Max(0, count))).ToArray()) : null;
        if (e.Op != "filter") throw new InvalidOperationException($"Expression operator '{e.Op}' is not a fact set.");
        if (EvaluateFactSet(e.Args[0], world, it, use, cards) is not { } facts) return null;
        var result = new List<CardUseHistoryFact>();
        foreach (var fact in facts)
        {
            var accepted = EvaluateBool(e.Args[1], world, it, fact, cards);
            if (accepted is null) return null;
            if (accepted == true) result.Add(fact);
        }
        return Array.AsReadOnly(result.ToArray());
    }

    private static IEnumerable<bool?>? EvaluatePredicates(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use, IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards) => e.Args[0].Type switch
    {
        ProgramExpressionType.FactSet => EvaluateFactSet(e.Args[0], world, it, use, cards)
            ?.Select(fact => EvaluateBool(e.Args[1], world, it, fact, cards)),
        ProgramExpressionType.CardSet => EvaluateCardSet(e.Args[0], world, it, use, cards)
            ?.Select(card => EvaluateBool(e.Args[1], world, it, use, BindCard(cards, e.Name!, card))),
        _ => EvaluateParticipantSet(e.Args[0], world, it, use, cards)
            ?.Select(seat => EvaluateBool(e.Args[1], world, seat, use, cards))
    };

    private static ProgramExpressionCardFact? EvaluateCard(ProgramExpression e,
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards) =>
        cards is not null && cards.TryGetValue(e.Name!, out var card) ? card : null;

    private static IReadOnlyDictionary<string, ProgramExpressionCardFact> BindCard(
        IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards, string name, ProgramExpressionCardFact card)
    {
        var scope = cards is null ? new Dictionary<string, ProgramExpressionCardFact>(StringComparer.Ordinal) :
            new Dictionary<string, ProgramExpressionCardFact>(cards, StringComparer.Ordinal);
        scope.Add(name, card);
        return scope;
    }

    private static CardUseHistoryFact? EvaluateFact(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use, IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards) => e.Op is "first" or "last"
        ? EvaluateFactSet(e.Args[0], world, it, use, cards) is { } facts ? e.Op == "first" ? facts.FirstOrDefault() : facts.LastOrDefault() : null
        : e.Name == FactIterator ? use : world.CurrentUse();

    private static int? EvaluateParticipant(ProgramExpression e, IProgramExpressionWorld world, int? it, CardUseHistoryFact? use, IReadOnlyDictionary<string, ProgramExpressionCardFact>? cards) =>
        e.Op is "useActor" or "useDeclarer" ? EvaluateFact(e.Args[0], world, it, use, cards) is { } fact
            ? e.Op == "useActor" ? fact.ActorSeat : fact.DeclarerSeat : null :
        e.Name == Iterator ? it : world.Participant(e.Name!);

    private static T ParseFactEnum<T>(JsonElement node, string path) where T : struct, Enum
    {
        if (node.ValueKind == JsonValueKind.String && Enum.TryParse<T>(node.GetString(), true, out var value) &&
            Enum.GetName(value) is { } name && node.GetString() == char.ToLowerInvariant(name[0]) + name[1..])
            return value;
        throw Failure(path, $"must be a supported camelCase {typeof(T).Name}");
    }

    private static string Camel(ProgramExpressionType type) => type switch
    {
        ProgramExpressionType.Bool => "a boolean",
        ProgramExpressionType.Int => "an integer",
        ProgramExpressionType.Participant => "a participant",
        ProgramExpressionType.ParticipantSet => "a participant set",
        ProgramExpressionType.Fact => "a card-use fact",
        ProgramExpressionType.FactSet => "a card-use fact set",
        ProgramExpressionType.Card => "a frozen card fact",
        _ => "a card set"
    };

    private static void Fail(string path, string message) => throw Failure(path, message);
    private static InvalidOperationException Failure(string path, string message) =>
        new($"Invalid skill program at {path}: {message}.");
}

/// <summary>
/// AI estimate view: only the owner and the selected target are known; every
/// other participant or set is unknown, so expressions that need them are false.
/// </summary>
internal sealed class SkillContextExpressionWorld(PlayerSkillContext owner, PlayerSkillContext? selectedTarget,
    int? ownerEquipmentCount = null, Func<string, string, int?>? bindingStat = null,
    Func<string, int?>? number = null, SkillProgramTriggerFacts? windowFacts = null,
    IProgramExpressionWorld? ownerWindowFacts = null, Func<int, string, bool?>? bindingContains = null,
    Func<string, IReadOnlyList<ProgramExpressionCardFact>?>? bindingCards = null,
    Func<ProgramExpressionCardFact, bool?>? stillAtSource = null) : IProgramExpressionWorld
{
    public int? WindowStat(string stat, string participant) => ProgramExpressions.ReadWindowStat(windowFacts, stat, participant, selectedTarget is not null);
    public int? BindingStat(string stat, string name) => bindingStat?.Invoke(stat, name);
    public int? Number(string name) => number?.Invoke(name);
    public IReadOnlyList<int>? UseResultTargets(string bind) => ownerWindowFacts?.UseResultTargets(bind);
    public int? WindowAmount() => ownerWindowFacts?.WindowAmount();
    public IReadOnlyList<ProgramExpressionCardFact>? GivenCards(int giver, int recipient) =>
        giver == owner.Seat || recipient == owner.Seat ? ownerWindowFacts?.GivenCards(giver, recipient) : null;
    public HandGainOccurrence? HandGain(int recipient) => recipient == owner.Seat ? ownerWindowFacts?.HandGain(recipient) : null;
    public bool? GainHasSource(int recipient, string moment) => recipient == owner.Seat ? ownerWindowFacts?.GainHasSource(recipient, moment) : null;
    public bool? CardAt(int cardId, int seat, CardZoneKind zone) =>
        seat == owner.Seat || zone == CardZoneKind.Equipment ? ownerWindowFacts?.CardAt(cardId, seat, zone) : null;
    public bool? CardCanDiscard(int seat, int cardId) => seat == owner.Seat ? ownerWindowFacts?.CardCanDiscard(seat, cardId) : null;
    public bool? CardInBinding(int cardId, string name) => bindingContains?.Invoke(cardId, name);
    public IReadOnlyList<ProgramExpressionCardFact>? PublicRevealCards(string scope, string purpose) => ownerWindowFacts?.PublicRevealCards(scope, purpose);
    public IReadOnlyList<ProgramExpressionCardFact>? BoundCards(string name) =>
        bindingCards is null ? ownerWindowFacts?.BoundCards(name) : bindingCards(name);
    public bool? CardStillAtSource(ProgramExpressionCardFact card)
    {
        // A formerly public identity does not reveal a foreign Hand's later
        // private departures. The native rules world retains its full read.
        if (card.CapturedLocation is not { } location ||
            location.OwnerSeat != owner.Seat && location.Zone is not (CardZoneKind.Equipment or CardZoneKind.DiscardPile))
            return null;
        return stillAtSource is null ? ownerWindowFacts?.CardStillAtSource(card) : stillAtSource(card);
    }
    private PlayerSkillContext? Context(int seat) =>
        seat == owner.Seat ? owner : selectedTarget?.Seat == seat ? selectedTarget : null;

    public int? Participant(string name) => name switch
    {
        "owner" => owner.Seat,
        "selectedTarget" => selectedTarget?.Seat,
        _ => null
    };

    public IReadOnlyList<int>? ParticipantSet(string name) => null;

    public int? Stat(string stat, int seat)
    {
        if (Context(seat) is not { } context) return null;
        return stat switch
        {
            "handCount" => context.HandCount,
            "handLimit" => context.HandLimit,
            "equipmentCount" => seat == owner.Seat ? ownerEquipmentCount : null,
            "maxHp" => context.MaxHp,
            "hp" => context.HpUndefined ? null : context.Hp,
            "lostHp" => context.HpUndefined ? null : context.MaxHp - context.Hp,
            _ => null
        };
    }

    public bool? Flag(string flag, int seat)
    {
        if (Context(seat) is not { } context) return null;
        return flag switch
        {
            "alive" => true,
            "hasHp" => !context.HpUndefined,
            "faceDown" => context.IsFaceDown,
            "chained" => context.IsChained,
            _ => null
        };
    }
}
