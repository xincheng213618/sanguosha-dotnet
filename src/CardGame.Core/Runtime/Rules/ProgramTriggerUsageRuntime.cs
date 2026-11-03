using CardGame.Core.EngineOwners;

namespace CardGame.Core;

internal interface IProgramTriggerUsageReservations : IProgramRuntimeOperations
{
    bool TryReserve(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context,
        out SkillUsageConsumedEvent? fact);
}

// Trigger admission and deferred issuance share one usage store and key policy.
// Event-scoped usage remains on its original native card-action window.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramTriggerUsageRuntime(IKernelSkillRuntimeStateAccess state,
    IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, IProgramBindingFacts definitions)
    : IProgramTriggerUsageQuery, IProgramTriggerUsageReservations
{
    public int AlivePlayerCount => participants.Players.Count(player => player.IsAlive);

    public int GetUsage(int ownerSeat, string skillId, string bindingId, SkillUsageScope scope,
        ProgramSkillWindowContext? context = null) =>
        scope != SkillUsageScope.Event ? state._skillRuntimeState.GetUsage(ownerSeat, skillId, bindingId, scope) :
        EventUsageFrame(context) is { } frame ? frame.EventUsages.SingleOrDefault(usage =>
            usage.OwnerSeat == ownerSeat && usage.SkillId == skillId && usage.BindingId == bindingId)?.Count ?? 0 : int.MaxValue;

    public bool TryConsumeUsage(int ownerSeat, string skillId, string bindingId, SkillUsageScope scope,
        int limit, ProgramSkillWindowContext? context = null)
    {
        if (scope != SkillUsageScope.Event)
            return state._skillRuntimeState.TryConsumeUsage(ownerSeat, skillId, bindingId, scope, limit);
        if (EventUsageFrame(context) is not { } frame) return false;
        var count = GetUsage(ownerSeat, skillId, bindingId, scope, context);
        if (count >= limit) return false;
        frames.Replace(frame.Id, frame with
        {
            EventUsages = frame.EventUsages.Where(usage =>
                usage.OwnerSeat != ownerSeat || usage.SkillId != skillId || usage.BindingId != bindingId)
                .Append(new(ownerSeat, skillId, bindingId, count + 1)).ToArray()
        });
        return true;
    }

    private ProgramCardTriggerWindowFrame? EventUsageFrame(ProgramSkillWindowContext? context) =>
        context?.CardUse is not { } action ? null : frames.OfType<ProgramCardTriggerWindowFrame>()
            .SingleOrDefault(frame => frame.Id == context.ParentFrameId && frame.Action.ActionId == action.CardActionId &&
                EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(frame) == context.Window);

    public void RecordUsageConsumed(SkillUsageConsumedEvent fact) => events.AdvanceFact(fact);

    public bool TryReserve(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context,
        out SkillUsageConsumedEvent? fact)
    {
        fact = null;
        var trigger = definitions.Trigger(candidate);
        if (trigger.UsageScope == SkillUsageScope.Event) return false;
        if (trigger.UsageScope is not { } scope ||
            ProgramTriggerUsageRules.Limit(trigger, AlivePlayerCount) is not { } limit)
            return true;
        if (limit <= 0 || !TryConsumeUsage(candidate.OwnerSeat, candidate.SkillId,
            ProgramTriggerUsageRules.Id(trigger, candidate), scope, limit, context))
            return false;
        fact = new(candidate.OwnerSeat, candidate.SkillId, candidate.BindingId, scope, 1);
        return true;
    }
}

internal static class ProgramTriggerUsageRules
{
    internal static string Id(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate)
    {
        if (trigger.UsageScope == SkillUsageScope.Event) return candidate.BindingId;
        if (trigger.NamedUsageGroup is { } named) return $"named-group:{named}";
        if (trigger.DynamicUsageLimit is not null) return $"named-binding:{candidate.BindingId}";
        var binding = trigger.ChoiceGroup is { } group ? $"choice-group:{group}" : candidate.BindingId;
        return $"{binding}@{candidate.SkillInstanceId}";
    }

    internal static int? Limit(SkillProgramTrigger trigger, int alivePlayerCount) =>
        trigger.DynamicUsageLimit is { Kind: SkillProgramDynamicUsageLimitKind.AlivePlayersCapped } dynamic
            ? Math.Min(dynamic.Cap, alivePlayerCount) : trigger.UsageLimit;
}
