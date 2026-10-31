#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaShiMouRuntime
{
    internal MouSimaShiRuntime MouSimaShiRules => _mouSimaShiOperations.Get();
    internal bool HasMouSimaShi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeMouSimaShiSanshi);

    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 36400, SkillProgramEffectOp.ResolveMouSimaShiChenlve)]
    internal bool CanStartMouSimaShiChenlve(CharacterState owner) => MouSimaShiRules.CanActivate(owner.Seat);
    int global::CardGame.Core.IMouSimaShiFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IMouSimaShiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouSimaShiFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouSimaShiFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouSimaShiFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool global::CardGame.Core.IMouSimaShiFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    int global::CardGame.Core.IMouSimaShiFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    bool global::CardGame.Core.IMouSimaShiFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouSimaShiRuntime.Chenlve && f.ActivationId == "claim-dead-soldiers" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((global::CardGame.Core.IMouSimaShiFacts)this).History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.SkillId == f.SkillId && s.OwnerSeat == f.OwnerSeat && s.ActivationId == f.ActivationId) == 1;
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex);
        if (f.SkillId == MouSimaShiRuntime.Chenlve)
            return ((global::CardGame.Core.IMouSimaShiFacts)this).Due(candidate, w)is not null;
        if (f.SkillId == MouSimaShiRuntime.Zhenrao)
            return ((global::CardGame.Core.IMouSimaShiFacts)this).Use(candidate, w)is not null;
        return _resolutionStackOperations._resolutionStack[n - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            TurnEndingBoundaryFrame p => p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            _ => false
        };
    }

    SkillProgram global::CardGame.Core.IMouSimaShiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    ActualDiscardRecoveryPhaseKey? global::CardGame.Core.IMouSimaShiFacts.Phase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
    IReadOnlyList<MouSimaShiDeckCard> global::CardGame.Core.IMouSimaShiFacts.Deck() => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => new MouSimaShiDeckCard(c.Id, c.Rank)).ToArray());
    IReadOnlyList<MouSimaShiCard> global::CardGame.Core.IMouSimaShiFacts.Entities(IEnumerable<int> ids) => Array.AsReadOnly(ids.Distinct().Select(id =>
    {
        var p = _cardZonesOperations._cardZones.GetLocation(id);
        return new MouSimaShiCard(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, p);
    }).ToArray());
    bool global::CardGame.Core.IMouSimaShiFacts.Quarantined(MouSimaShiRemovedEvent removed) => _cardZonesOperations._cardZones.GetLocation(removed.CardId) == CardLocation.OutsideGame && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == removed.CardId)is { } actual && actual.Sequence == removed.Sequence && actual.To == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSimaShiMovedEvent>().Count(m => m.DebtId == removed.DebtId && m.CardId == removed.CardId && m.Sequence == actual.Sequence && m.Operation == SkillProgramEffectOp.RemoveMouSimaShiChenlve && m.To == actual.To) == 1;
    IEnumerable<T> global::CardGame.Core.IMouSimaShiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    MouSimaShiUse? global::CardGame.Core.IMouSimaShiFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.Type != CardActionType.Use || p.Action.ActionId != info.CardActionId || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != p.Action.ActionId)
            return null;
        var actual = p.Candidates[p.CandidateIndex];
        if (actual.OwnerSeat != c.OwnerSeat || actual.SkillId != c.SkillId || actual.TriggerId != c.BindingId || actual.SkillInstanceId != c.SkillInstanceId || actual.GameplayHash != c.GameplayHash)
            return null;
        return new(use.Id, a.ActionId, a.ActorSeat, Array.AsReadOnly(use.TargetSeats.ToArray()));
    }

    MouSimaShiClaimIssuedEvent? global::CardGame.Core.IMouSimaShiFacts.Due(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouSimaShiDue(c, w);
    void global::CardGame.Core.IMouSimaShiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouSimaShiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouSimaShiNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    (int Id, ulong Before, ulong After) global::CardGame.Core.IMouSimaShiNative.Random(IReadOnlyList<MouSimaShiDeckCard> cards)
    {
        var before = _randomOperations._random.State;
        var selected = cards[_randomOperations._random.Next(cards.Count)];
        return (selected.Id, before, _randomOperations._random.State);
    }

    void global::CardGame.Core.IMouSimaShiNative.Move(ProgramSkillFrame f, MouSimaShiCard card, CardLocation destination)
    {
        if (!MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.MovementChildren } r || r.Index < 1 || r.Cards[r.Index - 1] != card || _cardZonesOperations._cardZones.GetLocation(card.Id) != card.From)
            throw new InvalidOperationException("Mou Sima Shi needs his exact current physical movement.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], destination, new("skill-program.mou:sima-shi." + r.Operation), (_, moves) => MouSimaShiRules.MovesPaid(f.Id, moves), disclosure: card.From.Zone is CardZoneKind.DrawPile or CardZoneKind.Hand || destination == CardLocation.DrawPile || destination == CardLocation.OutsideGame ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouSimaShiNative.Damage(ProgramSkillFrame f)
    {
        if (!MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.Damage, Operation: SkillProgramEffectOp.ResolveMouSimaShiZhenrao } r || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            throw new InvalidOperationException("Zhenrao damage requires the accepted actual-use target.");
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 1, explicitSourceSeat: f.OwnerSeat);
    }

    void global::CardGame.Core.IMouSimaShiNative.Shuffle(ProgramSkillFrame f)
    {
        if (!MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.Shuffle, Operation: SkillProgramEffectOp.ReturnMouSimaShiChenlve } || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSimaShiShuffledEvent>().Any(s => s.FrameId == f.Id))
            throw new InvalidOperationException("Chenlve shuffles only after its actual death return movements.");
        var before = _randomOperations._random.State;
        _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSimaShiShuffledEvent(f.Id, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile), before, _randomOperations._random.State));
    }
}
