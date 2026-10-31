#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardEffectProgramsRuntime
{
    internal bool HasCardEffectBeforeApplyPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.CardEffectBeforeApply);

    internal bool TryBeginOrdinaryCardEffectPrograms(NullificationWindowFrame pending)
    {
        if (!HasCardEffectBeforeApplyPrograms || CoreNativeCardClassificationsRuntime.IsDelayedCard(pending.EffectCardKind))
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(f => f.Id == pending.ParentFrameId);
        var action = use.Action ?? throw new InvalidOperationException("An actual trick effect needs its frozen action.");
        if (_coreNativeSlashTargetDuelOperations.TryGetNativeSlashTargetDuel(pending, out _, out _, out var nativeSlice))
            return BeginCardEffectPrograms(_coreNativeSlashTargetDuelOperations.NativeSlashTargetDuelEffectAction(use, nativeSlice), [nativeSlice.TargetSeat], pending, null);
        var designated = action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, i) => i % 2 == 0).Distinct().ToArray() : action.EffectiveDesignatedTargetSeats.Distinct().ToArray();
        if (designated.Length != 1)
            return pending.TargetSeats.Count == 1 && CoreGameEngineSequentialTrickTargetsRuntime.UsesSequentialTrickTargets(pending.ActionKind) && BeginCardEffectPrograms(action, pending.TargetSeats, pending, null, perTargetOnly: true);
        // Group cards enter their exact target boundary after their native
        // group cursor is created, not at the common whole-use nullification.
        if (pending.ActionKind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains)
            return false;
        return BeginCardEffectPrograms(action, designated, pending, null);
    }

    internal bool TryBeginDelayedCardEffectPrograms(CharacterState owner, Card card, CardKind kind)
    {
        if (!HasCardEffectBeforeApplyPrograms)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var continuation = new DelayedEffectContinuation(card.Id, kind, owner.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, parent?.Id ?? 0, parent?.Kind, parent?.Step);
        // This describes the delayed effect boundary; it does not accept another card use.
        var action = new CardActionContext(0, null, CardActionType.Use, owner.Seat, owner.Seat, null, null, null, kind, [owner.Seat], [], [], [owner.Seat]);
        return BeginCardEffectPrograms(action, [owner.Seat], null, continuation);
    }

    internal bool BeginCardEffectPrograms(CardActionContext action, IReadOnlyList<int> designated, NullificationWindowFrame? ordinary, DelayedEffectContinuation? delayed, NativeTrickTargetEffectReturn? nativeTargetReturn = null, bool perTargetOnly = false)
    {
        var candidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardEffectBeforeApply, designated, null).Where(candidate => !perTargetOnly || _characterLuXunMouMobileOperations.Value.HasPerTargetTrickEffectBinding(candidate)).OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(0, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ThenBy(c => c.TriggerId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0)
            return false;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var parentId = ordinary?.ParentFrameId ?? delayed?.ParentFrameId ?? (nativeTargetReturn!.WenheProgramFrameId ?? nativeTargetReturn.UseId);
        candidates = candidates.Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = id, CardUse = c.FrozenContext!.CardUse!with { ParentCardUseFrameId = nativeTargetReturn?.UseId ?? parentId } } }).ToArray();
        _coreRuntimeOperations.PushRuntimeFrame(new CardEffectBeforeApplyFrame(id, parentId, action, Array.AsReadOnly(designated.ToArray()), Array.AsReadOnly(candidates), ordinary, delayed) { NativeTargetReturn = nativeTargetReturn });
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 130, SkillProgramTriggerWindow.CardEffectBeforeApply)]
    internal bool CanRunCardEffectCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardEffectBeforeApplyFrame>().LastOrDefault();
        return frame is not null && frame.Id == context.ParentFrameId && frame.CandidateIndex < frame.Candidates.Count && CoreGameEngineCardActionsRuntime.ToSharedCandidate(frame.Candidates[frame.CandidateIndex]) == candidate && frame.Candidates[frame.CandidateIndex].FrozenContext == context && frame.FinalDesignatedTargetSeats.Count == 1 && frame.FinalDesignatedTargetSeats[0] == candidate.OwnerSeat && (frame.DelayedReturn is not null || frame.Action.ActorSeat != candidate.OwnerSeat);
    }

    [NativeFrameRoute(typeof(CardEffectBeforeApplyFrame), 180)]
    internal void ContinueCardEffectBeforeApply()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardEffectBeforeApplyFrame frame)
        {
            if (frame.CandidateIndex == frame.Candidates.Count)
            {
                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.CardEffectBeforeApply);
                if (frame.NativeTargetReturn is not null)
                    _characterLuXunMouMobileOperations.Value.ReturnNativeTrickTargetEffect(frame);
                else if (frame.OrdinaryReturn is { } ordinary)
                {
                    var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
                    if (parent is not CardUseFrame use || use.Id != ordinary.ParentFrameId || use.Action?.ActionId != frame.Action.ActionId)
                        throw new InvalidOperationException("Actual trick effect lost its exact use parent.");
                    // A paid effect child may kill its unique target or end the match.
                    // Finish this exact use; never reopen nullification or enter a dead response.
                    var targetSeat = frame.FinalDesignatedTargetSeats[0];
                    if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
                        _coreProgramCardTargetEffectsOperations.CompleteIneffectiveTrickTarget(ordinary, targetSeat);
                    else
                        _coreNativeNullificationOperations.ResolveNullifiableEffectCore(ordinary);
                }
                else
                    ContinueDelayedCardEffect(frame.DelayedReturn ?? throw new InvalidOperationException("Missing delayed effect return."));
                return;
            }

            var candidate = frame.Candidates[frame.CandidateIndex];
            var context = candidate.FrozenContext!;
            var shared = CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(shared, context))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CandidateIndex = frame.CandidateIndex + 1 });
                continue;
            }

            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.Triggers.Single(t => t.Id == candidate.TriggerId);
            if (trigger.Optional)
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(shared, context);
            else
                _coreProgramLifecycleOperations.BeginProgramBinding(shared, context);
            return;
        }
    }

    internal void CompleteCardEffectCandidate(ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardEffectBeforeApplyFrame frame || frame.Id != context.ParentFrameId || frame.CandidateIndex >= frame.Candidates.Count || frame.Candidates[frame.CandidateIndex].FrozenContext != context)
            throw new InvalidOperationException("The actual effect program lost its owning cursor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CandidateIndex = frame.CandidateIndex + 1 });
        _coreRuntimeOperations.AdvanceRuntimeFrame(frame.Id);
    }

    internal void ContinueDelayedCardEffect(DelayedEffectContinuation delayed)
    {
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (delayed.OriginalUseLightning is not null)
        {
            _coreOriginalUseLightningOperations.BeginOriginalUseLightningJudgment(delayed);
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[delayed.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(owner.Seat)).SingleOrDefault(c => c.Id == delayed.CardId);
        if (card is null)
        {
            _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(owner);
            return;
        }

        if (_coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card) != delayed.EffectiveKind)
            throw new InvalidOperationException("Delayed effect identity changed while suspended.");
        BeginActualDelayedJudgment(owner, card, delayed.EffectiveKind);
    }

    internal void BeginActualDelayedJudgment(CharacterState current, Card card, CardKind kind)
    {
        var info = CoreActualTurnsRuntime.GetDelayedJudgmentInfo(kind);
        var judgment = _coreNativeJudgmentOperations.BeginJudgment(attack: null, targetSeat: current.Seat, reason: info.Reason, parentFrameId: 0, sourceCard: kind, continuation: info.Continuation, delayedCard: card, sourceSeat: current.Seat);
        if (judgment is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
