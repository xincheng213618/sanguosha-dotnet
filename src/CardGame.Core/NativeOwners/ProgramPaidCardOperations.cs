#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPaidCardOperations(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<CardZoneStore> getCardZoneStore, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime, Func<RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IProgramPaidCardOperations
{
    public void Cancel(ProgramSkillFrame frame, string reason) => getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(frame, reason);
    public void SetCardSet(long frameId, string bind, IReadOnlyList<int> ids, SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? locations = null, Suit? frozenSuit = null) => getCoreGameEngineProgramLifecycleRuntime().SetProgramCardSet(frameId, bind, ids, visibility, locations, frozenSuit);
    public void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, source, destination, reason, disclosure: disclosure);
    public bool MoveToProcessing(ProgramSkillFrame frame, Card card, CardLocation source, CardMoveReason reason, CardMovementDisclosure disclosure)
    {
        var before = cardMovementJournalState.ReferenceToMovementSequence;
        getCorePhysicalMovementFacadesRuntime().MoveCard(card, source, CardLocation.Processing, reason, disclosure: disclosure);
        if (getCardZoneStore().GetLocation(card.Id) == CardLocation.Processing)
            return true;
        // A generated field entity can be destroyed by this exact first
        // departure. Credit that real payment; never run a second transfer
        // from Processing or report an obtain that did not take place.
        if (getFrameStore().LastOrDefault()?.Id == frame.Id && source.Zone == CardZoneKind.Equipment && cardMovementJournalState.ReferenceToCardMovements.Where(move => move.Sequence > before && move.CardId == card.Id).ToArray()is [var movement] && movement.CardKind == card.Kind && movement.From == source && movement.To == CardLocation.OutsideGame && movement.Reason == reason && getCardZoneStore().GetLocation(card.Id) == movement.To && getCoreGameEngineEphemeralCardMaterialsRuntime().IsEphemeralCardDestroyedMaterial(movement))
            return false;
        throw new InvalidOperationException("A selected transfer lost its exact first processing entry or native destruction proof.");
    }

    public bool SettleRetainedGain(ProgramSkillFrame frame, Card card, CardLocation source, CardLocation destination, CardMoveReason reason) => getCoreHappyWeiYanGainDestinationsRuntime().TrySettleHappyWeiYanRetainedGain(card, source, destination, reason, frame.Id);
    public void Give(ProgramSkillFrame frame, int giver, Card card, CardLocation source, CardLocation destination, CardMoveReason reason, bool throughProcessing, CardMovementDisclosure disclosure)
    {
        if (SettleRetainedGain(frame, card, source, destination, reason))
            return;
        var before = cardMovementJournalState.ReferenceToMovementSequence;
        if (throughProcessing && !MoveToProcessing(frame, card, source, reason, disclosure))
            return;
        getCorePhysicalMovementFacadesRuntime().MoveCard(card, throughProcessing ? CardLocation.Processing : source, destination, reason, getCharacterHuangQuanMouRuntime().NativeGiveCompletion(frame.Id, giver, destination.OwnerSeat!.Value, [card.Id], before), disclosure: disclosure);
    }

    public bool BeginMovementWindow() => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow();
    public bool BeginPublicHandReveal(long frameId, string returnKey) => getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow2(frameId, returnKey);
    public void ReturnMovement(long frameId) => getCoreGameEngineRuntimeRuntime().ReturnRuntimeProgramMovement(frameId);
    public void Log(string type, string message, int? actor = null) => getCoreMatchLogRuntime().AddLog(type, message, actor);
}
