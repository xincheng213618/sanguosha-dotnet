#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    internal IEnumerable<MoLuBuPairChangedEvent> MoLuBuPairs() => _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuPairChangedEvent>().GroupBy(e => e.Source.OwnerSeat).Select(g => g.Last());
    internal MoLuBuPairChangedEvent? MoLuBuDeathPair(int beneficiary, DeathFrame death) => MoLuBuPairs().FirstOrDefault(p => p.Source.OwnerSeat == beneficiary && p.OpponentSeat == death.VictimSeat || p.Source.OwnerSeat == death.VictimSeat && p.OpponentSeat == beneficiary && death.KillerSeat == beneficiary);
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMoLuBuKuangMarker(int owner, IReadOnlyList<PlayerMarkerSnapshot>? ordinary)
    {
        var count = MoLuBuPairs().Count(p => _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[p.OpponentSeat].IsAlive && (p.Source.OwnerSeat == owner || p.OpponentSeat == owner));
        return count == 0 ? ordinary : Array.AsReadOnly((ordinary ?? []).Where(m => m.Kind != PlayerMarkerKind.OlMoKuang).Append(new PlayerMarkerSnapshot(PlayerMarkerKind.OlMoKuang, "狂", count)).ToArray());
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 70)]
    internal int MoLuBuKuangDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!HasMoLuBu || attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => !e.SourceLess && e.SourceSeat == attack.SourceSeat && e.TargetSeat == attack.TargetSeat))
            return 0;
        return MoLuBuPairs().Count(p => p.Source.OwnerSeat == attack.SourceSeat && p.OpponentSeat == attack.TargetSeat || p.Source.OwnerSeat == attack.TargetSeat && p.OpponentSeat == attack.SourceSeat);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 30800, SkillProgramEffectOp.EnterMoLuBuKuangmo)]
    internal bool CanEnterMoKuangmo(ProgramActivationLegalityContext g) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == g.OwnerSeat && g.Owner.IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuKuangmoEnteredEvent>().Any(e => e.Source.OwnerSeat == g.OwnerSeat);
    internal SkillProgramStepOutcome BeginMoKuangmo(ProgramSkillFrame f)
    {
        if (f.SkillId != MoLuBuComposition.Kuangmo || f.ActivationId != "enter-and-pair" || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuKuangmoEnteredEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("Kuangmo needs its unpaid actual Play and living selected opponent.");
        var r = new MoLuBuReceipt(MoLuBuSource(f), f.GameplayHash, SkillProgramEffectOp.EnterMoLuBuKuangmo, 0, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, MoLuBuStage.EntryChildren);
        StartMoLuBu(f, r);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuKuangmoEnteredEvent(f.Id, r.Source, r.GameplayHash, phase, target));
        SetMoLuBuPair(f, target, true);
        _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueMoLuBu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void SetMoLuBuPair(ProgramSkillFrame f, int target, bool entry)
    {
        var r = f.MoLuBu ?? throw new InvalidOperationException("Kuangmo lost its original source.");
        if (r.Source.OwnerSeat != f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == f.OwnerSeat)
            throw new InvalidOperationException("A reciprocal pair requires its living issued owner and distinct opponent.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuPairChangedEvent(f.Id, r.Source, r.GameplayHash, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, entry));
    }

    internal bool IsPaidMoKuangmoOrigin(long frameId, CardConversionSource source, string hash) => source.SkillId == MoLuBuComposition.Kuangmo && source.BindingId == "enter-and-pair" && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program?.GameplayHash == hash && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuKuangmoEnteredEvent>().Where(e => e.FrameId == frameId).ToArray()is [var entered] && entered.Source == source && entered.GameplayHash == hash && entered.Phase.ActorSeat == source.OwnerSeat && entered.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(entered.Phase) && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuPairChangedEvent>().Count(e => e.FrameId == frameId && e.Entry && e.Source == source && e.GameplayHash == hash && e.OpponentSeat == entered.OpponentSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frameId && e.SkillId == source.SkillId && e.ActivationId == source.BindingId && e.OwnerSeat == source.OwnerSeat) == 1;
    internal bool IsPaidMoKuangmoEntry(ProgramSkillFrame f) => f.MoLuBu is { Operation: SkillProgramEffectOp.EnterMoLuBuKuangmo, Stage: MoLuBuStage.EntryChildren } r && r.Source == MoLuBuSource(f) && IsPaidMoKuangmoOrigin(f.Id, r.Source, f.GameplayHash);
    internal bool IsMoLuBuRumoOrigin(OlMoRumoEnteredEvent entry) => entry.Origin.SkillId == MoLuBuComposition.Kuangmo && IsPaidMoKuangmoOrigin(entry.EntryId, entry.Origin, entry.OriginGameplayHash) && (entry.EntryRound >= 1 || entry.EntryRound == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuOpeningStartedEvent>().Any(opening => opening.Source.OwnerSeat == entry.OwnerSeat && opening.ActualTurn == entry.ActualTurn && entry.TurnOwner == entry.OwnerSeat));
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 2)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMoLuBuDeathCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (!HasMoLuBu || window != SkillProgramTriggerWindow.CharacterDied)
            return ordinary;
        var remaining = ordinary.Where(c => c.SkillId != MoLuBuComposition.Kuangmo || c.BindingId != "pair-death-and-qi").ToList();
        if (!owner.IsAlive || _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault()is not { } death)
            return Array.AsReadOnly(remaining.ToArray());
        foreach (var pair in MoLuBuPairs().Where(p => p.Source.OwnerSeat == owner.Seat && p.OpponentSeat == death.VictimSeat || p.Source.OwnerSeat == death.VictimSeat && p.OpponentSeat == owner.Seat && death.KillerSeat == owner.Seat))
            remaining.Add(new(owner.Seat, MoLuBuComposition.Kuangmo, "pair-death-and-qi", pair.Source.SkillInstanceId, pair.GameplayHash, 0, pair.Source.OwnerSeat + 1));
        return Array.AsReadOnly(remaining.ToArray());
    }

    [ProgramIssuedTrigger(130)]
    internal bool IsIssuedMoLuBuDeathCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (!HasMoLuBu || candidate.SkillId != MoLuBuComposition.Kuangmo || candidate.BindingId != "pair-death-and-qi" || c.Window != SkillProgramTriggerWindow.CharacterDied || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramKillTriggerWindowFrame window || _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == window.DeathFrameId)is not { } death || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        return MoLuBuDeathPair(candidate.OwnerSeat, death)is { } pair && pair.Source.SkillInstanceId == candidate.SkillInstanceId && pair.GameplayHash == candidate.GameplayHash && candidate.OccurrenceIndex == pair.Source.OwnerSeat + 1 && c.TargetSeat == death.VictimSeat && c.SourceSeat == death.KillerSeat;
    }

    internal SkillProgramStepOutcome BeginMoKuangmoDeath(ProgramSkillFrame f)
    {
        var c = f.WindowContext ?? throw new InvalidOperationException("Kuangmo death needs its exact native death.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex);
        if (!IsIssuedMoLuBuDeathCandidate(candidate, c))
            throw new InvalidOperationException("Kuangmo lost native killer attribution or its issued reciprocal pair.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single(w => w.Id == c.ParentFrameId);
        var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(d => d.Id == window.DeathFrameId);
        var pair = MoLuBuDeathPair(f.OwnerSeat, death)!;
        var r = new MoLuBuReceipt(pair.Source, pair.GameplayHash, SkillProgramEffectOp.ResolveMoLuBuKuangmoDeath, c.ParentFrameId, death.VictimSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, MoLuBuStage.Claiming, Reassign: pair.Source.OwnerSeat == f.OwnerSeat)
        {
            Candidates = death.KillerSeat == f.OwnerSeat ? MoLuBuQiCandidates(death.VictimSeat) : []
        };
        StartMoLuBu(f, r);
        ContinueMoLuBu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
