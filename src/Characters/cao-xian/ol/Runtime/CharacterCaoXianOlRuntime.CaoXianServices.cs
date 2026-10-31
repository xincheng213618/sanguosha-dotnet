#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXianOlRuntime
{
    internal CaoXianRuntime CaoXianRules => _caoXianOperations.Get();
    internal bool HasCaoXian => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreCaoXianLingxi);

    int global::CardGame.Core.ICaoXianFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long global::CardGame.Core.ICaoXianFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.ICaoXianFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.ICaoXianFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.ICaoXianFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.ICaoXianFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int global::CardGame.Core.ICaoXianFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int global::CardGame.Core.ICaoXianFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int global::CardGame.Core.ICaoXianFacts.Keep(int owner, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[owner]);
    bool global::CardGame.Core.ICaoXianFacts.Friendly(int owner, int other) => owner == other || (_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(owner, out var brain) ? brain.ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : _coreMatchParticipantsPlayersAccess._players[owner].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[other].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[owner].TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[other].TeamId == team);
    SkillProgram global::CardGame.Core.ICaoXianFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> global::CardGame.Core.ICaoXianFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<int> global::CardGame.Core.ICaoXianFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    bool global::CardGame.Core.ICaoXianFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != w.ParentFrameId)
            return false;
        if (_resolutionStackOperations._resolutionStack[at - 1] is ProgramCardTriggerWindowFrame card)
        {
            if (card.CandidateIndex < 0 || card.CandidateIndex >= card.Candidates.Count || w.CardUse?.CardActionId != card.Action.ActionId || ((global::CardGame.Core.ICaoXianFacts)this).Completed(w, f.OwnerSeat)is null)
                return false;
            var c = card.Candidates[card.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        var candidate = _resolutionStackOperations._resolutionStack[at - 1] switch
        {
            PlayPhaseStartingBoundaryFrame p when w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            CardsMovedTriggerWindowFrame p when w.Window == SkillProgramTriggerWindow.CardsMoved && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            _ => null
        };
        return candidate is not null && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    }

    CaoXianCompletedUse? global::CardGame.Core.ICaoXianFacts.Completed(ProgramSkillWindowContext w, int owner)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != owner || w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActorSeat != owner || info.CardActionId != p.Action.ActionId || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].OwnerSeat != owner)
            return null;
        return new(info.ParentCardUseFrameId, p.Action.ActionId);
    }

    IReadOnlyList<PublicPersistentPileSource> global::CardGame.Core.ICaoXianFacts.Piles(int owner, string consumerInstance) => Array.AsReadOnly(_corePublicPileSourcesOperations.ReferencedPublicPileSources(owner, CaoXianRuntime.Lingxi, consumerInstance).ToArray());
    PublicPersistentPileSource? global::CardGame.Core.ICaoXianFacts.RemovedPile(ProgramSkillWindowContext w, string instance)
    {
        if (w.Window != SkillProgramTriggerWindow.CardsMoved || w.MovementBatch is not { } batch || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not CardsMovedTriggerWindowFrame p || p.Id != batch.Id || !p.Batch.Movements.SequenceEqual(batch.Movements) || !_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.TryGetValue((w.OwnerSeat, CaoXianRuntime.Lingxi, instance), out var pile))
            return null;
        return batch.Movements.Any(m => m.From == pile.Location && m.To != m.From && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) ? pile : null;
    }

    IReadOnlyList<CaoXianCard> global::CardGame.Core.ICaoXianFacts.PileCards(PublicPersistentPileSource pile) => Array.AsReadOnly(_corePublicPileSourcesOperations.PublicPileSources(pile.OwnerSeat).Any(p => p == pile) ? _corePublicPileSourcesOperations.PublicPileCards(pile).Select(c => new CaoXianCard(c.Id, c.Kind, c.Suit, pile.Location, c.DisplayName)).ToArray() : []);
    IReadOnlyList<CaoXianCard> global::CardGame.Core.ICaoXianFacts.Owned(int owner, bool handOnly, bool discard) => !((global::CardGame.Core.ICaoXianFacts)this).Alive(owner) ? [] : Array.AsReadOnly((handOnly ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, owner)).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, new(z, owner), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (!discard || !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, new(z, owner), OwnedCardMoveIntent.Discard))).Select(c => new CaoXianCard(c.Id, c.Kind, c.Suit, new(z, owner), c.DisplayName))).ToArray());
    PublicPersistentPileSource global::CardGame.Core.ICaoXianNative.EnsurePile(ProgramSkillFrame f) => f.SkillId == CaoXianRuntime.Lingxi && f.CaoXian?.Operation == SkillProgramEffectOp.StoreCaoXianLingxi ? _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue) : throw new InvalidOperationException("Only the actual Lingxi depositor may create her source-specific wings.");
    void global::CardGame.Core.ICaoXianNative.Move(ProgramSkillFrame f)
    {
        if (!CaoXianRules.Valid(f) || f.CaoXian is not { Cards.Count: > 0 } r)
            throw new InvalidOperationException("Cao Xian requires this exact frozen paid native segment.");
        var remove = r.Stage == CaoXianStage.RemoveChildren;
        var discard = r.Stage == CaoXianStage.AdjustChildren || r.Option == 2;
        var payer = r.Operation == SkillProgramEffectOp.StoreCaoXianLingxi || r.Stage == CaoXianStage.AdjustChildren ? f.OwnerSeat : r.Target;
        var current = remove ? _characterCaoXianOlOperations.CreateCaoXianFacts().PileCards(r.Pile!) : _characterCaoXianOlOperations.CreateCaoXianFacts().Owned(payer, r.Stage == CaoXianStage.AdjustChildren, discard);
        if (r.Cards.Any(c => !current.Contains(c)))
            throw new InvalidOperationException("Cao Xian cannot move stale wings or another holder's unavailable HE entity.");
        var destination = remove ? CardLocation.OutsideGame : discard ? CardLocation.DiscardPile : r.Pile!.Location;
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            if (moves.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Cao Xian's native movement changed its admitted destination.");
            CaoXianRules.MovesPaid(f.Id, moves);
        }

        if (discard)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, payer, r.Cards.Select(c => c.Id).ToArray(), new("skill-program.ol:lingxi-or-zhifou.discard"), Paid);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards.Select(c => c.Id).ToArray(), destination, new(remove ? "skill-program.ol:zhifou.remove-wing" : "skill-program.ol:lingxi.store-wing"), Paid, disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.ICaoXianNative.LoseHp(ProgramSkillFrame f) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.CaoXian!.Target, 1);
    bool global::CardGame.Core.ICaoXianNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.ICaoXianNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.ICaoXianNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
}
