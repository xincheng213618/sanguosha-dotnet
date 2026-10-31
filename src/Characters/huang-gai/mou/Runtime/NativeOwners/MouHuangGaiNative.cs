#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangGaiNative(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouEntries> getCharacterHuangGaiMouRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IMouHuangGaiNative
{
    public void SetGrants(int owner, IReadOnlyList<MouHgLiejiGrant>? grants) => matchParticipantsState.ReferenceToPlayers[owner].MouHgLiejiGrants = grants is null ? null : Array.AsReadOnly(grants.ToArray());
    public void SetUseBonuses(long useId, IReadOnlyList<MouHgUseBonus> bonuses) => getCoreGameEngineCardUseLifecycleRuntime().UpdateLifecycleCardUse(useId, u => u with { MouHgBonuses = bonuses });
    public MouHgReveal? Reveal(ProgramSkillFrame f, string binding)
    {
        var before = f.MouHuangGai!.Before;
        getCoreGameEngineProgramLifecycleRuntime().RevealProgramTopCards(f.Id, f.OwnerSeat, 1, null, binding, SkillProgramCardSetVisibility.Public);
        f = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
        var card = getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(f, binding).CardIds.SingleOrDefault();
        if (card == 0)
            return null;
        var actual = cardMovementJournalState.ReferenceToCardMovements.Last(m => m.Sequence > before && m.CardId == card && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        return new(card, actual.CardKind, actual.Sequence);
    }

    public void Obtain(ProgramSkillFrame f, IReadOnlyList<int> ids) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouHuangGaiRuntime.ObtainReason), (_, moves) => getCharacterHuangGaiMouRuntime().MouHuangGaiRules.MovesPaid(f.Id, moves), disclosure: CardMovementDisclosure.FaceUp);
    public void DiscardSlash(ProgramSkillFrame f)
    {
        var shown = f.MouHuangGai!.Reveals[^1];
        if (getCardZoneStore().GetLocation(shown.CardId) == CardLocation.Processing)
            getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(shown.CardId), CardLocation.Processing, CardLocation.DiscardPile, new(MouHuangGaiRuntime.DiscardReason), m => getCharacterHuangGaiMouRuntime().MouHuangGaiRules.MovesPaid(f.Id, [m]));
    }

    public void Slash(ProgramSkillFrame f, int target) => getCharacterHuangGaiMouRuntime().IssueMouHgSlash(f, target);
    public bool Drain(long id) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(id);
    public void Publish() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
}
