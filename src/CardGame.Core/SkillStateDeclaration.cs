using System.Text.Json;

namespace CardGame.Core;

public enum SkillStateTarget { Owner, All }

/// <summary>A continuous restriction declared by a real skill source, without a program frame.</summary>
public sealed class SkillStateDeclaration
{
    public SkillStateDeclaration(SkillStateTarget target, SkillStateSpec state,
        SkillStateBoundarySpec? on = null, ProgramExpression? when = null)
    {
        if (state.Action is not (SkillStateAction.Suppress or SkillStateAction.Lose) ||
            state.ChooseOne || state.Match.IsTransient ||
            on is null && (state.Until.Count != 0 || state.While is null || when is not null) ||
            !Persistent(state.While) || !Persistent(when))
            throw new ArgumentException("A skill state declaration requires persistent suppress/lose data without a frame or choice; acquisition declarations require while and no event conditions or expiry.", nameof(state));
        if (on is not null)
        {
            var requiredEnd = on switch
            {
                { Event: SkillStateBoundaryEvent.TurnStarted, Participant: SkillStateSubject.Owner,
                    Timing: SkillStateBoundaryTiming.Before, SkipIssued: false, Phase: null } =>
                    new SkillStateBoundarySpec(SkillStateBoundaryEvent.TurnEnded, SkillStateSubject.Owner),
                { Event: SkillStateBoundaryEvent.DyingStarted, Participant: null,
                    Timing: SkillStateBoundaryTiming.Before, SkipIssued: false, Phase: null } =>
                    new SkillStateBoundarySpec(SkillStateBoundaryEvent.DyingEnded, null),
                _ => throw new ArgumentException("on supports only owner turnStarted/before or global dyingStarted/before, without skipIssued or phase.", nameof(on))
            };
            if (!state.Until.Contains(requiredEnd))
                throw new ArgumentException("An event declaration must end after its issued owner turn or dying window; additional OR boundaries are allowed.", nameof(state));
        }
        Target = target; State = state; On = on; When = when;

        bool Persistent(ProgramExpression? expression) => expression is null ||
            SkillStateDescriptor.IsPersistentCondition(expression) && !expression.Reads("inIssuedPhase") &&
            (on is not null || !expression.Nodes().Any(node => node.Op is "eventTarget" or "inIssuedTurn" or "inIssuedDying"));
    }

    public SkillStateTarget Target { get; }
    public SkillStateSpec State { get; }
    public SkillStateBoundarySpec? On { get; }
    public ProgramExpression? When { get; }

    public bool StructurallyEquals(SkillStateDeclaration other) => Target == other.Target &&
        State.Action == other.State.Action && State.Match.StructurallyEquals(other.State.Match) &&
        State.Until.SequenceEqual(other.State.Until) && On == other.On && Same(State.While, other.State.While) && Same(When, other.When);

    private static bool Same(ProgramExpression? left, ProgramExpression? right) =>
        left is null ? right is null : right is not null && left.StructurallyEquals(right);
}

internal static class SkillStateDeclarationParser
{
    internal static SkillStateDeclaration Parse(JsonElement node, string path)
    {
        SkillStateDescriptor.ObjectShape(node, path, "target", "action", "match", "while", "on", "when", "until");
        var target = Required("target", JsonValueKind.String).GetString() switch
        {
            "owner" => SkillStateTarget.Owner,
            "all" => SkillStateTarget.All,
            _ => throw Invalid(path + ".target", "must be owner or all")
        };
        var action = Required("action", JsonValueKind.String).GetString() switch
        {
            "suppress" => SkillStateAction.Suppress,
            "lose" => SkillStateAction.Lose,
            _ => throw Invalid(path + ".action", "must be suppress or lose")
        };
        var match = SkillStateDescriptor.ParseMatch(Required("match", JsonValueKind.Object), path + ".match");
        var @while = node.TryGetProperty("while", out var condition) ? ProgramExpressions.Parse(condition, path + ".while") : null;
        var on = node.TryGetProperty("on", out var eventNode) ? SkillStateDescriptor.ParseBoundary(eventNode, path + ".on") : null;
        var when = node.TryGetProperty("when", out var whenNode) ? ProgramExpressions.Parse(whenNode, path + ".when") : null;
        IReadOnlyList<SkillStateBoundarySpec> until = node.TryGetProperty("until", out _)
            ? SkillStateDescriptor.ParseUntil(Required("until", JsonValueKind.Array), path + ".until") : [];
        if (on is null && node.TryGetProperty("until", out _)) throw Invalid(path + ".until", "requires on");
        try { return new(target, new(action, match, until, @while), on, when); }
        catch (ArgumentException exception) { throw Invalid(path, exception.Message); }

        JsonElement Required(string name, JsonValueKind kind) => node.TryGetProperty(name, out var value) && value.ValueKind == kind
            ? value : throw Invalid(path + "." + name, "must be " + kind);
    }

    private static InvalidOperationException Invalid(string path, string message) => new($"Invalid skill program at {path}: {message}.");
}
