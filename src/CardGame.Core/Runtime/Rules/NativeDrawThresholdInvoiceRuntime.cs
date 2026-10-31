namespace CardGame.Core;

internal interface INativeDrawThresholdInvoices : IProgramRuntimeOperations
{
    bool Tracks { get; }
    IReadOnlyList<NativeDrawThresholdEntitlement>? Capture(long invocationId, int ownerSeat, long sequenceBefore);
    IReadOnlyList<NativeDrawThresholdProgress>? Freeze(NativeDrawInvocationDraft draft, int actualCount);
    void Complete(NativeDrawInvocationProof proof);
    bool IsValid(NativeDrawInvocationProof proof);
}

// Source entitlements and cumulative counters come from the original journal.
// This module is independent of draw invoice validation to keep composition acyclic.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeDrawThresholdInvoiceRuntime(IProgramContentQueries content,
    IProgramEventJournalQueries journal, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, IProgramWindowCandidateQueries candidates,
    INativeDrawProducerQueries draws) : INativeDrawThresholdInvoices
{
    public bool Tracks => content
        .HasTriggerOperation(SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold);
    internal static bool UsesNativeDrawThreshold(SkillProgramTrigger trigger) =>
        trigger.Effects is [{ Op: SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold }];

    public IReadOnlyList<NativeDrawThresholdEntitlement>? Capture(
        long invocationId, int ownerSeat, long sequenceBefore)
    {
        if (!Tracks) return null;
        var history = journal.Complete().OfType<NativeDrawThresholdProgressedEvent>().ToArray();
        var owner = participants.At(ownerSeat);
        var sources = owner.IsAlive ? candidates.Shard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardsGained)
            .Where(binding => UsesNativeDrawThreshold(binding.Trigger) && draws.HasSkillInstance(owner.Seat, binding.SkillId, binding.SkillInstanceId))
            .Select(binding =>
            {
                var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, ownerSeat, binding.SkillInstanceId);
                var threshold = binding.Trigger.Effects[0].MinimumValue;
                var prior = history.LastOrDefault(item => item.Source == source && item.GameplayHash == binding.Program.GameplayHash && item.Threshold == threshold);
                return new NativeDrawThresholdEntitlement(source, binding.Program.GameplayHash, threshold, prior?.RemainderAfter ?? 0);
            }).Distinct().OrderBy(item => item.Source.SkillId, StringComparer.Ordinal)
                .ThenBy(item => item.Source.BindingId, StringComparer.Ordinal)
                .ThenBy(item => item.Source.SkillInstanceId, StringComparer.Ordinal).ToArray() : [];
        foreach (var entitlement in sources)
            events.AdvanceFact(new NativeDrawThresholdSourceCapturedEvent(invocationId, ownerSeat, sequenceBefore, entitlement));
        return Array.AsReadOnly(sources);
    }

    public IReadOnlyList<NativeDrawThresholdProgress>? Freeze(
        NativeDrawInvocationDraft draft, int actualCount) => draft.ThresholdEntitlements is not { } entitlements ? null :
        Array.AsReadOnly(entitlements.Select(item =>
        {
            var total = checked((long)item.RemainderBefore + actualCount);
            return new NativeDrawThresholdProgress(item.Source, item.GameplayHash, item.Threshold,
                item.RemainderBefore, actualCount, checked((int)(total / item.Threshold)), checked((int)(total % item.Threshold)));
        }).ToArray());

    public void Complete(NativeDrawInvocationProof proof)
    {
        if (proof.ThresholdProgress is not { } progress) return;
        foreach (var item in progress)
            events.AdvanceFact(new NativeDrawThresholdProgressedEvent(proof.InvocationId, proof.OwnerSeat,
                item.Source, item.GameplayHash, item.Threshold, item.RemainderBefore, item.ActualCount, item.Crossings, item.RemainderAfter));
    }

    public bool IsValid(NativeDrawInvocationProof proof)
    {
        if (proof.ThresholdProgress is not { } progress) return !Tracks;
        if (progress is not System.Collections.IList { IsReadOnly: true } ||
            progress.Select(item => item.Source).Distinct().Count() != progress.Count) return false;
        var history = journal.Complete().ToArray();
        if (history.OfType<NativeDrawThresholdSourceCapturedEvent>().Count(item => item.InvocationId == proof.InvocationId) != progress.Count ||
            history.OfType<NativeDrawThresholdProgressedEvent>().Count(item => item.InvocationId == proof.InvocationId) != progress.Count) return false;
        foreach (var item in progress)
        {
            if (item.Source.OwnerSeat != proof.OwnerSeat || item.Threshold <= 0 ||
                item.RemainderBefore < 0 || item.RemainderBefore >= item.Threshold || item.ActualCount != proof.ActualCount ||
                item.Crossings != ((long)item.RemainderBefore + item.ActualCount) / item.Threshold ||
                item.RemainderAfter != ((long)item.RemainderBefore + item.ActualCount) % item.Threshold) return false;
            var captured = history.Select((fact, index) => (fact, index)).Where(pair => pair.fact is NativeDrawThresholdSourceCapturedEvent e &&
                e.InvocationId == proof.InvocationId && e.OwnerSeat == proof.OwnerSeat && e.SequenceBefore == proof.SequenceBefore &&
                e.Entitlement == new NativeDrawThresholdEntitlement(item.Source, item.GameplayHash, item.Threshold, item.RemainderBefore)).ToArray();
            var recorded = history.OfType<NativeDrawThresholdProgressedEvent>().Where(e => e.InvocationId == proof.InvocationId && e.Source == item.Source).ToArray();
            if (captured is not [var capture] || recorded is not [var result] || result.GameplayHash != item.GameplayHash ||
                result.Threshold != item.Threshold || result.RemainderBefore != item.RemainderBefore || result.ActualCount != item.ActualCount ||
                result.Crossings != item.Crossings || result.RemainderAfter != item.RemainderAfter) return false;
            var prior = history.Take(capture.index).OfType<NativeDrawThresholdProgressedEvent>().LastOrDefault(e =>
                e.Source == item.Source && e.GameplayHash == item.GameplayHash && e.Threshold == item.Threshold);
            if ((prior?.RemainderAfter ?? 0) != item.RemainderBefore) return false;
        }
        return true;
    }

}
