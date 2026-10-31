using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_NativeDrawOrdinals;

namespace CardGame.Core;

// Issuance and validation retain each invoice's complete original event prefix.
// This service never depends on the invoice that consumes its ordinal proof.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeDrawOrdinalRuntime(IProgramContentQueries content, IProgramEventJournalQueries journal,
    IMatchParticipantStateAccess participants, IProgramWindowCandidateQueries bindings, IProgramTurnQueries turn,
    IProgramRuntimeEvents events, INativeDrawOrdinalSkillInstances instances) : INativeDrawOrdinalInvoices
{
    private bool TracksNativeDrawOrdinals => content
        .UsesTriggerCondition(SkillProgramTriggerConditionKind.NativeDrawActualTurnOrdinalIs);

    public IReadOnlyList<NativeDrawOrdinalEntitlement>? Capture(long invocationId, int ownerSeat, long before)
    {
        if (!TracksNativeDrawOrdinals) return null;
        var history = journal.Complete().ToArray();
        if (NativeDrawOrdinalAt(history, history.Length, ownerSeat, turn.TurnNumber, turn.TurnOwnerSeat) is null)
            return null;
        var owner = participants.At(ownerSeat);
        var sources = owner.IsAlive
            ? bindings.Shard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardsGained)
                .Where(binding => UsesNativeDrawOrdinal(binding.Trigger) &&
                    instances.HasRuntimeSkillInstance(owner, binding.SkillId, binding.SkillInstanceId))
                .Select(binding => new NativeDrawOrdinalEntitlement(
                    new(binding.SkillId, binding.Trigger.Id, ownerSeat, binding.SkillInstanceId), binding.Program.GameplayHash))
                .Distinct().OrderBy(source => source.Source.SkillId, StringComparer.Ordinal)
                .ThenBy(source => source.Source.BindingId, StringComparer.Ordinal)
                .ThenBy(source => source.Source.SkillInstanceId, StringComparer.Ordinal).ToArray()
            : [];
        events.AdvanceFact(new NativeDrawOrdinalEntitlementsCapturedEvent(invocationId, ownerSeat,
            turn.TurnNumber, turn.TurnOwnerSeat, before, sources.Length));
        foreach (var source in sources)
            events.AdvanceFact(new NativeDrawOrdinalSourceCapturedEvent(invocationId, ownerSeat, before, source));
        return Array.AsReadOnly(sources);
    }

    public int? Freeze(NativeDrawInvocationDraft draft)
    {
        if (draft.OrdinalEntitlements is null) return null;
        var history = journal.Complete().ToArray();
        return NativeDrawOrdinalAt(history, history.Length, draft.Owner, draft.Turn, draft.TurnOwner) ??
            throw new InvalidOperationException("A native draw ended outside its captured actual turn.");
    }

    public bool IsValid(NativeDrawInvocationProof proof)
    {
        if (!TracksNativeDrawOrdinals)
            return proof.ActualTurnDrawOrdinal is null && proof.OrdinalEntitlements is null;
        var history = journal.Complete().ToArray();
        var invoiceIndexes = history.Select((fact, index) => (fact, index))
            .Where(item => item.fact is NativeDrawInvocationRecordedEvent draw && draw.InvocationId == proof.InvocationId).ToArray();
        if (invoiceIndexes is not [var entry]) return false;
        var fact = (NativeDrawInvocationRecordedEvent)entry.fact;
        var expected = NativeDrawOrdinalAt(history, entry.index, proof.OwnerSeat, proof.ActualTurnNumber, proof.ActualTurnOwnerSeat);
        var summaries = history.Select((payload, index) => (payload, index))
            .Where(item => item.payload is NativeDrawOrdinalEntitlementsCapturedEvent summaryFact && summaryFact.InvocationId == proof.InvocationId).ToArray();
        var sources = history.Select((payload, index) => (payload, index))
            .Where(item => item.payload is NativeDrawOrdinalSourceCapturedEvent sourceFact && sourceFact.InvocationId == proof.InvocationId).ToArray();
        if (expected is null)
            return proof.ActualTurnDrawOrdinal is null && fact.ActualTurnDrawOrdinal is null && proof.OrdinalEntitlements is null &&
                summaries.Length == 0 && sources.Length == 0;
        if (proof.ActualTurnDrawOrdinal != expected || fact.ActualTurnDrawOrdinal != expected ||
            proof.OrdinalEntitlements is not { } entitlements || entitlements is not System.Collections.IList { IsReadOnly: true } ||
            summaries is not [var summary] || summary.index >= entry.index ||
            summary.payload is not NativeDrawOrdinalEntitlementsCapturedEvent captured ||
            captured.OwnerSeat != proof.OwnerSeat || captured.ActualTurnNumber != proof.ActualTurnNumber ||
            captured.ActualTurnOwnerSeat != proof.ActualTurnOwnerSeat || captured.SequenceBefore != proof.SequenceBefore ||
            captured.Count != entitlements.Count || sources.Length != entitlements.Count || entitlements.Distinct().Count() != entitlements.Count)
            return false;
        foreach (var entitlement in entitlements)
        {
            if (entitlement is null || entitlement.Source is not { } source || source.OwnerSeat != proof.OwnerSeat ||
                content.FindSkill(source.SkillId)?.Program is not { } program ||
                program.GameplayHash != entitlement.GameplayHash ||
                program.Triggers.SingleOrDefault(binding => binding.Id == source.BindingId) is not { } trigger || !UsesNativeDrawOrdinal(trigger) ||
                sources.Count(item => item.index > summary.index && item.index < entry.index &&
                    item.payload is NativeDrawOrdinalSourceCapturedEvent issued && issued.OwnerSeat == proof.OwnerSeat &&
                    issued.SequenceBefore == proof.SequenceBefore && issued.Entitlement == entitlement) != 1)
                return false;
        }
        return true;
    }
}
