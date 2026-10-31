#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDongChengFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CharacterDongChengMouEntries> getCharacterDongChengMouRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime) : IMouDongChengFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public double TargetScore(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new(0, 0, 1, 0, 0, 0, false, false));
    public int KeepValue(int owner, int card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[owner]);
    public IReadOnlyList<int> Living => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    public IReadOnlyList<EquipmentSlot> Slots(int owner, bool abolished) => Array.AsReadOnly(Enum.GetValues<EquipmentSlot>().Where(s => (matchParticipantsState.ReferenceToPlayers[owner].EquipmentSlotCapacity(s) == 0) == abolished).ToArray());
    public bool Parent(ProgramSkillFrame f) => getCharacterDongChengMouRuntime().MouDcParent(f);
    public MouDcOccurrence? Before(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterDongChengMouRuntime().MouDcBefore(c, w);
    public long ClaimUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterDongChengMouRuntime().MouDcClaimUse(c, w)?.Id ?? 0;
    public bool MayActivate(int owner) => matchOutcomeState.ReferenceToWinner == Winner.None && matchParticipantsState.ReferenceToPlayers[owner].IsAlive && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && actualTurnProgressState.ReferenceToCurrentSeat == owner && actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat == owner && matchParticipantsState.ReferenceToPlayers[owner].MouDongChengDisabledTurn != actualTurnProgressState.ReferenceToTurnNumber && Costs(owner).Count > 0;
    public IReadOnlyList<MouDcEntity> Costs(int owner) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[owner]).Concat(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(owner)).Where(c => EngineOwners.CharacterDongChengMouRuntime.MouDcDamageKind(getCardZoneStore().GetLocation(c.Id).Zone == CardZoneKind.Hand ? getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[owner], c) : c.Kind) && !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(owner, c, getCardZoneStore().GetLocation(c.Id), OwnedCardMoveIntent.Discard)).Select(c => new MouDcEntity(c.Id, getCardZoneStore().GetLocation(c.Id).Zone == CardZoneKind.Hand ? getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[owner], c) : c.Kind, getCardZoneStore().GetLocation(c.Id), c.DisplayName)).ToArray());
    public IReadOnlyList<int> Targets(ProgramSkillFrame f, CardKind kind) => getCharacterDongChengMouRuntime().MouDcTargets(f, kind);
    public (long Frame, long Use, long Damage)? DyingCause(PlayerDyingEvent fact)
    {
        if (getFrameStore().LastOrDefault()is not DyingFrame { Continuation: DyingContinuationKind.Damage } dying || dying.Id != fact.ResolutionId || getFrameStore().OfType<DamageFrame>().SingleOrDefault(d => d.Id == dying.ParentFrameId)is not { Amount: > 0 } damage || getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is not { DamageWasApplied: true } attack || attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != fact.VictimSeat || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(attack.ResolutionId)is not { MouDongChengDouwei: { } n } u || getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } parent || !getCharacterDongChengMouRuntime().MouDcUseRidesOn(parent, u))
            return null;
        return (n.FrameId, u.Id, damage.Id);
    }
}
