using System.Collections.ObjectModel;

namespace CardGame.Core;

// Entry owns participant enumeration, fact capture, issued candidate precedence
// and the exact frame receipt. Pending work stays on the scheduler's frame.
[ProgramRuntimeModule]
internal sealed class ProgramTurnWindowEntryRuntime(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    IMatchParticipantStateAccess participants, IProgramContentQueries content, IProgramTurnQueries turn,
    IProgramPhaseQueries phase, IProgramWindowCandidateQueries candidatesQuery, IProgramBindingFacts bindings,
    IProgramTurnWindowEntryFacts facts, IProgramTurnWindowIssuedCandidates issued, IProgramTurnWindowPhaseReceipts receipts)
{
    // Empty match-local slots need no frozen facts. Issued continuations retain
    // their authority even if the original skill instance has since disappeared.
    internal bool HasLivingProgramTriggerBindings(SkillProgramTriggerWindow window) =>
        participants.Players.Any(player => player.IsAlive &&
            (candidatesQuery.Shard(player).GetInstanceTriggers(window).Count != 0 ||
             candidatesQuery.IncludeIssuedReturns(player, window, Array.Empty<ProgramTriggerCandidate>()).Count != 0 ||
             window == SkillProgramTriggerWindow.PlayEnding && issued.PhaseHandDebt(player, window, 0).Any()));

    internal bool TryBeginTurnStartProgramWindow(CharacterState owner)
    {
        if (!HasLivingProgramTriggerBindings(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) &&
            !issued.HasUnissuedTurnStartObligation(owner.Seat)) return false;
        var ownerFacts = candidatesQuery.Capture(owner);
        var candidates = TracksForeignTurnStartPrograms
            ? CollectForeignTurnStartCandidates(owner, ownerFacts)
            : candidatesQuery.Eligible(owner, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, ownerFacts);
        if (candidates.Count == 0) return false;
        if (frames.Top is not null)
            throw new InvalidOperationException("Turn-start program bindings require a clean boundary.");
        var frame = new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), owner.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
            candidates, ProgramLifecycleContinuation.NormalTurnStart, ownerFacts)
        {
            ParticipantFacts = facts.HasHongjiCapability || content.UsesAnyLivingTurnStart
                ? new ReadOnlyDictionary<int, SkillProgramTriggerFacts>(participants.Players
                    .Where(player => player.IsAlive).ToDictionary(player => player.Seat, candidatesQuery.Capture))
                : null
        };
        frames.Push(frame);
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    // Opens once per actual turn after the round ledger advances, before other
    // turn-start contests and phase work. Extra actual turns retain the exclusion.
    internal bool TryBeginRoundStartingProgramWindow(CharacterState turnOwner)
    {
        if (facts.Winner != Winner.None || !turnOwner.IsAlive || turnOwner.Seat != turn.CurrentSeat ||
            facts.RoundNumber == 0 || facts.TurnKind == ActualTurnKind.Extra ||
            !content.HasTriggerWindow(SkillProgramTriggerWindow.RoundStarting)) return false;
        if (!HasLivingProgramTriggerBindings(SkillProgramTriggerWindow.RoundStarting)) return false;
        if (frames.Top is not null || prompts.Current is not null)
            throw new InvalidOperationException("A round-starting program window requires a clean turn boundary.");
        var living = participants.Players.Where(player => player.IsAlive).ToArray();
        var participantFacts = living.ToDictionary(player => player.Seat, candidatesQuery.Capture);
        var candidates = living.SelectMany(player =>
                candidatesQuery.Eligible(player, SkillProgramTriggerWindow.RoundStarting,
                    participantFacts[player.Seat]))
            .OrderBy(candidate => facts.RelativeTableOrder(turnOwner.Seat, candidate.OwnerSeat))
            .ThenByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0) return false;
        var frame = new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), turnOwner.Seat, SkillProgramTriggerWindow.RoundStarting,
            candidates, ProgramLifecycleContinuation.RoundProgramsTurnStart, participantFacts[turnOwner.Seat])
        { ParticipantFacts = participantFacts };
        frames.Push(frame);
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TracksForeignTurnStartPrograms => content.UsesAnyLivingTurnStart ||
        content.HasTriggerOperation(SkillProgramEffectOp.ZhuihuanRetaliate) ||
        content.HasTriggerOperation(SkillProgramEffectOp.XianlueRecord) || facts.HasHongjiCapability;

    internal IReadOnlyList<ProgramTriggerCandidate> CollectForeignTurnStartCandidates(
        CharacterState turnOwner, SkillProgramTriggerFacts turnOwnerFacts)
    {
        var living = participants.Players.Where(player => player.IsAlive).ToArray();
        var participantFacts = living.ToDictionary(player => player.Seat, player =>
            player.Seat == turnOwner.Seat ? turnOwnerFacts : candidatesQuery.Capture(player));
        return living.SelectMany(player =>
                candidatesQuery.Eligible(player, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                    participantFacts[player.Seat])
                    .Where(candidate => bindings.Trigger(candidate).TurnOwnerScope == SkillProgramTurnOwnerScope.AnyLiving ||
                        bindings.Trigger(candidate).TurnOwnerScope ==
                        (player.Seat == turnOwner.Seat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving)))
            .OrderBy(candidate => facts.RelativeTableOrder(turnOwner.Seat, candidate.OwnerSeat))
            .ThenByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
    }

    internal bool TryBeginPlayEndingProgramWindow(CharacterState owner)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Play ||
            prompts.Current is not null || frames.Top is not null) return false;
        if (!HasLivingProgramTriggerBindings(SkillProgramTriggerWindow.PlayEnding) && !issued.HasPhaseNamePredictions &&
            !issued.ZhuijiEnding(owner).Any() && !issued.QingzhongEnding(owner).Any() && !issued.WeiluEnding(owner).Any() &&
            !issued.HasOutstandingHongjiPhase(owner, SkillProgramTriggerWindow.PlayEnding) &&
            !issued.WangKuangRenxiaEnding(owner).Any() && !issued.TongduPlayEnding(owner).Any() &&
            !issued.MouCaoHongPlayEnding(owner).Any() && !issued.AnniversaryZhangXunPlayEnding(owner).Any()) return false;
        var living = participants.Players.Where(player => player.IsAlive).ToArray();
        var participantFacts = living.ToDictionary(player => player.Seat, candidatesQuery.Capture);
        var ownerFacts = participantFacts[owner.Seat];
        var candidates = living.SelectMany(player =>
                candidatesQuery.Eligible(player, SkillProgramTriggerWindow.PlayEnding,
                    participantFacts[player.Seat])
                    .Where(candidate => MatchesPlayEndingTurnOwnerScope(
                        bindings.Trigger(candidate).TurnOwnerScope, player.Seat == owner.Seat))).ToList();
        candidates = issued.IncludeHongjiPhaseCandidates(owner, SkillProgramTriggerWindow.PlayEnding, candidates).ToList();
        issued.AppendPhaseNamePredictionCandidates(candidates);
        foreach (var candidate in issued.QingzhongEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.WeiluEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.ZhuijiEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.WangKuangRenxiaEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.TongduPlayEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.MouCaoHongPlayEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        foreach (var candidate in issued.AnniversaryZhangXunPlayEnding(owner))
            if (!candidates.Contains(candidate)) candidates.Add(candidate);
        var orderedCandidates = candidates.OrderBy(candidate => facts.RelativeTableOrder(owner.Seat, candidate.OwnerSeat))
            .ThenByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (orderedCandidates.Length == 0) return false;
        var frame = new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), owner.Seat, SkillProgramTriggerWindow.PlayEnding,
            orderedCandidates, ProgramLifecycleContinuation.CompletePlayPhase, ownerFacts)
        {
            ParticipantFacts = participantFacts,
            WangKuangRenxiaEndingPhase = receipts.CaptureWangKuangRenxiaEndingPhase(owner),
            WeiluEndingPhase = receipts.CaptureWeiluEndingPhase(owner),
            QingzhongEndingPhase = receipts.CaptureQingzhongEndingPhase(owner),
            ZhuijiEndingPhase = receipts.CaptureZhuijiEndingPhase(owner),
            PhaseNamePredictionPhase = receipts.TracksPhaseNamePredictions
                ? new(turn.TurnNumber, turn.CurrentSeat, receipts.PhaseInstanceId) : null
        };
        frames.Push(frame);
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal static bool MatchesPlayEndingTurnOwnerScope(SkillProgramTurnOwnerScope scope, bool ownSeat) =>
        RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.MatchesPlayEndingTurnOwnerScope(scope, ownSeat);
}
