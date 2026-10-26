using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason ShanheTeamRankDrawReason = new("shanhe.team-rank.draw");

    private bool IsActualShanheAlly(int seat) => _modeDefinition.Id == ShanheBattleSetup.BattleModeId &&
        _options.ShanheBattle is { } setup && seat > 0 && seat <= setup.AllyRoster.Count &&
        _players[seat].IsAlive && OtherRevealedPublicTeamAlly(_players[0], _players[seat]);

    private int ShanheSharedRankRuleBonus(int seat, SkillRuleQuery query)
    {
        if (!IsActualShanheAlly(seat) || !HasShanheSchoolEffect(0, 6)) return 0;
        var bonus = ShanheGeneralRanks.GetBonuses(ShanheTeamRankPrograms.EffectiveRank(_options.ShanheBattle!));
        return query switch { SkillRuleQuery.DrawCount => bonus.Draw, SkillRuleQuery.SlashLimit => bonus.Slash, _ => 0 };
    }

    private bool ShanheExpeditionRebirthSpent => _options.ShanheBattle is { } setup &&
        ProgramEventHistory<SkillUsageConsumedEvent>().Any(e => ShanheTeamRankPrograms.IsExpeditionRebirthUsage(setup, e));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62025)]
    private bool AllowsShanheSharedRebirth(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == ShanheTeamRankPrograms.SharedRebirthSkillId)
            return IsActualShanheAlly(gate.OwnerSeat) && HasShanheSchoolEffect(0, 6) &&
                _options.ShanheBattle is { RebirthAvailable: true, PlayerRank: 5 } && !ShanheExpeditionRebirthSpent;
        return gate.SkillId != ShanheGeneralRanks.RebirthSkillId ||
            _options.ShanheBattle is null || !ShanheExpeditionRebirthSpent;
    }

    private bool HasShanheTeamHealthTactic => _modeDefinition.Id == ShanheBattleSetup.BattleModeId &&
        _options.ShanheBattle is { } setup && _players[0].IsAlive &&
        EnabledContentSkillIds(_players[0]).Contains(ShanheTeamRankPrograms.TeamHealthSkillId, StringComparer.Ordinal) &&
        (_shanheMarket is { } market ? market.Snapshot.TacticIds.Any(id => setup.MarketSetup!.Definition.Rewards
            .Any(r => r.Id == id && r.TacticSkillId == ShanheTeamRankPrograms.TeamHealthSkillId)) :
            setup.TacticSkillIds.Contains(ShanheTeamRankPrograms.TeamHealthSkillId, StringComparer.Ordinal));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62026, SkillProgramEffectOp.ApplyShanheAlliedHealthBonus)]
    private bool AllowsShanheTeamHealth(ProgramTriggerCandidate candidate) =>
        candidate.OwnerSeat == 0 && candidate.SkillId == ShanheTeamRankPrograms.TeamHealthSkillId && HasShanheTeamHealthTactic;

    private ProgramSkillFrame RequireShanheTeamOpening(ProgramSkillFrame supplied, string skillId, string binding, bool requireActive = true)
    {
        var frame = requireActive ? GetActiveProgramFrame(supplied.Id) :
            _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == supplied.Id) ??
                throw new InvalidOperationException("A team privilege lost its original ancestor frame.");
        if (frame.SkillId != skillId || frame.OwnerSeat != 0 || frame.TriggerId != binding || frame.InstructionIndex != 1 ||
            _modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context ||
            _resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId) is not ProgramLifecycleTriggerWindowFrame
                { Window: SkillProgramTriggerWindow.GameStarting, OwnerSeat: 0 } parent ||
            parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("A team privilege requires its exact original Shanhe opening instruction.");
        return frame;
    }

    [ProgramCapability]
    private sealed class ShanheSchoolTeamCapability(GameEngine engine) : IShanheSchoolTeamHost
    {
        public SkillProgramStepOutcome ApplyTeamRank(ProgramSkillFrame frame) => engine.BeginShanheSchoolTeamRank(frame);
        public void ApplyAlliedHealth(ProgramSkillFrame frame) => engine.ApplyShanheAlliedHealth(frame);
    }

    private void ApplyShanheAlliedHealth(ProgramSkillFrame supplied)
    {
        var frame = RequireShanheTeamOpening(supplied, ShanheTeamRankPrograms.TeamHealthSkillId, "allied-health-privilege");
        if (!HasShanheTeamHealthTactic) throw new InvalidOperationException("An allied health privilege lost its actual carried tactic.");
        if (ProgramEventHistory<ShanheAlliedHealthAppliedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("An allied health opening cannot apply twice.");
        foreach (var seat in Enumerable.Range(1, _options.ShanheBattle!.AllyRoster.Count).Where(IsActualShanheAlly))
            ApplyShanheAllyHealth(frame, seat, 3);
    }

    private void ApplyShanheAllyHealth(ProgramSkillFrame frame, int seat, int amount)
    {
        var target = _players[seat];
        if (!IsActualShanheAlly(seat) || !target.HasHpValue) return;
        var maximumBefore = target.MaxHp; var hpBefore = target.Hp;
        ChangeProgramParticipantMaximumHp(frame, seat, amount);
        var hpAfter = Math.Min(target.MaxHp, checked(hpBefore + Math.Max(0, target.MaxHp - maximumBefore)));
        if (target.Hp != hpAfter)
        {
            var before = target.Hp; target.Hp = hpAfter;
            RecordHpChange(frame.Id, frame.OwnerSeat, seat, before, hpAfter, HpChangeKind.Setting);
        }
        AdvanceEventRulesAndQueueFact(new ShanheAlliedHealthAppliedEvent(frame.Id, frame.OwnerSeat, seat,
            maximumBefore, target.MaxHp, hpBefore, target.Hp));
    }

    private SkillProgramStepOutcome BeginShanheSchoolTeamRank(ProgramSkillFrame supplied)
    {
        var frame = RequireShanheTeamOpening(supplied, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        if (!HasShanheSchoolEffect(0, 6) || frame.ShanheTeamRank is not null)
            throw new InvalidOperationException("A shared rank needs its actually completed school and original instruction.");
        if (ProgramEventHistory<ShanheTeamRankActivatedEvent>().Count > 0) return SkillProgramStepOutcome.Continue;
        var setup = _options.ShanheBattle!;
        var rank = ShanheTeamRankPrograms.EffectiveRank(setup);
        var entry = ShanheTeamRankPrograms.EntryBonuses(setup) != new ShanheTacticBonuses();
        ReplaceRuntimeTop(frame with { ShanheTeamRank = new(rank, entry,
            Enumerable.Range(1, setup.AllyRoster.Count).Where(IsActualShanheAlly).ToArray()) });
        AdvanceEventRulesAndQueueFact(new ShanheTeamRankActivatedEvent(frame.Id, rank, entry));
        ContinueShanheSchoolTeamRank(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62025)]
    private bool ResumeShanheSchoolTeamRank(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheTeamRank: not null } f || f.Id != frameId) return false;
        ContinueShanheSchoolTeamRank(frameId); return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62025)]
    private bool ReturnShanheTeamRankMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheTeamRank is null) return false;
        _ = RequireShanheTeamOpening(frame, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62025)]
    private bool MatchesShanheTeamRankMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ApplyShanheSchoolTeamRank &&
        frame.ShanheTeamRank is { EntryApplied: false, Stage: ShanheTeamRankStage.Next } &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62025)]
    private void AssertShanheTeamRankApplication(ProgramSkillFrame frame)
    {
        if (frame.ShanheTeamRank is not { } state) return;
        _ = RequireShanheTeamOpening(frame, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank", requireActive: false);
        var setup = _options.ShanheBattle!;
        if (state.Rank != ShanheTeamRankPrograms.EffectiveRank(setup) ||
            state.EntryApplied != (ShanheTeamRankPrograms.EntryBonuses(setup) != new ShanheTacticBonuses()) ||
            state.Index < 0 || state.Index > state.Seats.Count || !Enum.IsDefined(state.Stage) ||
            !state.Seats.SequenceEqual(state.Seats.Distinct().Order()) ||
            state.Seats.Any(seat => seat < 1 || seat > setup.AllyRoster.Count) ||
            ProgramEventHistory<ShanheTeamRankActivatedEvent>().Count(e => e.FrameId == frame.Id &&
                e.Rank == state.Rank && e.EntryApplied == state.EntryApplied) != 1)
            throw new InvalidOperationException("A team rank lost its original entry, rank or frozen recipient cursor.");
    }

    private void ContinueShanheSchoolTeamRank(long id)
    {
        while (_resolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheTeamRank: { } state } f && f.Id == id)
        {
            f = RequireShanheTeamOpening(f, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
            if (DrainShanheMarketChildren(f)) return;
            f = GetActiveProgramFrame(id);
            if (f.PendingMovementContinuation is not null) ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (state.Index >= state.Seats.Count)
            { ReplaceRuntimeTop(f with { ShanheTeamRank = null }); AdvanceRuntimeProgram(id); return; }
            var seat = state.Seats[state.Index];
            if (!IsActualShanheAlly(seat))
            { ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Index = state.Index + 1, Stage = ShanheTeamRankStage.Grant } }); continue; }
            var bonuses = ShanheGeneralRanks.GetBonuses(state.Rank);
            switch (state.Stage)
            {
                case ShanheTeamRankStage.Grant:
                    if (state.Rank == 5 && _options.ShanheBattle!.RebirthAvailable && !ShanheExpeditionRebirthSpent)
                        CommitProgramParticipantSkillGrants(f, seat, [ShanheTeamRankPrograms.SharedRebirthSkillId],
                            $"campaign:shanhe-team-rank:{seat}", null, null);
                    ReplaceRuntimeTop(GetActiveProgramFrame(id) with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Health } });
                    continue;
                case ShanheTeamRankStage.Health:
                    ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Draw } });
                    if (!state.EntryApplied) ApplyShanheAllyHealth(f, seat, bonuses.MaxHp);
                    continue;
                case ShanheTeamRankStage.Draw:
                    ReplaceRuntimeTop(f = f with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Next } });
                    if (!state.EntryApplied && bonuses.InitialHand > 0)
                    { BeginOperationProgramDraw(f, seat, bonuses.InitialHand, ShanheTeamRankDrawReason); return; }
                    continue;
                case ShanheTeamRankStage.Next:
                    ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Index = state.Index + 1, Stage = ShanheTeamRankStage.Grant } });
                    continue;
                default: throw new InvalidOperationException("A shared rank lost its original recipient cursor.");
            }
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ApplyShanheSchoolTeamRank)]
    private void ReturnShanheSchoolTeamRankDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = RequireShanheTeamOpening(supplied, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        if (f.ShanheTeamRank is not { EntryApplied: false, Stage: ShanheTeamRankStage.Next } state || f.NativeDraw is not null ||
            state.Index >= state.Seats.Count || outcome.RecipientSeat != state.Seats[state.Index] ||
            outcome.RequestedCount != ShanheGeneralRanks.GetBonuses(state.Rank).InitialHand ||
            outcome.SequenceAfter != _movementSequence || outcome.SequenceBefore > outcome.SequenceAfter ||
            outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !_cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter &&
                m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == ShanheTeamRankDrawReason)
                .OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("A shared rank draw lost its exact recipient, amount or original instruction.");
        CompleteProgramDrawCards(f.Id, outcome.RecipientSeat, null, SkillProgramCardSetVisibility.Private,
            ShanheTeamRankDrawReason, 0, outcome.ActualDrawIds);
        if (AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            AdvanceRuntimeProgram(f.Id);
    }
}
