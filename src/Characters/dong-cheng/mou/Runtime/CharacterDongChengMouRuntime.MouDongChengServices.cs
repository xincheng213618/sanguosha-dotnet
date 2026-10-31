#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongChengMouRuntime
{
    internal MouDongChengRuntime MouDongCheng => _mouDongChengOperations.Get();
    internal bool HasMouDongCheng => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouDongChengBaojia);

    int global::CardGame.Core.IMouDongChengFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    long global::CardGame.Core.IMouDongChengFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouDongChengFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouDongChengFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouDongChengFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouDongChengFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    double global::CardGame.Core.IMouDongChengFacts.TargetScore(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 1, 0, 0, 0, false, false));
    int global::CardGame.Core.IMouDongChengFacts.KeepValue(int owner, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[owner]);
    SkillProgram? global::CardGame.Core.IMouDongChengFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouDongChengFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouDongChengFacts.Parent(ProgramSkillFrame f) => MouDcParent(f);
    MouDcOccurrence? global::CardGame.Core.IMouDongChengFacts.Before(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouDcBefore(c, w);
    long global::CardGame.Core.IMouDongChengFacts.ClaimUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouDcClaimUse(c, w)?.Id ?? 0;
    bool global::CardGame.Core.IMouDongChengFacts.MayActivate(int owner) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == owner && _coreMatchParticipantsPlayersAccess._players[owner].MouDongChengDisabledTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && ((global::CardGame.Core.IMouDongChengFacts)this).Costs(owner).Count > 0;
    IReadOnlyList<int> global::CardGame.Core.IMouDongChengFacts.Living => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    IReadOnlyList<EquipmentSlot> global::CardGame.Core.IMouDongChengFacts.Slots(int owner, bool abolished) => Array.AsReadOnly(Enum.GetValues<EquipmentSlot>().Where(s => (_coreMatchParticipantsPlayersAccess._players[owner].EquipmentSlotCapacity(s) == 0) == abolished).ToArray());
    IReadOnlyList<MouDcEntity> global::CardGame.Core.IMouDongChengFacts.Costs(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(c => MouDcDamageKind(_cardZonesOperations._cardZones.GetLocation(c.Id).Zone == CardZoneKind.Hand ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c) : c.Kind) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).Select(c => new MouDcEntity(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id).Zone == CardZoneKind.Hand ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c) : c.Kind, _cardZonesOperations._cardZones.GetLocation(c.Id), c.DisplayName)).ToArray());
    IReadOnlyList<int> global::CardGame.Core.IMouDongChengFacts.Targets(ProgramSkillFrame f, CardKind kind) => MouDcTargets(f, kind);
    (long Frame, long Use, long Damage)? global::CardGame.Core.IMouDongChengFacts.DyingCause(PlayerDyingEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DyingFrame { Continuation: DyingContinuationKind.Damage } dying || dying.Id != fact.ResolutionId || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == dying.ParentFrameId)is not { Amount: > 0 } damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { DamageWasApplied: true } attack || attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != fact.VictimSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouDongChengDouwei: { } n } u || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } parent || !MouDcUseRidesOn(parent, u))
            return null;
        return (n.FrameId, u.Id, damage.Id);
    }

    internal static bool MouDcDamageKind(CardKind kind) => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(kind) || kind == CardKind.Lightning;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 37400, SkillProgramEffectOp.ResolveMouDongChengDouwei)]
    internal bool MouDcCanActivate(CharacterState owner) => _characterDongChengMouOperations.CreateMouDongChengFacts().MayActivate(owner.Seat);
    internal bool MouDcParent(ProgramSkillFrame f)
    {
        if (f.MouDongChengSettlement is not null)
            return _characterDongChengMouOperations.CreateMouDcLightningFacts().SettlementParent(f);
        if (f.TriggerId is null)
            return f.SkillId == MouDongChengRuntime.Douwei && f.ActivationId == "discard-damage-use" && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == f.OwnerSeat;
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != w.ParentFrameId)
            return false;
        if (f.TriggerId == "opening-link")
            return w.Window == SkillProgramTriggerWindow.GameStarting && _resolutionStackOperations._resolutionStack[i - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } life && life.CandidateIndex >= 0 && life.CandidateIndex < life.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, life.Candidates[life.CandidateIndex]);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0);
        return f.TriggerId == "first-card-damage" ? MouDcBefore(c, w)is not null || f.MouDongCheng is { Occurrence: { } o } && _resolutionStackOperations._resolutionStack[i - 1] is BeforeDamageProgramWindowFrame { MouDongChengOccurrence: { } original } before && original == o && original.Target == w.TargetSeat && before.CandidateIndex >= 0 && before.CandidateIndex < before.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, before.Candidates[before.CandidateIndex].Candidate) && MouDcHasPaidClaim(f, o, before.Id) : f.TriggerId == "claim-finished" && MouDcClaimUse(c, w, settled: f.MouDongCheng is not null)is not null;
    }

    internal MouDcOccurrence? ObserveMouDcCardDamage()
    {
        if (!HasMouDongCheng || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not global::CardGame.Core.NativeOwners.CardAttackHandle { IsProgramSkillDamage: false, IsProgramJudgmentDamage: false } a || a.EffectiveCardKind is null)
            return null;
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId);
        if (u?.Action is not { Type: CardActionType.Use } && !a.IsDelayedJudgmentDamage)
            return null;
        // The native Use owns its appearance even when it has zero physical
        // materials (for example Qizheng); such an appearance is not a zone entity.
        var card = u?.CardId ?? _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == a.ResolutionId)?.DelayedCardId ?? 0;
        return MouDongCheng.ObserveDamage(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, a.ResolutionId, u?.Id ?? 0, card, a.TargetSeat);
    }

    internal MouDcOccurrence? MouDcBefore(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouDongChengRuntime.Baojia || c.BindingId != "first-card-damage" || w.Window != SkillProgramTriggerWindow.BeforeDamageApplied || w.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(x => x.Id == w.ParentFrameId)is not { Prevented: false, Amount: > 0, MouDongChengOccurrence: { First: true } o } b || b.TargetSeat != o.Target || w.TargetSeat != o.Target || o.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !MouDcDamageParent(o) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].MouDongChengLinks?.Any(l => l.Source.SkillId == c.SkillId && l.Source.SkillInstanceId == c.SkillInstanceId && (o.Target == c.OwnerSeat || o.Target == l.Other)) != true || !Enum.GetValues<EquipmentSlot>().Any(s => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].EquipmentSlotCapacity(s) > 0) || b.CandidateIndex < 0 || b.CandidateIndex >= b.Candidates.Count)
            return null;
        var actual = b.Candidates[b.CandidateIndex].Candidate;
        return actual.OwnerSeat == c.OwnerSeat && actual.SkillId == c.SkillId && actual.BindingId == c.BindingId && actual.SkillInstanceId == c.SkillInstanceId && actual.GameplayHash == c.GameplayHash ? o : null;
    }

    internal CardUseFrame? MouDcClaimUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool settled = false)
    {
        if (c.SkillId != MouDongChengRuntime.Baojia || c.BindingId != "claim-finished" || w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, Action: { } a, MouDongChengClaims: { } claims } u || a.ActionId != info.CardActionId || !claims.Any(p => p.Source.OwnerSeat == c.OwnerSeat && p.Source.SkillInstanceId == c.SkillInstanceId && p.GameplayHash == c.GameplayHash && (settled || !p.Settled)) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(x => x.Id == w.ParentFrameId)is not { } window || window.ParentFrameId != u.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var active = window.Candidates[window.CandidateIndex];
        return active.OwnerSeat == c.OwnerSeat && active.SkillId == c.SkillId && active.TriggerId == c.BindingId && active.SkillInstanceId == c.SkillInstanceId && active.GameplayHash == c.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext(window, active) == w && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id) == 1 ? u : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 37400)]
    internal bool MouDcIssuedClaim(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouDcClaimUse(c, w)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 37400)]
    internal void MouDcClaimCandidates(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MouDongChengClaims: { } claims } u || u.Action?.ActionId != a.ActionId)
            return;
        foreach (var p in claims.Where(p => !p.Settled && _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive).DistinctBy(p => (p.Source.OwnerSeat, p.Source.SkillInstanceId)))
        {
            if (result.Any(c => c.OwnerSeat == p.Source.OwnerSeat && c.SkillId == p.Source.SkillId && c.SkillInstanceId == p.Source.SkillInstanceId && c.TriggerId == "claim-finished"))
                continue;
            result.Add(new(p.Source.OwnerSeat, p.Target, p.Source.SkillId, "claim-finished", p.GameplayHash, p.Source.SkillInstanceId, 0, _coreCardActionsOperations.CreateCardActionProgramContext(a, window, 0, p.Source.OwnerSeat, p.Target, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat], a))));
        }
    }

    void global::CardGame.Core.IMouDongChengNative.Finish(ProgramSkillFrame f)
    {
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        if (f.MouDongChengSettlement is { } s)
            MouDongCheng.DelayedSettlementReturned(s);
    }

    void global::CardGame.Core.IMouDongChengNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouDongChengNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouDongChengNative.Link(ProgramSkillFrame f, int other)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.Opening } r || other == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[other].IsAlive)
            throw new InvalidOperationException("Baojia requires one actual other living opening role.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        owner.MouDongChengLinks = Array.AsReadOnly((owner.MouDongChengLinks ?? []).Where(l => l.Source.SkillInstanceId != f.SkillInstanceId).Append(new(r.Source, other)).ToArray());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcLinkedEvent(f.Id, r.Source, other));
    }

    void global::CardGame.Core.IMouDongChengNative.Protect(ProgramSkillFrame f) => MouDcProtect(f);
    void global::CardGame.Core.IMouDongChengNative.Claim(ProgramSkillFrame f) => MouDcClaim(f);
    void global::CardGame.Core.IMouDongChengNative.Discard(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.CostChildren } r || !_characterDongChengMouOperations.CreateMouDongChengFacts().Costs(f.OwnerSeat).Any(c => c.Id == r.CardId && c.Kind == r.Kind))
            throw new InvalidOperationException("Douwei requires its exact real discardable damage-card cost.");
        var from = _cardZonesOperations._cardZones.GetLocation(r.CardId);
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [r.CardId], new("skill-program.mou:douwei.cost"), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != r.CardId || m.From != from || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Douwei's real logical discard invoice changed.");
            var own = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, own with { MouDongCheng = own.MouDongCheng!with { Actual = 1, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcMovementPaidEvent(f.Id, r.CardId, 1, r.Before, m.Sequence));
        });
    }

    void global::CardGame.Core.IMouDongChengNative.Use(ProgramSkillFrame f) => IssueMouDcUse(f);
    void global::CardGame.Core.IMouDongChengNative.Recover(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.RecoveryChildren, Returned: true, Dying.Count: 1 } r || (f.MouDongChengSettlement is { Reward: true } s ? !_characterDongChengMouOperations.CreateMouDcLightningFacts().SettlementParent(f) || s.DyingId != r.Dying[0] : r.Native is not { } n || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDcDyingEvent>().Count(x => x.FrameId == f.Id && x.UseId == n.UseId) != 1))
            throw new InvalidOperationException("Douwei's once-per-whole-use reward needs its actual first causal dying return.");
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
    }

    void global::CardGame.Core.IMouDongChengNative.Restore(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.RestoreChildren, Slot: { } slot } || !_coreAdvancedLifecycleOperations.RestoreEquipmentSlot(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot))
            throw new InvalidOperationException("Douwei requires one actual abolished equipment slot.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcRestoredEvent(f.Id, slot));
    }

    void global::CardGame.Core.IMouDongChengNative.Disable(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.NativeUse, Dying.Count: 1 })
            throw new InvalidOperationException("Only the first real causal dying may disable Douwei.");
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MouDongChengDisabledTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;
    }

    internal void MouDcProtect(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.SlotChildren, Slot: { } slot, Occurrence: { } o } r || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].EquipmentSlotCapacity(slot) <= 0 || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(x => x.Id == r.ParentId)is not { Prevented: false } b || b.MouDongChengOccurrence != o || !MouDcDamageParent(o) || MouDcHasPaidClaim(f, o, b.Id))
            throw new InvalidOperationException("Baojia must pay a real still-enabled slot in its one exact first card-damage window.");
        var claim = new MouDcClaim(f.Id, r.Source, r.GameplayHash, r.Turn, b.Id, o.AttackId, o.UseId, _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)?.Action?.ActionId ?? 0, o.Target, slot);
        if (o.UseId > 0)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(o.UseId, u => u with { MouDongChengClaims = (u.MouDongChengClaims ?? []).Append(claim).ToArray() });
        else
        {
            var judgment = MouDcDelayedDamage(o)!;
            _coreRuntimeOperations.ReplaceRuntimeFrame(judgment.Id, judgment with { MouDongChengClaims = (judgment.MouDongChengClaims ?? []).Append(claim).ToArray() });
        }

        _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot, 0);
        var own = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, own with { MouDongCheng = own.MouDongCheng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcSlotPaidEvent(f.Id, b.Id, o.Target, slot, o.UseId));
    }

    internal void MouDcClaim(ProgramSkillFrame f)
    {
        if (f.MouDongChengSettlement is not null)
        {
            ClaimMouDcDelayedCard(f);
            return;
        }

        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.ClaimChildren } r || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Step: ResolutionFrameStep.Completed, Action: { } a, MouDongChengClaims: { } claims } use)
            throw new InvalidOperationException("Baojia may obtain only its original fully resolved card.");
        var pending = claims.Where(p => p.Source.OwnerSeat == f.OwnerSeat && p.Source.SkillInstanceId == f.SkillInstanceId && !p.Settled).ToArray();
        if (pending.Length == 0)
            throw new InvalidOperationException("Baojia cannot repeat an already settled whole-use claim.");
        var ids = a.PhysicalCards.Select(c => c.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, id)).ToArray();
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouDongChengClaims = claims.Select(p => pending.Contains(p) ? p with { Settled = true, Before = r.Before, After = r.Before, Cards = ids } : p).ToArray() });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou:baojia.claim"), (_, moves) =>
            {
                if (moves.Count != ids.Length || moves.Any(m => !ids.Contains(m.CardId) || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame))
                    throw new InvalidOperationException("Baojia's completed original material claim lost its genuine movement.");
                var after = moves.Max(m => m.Sequence);
                var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouDongCheng = parent.MouDongCheng!with { After = after, Actual = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)) } });
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouDongChengClaims = u.MouDongChengClaims!.Select(p => pending.Any(old => old.ProtectionFrameId == p.ProtectionFrameId) ? p with { After = after } : p).ToArray() });
            });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcClaimSettledEvent(f.Id, use.Id, ids.Length));
    }

    internal bool IsMouDcClaimedEntity(long useId, int card) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } a, MouDongChengClaims: { } claims } u && claims.Any(p => p.Settled && p.UseId == u.Id && p.ActionId == a.ActionId && p.Cards.Contains(card) && p.After > p.Before && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == card && m.Sequence > p.Before && m.Sequence <= p.After && m.To == CardLocation.Hand(p.Source.OwnerSeat) && m.Reason.Value == "skill-program.mou:baojia.claim") == 1);
}
