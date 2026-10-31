#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuCheErFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime) : IMouHuCheErFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public bool HasHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].HasHpValue;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public string SkillName(string id) => getContentRegistry().GetSkill(id).Name;
    public string SkillDescription(string id) => getContentRegistry().GetSkill(id).Description;
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public bool Owns(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().CanInvokeProgramSkill(f);
    public ActualDiscardRecoveryPhaseKey? Phase() => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public bool Recorded(ActualDiscardRecoveryPhaseKey p) => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p);
    public IReadOnlyList<int> Targets(int owner) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(t => t.IsAlive && t.Seat != owner && getCorePhysicalOwnedCardQueriesRuntime().GetHand(t).Count > 0).Select(t => t.Seat).ToArray());
    public IReadOnlyList<MouHuCheErHand> Hand(int target) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Select((c, i) => new MouHuCheErHand(c.Id, i, c.Kind, getCoreGameEngineProgramCardPoliciesRuntime().GetProgramEffectiveSuit(matchParticipantsState.ReferenceToPlayers[target], c))).ToArray());
    public bool InHand(int target, int id) => getCardZoneStore().GetLocation(id) == CardLocation.Hand(target);
    public SkillProgramCardCategory Category(CardKind kind) => EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
    public IReadOnlyList<int> SameSuitDiscards(int target, Suit suit) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Where(c => getCoreGameEngineProgramCardPoliciesRuntime().GetProgramEffectiveSuit(matchParticipantsState.ReferenceToPlayers[target], c) == suit && !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(target, c, CardLocation.Hand(target), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray());
    public int CategoryCount(int target, SkillProgramCardCategory category) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Concat(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(target)).Count(c => EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == category);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public MouHuCheErDamageEvent? Damage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is not CardAttackHandle a || a.IsChainPropagation || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(a.ResolutionId)is not { MouHuCheErSlash: { } o } use || !getCharacterHuCheErMouRuntime().IsIssuedMouHuCheErUse(use) || applied.TargetSeat != o.Target || a.TargetSeat != o.Target || !a.DamageWasApplied)
            return null;
        return new(o.FrameId, use.Id, applied.TargetSeat, applied.Amount);
    }
}
