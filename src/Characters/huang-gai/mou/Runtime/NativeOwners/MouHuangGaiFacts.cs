#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangGaiFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouEntries> getCharacterHuangGaiMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime) : IMouHuangGaiFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public int LivingCount => matchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive);

    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public bool OwnPlay(int seat) => seat == actualTurnProgressState.ReferenceToCurrentSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play;
    public bool LiejiParent(ProgramSkillFrame f) => getCharacterHuangGaiMouRuntime().MouHgLiejiParent(f);
    public bool HasNativeUse(long id) => getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(id)is not null;
    public bool SameChoice(PromptChoice left, PromptChoice right) => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([left], [right]);
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public SkillProgram? Program(string id) => getContentRegistry().GetSkill(id).Program;
    public SkillPromptPresentation Presentation(string id)
    {
        var s = getContentRegistry().GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    public double TargetScore(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new(0, 0, 1, 0, 0, 0, false, false));
    public IReadOnlyList<int> Seats() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(p => p.Seat).ToArray());
    public IReadOnlyList<int> DamageHand(int owner) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[owner]).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[owner], c))).Select(c => c.Id).ToArray());
    public IReadOnlyList<int> Targets(ProgramSkillFrame f) => getCharacterHuangGaiMouRuntime().MouHgTargets(f);
    public IReadOnlyList<int> ProcessingReveals(ProgramSkillFrame f) => Array.AsReadOnly(f.MouHuangGai!.Reveals.Where(c => getCardZoneStore().GetLocation(c.CardId) == CardLocation.Processing).Select(c => c.CardId).ToArray());
    public IReadOnlyList<MouHgLiejiGrant> Grants(int owner) => Array.AsReadOnly((matchParticipantsState.ReferenceToPlayers[owner].MouHgLiejiGrants ?? []).ToArray());
    public MouHgUseFacts? Use(long id) => getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(id)is { Action: { Type: CardActionType.Use } a } u ? new(u.Id, a.ActionId, Array.AsReadOnly(a.PhysicalCards.Select(c => c.CardId).ToArray()), u.MouHgBonuses is { } b ? Array.AsReadOnly(b.ToArray()) : null) : null;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
}
