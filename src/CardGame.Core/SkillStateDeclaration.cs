using System.Text.Json;

namespace CardGame.Core;

public enum SkillStateTarget { Owner, All }

/// <summary>A continuous restriction declared by a real skill source, without a program frame.</summary>
public sealed class SkillStateDeclaration
{
    public SkillStateDeclaration(SkillStateTarget target, SkillStateSpec state)
    {
        if (state.Action is not (SkillStateAction.Suppress or SkillStateAction.Lose) ||
            state.Until.Count != 0 || state.ChooseOne || state.Match.IsTransient || state.While is null ||
            !SkillStateDescriptor.IsPersistentCondition(state.While) || state.While.Reads("inIssuedPhase"))
            throw new ArgumentException("A skill state declaration requires a persistent match and condition, suppress/lose, and no frame, choice or expiry.", nameof(state));
        Target = target; State = state;
    }

    public SkillStateTarget Target { get; }
    public SkillStateSpec State { get; }

    public bool StructurallyEquals(SkillStateDeclaration other) => Target == other.Target &&
        State.Action == other.State.Action && State.Match.StructurallyEquals(other.State.Match) &&
        State.While!.StructurallyEquals(other.State.While!);
}

internal static class SkillStateDeclarationParser
{
    internal static SkillStateDeclaration Parse(JsonElement node, string path)
    {
        SkillStateDescriptor.ObjectShape(node, path, "target", "action", "match", "while");
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
        if (!node.TryGetProperty("while", out var condition)) throw Invalid(path + ".while", "is required");
        var @while = ProgramExpressions.Parse(condition, path + ".while");
        try { return new(target, new(action, match, until: [], @while: @while)); }
        catch (ArgumentException exception) { throw Invalid(path, exception.Message); }

        JsonElement Required(string name, JsonValueKind kind) => node.TryGetProperty(name, out var value) && value.ValueKind == kind
            ? value : throw Invalid(path + "." + name, "must be " + kind);
    }

    private static InvalidOperationException Invalid(string path, string message) => new($"Invalid skill program at {path}: {message}.");
}
