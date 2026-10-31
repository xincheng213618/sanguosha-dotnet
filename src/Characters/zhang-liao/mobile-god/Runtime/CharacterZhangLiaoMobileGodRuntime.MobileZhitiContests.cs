#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoMobileGodRuntime
{
    internal bool TryBeginMobileZhitiPindianPrograms(PindianFrame parent) => parent.Result is { } result && BeginMobileZhitiContest(parent, result, MobileZhitiContestKind.PairPindian);
    internal bool TryBeginMobileZhitiSimultaneousPrograms(ProgramSkillFrame parent, PindianResult result) => BeginMobileZhitiContest(_coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id), result, MobileZhitiContestKind.SimultaneousPindian);
    internal bool BeginMobileZhitiContest(ResolutionFrame parent, PindianResult result, MobileZhitiContestKind kind)
    {
        if (!HasMobileShenZhangLiao)
            return false;
        var winner = result.SourceRank > result.OpponentRank ? result.SourceSeat : result.SourceRank < result.OpponentRank ? result.OpponentSeat : -1;
        var loser = winner == result.SourceSeat ? result.OpponentSeat : result.SourceSeat;
        if (winner < 0)
            return false;
        var frozen = parent.MobileZhitiContest;
        if (frozen is not null && frozen.SourceCardId == result.SourceCardId && frozen.OpponentCardId == result.OpponentCardId && frozen.SourceRank == result.SourceRank && frozen.OpponentRank == result.OpponentRank)
            return frozen.Finished ? false : ContinueMobileZhitiContest(parent.Id);
        if (!MobileZhitiOpponent(winner, loser) || MobileZhitiSlots(winner).Length == 0)
            return false;
        var candidates = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[winner], SkillProgramTriggerWindow.PindianResultDetermined).Where(c => c.SkillId == MobileShenZhangLiaoComposition.Zhiti && c.BindingId == "restore-pindian-win").ToArray();
        if (candidates.Length == 0)
            return false;
        var r = new MobileZhitiContestReceipt(kind, parent.Id, winner, loser, result.SourceCardId, result.OpponentCardId, result.SourceRank, result.OpponentRank)
        {
            Candidates = candidates
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileZhitiContest = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhitiContestIssuedEvent(parent.Id, kind, winner, loser, result.SourceCardId, result.OpponentCardId, result.SourceRank, result.OpponentRank, 0, candidates.Length));
        return ContinueMobileZhitiContest(parent.Id);
    }

    internal bool TryBeginMobileZhitiDuelPrograms(NativeOwners.CardAttackHandle attack)
    {
        if (!HasMobileShenZhangLiao || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { CardKind: CardKind.Duel } parent || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not { } duel || duel.ResponderSeat != attack.TargetSeat || duel.OpponentSeat != attack.SourceSeat)
            return false;
        var failed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DuelResponseEvent>().Where(e => e.ResolutionId == parent.Id && !e.UsedSlash).ToArray();
        if (failed.Length == 0 || failed[^1].ResponderSeat != attack.TargetSeat)
            throw new InvalidOperationException("Zhiti requires the actual accepted failed Duel response.");
        if (parent.MobileZhitiContest is { Kind: MobileZhitiContestKind.Duel } old && old.Occurrence == failed.Length)
            return old.Finished ? false : ContinueMobileZhitiContest(parent.Id);
        if (!MobileZhitiOpponent(attack.SourceSeat, attack.TargetSeat) || MobileZhitiSlots(attack.SourceSeat).Length == 0)
            return false;
        var candidates = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], SkillProgramTriggerWindow.DuelResponseFailed).Where(c => c.SkillId == MobileShenZhangLiaoComposition.Zhiti && c.BindingId == "restore-duel-win").ToArray();
        if (candidates.Length == 0)
            return false;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var r = new MobileZhitiContestReceipt(MobileZhitiContestKind.Duel, parent.Id, attack.SourceSeat, attack.TargetSeat, Occurrence: failed.Length)
        {
            Candidates = candidates
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileZhitiContest = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhitiContestIssuedEvent(parent.Id, r.Kind, r.WinnerSeat, r.LoserSeat, 0, 0, 0, 0, r.Occurrence, candidates.Length));
        return ContinueMobileZhitiContest(parent.Id);
    }

    internal ProgramSkillWindowContext MobileZhitiContestContext(ResolutionFrame parent, ProgramTriggerCandidate candidate) => new(parent.MobileZhitiContest!.Kind == MobileZhitiContestKind.Duel ? SkillProgramTriggerWindow.DuelResponseFailed : SkillProgramTriggerWindow.PindianResultDetermined, parent.Id, candidate.OwnerSeat, parent.MobileZhitiContest.WinnerSeat, parent.MobileZhitiContest.LoserSeat, Amount: 1, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]));
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 150, SkillProgramTriggerWindow.PindianResultDetermined)]
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 170, SkillProgramTriggerWindow.DuelResponseFailed)]
    internal bool MobileZhitiContestCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId);
        return parent?.MobileZhitiContest is { Finished: false } r && r.Cursor < r.Candidates.Count && r.Candidates[r.Cursor] == candidate && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == r.WinnerSeat && context.TargetSeat == r.LoserSeat && context.Window == (r.Kind == MobileZhitiContestKind.Duel ? SkillProgramTriggerWindow.DuelResponseFailed : SkillProgramTriggerWindow.PindianResultDetermined);
    }

    internal bool ContinueMobileZhitiContest(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { MobileZhitiContest: { } r } parent || parent.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return true;
        AssertMobileZhitiContest(parent);
        while (r.Cursor < r.Candidates.Count)
        {
            var candidate = r.Candidates[r.Cursor];
            var context = MobileZhitiContestContext(parent, candidate);
            if (_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
            {
                if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Optional)
                    _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(candidate, context);
                else
                    _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { MobileZhitiContest = r = r with { Cursor = r.Cursor + 1 } });
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { MobileZhitiContest = r with { Finished = true } });
        if (r.Kind == MobileZhitiContestKind.PairPindian)
        {
            if (!_corePindianClaimsOperations.BeginPindianClaims((PindianFrame)parent))
                _corePindianOperations.CompletePindian((PindianFrame)_resolutionStackOperations._resolutionStack[^1]);
        }
        else if (r.Kind == MobileZhitiContestKind.SimultaneousPindian)
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        else
        {
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(parent.Id);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }

        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 39950)]
    internal bool ReturnMobileZhitiContestBinding(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { MobileZhitiContest: { Finished: false } r } parent || f.WindowContext?.ParentFrameId != parent.Id)
            return false;
        if (!MobileZhitiProgramRidesOn(f, parent))
            throw new InvalidOperationException("Zhiti lost the exact original contest candidate return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileZhitiContest = r with { Cursor = r.Cursor + 1 } });
        ContinueMobileZhitiContest(parent.Id);
        return true;
    }

    internal bool TryGetMobileZhitiContestTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context)
    {
        candidate = null !;
        context = null !;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { MobileZhitiContest: { Finished: false } r } parent || r.Cursor >= r.Candidates.Count)
            return false;
        candidate = r.Candidates[r.Cursor];
        context = MobileZhitiContestContext(parent, candidate);
        return true;
    }

    internal bool TrySkipMobileZhitiContestTrigger(ProgramTriggerCandidate candidate)
    {
        if (!TryGetMobileZhitiContestTrigger(out var current, out var context) || current != candidate)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[^1];
        var r = parent.MobileZhitiContest!;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileZhitiContest = r with { Cursor = r.Cursor + 1 } });
        ContinueMobileZhitiContest(parent.Id);
        return true;
    }

    internal bool MobileZhitiProgramRidesOn(ProgramSkillFrame f, ResolutionFrame parent) => f.WindowContext is { } context && parent.MobileZhitiContest is { Finished: false } r && r.Cursor < r.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, r.Candidates[r.Cursor]) && MobileZhitiContestCandidateMatches(r.Candidates[r.Cursor], context);
    internal bool IsMobileZhitiContestSubtree(ResolutionFrame parent)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        if (parent.MobileZhitiContest is not { Finished: false } || index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame source || !MobileZhitiProgramRidesOn(source, parent) || !_coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreNativeEquipmentInvocationsOperations.NativeEquipmentObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)))
            return false;
        AssertMobileZhitiContest(parent);
        return true;
    }
    internal void AssertMobileZhitiContest(ResolutionFrame parent)
    {
        var r = parent.MobileZhitiContest ?? throw new InvalidOperationException("Zhiti lost its original contest receipt.");
        if (r.HostId != parent.Id || !Enum.IsDefined(r.Kind) || r.Candidates is not System.Collections.IList { IsReadOnly: true } || r.Candidates.Count == 0 || r.Cursor < 0 || r.Cursor > r.Candidates.Count || r.Candidates.Any(c => c.OwnerSeat != r.WinnerSeat || c.SkillId != MobileShenZhangLiaoComposition.Zhiti || c.BindingId != (r.Kind == MobileZhitiContestKind.Duel ? "restore-duel-win" : "restore-pindian-win") || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileZhitiContestIssuedEvent>().Count(e => e.HostId == parent.Id && e.Kind == r.Kind && e.WinnerSeat == r.WinnerSeat && e.LoserSeat == r.LoserSeat && e.SourceCardId == r.SourceCardId && e.OpponentCardId == r.OpponentCardId && e.SourceRank == r.SourceRank && e.OpponentRank == r.OpponentRank && e.Occurrence == r.Occurrence && e.Count == r.Candidates.Count) != 1)
            throw new InvalidOperationException("Zhiti changed its frozen native result, source binding or actual winner/loser.");
        if (r.Kind == MobileZhitiContestKind.Duel)
        {
            var failed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DuelResponseEvent>().Where(e => e.ResolutionId == parent.Id && !e.UsedSlash).ToArray();
            if (parent is not CardUseFrame { CardKind: CardKind.Duel } use || use.CardAttack is not { } attack || attack.SourceSeat != r.WinnerSeat || attack.TargetSeat != r.LoserSeat || r.Occurrence != failed.Length || failed[^1].ResponderSeat != r.LoserSeat || attack.DamageWasApplied)
                throw new InvalidOperationException("Zhiti's Duel win must retain the real failed response before any damage is attempted.");
        }
        else if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Any(e => e.FrameId == parent.Id && e.Result.SourceCardId == r.SourceCardId && e.Result.OpponentCardId == r.OpponentCardId && e.Result.SourceRank == r.SourceRank && e.Result.OpponentRank == r.OpponentRank && e.Result.WonBy(r.WinnerSeat) && (e.Result.SourceSeat == r.LoserSeat || e.Result.OpponentSeat == r.LoserSeat)) || r.Kind == MobileZhitiContestKind.PairPindian && parent is not PindianFrame || r.Kind == MobileZhitiContestKind.SimultaneousPindian && parent is not ProgramSkillFrame { SimultaneousPindian: not null })
            throw new InvalidOperationException("Zhiti's Pindian win must retain its original genuine revealed result and host.");
    }
}
