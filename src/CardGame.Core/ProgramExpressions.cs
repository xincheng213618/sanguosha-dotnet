using System.Text.Json;

namespace CardGame.Core;

public enum ProgramExpressionType { Bool, Int, Participant, ParticipantSet }

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
    int? Stat(string stat, int seat);
    bool? Flag(string flag, int seat);
    int? BindingStat(string stat, string name) => null;
    int? Number(string name) => null;
}

internal static class ProgramExpressions
{
    public const int MaximumDepth = 8;
    public const string Iterator = "it";
    public static readonly string[] Participants = ["owner", "selectedTarget", "eventSource", "eventTarget"];
    public static readonly string[] Sets = ["living", "livingOthers"];
    private static readonly string[] Stats = ["hp", "maxHp", "lostHp", "handCount", "handLimit", "equipmentCount"];
    private static readonly string[] Flags = ["alive", "hasHp", "faceDown", "chained"];
    private static readonly string[] Comparisons = ["<", "<=", ">", ">=", "==", "!="];

    public static ProgramExpression Parse(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Bool, 0, iteratorInScope: false);

    public static ProgramExpression ParseInt(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Int, 0, iteratorInScope: false);

    private static ProgramExpression Parse(JsonElement node, string path, ProgramExpressionType expected, int depth, bool iteratorInScope)
    {
        if (depth > MaximumDepth) Fail(path, $"expression nesting exceeds {MaximumDepth}");
        var result = node.ValueKind switch
        {
            JsonValueKind.True or JsonValueKind.False =>
                new ProgramExpression("bool", ProgramExpressionType.Bool, [], node.GetBoolean() ? 1 : 0),
            JsonValueKind.Number when node.TryGetInt32(out var value) && value is >= -99 and <= 99 =>
                new ProgramExpression("int", ProgramExpressionType.Int, [], value),
            JsonValueKind.String => ParseName(node.GetString()!, path, iteratorInScope),
            JsonValueKind.Array => ParseCall(node, path, depth, iteratorInScope),
            _ => throw Failure(path, "must be a boolean, an integer from -99 to 99, a name or an [operator, ...] array")
        };
        if (result.Type != expected) Fail(path, $"expected {Camel(expected)} but found {Camel(result.Type)}");
        return result;
    }

    private static ProgramExpression ParseName(string name, string path, bool iteratorInScope)
    {
        if (name == Iterator)
        {
            if (!iteratorInScope) Fail(path, "'it' is only available inside all, any or count");
            return new(name, ProgramExpressionType.Participant, [], name: name);
        }
        if (Participants.Contains(name)) return new(name, ProgramExpressionType.Participant, [], name: name);
        if (Sets.Contains(name)) return new(name, ProgramExpressionType.ParticipantSet, [], name: name);
        throw Failure(path, $"unknown name '{name}'");
    }

