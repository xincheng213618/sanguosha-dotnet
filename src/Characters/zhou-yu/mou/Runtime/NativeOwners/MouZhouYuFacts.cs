#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CharacterZhouYuMouEntries> getCharacterZhouYuMouRuntime) : IMouZhouYuFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public int FactionCount => getCoreGameEngineRuleQueriesRuntime().GetLivingFactionCount();

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public int HandCount(int s) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count;
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public string Instance(int s, string skill) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[s], skill);
    public bool Yin(int s, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(s, MouZhouYuRuntime.Yingmou, instance, "yin");
    public IReadOnlyList<CardConversionSource> FireSources() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && getCoreGameEngineSkillProgramsRuntime().EnabledSkillPrograms(p).Any(x => x.Id == MouZhouYuRuntime.Ronghuo)).Select(p => new CardConversionSource(MouZhouYuRuntime.Ronghuo, "next-fire-use", p.Seat, Instance(p.Seat, MouZhouYuRuntime.Ronghuo))).ToArray());
    public long? FireDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouZhouYuRuntime.Ronghuo || c.BindingId != "fire-after-damage" || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.OwnerSeat != c.OwnerSeat || w.Amount <= 0 || w.OccurrenceIndex != c.OccurrenceIndex || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not DamageTriggerWindowFrame window || window.ParentFrameId != w.DamageFrameId || window.TriggerWindow != w.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate() != c || getFrameStore().SingleOrDefault(x => x.Id == window.ParentFrameId)is not DamageFrame { Nature: DamageNature.Fire, Amount: > 0 } d || d.TargetSeat != w.TargetSeat || d.Amount != w.Amount)
            return null;
        return d.Id;
    }

    public MouZhouYuCompletedUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != card.CardActionId || window.Action is not { Type: CardActionType.Use } a || a.ActorSeat != c.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } now || now.OwnerSeat != c.OwnerSeat || now.SkillId != c.SkillId || now.TriggerId != c.BindingId || now.SkillInstanceId != c.SkillInstanceId || now.GameplayHash != c.GameplayHash)
            return null;
        var use = getFrameStore().OfType<CardUseFrame>().SingleOrDefault(x => x.Action?.ActionId == a.ActionId);
        return new(window.Id, use?.Id ?? 0, a);
    }

    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (n < 1 || getFrameStore()[n - 1].Id != w.ParentFrameId)
            return false;
        if (f.TriggerId == "opening-state")
            return w.Window == SkillProgramTriggerWindow.GameStarting && getFrameStore()[n - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting };
        return f.TriggerId == "used-other-targets" && Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
    }

    public IReadOnlyList<MouZhouYuCard> Hand(int s) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Select(c =>
    {
        var kind = getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[s], c);
        return new MouZhouYuCard(c.Id, CardLocation.Hand(s), c.DisplayName, kind, EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Trick && (EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(kind) || kind == CardKind.Lightning));
    }).ToArray());
    public IReadOnlyList<MouZhouYuCard> DiscardableHand(int s) => Hand(s).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(s, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(c.Id), c.From, OwnedCardMoveIntent.Discard)).ToArray();
    public IReadOnlyList<LegalAction> UseOptions(ProgramSkillFrame f) => getCharacterZhouYuMouRuntime().MouZyUseOptions(f);
    public IReadOnlyList<IReadOnlyList<int>> FireOptions(ProgramSkillFrame f) => getCharacterZhouYuMouRuntime().MouZyFireOptions(f);
}
