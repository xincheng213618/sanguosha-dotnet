#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSmFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries> getCoreGameEngineProgramDodgeResponsesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreDamageObserverCursorQueriesEntries> getCoreDamageObserverCursorQueriesRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime) : IMouSimaYiFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public ContentSkillDefinition Skill(string s) => getContentRegistry().GetSkill(s);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public int Keep(int s, int id) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[s]);
    public double DamageValue(int s, int t) => aiBrainsState.ReferenceToAiBrains[s].ScoreProgramTarget(createSnapshot(s), t, new(0, 0, 1, 0, 0, 0, false, false));
    public bool ActualPlay(int s) => s == actualTurnProgressState.ReferenceToCurrentSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == s && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p);
    public bool Yin(int s, string instance) => History<MouSmPolarityEvent>().LastOrDefault(x => x.Source.OwnerSeat == s && x.Source.SkillId == MouSimaYiComposition.Quanmou && x.Source.SkillInstanceId == instance)?.Yin ?? false;
    public IReadOnlyList<int> InRange(int s) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != s && getCoreGameEngineProgramDodgeResponsesRuntime().IsWithinAttackRange(s, p.Seat)).Select(p => p.Seat).Order().ToArray());
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<MouSmEntity> Gifts(ProgramSkillFrame f, int s) => !Alive(s) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, s)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(s, c, new(z, s), OwnedCardMoveIntent.Transfer)).Select(c => new MouSmEntity(c.Id, c.Kind, new(z, s), c.DisplayName))).ToArray());
    public IReadOnlyList<MouSmPlayedPlan> Plays(ProgramSkillFrame f) => getCharacterSimaYiMouRuntime().MouSmPlayedPlans(f);
    public bool Declaration(MouSmPlayedPlan plan) => plan.Source is { } source && getCoreGameEnginePlayCardHistoryRuntime().ViewAsRule(source)?.DeclarationValidation is not null;
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouSimaYiComposition.Quanmou && f.ActivationId == "in-range-give" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (n < 1 || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || getFrameStore()[n - 1].Id != w.ParentFrameId)
            return false;
        return getFrameStore()[n - 1] switch
        {
            ProgramCardTriggerWindowFrame p => p.MouSimaYiPrivate is not null && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && p.Candidates[p.CandidateIndex] is { } c && c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash && getCharacterSimaYiMouRuntime().ValidMouSmPrivateWindow(p),
            BeforeDamageProgramWindowFrame p => p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex].Candidate),
            DamageTriggerWindowFrame p => getCoreDamageObserverCursorQueriesRuntime().DamageProgramMatchesWindow(f, p),
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }

    public MouSmPrivateUse? PrivateUse(int owner, ProgramSkillWindowContext? w)
    {
        if (w?.Window != SkillProgramTriggerWindow.CardUseCommitted || w.OwnerSeat != owner || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame { MouSimaYiPrivate: { } back } p || !getCharacterSimaYiMouRuntime().ValidMouSmPrivateWindow(p) || p.Action.ActorSeat != owner || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].SkillId != MouSimaYiComposition.Pingliao)
            return null;
        var use = getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(back.UseId)!;
        return new(use.Id, p.Action.ActionId, use.TargetSeats);
    }

    public MouSmDebtIssuedEvent? Debt(int owner, ProgramSkillWindowContext? w, bool yin)
    {
        if (w is null || w.SourceSeat != owner || w.TargetSeat is not { } target || w.Amount <= 0 || w.OwnerSeat != owner)
            return null;
        var parent = getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId);
        if (yin ? (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || parent is not DamageTriggerWindowFrame d || d.SourceSeat != owner || d.TargetSeat != target) : (w.Window != SkillProgramTriggerWindow.BeforeDamageApplied || parent is not BeforeDamageProgramWindowFrame b || b.Prevented || b.SourceSeat != owner || b.TargetSeat != target))
            return null;
        return History<MouSmDebtIssuedEvent>().FirstOrDefault(d => d.Source.OwnerSeat == owner && d.Target == target && d.Yin == yin && d.Turn == Turn && d.TurnOwner == TurnOwner && d.PhaseId == PhaseId && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && Skill(d.Source.SkillId).Program?.GameplayHash == d.GameplayHash && History<MouSmStartedEvent>().Count(s => s.FrameId == d.FrameId && s.Source == d.Source && s.GameplayHash == d.GameplayHash && s.Operation == SkillProgramEffectOp.ResolveMouSimaYiQuanmou) == 1 && !History<MouSmDebtConsumedEvent>().Any(c => c.DebtId == d.FrameId));
    }
}
