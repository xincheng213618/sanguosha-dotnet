#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuChengMouMobileRuntime
{
    internal CardUseFrame? MouLiuChengAccrualUse(CardActionContext action) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action is { Type: CardActionType.Use } a && a.ActionId == action.ActionId && a.ActorSeat == action.ActorSeat && a.ProviderSeat == action.ProviderSeat && u.SourceSeat == action.ActorSeat && u.CardKind == action.EffectiveKind);
    internal IReadOnlyList<int> MouLiuChengPrimaryTargets(CardActionContext action) => Array.AsReadOnly((action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, i) => i % 2 == 0) : action.EffectiveDesignatedTargetSeats).Distinct().ToArray());
    internal int MouLiuChengPhaseUses(int owner, string skill, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengChuiEarnedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Phase == phase);
    internal void CaptureMouLiuChengAccrual(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasMouLiuChengCapability || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && !IsMouLiuChengNonDamageOrdinary(action.EffectiveKind) || MouLiuChengAccrualUse(action)is not { } use)
            return;
        var targets = MouLiuChengPrimaryTargets(action).Where(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Except(use.MouLiuChengAnnouncedTargets ?? []).ToArray();
        if (targets.Length == 0)
            return;
        var ordinal = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengTargetAnnouncedEvent>().Where(e => e.UseId == use.Id).Select(e => e.NotificationOrdinal).DefaultIfEmpty(0).Max() + 1;
        var claims = new List<MouLiuChengAccrualClaim>();
        if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            foreach (var b in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetInstanceTriggers(window).Where(b => MouLiuChengComposition.IsSkill(b.SkillId) && b.Trigger.Id == "after-designation" && MouLiuChengComposition.IsProgram(b.Program) && MouLiuChengKind(b.SkillId, action.EffectiveKind) && (b.SkillId != MouLiuChengComposition.Yingwu || _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], MouLiuChengComposition.Lueying))))
            {
                var actual = targets.Where(t => b.SkillId != MouLiuChengComposition.Lueying || t != action.ActorSeat).ToArray();
                if (actual.Length == 0)
                    continue;
                var q = new MouLiuChengAccrualClaim(new(b.SkillId, b.Trigger.Id, action.ActorSeat, b.SkillInstanceId), b.Program.GameplayHash, use.Id, action.ActionId, action.ActorSeat, action.EffectiveKind, phase, ordinal)
                {
                    Targets = actual
                };
                claims.Add(q);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengAccrualQualifiedEvent(q.UseId, q.ActionId, ordinal, q.Source, q.GameplayHash, phase, actual.Length));
            }

        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouLiuChengAnnouncedTargets = (u.MouLiuChengAnnouncedTargets ?? []).Concat(targets).ToArray(), MouLiuChengAccrualClaims = (u.MouLiuChengAccrualClaims ?? []).Concat(claims).ToArray() });
        foreach (var target in targets)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengTargetAnnouncedEvent(use.Id, action.ActionId, ordinal, target));
    }

    internal ProgramSkillWindowContext? AttachMouLiuChengAccrual(CardActionContext action, string skill, string instance, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || !MouLiuChengComposition.IsSkill(skill))
            return context;
        var claims = MouLiuChengAccrualUse(action)?.MouLiuChengAccrualClaims;
        var q = claims?.FirstOrDefault(q => q.Source.SkillId == skill && q.Source.SkillInstanceId == instance && q.ActorSeat == context.OwnerSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengAccrualStartedEvent>().Any(e => e.Source == q.Source && e.UseId == q.UseId && e.NotificationOrdinal == q.NotificationOrdinal) && !_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Candidates.Any(c => SameMouLiuChengAccrual(c.FrozenContext?.MouLiuChengAccrual, q))));
        return q is null ? null : context with
        {
            MouLiuChengAccrual = q
        };
    }

    internal static bool SameMouLiuChengAccrual(MouLiuChengAccrualClaim? a, MouLiuChengAccrualClaim? b) => RuntimeSkills.RuntimeRules_liu_cheng_mou_mobile_MouLiuChengAccrual.SameMouLiuChengAccrual(a, b);
    internal bool MouLiuChengCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window)
    {
        if (window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        var expected = _coreCardActionsOperations.CreateCardActionProgramContext2(window, current);
        return current.OwnerSeat == candidate.OwnerSeat && current.SkillId == candidate.SkillId && current.TriggerId == candidate.BindingId && current.SkillInstanceId == candidate.SkillInstanceId && current.GameplayHash == candidate.GameplayHash && (expected with
        {
            MouLiuChengAccrual = null
        }

        ) == (context with
        {
            MouLiuChengAccrual = null
        }

        ) && (expected.MouLiuChengAccrual is null && context.MouLiuChengAccrual is null || SameMouLiuChengAccrual(expected.MouLiuChengAccrual, context.MouLiuChengAccrual));
    }

    internal bool ValidMouLiuChengAccrualClaim(MouLiuChengAccrualClaim q) => q.Source.BindingId == "after-designation" && q.Source.OwnerSeat == q.ActorSeat && MouLiuChengKind(q.Source.SkillId, q.EffectiveKind) && q.Targets.Count > 0 && q.Targets.Distinct().Count() == q.Targets.Count && q.Targets.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && (q.Source.SkillId != MouLiuChengComposition.Lueying || !q.Targets.Contains(q.ActorSeat)) && q.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(q.Phase) && _coreCardUseLifecycleOperations.LifecycleCardUse(q.UseId)?.MouLiuChengAccrualClaims?.Any(c => SameMouLiuChengAccrual(c, q)) == true && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengAccrualQualifiedEvent>().Count(e => e == new MouLiuChengAccrualQualifiedEvent(q.UseId, q.ActionId, q.NotificationOrdinal, q.Source, q.GameplayHash, q.Phase, q.Targets.Count)) == 1 && q.Targets.All(t => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengTargetAnnouncedEvent>().Count(e => e.UseId == q.UseId && e.ActionId == q.ActionId && e.NotificationOrdinal == q.NotificationOrdinal && e.TargetSeat == t) == 1);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29400, SkillProgramEffectOp.AccrueMouLiuChengChui)]
    internal bool CanAccrueMouLiuCheng(ProgramTriggerLegalityContext c) => c.Owner.IsAlive && MouLiuChengComposition.IsProgram(c.Program) && c.Context.MouLiuChengAccrual is { } q && ValidMouLiuChengAccrualClaim(q) && q.Source == new CardConversionSource(c.SkillId, c.Trigger.Id, c.OwnerSeat, c.Candidate.SkillInstanceId) && q.GameplayHash == c.Candidate.GameplayHash && MouLiuChengPhaseUses(c.OwnerSeat, c.SkillId, q.Phase) < 2 && (c.SkillId != MouLiuChengComposition.Yingwu || _coreNationalSkillsOperations.HasRuntimeSkill(c.Owner, MouLiuChengComposition.Lueying));
    internal SkillProgramStepOutcome AccrueMouLiuCheng(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId != "after-designation" || f.ActivationId != f.TriggerId || f.MouLiuChengAccrual is not null || f.WindowContext is not { MouLiuChengAccrual: { } q } context || !ValidMouLiuChengAccrualClaim(q) || q.Source != MouLiuChengSource(f) || q.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != q.UseId || w.Action.ActionId != q.ActionId || !MouLiuChengCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context, w) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengAccrualStartedEvent>().Any(e => e.Source == q.Source && e.UseId == q.UseId && e.NotificationOrdinal == q.NotificationOrdinal))
            throw new InvalidOperationException("Liu Cheng accrual requires one exact after-designation native target notification.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengAccrual = q });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengAccrualStartedEvent(f.Id, q.Source, q.UseId, q.ActionId, q.NotificationOrdinal));
        foreach (var target in q.Targets)
        {
            var used = MouLiuChengPhaseUses(f.OwnerSeat, f.SkillId, q.Phase);
            if (used >= 2 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || f.SkillId == MouLiuChengComposition.Yingwu && !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], MouLiuChengComposition.Lueying))
                break;
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengChuiEarnedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.UseId == q.UseId && e.ActionId == q.ActionId && e.TargetSeat == target))
                continue;
            ChangeMouLiuChengChui(f, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengChuiEarnedEvent(f.Id, q.Source, q.GameplayHash, q.UseId, q.ActionId, q.NotificationOrdinal, target, q.Phase, used + 1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouLiuChengChui)));
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal void ChangeMouLiuChengChui(ProgramSkillFrame f, int delta)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var marker = PlayerMarkerKind.MouLiuChengChui;
        var old = owner.Markers.GetValueOrDefault(marker);
        var count = checked(old + delta);
        var source = (marker, f.OwnerSeat);
        if (delta is not (1 or -2) || count < 0 || owner.MarkerSourceCounts.GetValueOrDefault(source) != old)
            throw new InvalidOperationException("Liu Cheng must retain one exact permanent shared self-attributed Chui balance.");
        if (count == 0)
        {
            owner.Markers.Remove(marker);
            owner.MarkerSourceCounts.Remove(source);
        }
        else
        {
            owner.Markers[marker] = count;
            owner.MarkerSourceCounts[source] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, marker, delta, count, f.OwnerSeat, $"skill-program.{(f.SkillId)}.{(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f))}.chui"));
    }

    internal ProgramCardTriggerWindowFrame AppendMouLiuChengAccrual(ProgramCardTriggerWindowFrame frame)
    {
        if (!HasMouLiuChengCapability || CoreGameEngineCardActionsRuntime.GetCardActionWindow(frame) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)is not { Action: { Type: CardActionType.Use } } use || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return frame;
        var action = CoreGameEngineActorHandLimitTargetProgramsRuntime.ActorHandLimitTargetCollectionAction(use);
        CaptureMouLiuChengAccrual(action, SkillProgramTriggerWindow.CardUseTargetsFinalized);
        var added = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, null, frame.Continuation).Where(c => c.FrozenContext?.MouLiuChengAccrual is not null).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = frame.Id, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (added.Length == 0)
            return frame;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Candidates = Array.AsReadOnly(frame.Candidates.Concat(added).ToArray()) });
        return frame;
    }

    internal bool TryBeginMouLiuChengLateSlashTargets(NativeOwners.CardAttackHandle attack)
    {
        if (!HasMouLiuChengCapability || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ProgramUseAccepted: true, Action: { Type: CardActionType.Use } action } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || use.CardAttack is not { } current || current.TargetSeat != attack.TargetSeat || !use.TargetSeats.Contains(attack.TargetSeat))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id)
            throw new InvalidOperationException("Liu Cheng's late native target must remain on its exact accepted Slash.");
        var projected = CoreGameEngineActorHandLimitTargetProgramsRuntime.ActorHandLimitTargetCollectionAction(use);
        CaptureMouLiuChengAccrual(projected, SkillProgramTriggerWindow.CardUseTargetsFinalized);
        var windowId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var candidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(projected, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, null, ProgramCardContinuation.Slash).Where(c => c.FrozenContext?.MouLiuChengAccrual is not null).OrderByDescending(c => c.Priority).ThenBy(c => c.OwnerSeat).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ThenBy(c => c.TriggerId, StringComparer.Ordinal).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = windowId, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (candidates.Length == 0)
            return false;
        _coreResolutionIdentityBoundaryOperations._resolutionSequence++;
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramCardTriggerWindowFrame(windowId, use.Id, action, ProgramCardContinuation.Slash, Array.AsReadOnly(candidates), AttackOwnerFrameId: use.Id));
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }
}
