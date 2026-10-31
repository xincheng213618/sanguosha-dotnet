#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangJiFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CharacterJiangJiMouEntries> getCharacterJiangJiMouRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime) : IMouJiangJiFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public int EquipmentCount(int s) => Alive(s) ? getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[s]).Count : 0;
    public bool ActualPlay(int s) => Alive(s) && actualTurnProgressState.ReferenceToCurrentSeat == s && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && PhaseId > 0;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> ShijuOwners(int actor) => !ActualPlay(actor) || Cards(actor, false).Count == 0 ? [] : Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && getCoreGameEngineSkillProgramsRuntime().EnabledSkillPrograms(p).Any(s => s.Id == MouJiangJiRuntime.Shiju)).Select(p => p.Seat).ToArray());
    public IReadOnlyList<MouJiangJiCard> Cards(int seat, bool discard) => !Alive(seat) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, seat)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (!discard || !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(seat, c, new(z, seat), OwnedCardMoveIntent.Discard))).Select(c => new MouJiangJiCard(c.Id, new(z, seat), c.Kind, c.DisplayName))).ToArray());
    public bool Disabled(int seat, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(seat, MouJiangJiRuntime.Yingshi, instance, "disabled");
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouJiangJiRuntime.Shiju)
        {
            if (f.TriggerId is not null || f.ActivationId != "give-and-use" || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.OwnerSeat == f.OwnerSeat && p.SkillId == f.SkillId && p.ActivationId == f.ActivationId) != 1)
                return false;
            return f.ActivatingSeat is not { } actor ? f.ActivationPhase is null : f.ActivationPhase is { } phase && History<ProgramSharedActivationStartedEvent>().Count(p => p.FrameId == f.Id && p.ActorSeat == actor && p.Source == MouJiangJiRuntime.Source(f) && p.Phase == phase) == 1;
        }

        if (f.TriggerId != "ordinary-trick-target" || f.WindowContext is not { } w || f.ActivationId != f.TriggerId)
            return false;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        return at > 0 && getFrameStore()[at - 1].Id == w.ParentFrameId && Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex), w)is not null;
    }

    public MouJiangJiUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouJiangJiRuntime.Yingshi || w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != card.ParentCardUseFrameId || window.CompletedResponseReturn is not null || window.Continuation != ProgramCardContinuation.FinalizedTrick || window.TrickContinuation is null || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActorSeat != c.OwnerSeat || a.ActionId != card.CardActionId || window.Action.ActionId != a.ActionId || use.SourceSeat != c.OwnerSeat || !EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var now = window.Candidates[window.CandidateIndex];
        if (now.OwnerSeat != c.OwnerSeat || now.SkillId != c.SkillId || now.TriggerId != c.BindingId || now.SkillInstanceId != c.SkillInstanceId || now.GameplayHash != c.GameplayHash)
            return null;
        var targets = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : use.TargetSeats.ToArray();
        return new(use.Id, a.ActionId, window.Id, use.CardKind)
        {
            Targets = targets
        };
    }

    public IReadOnlyList<LegalAction> UseOptions(ProgramSkillFrame f) => getCharacterJiangJiMouRuntime().MouJjUseOptions(f);
    public CardKind UseKind(int actor, LegalAction action) => action.PlayedCardKind ?? getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[actor], getCoreGameEnginePlayCardHistoryRuntime().ApplyProgramUseAppearance(matchParticipantsState.ReferenceToPlayers[actor], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(action.CardId!.Value), action.ConversionSource));
}
