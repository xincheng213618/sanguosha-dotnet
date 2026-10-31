#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXuShengFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : IMouXuShengFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string CardLabel(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public int KeepValue(int owner, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[owner]);
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<MouXuShengCard> Hand(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Select(c => new MouXuShengCard(c.Id, seat, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], c)))).ToArray());
    public bool Discardable(int chooser, MouXuShengCard card) => getCardZoneStore().GetLocation(card.CardId) == CardLocation.Hand(card.Holder) && !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(chooser, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card.CardId), CardLocation.Hand(card.Holder), OwnedCardMoveIntent.Discard);
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public MouXuShengUse? Use(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.OwnerSeat != candidate.OwnerSeat || context.CardUse is not { } info || getFrameStore().SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != info.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash)
            return null;
        var use = getFrameStore().OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == info.CardActionId);
        return new(window.Id, window.Action, use?.Id ?? 0);
    }

    public bool SourcedDamage(ProgramSkillWindowContext context, int owner) => context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: var target, DamageFrameId: not null, Amount: > 0 } && target == owner && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(source);
    public bool DamageParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouXuShengComposition.Yizhen || f.TriggerId != "mutual-private-hand" || f.WindowContext is not { } context || !SourcedDamage(context, f.OwnerSeat))
            return false;
        var index = getFrameStore().FindIndex(x => x.Id == f.Id);
        return index > 0 && getFrameStore()[index - 1] is DamageTriggerWindowFrame window && window.Id == context.ParentFrameId && window.ParentFrameId == context.DamageFrameId && window.SourceSeat == context.SourceSeat && window.TargetSeat == f.OwnerSeat && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate());
    }
}
