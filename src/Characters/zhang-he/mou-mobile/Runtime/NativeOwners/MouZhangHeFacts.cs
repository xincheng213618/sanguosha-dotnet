#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangHeFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.EngineStates.ProgramPhaseScheduleStorage phaseScheduleStorage, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangHeMouMobileEntries> getCharacterZhangHeMouMobileRuntime) : IMouZhangHeFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public IReadOnlyList<int> Seats => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(p => p.Seat).ToArray());

    public MouZhangHePlayer Player(int s) => new(s, matchParticipantsState.ReferenceToPlayers[s].IsAlive, matchParticipantsState.ReferenceToPlayers[s].IsHuman, getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count, getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[s]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetJudgment(matchParticipantsState.ReferenceToPlayers[s]).Count, getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[s]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetJudgment(matchParticipantsState.ReferenceToPlayers[s]).Count);
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<MouZhangHeMovement> Movements => Array.AsReadOnly(cardMovementJournalState.ReferenceToCardMovements.Select(m => new MouZhangHeMovement(m.Sequence, m.CardId, m.From, m.To, m.Reason.Value)).ToArray());

    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public ProgramPhaseSchedule? Schedule => phaseScheduleStorage.Current;

    public bool ExactStart(ProgramSkillFrame f, SkillProgramEffectOp op) => f.InstructionIndex == 1 && f.ActivationId == f.TriggerId && f.SkillId == MouQiaobianRuntime.Qiaobian && Program(f.SkillId)?.GameplayHash == f.GameplayHash && getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(f)?.Op == op && NativeParent(f);
    public bool NativeParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } x || x.OwnerSeat != f.OwnerSeat || x.SourceSeat != f.OwnerSeat || x.TargetSeat != f.OwnerSeat || f.OwnerSeat != actualTurnProgressState.ReferenceToCurrentSeat)
            return false;
        var n = getFrameStore().FindIndex(p => p.Id == f.Id);
        if (n < 1 || getFrameStore()[n - 1].Id != x.ParentFrameId)
            return false;
        var parent = getFrameStore()[n - 1];
        if (parent is ProgramLifecycleTriggerWindowFrame w)
            return w.Window == x.Window && w.OwnerSeat == f.OwnerSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && (x.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && w.Continuation == ProgramLifecycleContinuation.NormalTurnStart || x.Window == SkillProgramTriggerWindow.ActualPhaseSkipped && w.Continuation == ProgramLifecycleContinuation.CompleteActualSkippedPhase && w.MouZhangHeSkip is { } s && s.Entitlement.Source.OwnerSeat == f.OwnerSeat && s.Entitlement.Source.SkillInstanceId == f.SkillInstanceId && s.Entitlement.GameplayHash == f.GameplayHash && s.NativeReturn.ActualTurn == actualTurnProgressState.ReferenceToTurnNumber);
        return parent is TurnEndingBoundaryFrame t && x.Window == SkillProgramTriggerWindow.TurnEnding && t.OwnerSeat == f.OwnerSeat && t.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && t.ItemIndex >= 0 && t.ItemIndex < t.Items.Count && t.Items[t.ItemIndex].Candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    public IReadOnlyList<MouZhangHeCard> Discardable(int owner) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, owner)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(owner, c, new(z, owner), OwnedCardMoveIntent.Discard)).Select(c => new MouZhangHeCard(c.Id, new(z, owner)))).ToArray());
    public IReadOnlyList<MouZhangHeCard> FieldCards => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).SelectMany(p => new[] { CardZoneKind.Equipment, CardZoneKind.Judgment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, p.Seat)).Select(c => new MouZhangHeCard(c.Id, new(z, p.Seat))))).ToArray());

    public bool CanMoveField(int id, int target)
    {
        var from = Location(id);
        var card = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        return from.OwnerSeat is { } source && source != target && matchParticipantsState.ReferenceToPlayers[source].IsAlive && from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && getCoreGameEngineProgramCardPaymentsRuntime().CanMoveProgramCardToCorrespondingZone(card, from.Zone, target) && (from.Zone != CardZoneKind.Equipment || matchParticipantsState.ReferenceToPlayers[target].EquipmentSlotCapacity(getEquipmentContentRegistry().Get(card.Kind).Slot) > 0) && !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(actualTurnProgressState.ReferenceToCurrentSeat, card, from, OwnedCardMoveIntent.Transfer);
    }

    public IReadOnlyList<PromptChoice> HandChoices(ProgramSkillFrame f, int target) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand], OwnedCardMoveIntent.Obtain, destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat);
    public int ResolveHandChoice(PromptChoice c, int subject)
    {
        if (c.Cards is [var id])
            return id;
        if (c.Parameters.GetValueOrDefault("card-owner-seat") != subject.ToString(CultureInfo.InvariantCulture) || c.Parameters.GetValueOrDefault("source-zone") != "Hand" || !int.TryParse(c.Parameters.GetValueOrDefault("slot-index"), out var slot) || slot < 0 || slot >= getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[subject]).Count)
            throw new InvalidOperationException("Qiaobian's opaque Hand slot is stale.");
        return getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[subject])[slot].Id;
    }

    public int KeepValue(int s, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[s]);
    public bool Friendly(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public bool FirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => getCharacterZhangHeMouMobileRuntime().MouQiaobianFirstNativeChild(f, child);
}
