#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouLuSuAnniversaryNative(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries> getCharacterLuSuMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries> getCoreSnapshotValueProjectionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost) : IMouLuSuAnniversaryNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Yin(ProgramSkillFrame f, bool yin)
    {
        if (!getCharacterLuSuMouRuntime().MouLuSuAnniversary.Valid(f) || f.SkillId != MouLuSuAnniversaryRuntime.Mengmou)
            throw new InvalidOperationException("Mengmou state requires its accepted native branch.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, f.SkillId, f.SkillInstanceId, "yin", yin);
    }

    public void Reveal(ProgramSkillFrame f)
    {
        if (!getCharacterLuSuMouRuntime().MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.Recipient } r || r.Cards.Any(c => getCardZoneStore().GetLocation(c) != CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Mingshi requires its actual selected hand entities to reveal.");
        advanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(r.Cards.Select(id => getCoreSnapshotValueProjectionRuntime().ToSnapshot(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id))).ToArray())));
        foreach (var id in r.Cards)
            advanceEventRulesAndQueueFact(new MouLuSuAnniversaryShownEvent(f.Id, id));
        getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow2(f.Id, "mou-lu-su-mingshi-three");
    }

    public void Gift(ProgramSkillFrame f, int card)
    {
        if (!getCharacterLuSuMouRuntime().MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.GiftChildren } r || !r.Cards.Contains(card) || !matchParticipantsState.ReferenceToPlayers[r.Other].IsAlive || getCardZoneStore().GetLocation(card) != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Mingshi requires the recipient's exact publicly shown hand entity.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(r.Other), new("skill-program.mou:mingshi.give"), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            if (moves is not [var m] || m.CardId != card || m.From != CardLocation.Hand(f.OwnerSeat) || m.To != CardLocation.Hand(r.Other))
                throw new InvalidOperationException("Mingshi gift changed its physical owner or recipient.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouLuSuAnniversary = parent.MouLuSuAnniversary!with { After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouLuSuAnniversaryGiftEvent(f.Id, r.Other, card, m.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    public void Use(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().IssueMouLuSuUse(f);
    public void Declare(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().IssueMouLuSuPlayedDeclaration(f);
    public void Play(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().IssueMouLuSuPlayed(f);
    public void CompletePlayed(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().BeginMouLuSuPlayedCompleted(f);
    public void LoseHp(ProgramSkillFrame f, int target) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
    public void Recover(ProgramSkillFrame f, int target) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
}
