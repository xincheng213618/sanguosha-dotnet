#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSunShangxiangFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries> getCharacterSunShangxiangMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime) : IMouSunShangxiangFacts
{
    public bool GameOver => matchOutcomeState.ReferenceToWinner != Winner.None;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public string? Faction(int seat) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(matchParticipantsState.ReferenceToPlayers[seat]);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IReadOnlyList<int> Living => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool ParentMatches(ProgramSkillFrame f) => getCharacterSunShangxiangMouMobileRuntime().MouSsxParentMatches(f);
    public bool MovementRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouSsx is not { Paid: true, Stage: MouSsxStage.DepositChildren or MouSsxStage.DiscardChildren } r || r.Entities is not [{ Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } entity])
            return false;
        var seat = entity.From.OwnerSeat!.Value;
        var reason = r.Stage == MouSsxStage.DepositChildren ? global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxDepositReason : global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxDiscardReason;
        if (!cardMovementJournalState.ReferenceToCardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == entity.CardId && m.CardKind == CardKind.SilverLion && m.From == entity.From && m.Reason.Value == reason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.ResolutionId > f.Id && x.ResolutionId < hp.Change.Id && x.PlayerSeat == seat && x.Reason.Value == reason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter);
    }

    public IReadOnlyList<MouSsxEntity> Field(int owner, bool equipmentOnly) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && (!equipmentOnly || p.Seat != owner)).SelectMany(p => (equipmentOnly ? new[] { CardZoneKind.Equipment } : new[] { CardZoneKind.Equipment, CardZoneKind.Judgment }).SelectMany(zone => getCardZoneStore().CardsAt(new(zone, p.Seat)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(owner, c, new(zone, p.Seat), equipmentOnly ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Discard)).Select(c => new MouSsxEntity(c.Id, c.Kind, new(zone, p.Seat))))).ToArray());
    public IReadOnlyList<MouSsxEntity> Zhuang(int owner) => Array.AsReadOnly(getCoreGameEnginePublicPileSourcesRuntime().PublicPileSources(owner).Where(p => p.SkillId == MouSunShangxiangRuntime.Liangzhu).SelectMany(p => getCoreGameEnginePublicPileSourcesRuntime().PublicPileCards(p).Select(c => new MouSsxEntity(c.Id, c.Kind, p.Location))).ToArray());
    public double HelpValue(int actor, int target) => aiBrainsState.ReferenceToAiBrains[actor].ScoreProgramTarget(createSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false));
    public double KeepValue(int actor, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[actor]);
}
