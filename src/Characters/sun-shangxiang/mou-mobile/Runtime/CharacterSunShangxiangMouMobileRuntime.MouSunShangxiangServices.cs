#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShangxiangMouMobileRuntime
{
    internal bool HasMouSunShangxiangMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouSunShangxiangJieyin) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouSunShangxiangLiangzhu);
    internal MouSunShangxiangRuntime MouSunShangxiang => _mouSunShangxiangOperations.Get();

    internal const string MouSsxGiveReason = "skill-program.mou-mobile:jieyin.give", MouSsxDepositReason = "skill-program.mou-mobile:liangzhu.zhuang", MouSsxObtainReason = "skill-program.mou-mobile:jieyin.failure-obtain", MouSsxDiscardReason = "skill-program.mou-mobile:xiaoji.discard";
    bool global::CardGame.Core.IMouSunShangxiangFacts.GameOver => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    long global::CardGame.Core.IMouSunShangxiangFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouSunShangxiangFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouSunShangxiangFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouSunShangxiangFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string? global::CardGame.Core.IMouSunShangxiangFacts.Faction(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]);
    SkillProgram global::CardGame.Core.IMouSunShangxiangFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IReadOnlyList<int> global::CardGame.Core.IMouSunShangxiangFacts.Living => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    IReadOnlyList<Card> global::CardGame.Core.IMouSunShangxiangFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    CardLocation global::CardGame.Core.IMouSunShangxiangFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    IReadOnlyList<MouSsxEntity> global::CardGame.Core.IMouSunShangxiangFacts.Field(int owner, bool equipmentOnly) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (!equipmentOnly || p.Seat != owner)).SelectMany(p => (equipmentOnly ? new[] { CardZoneKind.Equipment } : new[] { CardZoneKind.Equipment, CardZoneKind.Judgment }).SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, p.Seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, new(zone, p.Seat), equipmentOnly ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Discard)).Select(c => new MouSsxEntity(c.Id, c.Kind, new(zone, p.Seat))))).ToArray());
    IReadOnlyList<MouSsxEntity> global::CardGame.Core.IMouSunShangxiangFacts.Zhuang(int owner) => Array.AsReadOnly(_corePublicPileSourcesOperations.PublicPileSources(owner).Where(p => p.SkillId == MouSunShangxiangRuntime.Liangzhu).SelectMany(p => _corePublicPileSourcesOperations.PublicPileCards(p).Select(c => new MouSsxEntity(c.Id, c.Kind, p.Location))).ToArray());
    IEnumerable<T> global::CardGame.Core.IMouSunShangxiangFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouSunShangxiangFacts.ParentMatches(ProgramSkillFrame f) => MouSsxParentMatches(f);
    bool global::CardGame.Core.IMouSunShangxiangFacts.MovementRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouSsx is not { Paid: true, Stage: MouSsxStage.DepositChildren or MouSsxStage.DiscardChildren } r || r.Entities is not [{ Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } entity])
            return false;
        var seat = entity.From.OwnerSeat!.Value;
        var reason = r.Stage == MouSsxStage.DepositChildren ? MouSsxDepositReason : MouSsxDiscardReason;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == entity.CardId && m.CardKind == CardKind.SilverLion && m.From == entity.From && m.Reason.Value == reason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.ResolutionId > f.Id && x.ResolutionId < hp.Change.Id && x.PlayerSeat == seat && x.Reason.Value == reason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter);
    }

    double global::CardGame.Core.IMouSunShangxiangFacts.HelpValue(int actor, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[actor].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false));
    double global::CardGame.Core.IMouSunShangxiangFacts.KeepValue(int actor, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[actor]);
    void global::CardGame.Core.IMouSunShangxiangNative.Mark(ProgramSkillFrame f, int previous, int current, bool initial) => ChangeMouSsxZhu(f, previous, current, initial);
    void global::CardGame.Core.IMouSunShangxiangNative.Give(ProgramSkillFrame f, int payer, IReadOnlyList<int> ids) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouSsxGiveReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, payer, f.OwnerSeat, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, moves) =>
    {
        foreach (var move in moves)
            SealMouSsxMove(f.Id, move, false);
    }), disclosure: CardMovementDisclosure.FaceDown);
    void global::CardGame.Core.IMouSunShangxiangNative.Deposit(ProgramSkillFrame f, MouSsxEntity entity)
    {
        if (!_characterSunShangxiangMouMobileOperations.CreateMouSunShangxiangFacts().Field(f.OwnerSeat, true).Contains(entity))
            throw new InvalidOperationException("Liangzhu lost its actual foreign Equipment entity or transfer permission.");
        var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue);
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), entity.From, pile.Location, new(MouSsxDepositReason), m => SealMouSsxMove(f.Id, m, true), disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouSunShangxiangNative.ObtainZhuang(ProgramSkillFrame f, IReadOnlyList<MouSsxEntity> entities)
    {
        var live = entities.Where(x => _cardZonesOperations._cardZones.GetLocation(x.CardId) == x.From).Select(x => x.CardId).ToArray();
        if (live.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(live, CardLocation.Hand(f.OwnerSeat), new(MouSsxObtainReason), (_, moves) =>
            {
                foreach (var move in moves)
                    SealMouSsxMove(f.Id, move, true);
            }, disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouSunShangxiangNative.Discard(ProgramSkillFrame f, MouSsxEntity entity)
    {
        if (!_characterSunShangxiangMouMobileOperations.CreateMouSunShangxiangFacts().Field(f.OwnerSeat, false).Contains(entity))
            throw new InvalidOperationException("Xiaoji lost its current legal field entity.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, entity.From.OwnerSeat!.Value, [entity.CardId], new(MouSsxDiscardReason), (_, moves) =>
        {
            foreach (var m in moves)
                SealMouSsxMove(f.Id, m, true);
        });
    }

    void global::CardGame.Core.IMouSunShangxiangNative.Armor(ProgramSkillFrame f, int seat) => GrantMouSsxArmor(f, seat);
    void global::CardGame.Core.IMouSunShangxiangNative.Recover(ProgramSkillFrame f, int seat) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, seat, 1, null, null);
    void global::CardGame.Core.IMouSunShangxiangNative.Faction(ProgramSkillFrame f) => ChangeMouSsxFailureFaction(f);
    void global::CardGame.Core.IMouSunShangxiangNative.MaximumHp(ProgramSkillFrame f) => _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
    bool global::CardGame.Core.IMouSunShangxiangNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    void global::CardGame.Core.IMouSunShangxiangNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouSunShangxiangNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    internal bool MouSsxParentMatches(ProgramSkillFrame f)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (f.SkillId == MouSunShangxiangRuntime.Liangzhu)
            return f.TriggerId is null && f.ActivationId == "deposit-zhuang" && f.WindowContext is null && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0;
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != context.ParentFrameId)
            return false;
        ProgramTriggerCandidate? candidate = _resolutionStackOperations._resolutionStack[at - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == context.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            PlayPhaseStartingBoundaryFrame p when p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            ProgramKillTriggerWindowFrame p when context.Window == SkillProgramTriggerWindow.CharacterDied && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && p.VictimSeat == context.TargetSeat && p.KillerSeat == context.SourceSeat => p.Candidates[p.CandidateIndex],
            CardsMovedTriggerWindowFrame p when context.Window == SkillProgramTriggerWindow.CardsMoved && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            _ => null
        };
        return candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    internal void ChangeMouSsxZhu(ProgramSkillFrame f, int previous, int current, bool initial)
    {
        if (!MouSunShangxiang.Valid(f) || f.SkillId != MouSunShangxiangRuntime.Jieyin || f.MouSsx is not { } r || previous != r.HolderSeat || current == previous || current >= 0 && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(current) || !_coreMatchParticipantsPlayersAccess._players[current].IsAlive || current == f.OwnerSeat))
            throw new InvalidOperationException("Jieyin must change its exact source-owned current holder.");
        void Change(int seat, int delta)
        {
            var p = _coreMatchParticipantsPlayersAccess._players[seat];
            var key = (PlayerMarkerKind.MouSunShangxiangZhu, f.OwnerSeat);
            var old = p.MarkerSourceCounts.GetValueOrDefault(key);
            var next = old + delta;
            if (next < 0 || next > 1)
                throw new InvalidOperationException("Jieyin's one source cannot duplicate or repay its named Zhu.");
            if (next == 0)
                p.MarkerSourceCounts.Remove(key);
            else
                p.MarkerSourceCounts[key] = next;
            var total = p.Markers.GetValueOrDefault(PlayerMarkerKind.MouSunShangxiangZhu) + delta;
            if (total == 0)
                p.Markers.Remove(PlayerMarkerKind.MouSunShangxiangZhu);
            else
                p.Markers[PlayerMarkerKind.MouSunShangxiangZhu] = total;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, seat, PlayerMarkerKind.MouSunShangxiangZhu, delta, total, f.OwnerSeat, "skill-program.mou-mobile:jieyin.zhu"));
        }

        if (previous >= 0 && _coreMatchParticipantsPlayersAccess._players[previous].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.MouSunShangxiangZhu, f.OwnerSeat)) > 0)
            Change(previous, -1);
        if (current >= 0)
            Change(current, 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSsxZhuChangedEvent(f.Id, f.OwnerSeat, previous, current, initial));
    }

    internal void SealMouSsxMove(long id, CardMovementRecord move, bool isPublic)
    {
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == id);
        var r = f.MouSsx!;
        if (!MouSunShangxiang.Valid(f) || move.Sequence <= r.Before || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move))
            throw new InvalidOperationException("Sun Shangxiang lost her real movement receipt.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouSsx = r with { After = move.Sequence } });
        if (isPublic)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSsxMovedEvent(id, move.CardId, move.From, move.To, move.Sequence));
    }

    internal void GrantMouSsxArmor(ProgramSkillFrame f, int seat)
    {
        if (f.MouSsx is not { Stage: MouSsxStage.GiftChildren, Paid: true } r || !MouSunShangxiang.Valid(f) || r.HolderSeat != seat || _coreProgramEventHistoryOperations.ProgramEventHistory<ArmorGrantedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Jieyin cannot grant armor twice or to a different giver.");
        if (_coreWushuangModeProjectionOperations.IsWushuangMode || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || r.Selected.Any(id => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.Hand(seat) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouSsxGiveReason)))
            return;
        var before = _coreMatchParticipantsPlayersAccess._players[seat].Armor;
        _coreMatchParticipantsPlayersAccess._players[seat].Armor = checked(before + 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(f.Id, f.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), seat, 1, before, before + 1));
    }

    internal bool IsIssuedMouSsxZhuang(PublicPersistentPileSource source) => source.SkillId == MouSunShangxiangRuntime.Liangzhu && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OwnerSeat == source.OwnerSeat && f.MouSsx is { Paid: true, Stage: MouSsxStage.FailureRecoverChildren or MouSsxStage.FailureObtainChildren } && MouSunShangxiang.Valid(f));
    internal void ChangeMouSsxFailureFaction(ProgramSkillFrame f)
    {
        if (!_coreNativeFactionChangesOperations.TracksNativeEffectiveFactionChanges || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !MouSunShangxiang.Valid(f) || f.SkillId != MouSunShangxiangRuntime.Jieyin || f.MouSsx is not { Stage: MouSsxStage.FailureFaction, Paid: true } || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouSsxMissionFailedEvent>().Any(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Jieyin faction change requires its actual once-paid failed mission parent.");
        _coreNativeFactionChangesOperations.SynchronizeNativeEffectiveFactions();
        var p = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var previous = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p);
        p.NativeEffectiveFactionOverrideId = "wu";
        p.NativeEffectiveFactionWasDeclared = true;
        _coreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionObservations[p.Seat] = new("wu", _coreNativeFactionChangesOperations.CaptureNativeFactionAvatar(p));
        if (previous == "wu")
            return;
        _coreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, p.Seat, previous, "wu", f.Id, MouSunShangxiangRuntime.Source(f), "jieyin-failure"));
    }
}
