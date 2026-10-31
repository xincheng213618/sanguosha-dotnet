#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuLieMouRuntime
{
    internal MouHuLieRuntime MouHuLieRules => _mouHuLieOperations.Get();
    internal bool HasMouHuLie => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StartMouHuLieChuanyu);

    internal MouHuLieYu? CurrentMouHuLieYu(int owner, string instance) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieMarkedEvent>().Select(e => e.Yu).LastOrDefault(y => y.Source.OwnerSeat == owner && y.Source.SkillInstanceId == instance && y.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && _contentRegistryOperations._contentRegistry.GetSkill(y.Source.SkillId).Program?.GameplayHash == y.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieClearedEvent>().Any(e => e.MarkId == y.MarkId));
    [ProgramPipelineHook(ProgramPipelinePhase.CardEntitySkillLabel, 37500)]
    internal string? MouHuLieCardLabel(int cardId) => HasMouHuLie && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieMarkedEvent>().Any(e => e.Yu.CardId == cardId && CurrentMouHuLieYu(e.Yu.Source.OwnerSeat, e.Yu.Source.SkillInstanceId)?.MarkId == e.Yu.MarkId) ? "舆" : null;
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 37500)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMouHuLieRoundDebt(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (!HasMouHuLie || window != SkillProgramTriggerWindow.RoundEnding || !owner.IsAlive)
            return ordinary;
        var result = ordinary.ToList();
        foreach (var mark in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieMarkedEvent>().Select(e => e.Yu).Where(y => y.Source.OwnerSeat == owner.Seat && CurrentMouHuLieYu(owner.Seat, y.Source.SkillInstanceId)?.MarkId == y.MarkId))
        {
            var candidate = new ProgramTriggerCandidate(owner.Seat, MouHuLieRuntime.Chuanyu, "round-clear-and-slashes", mark.Source.SkillInstanceId, mark.GameplayHash, 0);
            if (!result.Contains(candidate))
                result.Add(candidate);
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void CaptureMouHuLieUsedDiscard(CardMovementBatchContext batch)
    {
        if (!HasMouHuLie || batch.ParentFrameId is not { } parent)
            return;
        foreach (var yu in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieMarkedEvent>().Select(e => e.Yu).Where(y => CurrentMouHuLieYu(y.Source.OwnerSeat, y.Source.SkillInstanceId)?.MarkId == y.MarkId))
            foreach (var move in batch.Movements.Where(m => m.CardId == yu.CardId && _coreNativeUsedDiscardOriginsOperations.IsNativeTrueUseDiscardMovement(batch, m)))
            {
                var fact = new MouHuLieUsedDiscardEvent(yu, batch.Id, parent, batch.TurnNumber, move.CardId, move.Sequence);
                if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieUsedDiscardEvent>().Contains(fact))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
            }
    }

    internal bool IsMouHuLieUsedDiscard(CardMovementBatchContext batch, CardMovementRecord move, MouHuLieYu yu) => batch.ParentFrameId is { } parent && move.CardId == yu.CardId && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && batch.Movements.Contains(move) && _cardZonesOperations._cardZones.GetLocation(move.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == move.CardId) == move && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieUsedDiscardEvent>().Count(e => e == new MouHuLieUsedDiscardEvent(yu, batch.Id, parent, batch.TurnNumber, move.CardId, move.Sequence)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 37500)]
    internal int[]? MouHuLieUsedIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => !features.HasOperation(SkillProgramEffectOp.RegiveMouHuLieChuanyu) ? null : CurrentMouHuLieYu(candidate.OwnerSeat, candidate.SkillInstanceId)is { } yu ? batch.Movements.Select((m, i) => (m, i)).Where(x => IsMouHuLieUsedDiscard(batch, x.m, yu)).Select(x => x.i).ToArray() : [];
    int global::CardGame.Core.IMouHuLieFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IMouHuLieFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IMouHuLieFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouHuLieFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouHuLieFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouHuLieFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string global::CardGame.Core.IMouHuLieFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    int global::CardGame.Core.IMouHuLieFacts.HandCount(int s) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count;
    int global::CardGame.Core.IMouHuLieFacts.Keep(int actor, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[actor]);
    bool global::CardGame.Core.IMouHuLieFacts.Friendly(int actor, int target) => actor == target || (_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(actor, out var brain) ? brain.ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(actor), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : _coreMatchParticipantsPlayersAccess._players[actor].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[target].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[actor].TeamId is { } t && _coreMatchParticipantsPlayersAccess._players[target].TeamId == t);
    SkillProgram global::CardGame.Core.IMouHuLieFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> global::CardGame.Core.IMouHuLieFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouHuLieFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != w.ParentFrameId)
            return false;
        var candidate = _resolutionStackOperations._resolutionStack[at - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            PlayPhaseStartingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            CardsMovedTriggerWindowFrame p when p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            _ => null
        };
        if (candidate is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        return w.Window == SkillProgramTriggerWindow.AfterDamageApplied && ((global::CardGame.Core.IMouHuLieFacts)this).Damage(new(f.OwnerSeat, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex), w)is not null;
    }

    IReadOnlyList<int> global::CardGame.Core.IMouHuLieFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<int> global::CardGame.Core.IMouHuLieFacts.ActionOrder(IEnumerable<int> seats) => Array.AsReadOnly(seats.Distinct().OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, s)).ToArray());
    IReadOnlyList<MouHuLieCard> global::CardGame.Core.IMouHuLieFacts.Owned(int owner, bool handOnly) => !((global::CardGame.Core.IMouHuLieFacts)this).Alive(owner) ? [] : Array.AsReadOnly((handOnly ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, owner)).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, new(z, owner), OwnedCardMoveIntent.Transfer)).Select(c => new MouHuLieCard(c.Id, c.Kind, new(z, owner), c.DisplayName))).ToArray());
    MouHuLieCard global::CardGame.Core.IMouHuLieFacts.Current(int id)
    {
        var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return new(id, c.Kind, _cardZonesOperations._cardZones.GetLocation(id), c.DisplayName);
    }

    MouHuLieYu? global::CardGame.Core.IMouHuLieFacts.ActiveYu(int owner, string instance) => CurrentMouHuLieYu(owner, instance);
    MouHuLieYu? global::CardGame.Core.IMouHuLieFacts.UsedYu(ProgramSkillWindowContext w, int owner, string instance)
    {
        if (w.Window != SkillProgramTriggerWindow.DiscardPileReceived || w.MovementBatch is not { } b || w.MovementIndex is not { } i || i < 0 || i >= b.Movements.Count || ((global::CardGame.Core.IMouHuLieFacts)this).ActiveYu(owner, instance)is not { } yu)
            return null;
        var m = b.Movements[i];
        return IsMouHuLieUsedDiscard(b, m, yu) ? yu : null;
    }

    int? global::CardGame.Core.IMouHuLieFacts.YitouTarget(ProgramSkillWindowContext w, int owner)
    {
        if (w.Window != SkillProgramTriggerWindow.PlayPhaseStarting || w.TargetSeat is not { } target || target == owner || !((global::CardGame.Core.IMouHuLieFacts)this).Alive(target) || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != target || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count != _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Max(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count))
            return null;
        return target;
    }

    MouHuLieDamageEvent? global::CardGame.Core.IMouHuLieFacts.Damage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouHuLieDamageReceipt(c, w);
    bool global::CardGame.Core.IMouHuLieFacts.CanSlash(ProgramSkillFrame f) => CanMouHuLieSlash(f);
    void global::CardGame.Core.IMouHuLieNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMouHuLieNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool global::CardGame.Core.IMouHuLieNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouHuLieNative.Give(ProgramSkillFrame f)
    {
        if (!MouHuLieRules.Valid(f) || f.MouHuLie is not { Stage: MouHuLieStage.GiveChildren } r || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            throw new InvalidOperationException("Chuanyu/Yitou needs its accepted real recipient.");
        var initial = r.Operation == SkillProgramEffectOp.StartMouHuLieChuanyu;
        if (initial && r.Target == f.OwnerSeat)
        {
            if (r.Cards is not [var card] || !_characterHuLieMouOperations.CreateMouHuLieFacts().Owned(f.OwnerSeat, false).Contains(card))
                throw new InvalidOperationException("Self Chuanyu marks only its exact unchanged own HE entity.");
            MouHuLieRules.Given(f.Id, [], true);
            return;
        }

        if (r.Cards.Count == 0 || r.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != c.From) || (initial || r.Operation == SkillProgramEffectOp.ResolveMouHuLieYitou) && r.Cards.Any(c => !_characterHuLieMouOperations.CreateMouHuLieFacts().Owned(f.OwnerSeat, r.Operation == SkillProgramEffectOp.ResolveMouHuLieYitou).Contains(c)) || r.Operation == SkillProgramEffectOp.RegiveMouHuLieChuanyu && (r.Yu is null || r.Cards is not [var used] || used.Id != r.Yu.CardId || used.From != CardLocation.DiscardPile))
            throw new InvalidOperationException("Chuanyu/Yitou lost its exact current physical Give material.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards.Select(c => c.Id).ToArray(), CardLocation.Hand(r.Target), new("skill-program.mou:chuanyu-or-yitou.give"), (_, moves) =>
        {
            MouHuLieRules.Given(f.Id, moves);
            if (r.Cards.All(c => c.From.OwnerSeat == f.OwnerSeat) && r.Target != f.OwnerSeat)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, r.Cards.Select(c => c.Id).ToArray(), r.Before);
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouHuLieNative.Clear(ProgramSkillFrame f, MouHuLieCard card)
    {
        if (!MouHuLieRules.Valid(f) || f.MouHuLie is not { Stage: MouHuLieStage.ClearChildren } r || r.Yu?.CardId != card.Id || r.Index < 1 || r.Cards[r.Index - 1] != card || _cardZonesOperations._cardZones.GetLocation(card.Id) != card.From || card.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.DrawPile))
            throw new InvalidOperationException("Chuanyu may discard only its own marked actual reachable entity.");
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves) => MouHuLieRules.ClearedMoves(f.Id, moves);
        if (card.From.OwnerSeat is { } holder)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, holder, [card.Id], new("skill-program.mou:chuanyu.round-discard"), Paid);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.DiscardPile, new("skill-program.mou:chuanyu.round-deck-discard"), Paid, disclosure: CardMovementDisclosure.FaceUp);
    }

    void global::CardGame.Core.IMouHuLieNative.Use(ProgramSkillFrame f) => IssueMouHuLieSlash(f);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4300)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouHuLieUsedDiscard(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouHuLieUsedDiscard(completed);
        return completed;
    }
}
