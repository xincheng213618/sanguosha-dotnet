#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangLingMouRuntime
{
    int global::CardGame.Core.IMouWangLingFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IMouWangLingFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IMouWangLingFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    long global::CardGame.Core.IMouWangLingFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouWangLingFacts.GameOver => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouWangLingFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouWangLingFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    int global::CardGame.Core.IMouWangLingFacts.MaximumHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    string global::CardGame.Core.IMouWangLingFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    SkillProgram global::CardGame.Core.IMouWangLingFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    CardLocation global::CardGame.Core.IMouWangLingFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    IReadOnlyList<Card> global::CardGame.Core.IMouWangLingFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    CardKind global::CardGame.Core.IMouWangLingFacts.HandKind(int seat, int id) => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
    IEnumerable<T> global::CardGame.Core.IMouWangLingFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IMouWangLingFacts.Movements(long before, long after) => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray());
    double global::CardGame.Core.IMouWangLingFacts.KeepValue(int owner, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[owner]);
    double global::CardGame.Core.IMouWangLingFacts.TargetValue(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 0, 0, 1, 0, true, false));
    bool global::CardGame.Core.IMouWangLingFacts.ParentMatches(ProgramSkillFrame f) => MouWangLingParentMatches(f);
    void global::CardGame.Core.IMouWangLingOperations.Give(ProgramSkillFrame f, int id, int target) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(target), new CardMoveReason(MouWangLingRuntime.GiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
    void global::CardGame.Core.IMouWangLingOperations.Discard(ProgramSkillFrame f, IReadOnlyList<int> ids) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(MouWangLingRuntime.DiscardReason));
    bool global::CardGame.Core.IMouWangLingOperations.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    void global::CardGame.Core.IMouWangLingOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    internal bool MouWangLingParentMatches(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != context.ParentFrameId)
            return false;
        ProgramTriggerCandidate? candidate = _resolutionStackOperations._resolutionStack[i - 1] switch
        {
            TurnEndingBoundaryFrame p when p.OwnerSeat == f.OwnerSeat && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            PlayPhaseStartingBoundaryFrame p when p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            ProgramLifecycleTriggerWindowFrame p when p.OwnerSeat == f.OwnerSeat && p.Window == context.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            DeferredTurnEndFrame p when p.OwnerSeat == f.OwnerSeat && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.AfterTurnEnded is not null => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p),
            _ => null
        };
        return candidate is { } c && c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.BindingId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
    }
}
