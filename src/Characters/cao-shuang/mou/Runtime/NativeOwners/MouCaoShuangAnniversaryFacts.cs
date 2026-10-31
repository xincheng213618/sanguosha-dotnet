#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoShuangAnniversaryFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime) : IMouCaoShuangAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int Keep(int seat, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[seat]);
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool ActualPlay(int owner) => actualTurnProgressState.ReferenceToCurrentSeat == owner && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p);
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var i = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (i < 1 || getFrameStore()[i - 1].Id != w.ParentFrameId)
            return false;
        var parent = getFrameStore()[i - 1];
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

    public MouCaoShuangAnniversaryUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.CardUse is not { } info || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActionId != info.CardActionId || p.Action.ActorSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return null;
        var candidate = p.Candidates[p.CandidateIndex];
        if (candidate.OwnerSeat != c.OwnerSeat || candidate.SkillId != c.SkillId || candidate.TriggerId != c.BindingId || candidate.SkillInstanceId != c.SkillInstanceId || candidate.GameplayHash != c.GameplayHash)
            return null;
        var use = getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(info.ParentCardUseFrameId);
        if (w.Window == SkillProgramTriggerWindow.CardUseCommitted)
            return new(p.Id, p.Action, use?.Id ?? info.ParentCardUseFrameId, -1, p.Action.EffectiveIsRed);
        if (w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || use is null || use.TargetSeats is not [var target] || target == c.OwnerSeat || !Alive(target))
            return null;
        return new(p.Id, p.Action, use.Id, target, p.Action.EffectiveIsRed ?? (p.Action.EffectiveSuit is { } suit && suit != Suit.None ? suit is Suit.Heart or Suit.Diamond : null));
    }

    public IReadOnlyList<MouCaoShuangAnniversaryCard> Payable(int payer, bool recast) => !Alive(payer) ? [] : Array.AsReadOnly((recast ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => getCardZoneStore().CardsAt(new(z, payer)).Where(c => recast || !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(payer, c, new(z, payer), OwnedCardMoveIntent.Discard)).Select(c => new MouCaoShuangAnniversaryCard(c.Id, c.Kind, new(z, payer), c.DisplayName))).ToArray());
    public bool Hurt(int source, int victim) => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<DamageAppliedEvent>().Any(d => !d.SourceLess && d.Amount > 0 && d.SourceSeat == source && d.TargetSeat == victim);
}
