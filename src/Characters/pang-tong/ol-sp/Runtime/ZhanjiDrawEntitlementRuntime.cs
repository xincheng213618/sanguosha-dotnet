namespace CardGame.Core;

internal interface IZhanjiDrawEntitlements : IProgramRuntimeOperations
{
    IReadOnlyList<ZhanjiDrawEntitlement>? Capture(long invocationId, int owner, long before);
    bool IsValid(NativeDrawInvocationProof proof);
}

// Entitlements freeze active sources before the physical draw. Their original
// journal facts retain authority after a source changes or loses its skill.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ZhanjiDrawEntitlementRuntime(IProgramContentQueries content,
    IProgramWindowCandidateQueries bindings, IMatchParticipantStateAccess participants,
    IProgramRuntimeEvents events, IProgramEventJournalQueries journal) : IZhanjiDrawEntitlements
{
    public IReadOnlyList<ZhanjiDrawEntitlement>? Capture(long invocationId, int owner, long before)
    {
        if (!content.HasTriggerOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation)) return null;
        var sources = bindings.Shard(participants.At(owner)).GetInstanceTriggers(SkillProgramTriggerWindow.CardsGained)
            .Where(binding => ProgramInstructionResolver.Default.Features(binding.Trigger)
                .HasOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation))
            .Select(binding => new ZhanjiDrawEntitlement(
                new(binding.SkillId, binding.Trigger.Id, owner, binding.SkillInstanceId), binding.Program.GameplayHash))
            .Distinct().OrderBy(entitlement => entitlement.Source.SkillId, StringComparer.Ordinal)
            .ThenBy(entitlement => entitlement.Source.BindingId, StringComparer.Ordinal)
            .ThenBy(entitlement => entitlement.Source.SkillInstanceId, StringComparer.Ordinal).ToArray();
        events.AdvanceFact(new ZhanjiDrawEntitlementsCapturedEvent(invocationId, owner, before, sources.Length));
        foreach (var source in sources)
            events.AdvanceFact(new ZhanjiDrawSourceCapturedEvent(invocationId, owner, before, source));
        return Array.AsReadOnly(sources);
    }

    public bool IsValid(NativeDrawInvocationProof proof)
    {
        if (proof.ZhanjiEntitlements is not { } sources)
            return !content.HasTriggerOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation);
        var summary = journal.Complete().OfType<ZhanjiDrawEntitlementsCapturedEvent>()
            .Where(item => item.InvocationId == proof.InvocationId).ToArray();
        if (summary is not [var captured] || captured.OwnerSeat != proof.OwnerSeat ||
            captured.SequenceBefore != proof.SequenceBefore || captured.Count != sources.Count ||
            sources.Distinct().Count() != sources.Count) return false;
        var facts = journal.Complete().OfType<ZhanjiDrawSourceCapturedEvent>()
            .Where(item => item.InvocationId == proof.InvocationId).ToArray();
        return facts.Length == sources.Count &&
            facts.All(item => item.OwnerSeat == proof.OwnerSeat && item.SequenceBefore == proof.SequenceBefore) &&
            sources.All(source => source.Source.OwnerSeat == proof.OwnerSeat &&
                content.Skill(source.Source.SkillId).Program is { } program && program.GameplayHash == source.GameplayHash &&
                program.Triggers.Any(trigger => trigger.Id == source.Source.BindingId && ProgramInstructionResolver.Default.Features(trigger)
                    .HasOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation)) &&
                facts.Count(item => item.Entitlement == source) == 1);
    }

    internal static bool HasEntitlement(NativeDrawInvocationProof proof, int owner, string skill,
        string binding, string instance, string hash) =>
        proof.ZhanjiEntitlements?.Contains(new(new(skill, binding, owner, instance), hash)) == true;
}
