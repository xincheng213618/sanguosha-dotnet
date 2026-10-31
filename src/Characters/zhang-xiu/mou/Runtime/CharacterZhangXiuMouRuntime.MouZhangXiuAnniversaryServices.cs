#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouRuntime
{
    internal MouZhangXiuAnniversaryRuntime MouZhangXiuAnniversaryRules => _mouZhangXiuAnniversaryOperations.Get();
    internal bool HasMouZhangXiuAnniversary => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouZhangXiuHaoyi);

    internal bool ValidMouZhangXiuAnniversary(ProgramSkillFrame f) => MouZhangXiuAnniversaryRules.Valid(f);
    internal void ContinueMouZhangXiuAnniversary(long id) => MouZhangXiuAnniversaryRules.Continue(id);
    int global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    string global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    int global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Keep(int owner, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[owner]);
    SkillProgram global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[n - 1] switch
        {
            PlayPhaseStartingBoundaryFrame p => f.SkillId == MouZhangXiuAnniversaryRuntime.Fuxi && f.TriggerId == "other-largest-hand" && w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && ((global::CardGame.Core.IMouZhangXiuAnniversaryFacts)this).PlayActor(w) == p.OwnerSeat && p.OwnerSeat != f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            TurnEndingBoundaryFrame p => f.SkillId == MouZhangXiuAnniversaryRuntime.Haoyi && f.TriggerId == "turn-damage-card-claim" && w.Window == SkillProgramTriggerWindow.TurnEnding && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            _ => false
        };
    }

    bool global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Largest(int seat) => ((global::CardGame.Core.IMouZhangXiuAnniversaryFacts)this).Alive(seat) && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count);
    int global::CardGame.Core.IMouZhangXiuAnniversaryFacts.PlayActor(ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is PlayPhaseStartingBoundaryFrame p && w.SourceSeat == p.OwnerSeat && w.TargetSeat == p.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == p.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play ? p.OwnerSeat : -1;
    IReadOnlyList<int> global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Others(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).Select(p => p.Seat).ToArray();
    IReadOnlyList<MouZhangXiuAnniversaryCard> global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Cards(int actor, int holder, bool discard) => !((global::CardGame.Core.IMouZhangXiuAnniversaryFacts)this).Alive(actor) || !((global::CardGame.Core.IMouZhangXiuAnniversaryFacts)this).Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, holder)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (actor != holder || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(holder, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer))).Select(c => new MouZhangXiuAnniversaryCard(c.Id, c.Kind, new(z, holder), c.DisplayName))).ToArray());
    IReadOnlyList<MouZhangXiuAnniversaryCard> global::CardGame.Core.IMouZhangXiuAnniversaryFacts.Entities(IEnumerable<int> ids) => Array.AsReadOnly(ids.Distinct().Select(id =>
    {
        var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return new MouZhangXiuAnniversaryCard(c.Id, c.Kind, _cardZonesOperations._cardZones.GetLocation(id), c.DisplayName);
    }).ToArray());
    IEnumerable<T> global::CardGame.Core.IMouZhangXiuAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    void global::CardGame.Core.IMouZhangXiuAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouZhangXiuAnniversaryNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouZhangXiuAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouZhangXiuAnniversaryNative.Move(ProgramSkillFrame f)
    {
        if (!ValidMouZhangXiuAnniversary(f) || f.MouZhangXiuAnniversary is not { Cards.Count: > 0 } r || r.Stage is not (MouZhangXiuAnniversaryStage.GiveChildren or MouZhangXiuAnniversaryStage.DiscardChildren or MouZhangXiuAnniversaryStage.ObtainChildren or MouZhangXiuAnniversaryStage.DistributionChildren))
            throw new InvalidOperationException("Mou Zhang Xiu requires his exact issued physical movement segment.");
        var discard = r.Stage == MouZhangXiuAnniversaryStage.DiscardChildren;
        var obtain = r.Stage == MouZhangXiuAnniversaryStage.ObtainChildren;
        var distribute = r.Stage == MouZhangXiuAnniversaryStage.DistributionChildren;
        var recipient = distribute ? r.Recipient : obtain ? f.OwnerSeat : r.Target;
        var destination = discard ? CardLocation.DiscardPile : CardLocation.Hand(recipient);
        if (r.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != c.From) || obtain && r.Cards.Any(c => c.From != CardLocation.DiscardPile) || distribute && (r.Cards.Count != 1 || r.Cards.Any(c => c.From != CardLocation.Hand(f.OwnerSeat) || !r.Gained.Contains(c.Id) || r.Given.Contains(c.Id))) || !obtain && !distribute && r.Cards.Any(c => !_characterZhangXiuMouOperations.CreateMouZhangXiuAnniversaryFacts().Cards(f.OwnerSeat, discard ? r.Target : f.OwnerSeat, discard).Contains(c)))
            throw new InvalidOperationException("Mou Zhang Xiu cannot pay with another entity, unacquired card or different region.");
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            if (moves.Any(m => m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Zhang Xiu native movement changed its actual destination.");
            MouZhangXiuAnniversaryRules.MovesPaid(f.Id, moves);
            if (!obtain && !discard)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, f.OwnerSeat, recipient, ids, r.Before);
        }

        var reason = new CardMoveReason(discard ? MouZhangXiuAnniversaryRuntime.DiscardReason : obtain ? MouZhangXiuAnniversaryRuntime.ClaimReason : distribute ? MouZhangXiuAnniversaryRuntime.DistributionReason : MouZhangXiuAnniversaryRuntime.GiveReason);
        if (discard)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, ids, reason, Paid);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, Paid);
    }

    MouZhangXiuAnniversaryCard global::CardGame.Core.IMouZhangXiuAnniversaryNative.RandomHand(ProgramSkillFrame f, IReadOnlyList<MouZhangXiuAnniversaryCard> cards)
    {
        if (!ValidMouZhangXiuAnniversary(f) || f.MouZhangXiuAnniversary is not { Stage: MouZhangXiuAnniversaryStage.DiscardCard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Fuxi requires the exact opposing anonymous-Hand selection.");
        var before = _randomOperations._random.State;
        var card = cards[_randomOperations._random.Next(cards.Count)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuAnniversaryHandChosenEvent(f.Id, r.Target, card.Id, before, _randomOperations._random.State));
        return card;
    }

    bool global::CardGame.Core.IMouZhangXiuAnniversaryNative.CanSlash(ProgramSkillFrame f) => CanMouZhangXiuAnniversarySlash(f);
    void global::CardGame.Core.IMouZhangXiuAnniversaryNative.Slash(ProgramSkillFrame f) => IssueMouZhangXiuAnniversarySlash(f);
}
