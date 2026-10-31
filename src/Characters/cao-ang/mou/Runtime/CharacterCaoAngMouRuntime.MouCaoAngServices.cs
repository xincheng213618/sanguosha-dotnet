#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoAngMouRuntime
{
    internal MouCaoAngRuntime MouCaoAng => _mouCaoAngOperations.Get();

    int global::CardGame.Core.IMouCaoAngFacts.Turn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMouCaoAngFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long global::CardGame.Core.IMouCaoAngFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouCaoAngFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouCaoAngFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouCaoAngFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string global::CardGame.Core.IMouCaoAngFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    SkillProgram? global::CardGame.Core.IMouCaoAngFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouCaoAngFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouCaoAngFacts.Parent(ProgramSkillFrame f) => MouCaParent(f);
    int global::CardGame.Core.IMouCaoAngFacts.LostEquipmentOwner(ProgramSkillFrame f) => f.WindowContext?.MovementBatch is { } batch && f.WindowContext.MovementIndex is { } index && index >= 0 && index < batch.Movements.Count && batch.Movements[index] is { From.Zone: CardZoneKind.Equipment } m && m.To != m.From && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) ? m.From.OwnerSeat ?? -1 : -1;
    int global::CardGame.Core.IMouCaoAngFacts.EmptySlots(int seat) => Enum.GetValues<EquipmentSlot>().Sum(slot => Math.Max(0, _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot) - _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Count(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot)));
    CardKind global::CardGame.Core.IMouCaoAngFacts.Kind(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind;
    IReadOnlyList<MouCaEntity> global::CardGame.Core.IMouCaoAngFacts.Equipment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, CardLocation.Equipment(seat), OwnedCardMoveIntent.Discard)).Select(c => new MouCaEntity(c.Id, c.Kind, CardLocation.Equipment(seat))).ToArray());
    IReadOnlyList<MouCaPair> global::CardGame.Core.IMouCaoAngFacts.Pairs(ProgramSkillFrame f) => MouCaPairs(f);
    IReadOnlyList<LegalAction> global::CardGame.Core.IMouCaoAngFacts.Plans(ProgramSkillFrame f) => f.MouCaoAng is { } r ? MouCaPlans(f, r.Actor, r.CardId) : [];
    double global::CardGame.Core.IMouCaoAngFacts.TargetValue(int actor, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[actor].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(actor), target, new(0, 0, 1, 0, 0, 0, false, false));
    void global::CardGame.Core.IMouCaoAngNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouCaoAngNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouCaoAngNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouCaoAngNative.Discard(ProgramSkillFrame f)
    {
        if (!MouCaoAng.Valid(f) || f.MouCaoAng is not { Stage: MouCaoAngStage.DiscardChildren, ActualDiscard: 0 } r || r.Equipment.Count == 0 || r.Equipment.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != c.From))
            throw new InvalidOperationException("Zhiwang requires its real current equipment group once.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Equipment.Select(c => c.Id).ToArray(), new(MouCaoAngRuntime.DiscardReason), (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.MouCaoAng!;
            if (!MouCaoAng.Valid(current) || moves.Count != r.Equipment.Count || moves.Any(m => !r.Equipment.Any(c => c.Id == m.CardId && c.Kind == m.CardKind && c.From == m.From) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhiwang lost its actual native logical discard group.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, current with { MouCaoAng = paid with { After = moves.Max(m => m.Sequence), ActualDiscard = moves.Count } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaDiscardedEvent(f.Id, m.CardId, m.CardKind, m.To, m.Sequence));
        });
    }

    void global::CardGame.Core.IMouCaoAngNative.Use(ProgramSkillFrame f) => IssueMouCaUse(f);
    internal bool MouCaParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != w.ParentFrameId)
            return false;
        ProgramTriggerCandidate? candidate = _resolutionStackOperations._resolutionStack[at - 1] switch
        {
            CardsMovedTriggerWindowFrame m when f.SkillId == MouCaoAngRuntime.Fengmin && w.Window == SkillProgramTriggerWindow.CardsMoved && m.CandidateIndex >= 0 && m.CandidateIndex < m.Candidates.Count && m.Batch.Id == w.MovementBatch?.Id => m.Candidates[m.CandidateIndex],
            TurnEndingBoundaryFrame end when f.SkillId == MouCaoAngRuntime.Zhiwang && w.Window == SkillProgramTriggerWindow.TurnEnding && end.OwnerSeat == f.OwnerSeat && end.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && end.ItemIndex >= 0 && end.ItemIndex < end.Items.Count => end.Items[end.ItemIndex].Candidate,
            ProgramLifecycleTriggerWindowFrame life when f.SkillId == MouCaoAngRuntime.Zhiwang && life.Window == SkillProgramTriggerWindow.TurnEnding && w.Window == life.Window && life.CandidateIndex >= 0 && life.CandidateIndex < life.Candidates.Count => life.Candidates[life.CandidateIndex],
            _ => null
        };
        return candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }
}
