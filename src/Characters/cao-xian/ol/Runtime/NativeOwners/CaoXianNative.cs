#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoXianNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXianOlEntries> getCharacterCaoXianOlRuntime, global::CardGame.Core.NativeOwners.CaoXianFactsFactory createCaoXianFacts, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : ICaoXianNative
{
    public PublicPersistentPileSource EnsurePile(ProgramSkillFrame f) => f.SkillId == CaoXianRuntime.Lingxi && f.CaoXian?.Operation == SkillProgramEffectOp.StoreCaoXianLingxi ? getCoreGameEnginePublicPileSourcesRuntime().EnsurePublicPileSource(f, int.MaxValue) : throw new InvalidOperationException("Only the actual Lingxi depositor may create her source-specific wings.");
    public void Publish() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void LoseHp(ProgramSkillFrame f) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, f.CaoXian!.Target, 1);
    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterCaoXianOlRuntime().CaoXianRules.Valid(f) || f.CaoXian is not { Cards.Count: > 0 } r)
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
            getCharacterCaoXianOlRuntime().CaoXianRules.MovesPaid(f.Id, moves);
        }

        if (discard)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, payer, r.Cards.Select(c => c.Id).ToArray(), new("skill-program.ol:lingxi-or-zhifou.discard"), Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(r.Cards.Select(c => c.Id).ToArray(), destination, new(remove ? "skill-program.ol:zhifou.remove-wing" : "skill-program.ol:lingxi.store-wing"), Paid, disclosure: CardMovementDisclosure.FaceUp);
    }
}
