#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangQuanNative(Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouHuangQuanNative
{
    public void Give(ProgramSkillFrame f, int giver, int recipient, IReadOnlyList<int> ids, bool reply, long before)
    {
        if (ids.Count == 0)
            return;
        if (!getCharacterHuangQuanMouRuntime().ValidMouHq(f) || f.MouHuangQuan is not { Paid: true } r || r.Selected.Count != ids.Count || r.Selected.Any(x => !ids.Contains(x.CardId) || getCardZoneStore().GetLocation(x.CardId) != x.From) || (reply ? r.Recipient : f.OwnerSeat) != giver || (reply ? f.OwnerSeat : r.Recipient) != recipient || before != r.Before)
            throw new InvalidOperationException("Qiaodui/Tuicheng native payment lost its exact issued sources and payer.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(reply ? MouHuangQuanRuntime.MouHqReplyReason : MouHuangQuanRuntime.MouHqGiveReason), (_, moves) =>
        {
            var current = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(current with { MouHuangQuan = current.MouHuangQuan!with { After = cardMovementJournalState.ReferenceToMovementSequence } });
            getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, giver, recipient, ids, before);
        }, disclosure: CardMovementDisclosure.FaceDown);
    }

    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id);
    public void OriginalEffect(ProgramSkillFrame f) => getCharacterHuangQuanMouRuntime().IssueMouHqOriginalEffect(f);
    public void Finish(ProgramSkillFrame f, bool accepted) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, accepted);
    public void Publish() => advanceRulesAndPublishState();
}
