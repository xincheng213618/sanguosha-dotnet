using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SkillStateAction { Suppress, Lose, Transfer }
public enum SkillStateScope { All, NonLocked, Named, ChosenFromTarget, Expression }
public enum SkillStateDuration { CurrentTurn, UntilTurnEndOf, Permanent, CurrentRound }
public enum SkillStateEndTiming { AfterTurnEnded, BeforeTurnEnded }

public sealed record SkillStateSpec(SkillStateAction Action, SkillStateScope Scope, SkillStateDuration Duration)
{
    public ProgramExpression? Filter { get; init; }
    public ProgramParticipantReference? UntilTurnEndOf { get; init; }
    public bool PrintedOnly { get; init; }
    public bool CharacterOnly { get; init; } = true;
    public bool ExcludeLocked { get; init; }
    public bool ExpireOnDeath { get; init; }
    public bool AfterCurrentTurn { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SkillStateEndTiming EndTiming { get; init; }
}

// These entries live in CharacterSkillSet, alongside grants. An entry with
// IsEnabled=false describes a restriction, rather than another owned skill.
public sealed record SkillStateLease(SkillStateAction Action, SkillStateScope Scope,
    SkillStateDuration Duration, int IssuedTurn, int EndSeat, string? NamedSkill = null,
    bool PrintedOnly = false, bool ExcludeLocked = false, bool ExpireOnDeath = false,
    bool Acquisition = false, bool CharacterOnly = true, bool AfterCurrentTurn = false)
{
    public SkillStateOrigin? Origin { get; init; }
    public int? DeathSeat { get; init; }
    public string? TargetGrantId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IssuedRound { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SkillStateEndTiming EndTiming { get; init; }
}
public sealed record SkillStateOrigin(long FrameId, int EffectIndex, CardUseEffectSource Source,
    long? CardUseFrameId = null, long? CardActionId = null);

public sealed partial record SkillGrant
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkillStateLease? SkillState { get; init; }
}

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkillStateSpec? SkillState { get; internal init; }
}

public sealed record ProgramSkillStateChoice(int Seat, IReadOnlyList<string> Skills);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramSkillStateChoice? SkillStateChoice { get; init; }
}

public sealed record SkillStateChangedEvent(int Seat, int SourceSeat, string SkillId,
    SkillStateAction Action, string SourceId) : IGameEvent
{
    public SkillStateOrigin? Origin { get; init; }
    public int IssuedTurn { get; init; }
}
public sealed record SkillStateExpiredEvent(int Seat, string SourceId, SkillStateOrigin? Origin) : IGameEvent;

internal interface ISkillStateProgramHost
{
    SkillProgramStepOutcome ChangeSkillState(ProgramSkillFrame frame, SkillProgramEffect effect, int targetSeat);
}

internal sealed class SkillStateDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SkillState;
    public override ISkillProgramEffectHandler Handler { get; } = new SkillStateHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "targetRef", "action", "scope", "duration", "untilTurnEndOf", "skillIds",
            "filter", "printedOnly", "characterOnly", "excludeLocked", "expireOnDeath", "afterCurrentTurn", "endTiming", "resultBind", "condition");
        var action = r.RequiredEnum<SkillStateAction>("action");
        var scope = r.RequiredEnum<SkillStateScope>("scope");
        var duration = r.RequiredEnum<SkillStateDuration>("duration");
        if ((scope == SkillStateScope.Expression) != r.Has("filter") ||
            (duration == SkillStateDuration.UntilTurnEndOf) != r.Has("untilTurnEndOf") ||
            (scope == SkillStateScope.Named) != r.Has("skillIds"))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}: scope and duration require their matching operands.");
        var ids = r.Has("skillIds") ? r.RequiredIdentifierArray("skillIds") : [];
        if (scope == SkillStateScope.Named && ids.Count == 0)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.skillIds: must not be empty.");
        if (r.Has("afterCurrentTurn") && duration != SkillStateDuration.UntilTurnEndOf)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.afterCurrentTurn: requires untilTurnEndOf.");
        if (r.Has("endTiming") && duration != SkillStateDuration.UntilTurnEndOf)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.endTiming: requires untilTurnEndOf.");
        return new(Op, r.RequiredEnum<SkillProgramEffectTarget>("target"), 0, r.Condition(),
            targetReference: r.Has("targetRef") ? r.RequiredParticipantReference("targetRef") : null,
            skillIds: ids, resultBind: r.OptionalIdentifier("resultBind"))
        {
            SkillState = new(action, scope, duration)
            {
                Filter = r.Has("filter") ? r.RequiredBoolExpression("filter") : null,
                UntilTurnEndOf = r.Has("untilTurnEndOf") ? r.RequiredParticipantReference("untilTurnEndOf") : null,
                PrintedOnly = r.Has("printedOnly") && r.RequiredBool("printedOnly"),
                CharacterOnly = !r.Has("characterOnly") || r.RequiredBool("characterOnly"),
                ExcludeLocked = r.Has("excludeLocked") && r.RequiredBool("excludeLocked"),
                ExpireOnDeath = r.Has("expireOnDeath") && r.RequiredBool("expireOnDeath"),
                AfterCurrentTurn = r.Has("afterCurrentTurn") && r.RequiredBool("afterCurrentTurn"),
                EndTiming = r.Has("endTiming") ? r.RequiredEnum<SkillStateEndTiming>("endTiming") : SkillStateEndTiming.AfterTurnEnded
            }
        };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        WithSelectedTarget(e).Concat(ParticipantResources(e.TargetReference, e.SkillState!.UntilTurnEndOf))
            .Concat((e.SkillState.Filter?.Nodes() ?? []).SelectMany(node => node.Op switch
            {
                "number" when !SkillStateQuery.IsFilterFact(node.Name!) => new ProgramResourceOperation[] { new ReadNumberBinding(node.Name!) },
                var op when op.StartsWith("bound", StringComparison.Ordinal) => [new ReadCardSet(node.Name!)],
                "selectedTarget" => [new ReadSelectedTarget()],
                "selectedFirst" => [new ReadTargetSet(1)],
                "selectedSecond" => [new ReadTargetSet(2)],
                _ => Array.Empty<ProgramResourceOperation>()
            }))
            .Concat(e.ResultBind is { } bind ? [new CreateNumberBinding(bind)] : Array.Empty<ProgramResourceOperation>()).ToArray();
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        context.Effect.SkillState!.Action == SkillStateAction.Lose;
}

public sealed class SkillStateHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SkillState;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<ISkillStateProgramHost>().ChangeSkillState(f, e, seat);
}

internal static class SkillStateQuery
{
    internal static bool IsPrinted(SkillGrant g) => g.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource;
    internal static bool IsCharacter(SkillGrant g) => !g.IsModeRule && !g.SourceId.StartsWith("equipment:", StringComparison.Ordinal);
    internal static bool IsFilterFact(string name) => name is
        "skill.locked" or "skill.limited" or "skill.awakening" or "skill.lord" or "skill.printed" or "skill.ordinal";
}
