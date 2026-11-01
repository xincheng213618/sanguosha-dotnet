using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ConditionalSkillGrantContinuation { Cancel, FinishInvocation, FinishPaidInstruction }
public sealed record ConditionalSkillGrantRule(string Id, string SkillId, ProgramExpression While,
    ConditionalSkillGrantContinuation OnSourceLoss = ConditionalSkillGrantContinuation.Cancel);
public sealed record ConditionalSkillGrantSource(string CapabilityGrantId, string CapabilitySkillId,
    string CapabilitySkillInstanceId, string RuleId);
public sealed record ConditionalSkillGrantChangedEvent(int OwnerSeat, string GrantId, string SkillId,
    string SkillInstanceId, ConditionalSkillGrantSource Source, bool Added, int? OwnerHp) : IGameEvent;
public sealed record ConditionalSkillGrantProgramReceipt(ConditionalSkillGrantSource Source,
    ConditionalSkillGrantContinuation OnSourceLoss);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConditionalSkillGrantProgramReceipt? ConditionalSourceReceipt { get; init; }
}

internal static class ConditionalSkillGrantParser
{
    internal static bool IsCondition(ProgramExpression expression) => expression.Nodes().All(n =>
        (SkillStateDescriptor.IsPersistentConditionNode(n) || n.Op is "equipmentSuitCount" or "ruleWounded") &&
        (n.Type != ProgramExpressionType.Participant || n.Op is "owner" or "it") &&
        (n.Type != ProgramExpressionType.ParticipantSet || n.Op is "living" or "livingOthers") &&
        n.Op is not ("handLimit" or "inIssuedPhase" or "inUpcomingTurnPrelude" or "isTurnOwner" or "inIssuedTurn" or "inIssuedDying"));

    internal static IReadOnlyList<ConditionalSkillGrantRule> Parse(JsonElement skill, string path)
    {
        if (!skill.TryGetProperty("skillGrants", out var array)) return [];
        path += ".skillGrants";
        if (array.ValueKind != JsonValueKind.Array || array.GetArrayLength() is < 1 or > 16)
            throw new InvalidOperationException(path + " requires one to sixteen rules.");
        var result = new List<ConditionalSkillGrantRule>();
        foreach (var node in array.EnumerateArray())
        {
            SkillStateDescriptor.ObjectShape(node, path, "id", "skillId", "while", "onSourceLoss");
            string Id(string name) => node.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String &&
                !string.IsNullOrWhiteSpace(value.GetString()) && value.GetString()!.Length <= 128 ? value.GetString()! :
                throw new InvalidOperationException(path + " requires " + name + ".");
            var id = Id("id");
            var skillId = Id("skillId");
            try { _ = new ContentId(skillId); }
            catch (ArgumentException error) { throw new InvalidOperationException(path + " has an invalid skillId.", error); }
            if (!node.TryGetProperty("while", out var condition))
                throw new InvalidOperationException(path + " requires a while condition.");
            var expression = ProgramExpressions.Parse(condition, path + ".while");
            if (!IsCondition(expression))
                throw new InvalidOperationException(path + " while requires raw owner/living facts without a frame or issued window.");
            var continuation = node.TryGetProperty("onSourceLoss", out var policy) && policy.ValueKind == JsonValueKind.String ? policy.GetString() switch
            {
                "cancel" => ConditionalSkillGrantContinuation.Cancel,
                "finishInvocation" => ConditionalSkillGrantContinuation.FinishInvocation,
                "finishPaidInstruction" => ConditionalSkillGrantContinuation.FinishPaidInstruction,
                _ => throw new InvalidOperationException(path + " has an invalid onSourceLoss policy.")
            } : node.TryGetProperty("onSourceLoss", out _) ? throw new InvalidOperationException(path + " onSourceLoss must be a string.") : ConditionalSkillGrantContinuation.Cancel;
            if (result.Any(rule => rule.Id == id || rule.SkillId == skillId))
                throw new InvalidOperationException(path + " requires distinct rule and target skill ids.");
            result.Add(new(id, skillId, expression, continuation));
        }
        return Array.AsReadOnly(result.ToArray());
    }
}
