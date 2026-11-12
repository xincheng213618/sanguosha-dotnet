#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    internal void IssueMouHqOriginalEffect(ProgramSkillFrame f)
    {
        if (!ValidMouHq(f) || f.MouHuangQuan is not { Paid: true, Stage: MouHqStage.GiveChildren } r ||
            !TryMouHqOriginalUse(f.OwnerSeat, f.WindowContext, out var use, out var window) ||
            use.Id != r.UseId || use.Action!.ActionId != r.ActionId ||
            use.EffectEpisodes?.Episodes.Any(e => e.Issue.IssuerFrameId == f.Id) == true)
            throw new InvalidOperationException("Qiaodui requires the exact once-paid original Use after all Give children return.");
        var grant = new MouHqEffectGrant(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, r.Recipient, r.Count);
        if (!r.Outgoing)
        {
            if (use.SourceSeat == f.OwnerSeat || !use.Action.EffectiveDesignatedTargetSeats.Contains(f.OwnerSeat))
                throw new InvalidOperationException("Incoming Qiaodui lost its original other-user target identity.");
            _nativeInvalidations.IssueUse(use.Id, use.Action, new());
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectIssuedEvent(grant, false));
            foreach (var target in use.TargetSeats.Distinct())
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, target, use.CardKind, CardEffectSkipReason.SkillNullified));
            return;
        }
        if (use.SourceSeat != f.OwnerSeat || _effectEpisodes.Ordinal(use.Id) != 0)
            throw new InvalidOperationException("Outgoing Qiaodui cannot attach another card or restart an effect already in progress.");
        _effectEpisodes.Issue(use.Id, new(f.Id, f.InstructionIndex - 1, 0, r.Source, r.GameplayHash,
            new(60, Replay: CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind)
                ? NativeCardEffectEpisodeReplay.ConsumePhysicalPlacement : NativeCardEffectEpisodeReplay.Original)),
            window.TrickContinuation, window.SimpleContinuation);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectIssuedEvent(grant, true));
    }

}
