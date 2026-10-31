#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangWeiAnniversaryFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMouEntries> getCharacterJiangWeiMouRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime) : IMouJiangWeiAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public string Instance(int owner) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[owner], MouJiangWeiAnniversaryRuntime.Juemou);
    public bool State(int owner, string instance, string state) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(owner, MouJiangWeiAnniversaryRuntime.Juemou, instance, state);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var i = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (i < 1 || getFrameStore()[i - 1].Id != w.ParentFrameId)
            return false;
        var parent = getFrameStore()[i - 1];
        if (parent is DeferredTurnEndFrame ended)
            return w.Window == SkillProgramTriggerWindow.AfterTurnEnded && getCoreGameEngineAfterTurnEndedProgramsRuntime().IsActualAfterTurnEndedParent(ended) && ended.OwnerSeat == f.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, getCoreGameEngineAfterTurnEndedProgramsRuntime().AfterTurnEndedCandidate(ended));
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

    public long Action(ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.CardUseCommitted && w.CardUse is { } info && getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is ProgramCardTriggerWindowFrame p && p.Action.ActionId == info.CardActionId && p.Action.ActorSeat == w.OwnerSeat && CardUseCategoryCatalog.Get(p.Action.EffectiveKind)is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick ? p.Action.ActionId : 0;
    public (long Cause, long Dying)? CausalDying(int owner, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.DyingEntering || getCoreDyingCursorRuntimeRuntime().ActiveDying is not { } dying || dying.VictimSeat != owner || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering, Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry || entry.ResumeDyingFrameId != dying.Id || entry.OwnerSeat != owner || getFrameStore().SingleOrDefault(f => f.Id == dying.ParentFrameId)is not DamageFrame damage || damage.TargetSeat != owner || getFrameStore().SingleOrDefault(f => f.Id == damage.ParentFrameId)is not ProgramSkillFrame { MouJiangWeiAnniversary: { Operation: SkillProgramEffectOp.ResolveMouJiangWeiJuemou, Stage: MouJiangWeiAnniversaryStage.Damage } r } root || root.OwnerSeat != owner || root.AttackAttempt is not { } attempt || attempt.TargetSeat != owner || attempt.SourceSeat != r.DamageSource || r.Victim != owner || !getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(root) || !getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<PlayerDyingEvent>().Any(p => p.ResolutionId == dying.Id && p.VictimSeat == owner) || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouJiangWeiAnniversaryDamageIssuedEvent>().Count(p => p.FrameId == root.Id && p.Victim == owner && p.Source == attempt.SourceSeat) != 1)
            return null;
        return (root.Id, dying.Id);
    }

    public bool CanTrigger(ProgramTriggerCandidate c, ProgramSkillWindowContext w, SkillProgramEffectOp operation)
    {
        if (w.OwnerSeat != c.OwnerSeat || !Alive(c.OwnerSeat))
            return false;
        if (operation == SkillProgramEffectOp.InitializeMouJiangWeiJuemou)
            return c.SkillId == MouJiangWeiAnniversaryRuntime.Juemou && w.Window == SkillProgramTriggerWindow.GameStarting;
        if (operation == SkillProgramEffectOp.RestoreMouJiangWeiJuemou)
            return c.SkillId == MouJiangWeiAnniversaryRuntime.Juemou && CausalDying(c.OwnerSeat, w)is not null;
        if (operation == SkillProgramEffectOp.ResolveMouJiangWeiJuemou)
        {
            if (c.SkillId != MouJiangWeiAnniversaryRuntime.Juemou || State(c.OwnerSeat, Instance(c.OwnerSeat), "yin") && Living().Count < 2)
                return false;
            return Action(w) > 0 || State(c.OwnerSeat, Instance(c.OwnerSeat), "modified") && w.Window is SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterTurnEnded && w.TargetSeat == c.OwnerSeat;
        }

        if (operation != SkillProgramEffectOp.ResolveMouJiangWeiFuzhan || c.SkillId != MouJiangWeiAnniversaryRuntime.Fuzhan || w.Window != SkillProgramTriggerWindow.DyingExited || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } exited || w.SourceSeat != ret.VictimSeat || w.TargetSeat != ret.VictimSeat || !Alive(ret.VictimSeat) || Hp(ret.VictimSeat) <= 0)
            return false;
        return getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<DyingResolvedEvent>().Count(p => p.ResolutionId == ret.DyingFrameId && p.VictimSeat == ret.VictimSeat && p.Survived) == 1;
    }

    public IReadOnlyList<MouJiangWeiAnniversaryCard> Discardable(int chooser, int holder) => !Alive(chooser) || !Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, holder)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(chooser, c, new(z, holder), OwnedCardMoveIntent.Discard)).Select(c => new MouJiangWeiAnniversaryCard(c.Id, new(z, holder), c.DisplayName))).ToArray());
}
