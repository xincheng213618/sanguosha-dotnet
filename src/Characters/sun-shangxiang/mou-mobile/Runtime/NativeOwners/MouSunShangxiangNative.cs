#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSunShangxiangNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries> getCharacterSunShangxiangMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.NativeOwners.MouSunShangxiangFactsFactory createMouSunShangxiangFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouSunShangxiangNative
{
    public void Mark(ProgramSkillFrame f, int previous, int current, bool initial) => getCharacterSunShangxiangMouMobileRuntime().ChangeMouSsxZhu(f, previous, current, initial);
    public void Give(ProgramSkillFrame f, int payer, IReadOnlyList<int> ids) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxGiveReason), getCharacterHuangQuanMouRuntime().NativeGiveCompletionRecords(f.Id, payer, f.OwnerSeat, ids, cardMovementJournalState.ReferenceToMovementSequence, (_, moves) =>
    {
        foreach (var move in moves)
            getCharacterSunShangxiangMouMobileRuntime().SealMouSsxMove(f.Id, move, false);
    }), disclosure: CardMovementDisclosure.FaceDown);
    public void Deposit(ProgramSkillFrame f, MouSsxEntity entity)
    {
        if (!createMouSunShangxiangFacts().Field(f.OwnerSeat, true).Contains(entity))
            throw new InvalidOperationException("Liangzhu lost its actual foreign Equipment entity or transfer permission.");
        var pile = getCoreGameEnginePublicPileSourcesRuntime().EnsurePublicPileSource(f, int.MaxValue);
        getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(entity.CardId), entity.From, pile.Location, new(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxDepositReason), m => getCharacterSunShangxiangMouMobileRuntime().SealMouSsxMove(f.Id, m, true), disclosure: CardMovementDisclosure.FaceUp);
    }

    public void ObtainZhuang(ProgramSkillFrame f, IReadOnlyList<MouSsxEntity> entities)
    {
        var live = entities.Where(x => getCardZoneStore().GetLocation(x.CardId) == x.From).Select(x => x.CardId).ToArray();
        if (live.Length > 0)
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(live, CardLocation.Hand(f.OwnerSeat), new(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxObtainReason), (_, moves) =>
            {
                foreach (var move in moves)
                    getCharacterSunShangxiangMouMobileRuntime().SealMouSsxMove(f.Id, move, true);
            }, disclosure: CardMovementDisclosure.FaceUp);
    }

    public void Discard(ProgramSkillFrame f, MouSsxEntity entity)
    {
        if (!createMouSunShangxiangFacts().Field(f.OwnerSeat, false).Contains(entity))
            throw new InvalidOperationException("Xiaoji lost its current legal field entity.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, entity.From.OwnerSeat!.Value, [entity.CardId], new(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime.MouSsxDiscardReason), (_, moves) =>
        {
            foreach (var m in moves)
                getCharacterSunShangxiangMouMobileRuntime().SealMouSsxMove(f.Id, m, true);
        });
    }

    public void Armor(ProgramSkillFrame f, int seat) => getCharacterSunShangxiangMouMobileRuntime().GrantMouSsxArmor(f, seat);
    public void Recover(ProgramSkillFrame f, int seat) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, seat, 1, null, null);
    public void Faction(ProgramSkillFrame f) => getCharacterSunShangxiangMouMobileRuntime().ChangeMouSsxFailureFaction(f);
    public void MaximumHp(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(f, -1);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
}
