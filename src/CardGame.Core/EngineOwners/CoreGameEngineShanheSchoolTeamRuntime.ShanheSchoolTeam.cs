#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolTeamRuntime
{
    internal bool IsActualShanheAlly(int seat) => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is { } setup && seat > 0 && seat <= setup.AllyRoster.Count && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[0], _coreMatchParticipantsPlayersAccess._players[seat]);
    internal int ShanheSharedRankRuleBonus(int seat, SkillRuleQuery query)
    {
        if (!IsActualShanheAlly(seat) || !_coreShanheSchoolsOperations.HasShanheSchoolEffect(0, 6))
            return 0;
        var bonus = ShanheGeneralRanks.GetBonuses(ShanheTeamRankPrograms.EffectiveRank(_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle!));
        return query switch
        {
            SkillRuleQuery.DrawCount => bonus.Draw,
            SkillRuleQuery.SlashLimit => bonus.Slash,
            _ => 0
        };
    }

    internal bool ShanheAllyExpeditionRebirthAvailable(int seat) => _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is { PlayerRank: 5 } setup && !setup.AllyRankRebirthUsedSeats.Contains(seat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<SkillUsageConsumedEvent>().Any(e => e.SkillOwnerSeat == seat && ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(setup, e));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62025)]
    internal bool AllowsShanheSharedRebirth(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == ShanheTeamRankPrograms.SharedRebirthSkillId)
            return IsActualShanheAlly(gate.OwnerSeat) && _coreShanheSchoolsOperations.HasShanheSchoolEffect(0, 6) && ShanheAllyExpeditionRebirthAvailable(gate.OwnerSeat);
        return true;
    }

    internal bool HasShanheTeamHealthTactic => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is { } setup && _coreMatchParticipantsPlayersAccess._players[0].IsAlive && _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[0]).Contains(ShanheTeamRankPrograms.TeamHealthSkillId, StringComparer.Ordinal) && (_shanheMarketOperations._shanheMarket is { } market ? market.Snapshot.TacticIds.Any(id => setup.MarketSetup!.Definition.Rewards.Any(r => r.Id == id && r.TacticSkillId == ShanheTeamRankPrograms.TeamHealthSkillId)) : setup.TacticSkillIds.Contains(ShanheTeamRankPrograms.TeamHealthSkillId, StringComparer.Ordinal));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62026, SkillProgramEffectOp.ApplyShanheAlliedHealthBonus)]
    internal bool AllowsShanheTeamHealth(ProgramTriggerCandidate candidate) => candidate.OwnerSeat == 0 && candidate.SkillId == ShanheTeamRankPrograms.TeamHealthSkillId && HasShanheTeamHealthTactic;
    internal ProgramSkillFrame RequireShanheTeamOpening(ProgramSkillFrame supplied, string skillId, string binding, bool requireActive = true)
    {
        var frame = requireActive ? _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id) : _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == supplied.Id) ?? throw new InvalidOperationException("A team privilege lost its original ancestor frame.");
        if (frame.SkillId != skillId || frame.OwnerSeat != 0 || frame.TriggerId != binding || frame.InstructionIndex != 1 || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, OwnerSeat: 0 } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("A team privilege requires its exact original Shanhe opening instruction.");
        return frame;
    }

    SkillProgramStepOutcome global::CardGame.Core.IShanheSchoolTeamHost.ApplyTeamRank(ProgramSkillFrame frame) => BeginShanheSchoolTeamRank(frame);
    void global::CardGame.Core.IShanheSchoolTeamHost.ApplyAlliedHealth(ProgramSkillFrame frame) => ApplyShanheAlliedHealth(frame);
    internal void ApplyShanheAlliedHealth(ProgramSkillFrame supplied)
    {
        var frame = RequireShanheTeamOpening(supplied, ShanheTeamRankPrograms.TeamHealthSkillId, "allied-health-privilege");
        if (!HasShanheTeamHealthTactic)
            throw new InvalidOperationException("An allied health privilege lost its actual carried tactic.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheAlliedHealthAppliedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("An allied health opening cannot apply twice.");
        foreach (var seat in Enumerable.Range(1, _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle!.AllyRoster.Count).Where(IsActualShanheAlly))
            ApplyShanheAllyHealth(frame, seat, 3);
    }

    internal void ApplyShanheAllyHealth(ProgramSkillFrame frame, int seat, int amount)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!IsActualShanheAlly(seat) || !target.HasHpValue)
            return;
        var maximumBefore = target.MaxHp;
        var hpBefore = target.Hp;
        _coreProgramLifecycleOperations.ChangeProgramParticipantMaximumHp(frame, seat, amount);
        var hpAfter = Math.Min(target.MaxHp, checked(hpBefore + Math.Max(0, target.MaxHp - maximumBefore)));
        if (target.Hp != hpAfter)
        {
            var before = target.Hp;
            target.Hp = hpAfter;
            _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, frame.OwnerSeat, seat, before, hpAfter, HpChangeKind.Setting);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheAlliedHealthAppliedEvent(frame.Id, frame.OwnerSeat, seat, maximumBefore, target.MaxHp, hpBefore, target.Hp));
    }

    internal SkillProgramStepOutcome BeginShanheSchoolTeamRank(ProgramSkillFrame supplied)
    {
        var frame = RequireShanheTeamOpening(supplied, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        if (!_coreShanheSchoolsOperations.HasShanheSchoolEffect(0, 6) || frame.ShanheTeamRank is not null)
            throw new InvalidOperationException("A shared rank needs its actually completed school and original instruction.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheTeamRankActivatedEvent>().Count > 0)
            return SkillProgramStepOutcome.Continue;
        var setup = _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle!;
        var rank = ShanheTeamRankPrograms.EffectiveRank(setup);
        var entry = ShanheTeamRankPrograms.EntryBonuses(setup) != new ShanheTacticBonuses();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheTeamRank = new(rank, entry, Enumerable.Range(1, setup.AllyRoster.Count).Where(IsActualShanheAlly).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheTeamRankActivatedEvent(frame.Id, rank, entry));
        ContinueShanheSchoolTeamRank(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62025)]
    internal bool ResumeShanheSchoolTeamRank(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheTeamRank: not null } f || f.Id != frameId)
            return false;
        ContinueShanheSchoolTeamRank(frameId);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62025)]
    internal bool ReturnShanheTeamRankMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheTeamRank is null)
            return false;
        _ = RequireShanheTeamOpening(frame, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62025)]
    internal bool MatchesShanheTeamRankMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.ApplyShanheSchoolTeamRank && frame.ShanheTeamRank is { EntryApplied: false, Stage: ShanheTeamRankStage.Next } && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;

    internal void ContinueShanheSchoolTeamRank(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShanheTeamRank: { } state } f && f.Id == id)
        {
            f = RequireShanheTeamOpening(f, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
            if (_coreShanheMarketApplicationOperations.DrainShanheMarketChildren(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (state.Index >= state.Seats.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheTeamRank = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return;
            }

            var seat = state.Seats[state.Index];
            if (!IsActualShanheAlly(seat))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Index = state.Index + 1, Stage = ShanheTeamRankStage.Grant } });
                continue;
            }

            var bonuses = ShanheGeneralRanks.GetBonuses(state.Rank);
            switch (state.Stage)
            {
                case ShanheTeamRankStage.Grant:
                    if (state.Rank == 5 && ShanheAllyExpeditionRebirthAvailable(seat))
                        _coreProgramSkillChangesOperations.CommitProgramParticipantSkillGrants(f, seat, [ShanheTeamRankPrograms.SharedRebirthSkillId], $"campaign:shanhe-team-rank:{(seat)}", null, null);
                    _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Health } });
                    continue;
                case ShanheTeamRankStage.Health:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Draw } });
                    if (!state.EntryApplied)
                        ApplyShanheAllyHealth(f, seat, bonuses.MaxHp);
                    continue;
                case ShanheTeamRankStage.Draw:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheTeamRank = state with { Stage = ShanheTeamRankStage.Next } });
                    if (!state.EntryApplied && bonuses.InitialHand > 0)
                    {
                        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, bonuses.InitialHand, global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamDefinitions.ShanheTeamRankDrawReason);
                        return;
                    }

                    continue;
                case ShanheTeamRankStage.Next:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheTeamRank = state with { Index = state.Index + 1, Stage = ShanheTeamRankStage.Grant } });
                    continue;
                default:
                    throw new InvalidOperationException("A shared rank lost its original recipient cursor.");
            }
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ApplyShanheSchoolTeamRank)]
    internal void ReturnShanheSchoolTeamRankDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = RequireShanheTeamOpening(supplied, ShanheSchoolEffects.TeamRankSkillId, "opening-team-rank");
        if (f.ShanheTeamRank is not { EntryApplied: false, Stage: ShanheTeamRankStage.Next } state || f.NativeDraw is not null || state.Index >= state.Seats.Count || outcome.RecipientSeat != state.Seats[state.Index] || outcome.RequestedCount != ShanheGeneralRanks.GetBonuses(state.Rank).InitialHand || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.SequenceBefore > outcome.SequenceAfter || outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamDefinitions.ShanheTeamRankDrawReason).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("A shared rank draw lost its exact recipient, amount or original instruction.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(f.Id, outcome.RecipientSeat, null, SkillProgramCardSetVisibility.Private, global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamDefinitions.ShanheTeamRankDrawReason, 0, outcome.ActualDrawIds);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