    private static ProgramExpression ParseCall(JsonElement node, string path, int depth, bool iteratorInScope)
    {
        var items = node.EnumerateArray().ToArray();
        if (items.Length == 0 || items[0].ValueKind != JsonValueKind.String) Fail(path, "an array must start with an operator name");
        var op = items[0].GetString()!;
        var args = items.Skip(1).ToArray();
        ProgramExpression Arg(int index, ProgramExpressionType type, bool iterator = false) =>
            Parse(args[index], $"{path}[{index + 1}]", type, depth + 1, iteratorInScope || iterator);
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
        if (Flags.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (Stats.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Participant)); }
        if (op is "+" or "min" or "max") { Arity(2, 8); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "-") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "floorDiv") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "when")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Bool), Arg(1, ProgramExpressionType.Int)]);
        }
        if (op is "number" or "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank" or "boundIsEquipment")
        {
            Arity(1, 1);
            if (args[0].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[0].GetString()) || args[0].GetString()!.Length > 128)
                Fail(path, "a binding reference requires a nonempty identifier of at most 128 characters");
            return new(op, op == "boundIsEquipment" ? ProgramExpressionType.Bool : ProgramExpressionType.Int, [], name: args[0].GetString());
        }
        if (op is "all" or "any" or "count")
        {
            Arity(2, 2);
            return new(op, op == "count" ? ProgramExpressionType.Int : ProgramExpressionType.Bool,
                [Arg(0, ProgramExpressionType.ParticipantSet), Arg(1, ProgramExpressionType.Bool, iterator: true)]);
        }
        throw Failure(path, $"unknown operator '{op}'");
    }

    /// <summary>Three-valued result: null when a read fact is unknown or undefined.</summary>
    public static bool? EvaluateBool(ProgramExpression e, IProgramExpressionWorld world, int? it = null)
    {
        if (e.Op == "boundIsEquipment") return world.BindingStat(e.Op, e.Name!) is { } value ? value != 0 : null;
        switch (e.Op)
        {
            case "bool": return e.Literal != 0;
            case "not": return !EvaluateBool(e.Args[0], world, it);
            case "and":
            {
                bool? result = true;
                foreach (var arg in e.Args)
                {
                    var value = EvaluateBool(arg, world, it);
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
                    var value = EvaluateBool(arg, world, it);
                    if (value == true) return true;
                    if (value is null) result = null;
                }
                return result;
            }
            case "same":
                return EvaluateParticipant(e.Args[0], world, it) is { } left && EvaluateParticipant(e.Args[1], world, it) is { } right
                    ? left == right : null;
            case "all" or "any":
            {
                if (world.ParticipantSet(e.Args[0].Name!) is not { } seats) return null;
                bool? result = e.Op == "all";
                foreach (var seat in seats)
                {
                    var value = EvaluateBool(e.Args[1], world, seat);
                    if (value == (e.Op == "any")) return value;
                    if (value is null) result = null;
                }
                return result;
            }
        }
        if (Comparisons.Contains(e.Op))
        {
            if (EvaluateInt(e.Args[0], world, it) is not { } left || EvaluateInt(e.Args[1], world, it) is not { } right) return null;
            return e.Op switch
            {
                "<" => left < right, "<=" => left <= right, ">" => left > right,
                ">=" => left >= right, "==" => left == right, _ => left != right
            };
        }
        if (Flags.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it) is { } seat ? world.Flag(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not boolean.");
    }

    public static int? EvaluateInt(ProgramExpression e, IProgramExpressionWorld world, int? it = null)
    {
        switch (e.Op)
        {
            case "int": return e.Literal;
            case "when": return EvaluateBool(e.Args[0], world, it) == true ? EvaluateInt(e.Args[1], world, it) : null;
            case "number": return world.Number(e.Name!);
            case "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank": return world.BindingStat(e.Op, e.Name!);
            case "-":
                return EvaluateInt(e.Args[0], world, it) is { } left && EvaluateInt(e.Args[1], world, it) is { } right ? left - right : null;
            case "floorDiv":
            {
                if (EvaluateInt(e.Args[0], world, it) is not { } dividend ||
                    EvaluateInt(e.Args[1], world, it) is not { } divisor || divisor == 0) return null;
                var quotient = dividend / (long)divisor;
                if (dividend % (long)divisor != 0 && (dividend < 0) != (divisor < 0)) quotient--;
                return quotient is >= int.MinValue and <= int.MaxValue ? (int)quotient : null;
            }
            case "+" or "min" or "max":
            {
                var values = e.Args.Select(arg => EvaluateInt(arg, world, it)).ToArray();
                if (values.Any(value => value is null)) return null;
                var numbers = values.Select(value => value!.Value);
                return e.Op == "+" ? numbers.Sum() : e.Op == "min" ? numbers.Min() : numbers.Max();
            }
            case "count":
            {
                if (world.ParticipantSet(e.Args[0].Name!) is not { } seats) return null;
                var count = 0;
                foreach (var seat in seats)
                    switch (EvaluateBool(e.Args[1], world, seat))
                    {
                        case null: return null;
                        case true: count++; break;
                    }
                return count;
            }
        }
        if (Stats.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it) is { } seat ? world.Stat(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not an integer.");
    }

    private static int? EvaluateParticipant(ProgramExpression e, IProgramExpressionWorld world, int? it) =>
        e.Name == Iterator ? it : world.Participant(e.Name!);

    private static string Camel(ProgramExpressionType type) => type switch
    {
        ProgramExpressionType.Bool => "a boolean",
        ProgramExpressionType.Int => "an integer",
        ProgramExpressionType.Participant => "a participant",
        _ => "a participant set"
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
    Func<string, int?>? number = null) : IProgramExpressionWorld
{
    public int? BindingStat(string stat, string name) => bindingStat?.Invoke(stat, name);
    public int? Number(string name) => number?.Invoke(name);
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
