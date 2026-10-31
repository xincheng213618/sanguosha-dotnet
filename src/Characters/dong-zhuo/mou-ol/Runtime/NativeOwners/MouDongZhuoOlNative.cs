#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDongZhuoOlNative(Func<RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CharacterDongZhuoMouOlEntries> getCharacterDongZhuoMouOlRuntime, MouDongZhuoOlFactsFactory createMouDongZhuoOlFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.CoreGameEngineCharacterHostEntries> getCoreGameEngineCharacterHostRuntime) : IMouDongZhuoOlNative
{
    public void SetGuanbian(ProgramSkillFrame f, int amount) => getCoreGameEngineStrategicProgramsRuntime().SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, amount, f);
    public void LoseGuanbian(ProgramSkillFrame f)
    {
        var owner = matchParticipantsState.ReferenceToPlayers[f.OwnerSeat];
        var enabled = owner.SkillGrants.Grants.Where(g => g.IsEnabled && g.SkillId == MouDongZhuoOlRuntime.Guanbian).ToArray();
        if (enabled.Length == 0)
            return;
        getCoreGameEngineStrategicProgramsRuntime().SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, 0, f);
        foreach (var grant in enabled)
            owner.SkillGrants.RemoveGrant(grant.GrantId);
        advanceEventRulesAndQueueFact(new MouDongZhuoOlGuanbianEvent(f.Id, f.OwnerSeat, 0, true));
    }

    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Discard(ProgramSkillFrame f, int payer, MouDongZhuoOlCard card)
    {
        if (!getCharacterDongZhuoMouOlRuntime().MouDongZhuoOl.Valid(f) || f.MouDongZhuoOl is not { Paid: true } r || r.Payer != payer || !r.Materials.SequenceEqual([card]) || !createMouDongZhuoOlFacts().Discardable(payer).Contains(card))
            throw new InvalidOperationException("Xiongni requires its actual payable HE entity.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, payer, [card.Id], new(MouDongZhuoOlRuntime.CostReason), (_, records) =>
        {
            var parent = getFrameStore().OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            var paid = parent.MouDongZhuoOl!;
            if (!getCharacterDongZhuoMouOlRuntime().MouDongZhuoOl.Valid(parent) || records is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Xiongni's real discard return changed its paid material.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouDongZhuoOl = paid with { After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouDongZhuoOlPaidEvent(f.Id, payer, card.Id, card.From, card.Suit, m.Sequence));
        });
    }

    public void Give(ProgramSkillFrame f, int id, int recipient)
    {
        var before = cardMovementJournalState.ReferenceToMovementSequence;
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), new(MouDongZhuoOlRuntime.GiftReason), getCharacterHuangQuanMouRuntime().NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, [id], cardMovementJournalState.ReferenceToMovementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
        var parent = getFrameStore().OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
        var paid = parent.MouDongZhuoOl!;
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouDongZhuoOl = paid with { Before = before, After = cardMovementJournalState.ReferenceToMovementSequence } });
        foreach (var m in cardMovementJournalState.ReferenceToCardMovements.Where(m => m.Sequence > before && m.CardId == id && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(recipient)))
            advanceEventRulesAndQueueFact(new MouDongZhuoOlGiftEvent(f.Id, id, recipient, m.Sequence));
    }

    public void Damage(ProgramSkillFrame f, int target) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, 1);
    public bool BeginWine(ProgramSkillFrame f) => getCharacterDongZhuoMouOlRuntime().BeginMouDongZhuoOlWine(f);
    public void MaximumHp(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(f, 1);
    public void Recover(ProgramSkillFrame f) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
    public void Grant(ProgramSkillFrame f, string skill) => getCoreGameEngineCharacterHostRuntime().AcquireRuntimeSkills(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], f.SkillId, [skill]);
}
