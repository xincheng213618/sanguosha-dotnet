#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuAnniversaryNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries> getCharacterChengYuMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouChengYuAnniversaryNative
{
    public IReadOnlyList<MouCyCard> Peek(ProgramSkillFrame f)
    {
        if (!getCharacterChengYuMouRuntime().MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary!.Stage != MouCyStage.Peek)
            throw new InvalidOperationException("Shizha requires its paid HP window before privately viewing the real deck.");
        _ = getCorePhysicalDrawFacadesRuntime().EnsureDrawPile();
        return Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DrawPile).TakeLast(3).Reverse().Select(c => new MouCyCard(c.Id, c.Kind, c.DisplayName)).ToArray());
    }

    public MouCyCard? Reveal(ProgramSkillFrame f)
    {
        if (!getCharacterChengYuMouRuntime().MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { Stage: MouCyStage.RevealChildren, Target: >= 0 } r || !matchParticipantsState.ReferenceToPlayers[r.Target].IsAlive)
            throw new InvalidOperationException("Gaojian requires its current living target and own reveal slot.");
        var bind = "gaojian-" + r.RevealCount.ToString(System.Globalization.CultureInfo.InvariantCulture);
        getCoreGameEngineProgramLifecycleRuntime().RevealProgramTopCards(f.Id, r.Target, 1, null, bind, SkillProgramCardSetVisibility.Public);
        f = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
        var set = getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(f, bind);
        if (set.CardIds.Count == 0)
            return null;
        var id = set.CardIds.Single();
        var move = cardMovementJournalState.ReferenceToCardMovements.Single(m => m.CardId == id && m.Sequence > r.Before && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        return new(id, move.CardKind, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName);
    }

    public void Move(ProgramSkillFrame f, int id, CardLocation from, CardLocation to)
    {
        if (!getCharacterChengYuMouRuntime().MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { } r || r.CardId != id || getCardZoneStore().GetLocation(id) != from || (r.Stage switch
        {
            MouCyStage.ObtainChildren => from != CardLocation.DrawPile || to != CardLocation.Hand(f.OwnerSeat) || !r.View.Any(c => c.Id == id),
            MouCyStage.GainChildren => from != CardLocation.Processing || to != CardLocation.Hand(r.Target),
            MouCyStage.DiscardChildren => from != CardLocation.Processing || to != CardLocation.DiscardPile,
            _ => true
        }))
            throw new InvalidOperationException("Mou Cheng Yu can move only his exact current viewed or revealed physical card.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([id], to, new("skill-program.mou-cheng-yu." + r.Stage), (_, moves) =>
        {
            if (moves.Any(m => m.From != from || m.To != to && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Cheng Yu lost the native movement replacement receipt.");
            getCharacterChengYuMouRuntime().MouChengYuAnniversary.Moved(f.Id, moves);
        }, disclosure: r.Stage == MouCyStage.ObtainChildren ? CardMovementDisclosure.Unspecified : CardMovementDisclosure.FaceUp);
    }

    public void Invalidate(ProgramSkillFrame f, MouCyUseInvalidatedEvent proof) => getCharacterChengYuMouRuntime().ApplyMouCyInvalidation(f, proof);
    public void Damage(ProgramSkillFrame f, int target, int amount)
    {
        if (!getCharacterChengYuMouRuntime().MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { Stage: MouCyStage.Damage, DamageCard: true } r || r.Target != target || r.RevealCount != amount)
            throw new InvalidOperationException("Gaojian damage requires the actual revealed damage card and total count.");
        getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, amount, nature: DamageNature.Fire);
    }

    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
}
