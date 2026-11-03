using System.Text.Json;

namespace CardGame.Core;

internal static class AttackRangeRelationParser
{
    internal static AttackRangeRelationDeclaration Parse(JsonElement node, string path)
    {
        SkillStateDescriptor.ObjectShape(node, path, "id", "while", "ownerRole");
        var id = SkillStateDescriptor.Required(node, "id", JsonValueKind.String, path).GetString();
        if (string.IsNullOrWhiteSpace(id) || id.Length > 128)
            throw Invalid(path + ".id", "requires an identifier of at most 128 characters");
        Role? role = null;
        if (node.TryGetProperty("ownerRole", out var roleNode))
        {
            var name = roleNode.ValueKind == JsonValueKind.String ? roleNode.GetString() : null;
            if (!Enum.GetNames<Role>().Any(value => string.Equals(value, name, StringComparison.OrdinalIgnoreCase)))
                throw Invalid(path + ".ownerRole", "requires a known role name");
            role = Enum.Parse<Role>(name!, ignoreCase: true);
        }
        if (!node.TryGetProperty("while", out var condition))
            throw Invalid(path + ".while", "is required");
        return new(id, ParseWhile(condition, path + ".while"), role);
    }

    internal static ProgramExpression ParseWhile(JsonElement node, string path)
    {
        var expression = ProgramExpressions.Parse(node, path);
        if (expression.Nodes().Any(part => !(part.Op is "eventSource" or "isFaction" or "distance" or "sameTeam" ||
                SkillStateDescriptor.IsPersistentConditionNode(part) && part.Op is not
                    ("selectedTarget" or "inIssuedPhase" or "inIssuedTurn" or "inIssuedDying" or "inUpcomingTurnPrelude"))))
            throw Invalid(path, "must read persistent owner/eventSource/eventTarget seat facts, without frame bindings, window facts or skill-dependent queries");
        return expression;
    }

    private static InvalidOperationException Invalid(string path, string reason) =>
        new($"Invalid skill program at {path}: {reason}.");
}
