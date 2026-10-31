#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangMouRuntime
{
    internal MouCaoShuangAnniversaryRuntime MouCaoShuangAnniversary => _mouCaoShuangAnniversaryOperations.Get();

    int global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IMouCaoShuangAnniversaryFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IMouCaoShuangAnniversaryFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    long global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Keep(int seat, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    IReadOnlyList<int> global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    SkillProgram global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> global::CardGame.Core.IMouCaoShuangAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != w.ParentFrameId)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[i - 1];
        if (parent is ProgramCardTriggerWindowFrame card)
        {
            if (card.CandidateIndex < 0 || card.CandidateIndex >= card.Candidates.Count || w.CardUse?.CardActionId != card.Action.ActionId)
                return false;
            var c = card.Candidates[card.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        var candidate = parent switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            TurnEndingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            _ => null
        };
        return candidate is not null && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    }

    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.ActualPlay(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p);
    MouCaoShuangAnniversaryUse? global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActionId != info.CardActionId || p.Action.ActorSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return null;
        var candidate = p.Candidates[p.CandidateIndex];
        if (candidate.OwnerSeat != c.OwnerSeat || candidate.SkillId != c.SkillId || candidate.TriggerId != c.BindingId || candidate.SkillInstanceId != c.SkillInstanceId || candidate.GameplayHash != c.GameplayHash)
            return null;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId);
        if (w.Window == SkillProgramTriggerWindow.CardUseCommitted)
            return new(p.Id, p.Action, use?.Id ?? info.ParentCardUseFrameId, -1, p.Action.EffectiveIsRed);
        if (w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || use is null || use.TargetSeats is not [var target] || target == c.OwnerSeat || !((global::CardGame.Core.IMouCaoShuangAnniversaryFacts)this).Alive(target))
            return null;
        return new(p.Id, p.Action, use.Id, target, p.Action.EffectiveIsRed ?? (p.Action.EffectiveSuit is { } suit && suit != Suit.None ? suit is Suit.Heart or Suit.Diamond : null));
    }

    IReadOnlyList<MouCaoShuangAnniversaryCard> global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Payable(int payer, bool recast) => !((global::CardGame.Core.IMouCaoShuangAnniversaryFacts)this).Alive(payer) ? [] : Array.AsReadOnly((recast ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, payer)).Where(c => recast || !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(payer, c, new(z, payer), OwnedCardMoveIntent.Discard)).Select(c => new MouCaoShuangAnniversaryCard(c.Id, c.Kind, new(z, payer), c.DisplayName))).ToArray());
    bool global::CardGame.Core.IMouCaoShuangAnniversaryFacts.Hurt(int source, int victim) => _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Any(d => !d.SourceLess && d.Amount > 0 && d.SourceSeat == source && d.TargetSeat == victim);
    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool global::CardGame.Core.IMouCaoShuangAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Move(ProgramSkillFrame f)
    {
        if (!MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Stage: MouCaoShuangAnniversaryStage.MoveChildren, Cards.Count: > 0 } r || r.Cards.Any(c => !_characterCaoShuangMouOperations.CreateMouCaoShuangAnniversaryFacts().Payable(r.Payer, r.Option == 3).Contains(c)))
            throw new InvalidOperationException("Jianzhuan requires the actual chooser's frozen available entities.");
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> records)
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouCaoShuangAnniversary!;
            if (records.Count != r.Cards.Count || !records.Zip(r.Cards).All(pair => pair.First.CardId == pair.Second.Id && pair.First.CardKind == pair.Second.Kind && pair.First.From == pair.Second.From && pair.First.Sequence > r.Before && (pair.First.To == CardLocation.DiscardPile || pair.First.To == CardLocation.OutsideGame)))
                throw new InvalidOperationException("Jianzhuan lost its actual discard or recast entities.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouCaoShuangAnniversary = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var m in records)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryMaterialEvent(f.Id, r.Repeat, r.Payer, m.CardId, m.From, m.Sequence, r.Option == 3));
        }

        if (r.Option == 3)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, r.Payer, ids, r.Repeat));
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, Paid, disclosure: CardMovementDisclosure.FaceUp);
        }
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, ids, new(MouCaoShuangAnniversaryRuntime.DiscardReason), Paid);
    }

    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.CompleteRecast(ProgramSkillFrame f, int actual)
    {
        var r = f.MouCaoShuangAnniversary!;
        if (!MouCaoShuangAnniversary.Valid(f) || r.Option != 3 || r.Stage != MouCaoShuangAnniversaryStage.DrawChildren || r.Draw?.ActualDrawIds.Count != actual)
            throw new InvalidOperationException("Jianzhuan recast requires its actual typed draw result.");
        for (var i = 0; i < r.Cards.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(r.Payer, r.Cards[i].Id, r.Cards[i].Kind, i < actual ? 1 : 0));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, actual);
    }

    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Delete(ProgramSkillFrame f)
    {
        if (!MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Operation: SkillProgramEffectOp.DeleteMouCaoShuangJianzhuanOption } r)
            throw new InvalidOperationException("Jianzhuan deletion requires its actual completed Play.");
        var items = MouCaoShuangAnniversaryRuntime.Items(r.Remaining);
        var before = _randomOperations._random.State;
        var item = items[_randomOperations._random.Next(items.Length)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryDeletedEvent(f.Id, f.OwnerSeat, r.Turn, r.PhaseId, item, r.Remaining, r.Remaining & ~(1 << (item - 1)), before, _randomOperations._random.State));
    }

    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Maximum(ProgramSkillFrame f) => _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 2);
    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Recover(ProgramSkillFrame f) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 2, null, null);
    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.Grant(ProgramSkillFrame f)
    {
        if (!MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Stage: MouCaoShuangAnniversaryStage.Grant })
            throw new InvalidOperationException("Fanshi grant requires its completed native repeats and recovery.");
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == MouCaoShuangAnniversaryRuntime.Jianzhuan).ToArray())
            owner.SkillGrants.RemoveGrant(grant.GrantId);
        _coreCharacterHostOperations.AcquireRuntimeSkills(owner, f.SkillId, [MouCaoShuangAnniversaryRuntime.Fudou]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryGrantedEvent(f.Id, f.OwnerSeat));
    }

    void global::CardGame.Core.IMouCaoShuangAnniversaryNative.LoseHp(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
}
