using System.Text.Json;

namespace CardGame.Core;

internal sealed class SkillStateDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SkillState;
    public override ISkillProgramEffectHandler Handler { get; } = new SkillStateHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills, static (_, _) => { });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "targetRef", "action", "match", "until", "while", "chooseOne", "resultBind", "condition");
        if (!reader.Has("target") && !reader.Has("targetRef")) Fail(reader.Path, "target or targetRef is required");
        var match = ParseMatch(Required(reader.Node, "match", JsonValueKind.Object, reader.Path), reader.Path + ".match");
        IReadOnlyList<SkillStateBoundarySpec> until = reader.Has("until")
            ? ParseUntil(Required(reader.Node, "until", JsonValueKind.Array, reader.Path), reader.Path + ".until") : [];
        var condition = reader.Has("while") ? reader.RequiredBoolExpression("while") : null;
        if (condition is not null && !IsPersistentCondition(condition))
            Fail(reader.Path + ".while", "must read only persistent owner/selectedTarget seat facts; bindings, handLimit and skill queries are unavailable");
        var action = reader.RequiredEnum<SkillStateAction>("action");
        if (action == SkillStateAction.Transfer && condition is not null)
            Fail(reader.Path + ".while", "is unavailable for transfer");
        return new(Op, reader.Has("target") ? reader.RequiredEnum<SkillProgramEffectTarget>("target") : SkillProgramEffectTarget.Owner,
            0, reader.Condition(), targetReference: reader.Has("targetRef") ? reader.RequiredParticipantReference("targetRef") : null,
            resultBind: reader.OptionalIdentifier("resultBind"))
        {
            SkillState = new(action, match, until, condition,
                reader.Has("chooseOne") && reader.RequiredBool("chooseOne"))
        };
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)
    {
        var resources = (effect.TargetReference is null ? WithSelectedTarget(effect) : ParticipantResources(effect.TargetReference)).ToList();
        foreach (var node in effect.SkillState!.Match.Filter?.Nodes() ?? [])
        {
            if (node.Op.StartsWith("bound", StringComparison.Ordinal)) resources.Add(new ReadCardSet(node.Name!));
            else if (node.Op == "number" && !SkillStateMatcher.IsFilterFact(node.Name)) resources.Add(new ReadNumberBinding(node.Name!));
            var participant = node.Op switch
            {
                "selectedTarget" => ProgramParticipantRef.SelectedTarget,
                "selectedFirst" => ProgramParticipantRef.SelectedFirst,
                "selectedSecond" => ProgramParticipantRef.SelectedSecond,
                "eventSource" => ProgramParticipantRef.EventSource,
                "eventTarget" => ProgramParticipantRef.EventTarget,
                _ => (ProgramParticipantRef?)null
            };
            if (participant is { } kind) resources.AddRange(ParticipantResources(new ProgramParticipantReference(kind)));
        }
        if (effect.ResultBind is { } bind) resources.Add(new CreateNumberBinding(bind));
        return Array.AsReadOnly(resources.Distinct().ToArray());
    }

    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.Effect.SkillState?.Action == SkillStateAction.Lose;

    internal static SkillStateMatch ParseMatch(JsonElement node, string path)
    {
        ObjectShape(node, path, "skillIds", "filter");
        var ids = node.TryGetProperty("skillIds", out var idNode) ? Identifiers(idNode, path + ".skillIds") : null;
        var filter = node.TryGetProperty("filter", out var filterNode) ? ProgramExpressions.Parse(filterNode, path + ".filter") : null;
        if (filter is not null && !SkillStateMatcher.IsFilterExpression(filter))
            Fail(path + ".filter", "contains an unknown reserved skill metadata fact");
        return new(ids, filter);
    }

    private static IReadOnlyList<SkillStateBoundarySpec> ParseUntil(JsonElement node, string path)
    {
        var boundaries = new List<SkillStateBoundarySpec>();
        foreach (var item in node.EnumerateArray())
        {
            var at = $"{path}[{boundaries.Count}]";
            ObjectShape(item, at, "event", "participant", "timing", "skipIssued", "phase");
            var kind = EnumValue<SkillStateBoundaryEvent>(Required(item, "event", JsonValueKind.String, at), at + ".event");
            SkillStateSubject? subject = null;
            if (item.TryGetProperty("participant", out var participant))
            {
                ObjectShape(participant, at + ".participant", "kind");
                subject = EnumValue<SkillStateSubject>(Required(participant, "kind", JsonValueKind.String, at + ".participant"), at + ".participant.kind");
            }
            if ((kind == SkillStateBoundaryEvent.RoundAdvanced) == (subject is not null))
                Fail(at + ".participant", kind == SkillStateBoundaryEvent.RoundAdvanced ? "is unavailable for a global round boundary" : "is required for a participant boundary");
            var timing = item.TryGetProperty("timing", out var timingNode)
                ? EnumValue<SkillStateBoundaryTiming>(timingNode, at + ".timing") : SkillStateBoundaryTiming.After;
            var skip = item.TryGetProperty("skipIssued", out var skipNode) && Bool(skipNode, at + ".skipIssued");
            var phase = item.TryGetProperty("phase", out var phaseNode)
                ? EnumValue<ActualDiscardRecoveryPhaseKind>(phaseNode, at + ".phase") : (ActualDiscardRecoveryPhaseKind?)null;
            if ((kind is SkillStateBoundaryEvent.PhaseStarted or SkillStateBoundaryEvent.PhaseEnded) != (phase is not null))
                Fail(at + ".phase", "is required exactly for phaseStarted and phaseEnded boundaries");
            boundaries.Add(new(kind, subject, timing, skip, phase));
        }
        return Array.AsReadOnly(boundaries.ToArray());
    }

    internal static bool IsPersistentCondition(ProgramExpression expression) => expression.Nodes().All(node => node.Op is
        "bool" or "int" or "and" or "or" or "not" or "<" or "<=" or ">" or ">=" or "==" or "!=" or "+" or "-" or
        "min" or "max" or "floorDiv" or "when" or "same" or "alive" or "hasHp" or "faceDown" or "chained" or "inIssuedPhase" or "inUpcomingTurnPrelude" or "hp" or "maxHp" or
        "lostHp" or "handCount" or "equipmentCount" or "all" or "any" or "count" or "owner" or "selectedTarget" or "it" or "living" or "livingOthers");

    private static IReadOnlyList<string> Identifiers(JsonElement node, string path)
    {
        if (node.ValueKind != JsonValueKind.Array || node.GetArrayLength() > 256) Fail(path, "must be an array of at most 256 identifiers");
        var ids = node.EnumerateArray().Select((item, index) =>
        {
            if (item.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(item.GetString()) || item.GetString()!.Length > 128)
                Fail($"{path}[{index}]", "must contain 1 to 128 characters");
            return item.GetString()!;
        }).ToArray();
        if (ids.Distinct(StringComparer.Ordinal).Count() != ids.Length) Fail(path, "contains duplicate identifiers");
        return Array.AsReadOnly(ids);
    }

    internal static void ObjectShape(JsonElement node, string path, params string[] names)
    {
        if (node.ValueKind != JsonValueKind.Object) Fail(path, "must be an object");
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in node.EnumerateObject())
            if (!seen.Add(property.Name)) Fail(path + "." + property.Name, "duplicate property");
            else if (!names.Contains(property.Name, StringComparer.Ordinal)) Fail(path + "." + property.Name, "unsupported property");
    }

    private static JsonElement Required(JsonElement node, string name, JsonValueKind kind, string path)
    {
        if (!node.TryGetProperty(name, out var value)) Fail(path + "." + name, "is required");
        if (value.ValueKind != kind) Fail(path + "." + name, "must be " + kind);
        return value;
    }

    private static T EnumValue<T>(JsonElement node, string path) where T : struct, Enum
    {
        if (node.ValueKind != JsonValueKind.String) Fail(path, "must be a camelCase string");
        foreach (var value in Enum.GetValues<T>())
        {
            var name = Enum.GetName(value)!;
            if (node.GetString() == char.ToLowerInvariant(name[0]) + name[1..]) return value;
        }
        throw Failure(path, "unsupported " + typeof(T).Name);
    }

    private static bool Bool(JsonElement node, string path) => node.ValueKind is JsonValueKind.True or JsonValueKind.False
        ? node.GetBoolean() : throw Failure(path, "must be a boolean");
    private static void Fail(string path, string message) => throw Failure(path, message);
    private static InvalidOperationException Failure(string path, string message) => new($"Invalid skill program at {path}: {message}.");
}

public sealed class SkillStateHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SkillState;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<ISkillStateProgramHost>().ChangeSkillState(frame, effect, seat);
}
