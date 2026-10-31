#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoXianNative(Func<RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoXianOlRuntime>> getCharacterCaoXianOlRuntime, CaoXianFactsFactory createCaoXianFacts, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : ICaoXianNative
{
    public PublicPersistentPileSource EnsurePile(ProgramSkillFrame f) => f.SkillId == CaoXianRuntime.Lingxi && f.CaoXian?.Operation == SkillProgramEffectOp.StoreCaoXianLingxi ? getCoreGameEnginePublicPileSourcesRuntime().EnsurePublicPileSource(f, int.MaxValue) : throw new InvalidOperationException("Only the actual Lingxi depositor may create her source-specific wings.");
    public void Publish() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void LoseHp(ProgramSkillFrame f) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, f.CaoXian!.Target, 1);
    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterCaoXianOlRuntime().Value.CaoXianRules.Valid(f) || f.CaoXian is not { Cards.Count: > 0 } r)
            throw new InvalidOperationException("Cao Xian requires this exact frozen paid native segment.");
        var remove = r.Stage == CaoXianStage.RemoveChildren;
        var discard = r.Stage == CaoXianStage.AdjustChildren || r.Option == 2;
        var payer = r.Operation == SkillProgramEffectOp.StoreCaoXianLingxi || r.Stage == CaoXianStage.AdjustChildren ? f.OwnerSeat : r.Target;
        var current = remove ? createCaoXianFacts().PileCards(r.Pile!) : createCaoXianFacts().Owned(payer, r.Stage == CaoXianStage.AdjustChildren, discard);
        if (r.Cards.Any(c => !current.Contains(c)))
            throw new InvalidOperationException("Cao Xian cannot move stale wings or another holder's unavailable HE entity.");
        var destination = remove ? CardLocation.OutsideGame : discard ? CardLocation.DiscardPile : r.Pile!.Location;
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            if (moves.Any(m => !cardMovementJournalState.ReferenceToCardMovements.Contains(m) || m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Cao Xian's native movement changed its admitted destination.");
            getCharacterCaoXianOlRuntime().Value.CaoXianRules.MovesPaid(f.Id, moves);
        }

        if (discard)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, payer, r.Cards.Select(c => c.Id).ToArray(), new("skill-program.ol:lingxi-or-zhifou.discard"), Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(r.Cards.Select(c => c.Id).ToArray(), destination, new(remove ? "skill-program.ol:zhifou.remove-wing" : "skill-program.ol:lingxi.store-wing"), Paid, disclosure: CardMovementDisclosure.FaceUp);
    }
}
