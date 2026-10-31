#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChenLinNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterChenLinMouEntries> getCharacterChenLinMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, MouChenLinFactsFactory createMouChenLinFacts, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<CardZoneStore> getCardZoneStore) : IMouChenLinNative
{
    public void Publish() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Reveal(ProgramSkillFrame f)
    {
        if (!getCharacterChenLinMouRuntime().ValidMouChenLin(f) || f.MouChenLin is not { Stage: MouChenLinStage.Revealing } r || f.CardSetBindings.Any(b => b.Name == MouChenLinRuntime.RevealBind))
            throw new InvalidOperationException("Zhuanwen needs its original unspent native top reveal.");
        getCoreGameEngineProgramLifecycleRuntime().RevealProgramTopCards(f.Id, f.OwnerSeat, r.Requested, null, MouChenLinRuntime.RevealBind, SkillProgramCardSetVisibility.Public);
    }

    public void Use(ProgramSkillFrame f) => getCharacterChenLinMouRuntime().IssueMouChenLinUse(f);
    public void Give(ProgramSkillFrame f)
    {
        if (!getCharacterChenLinMouRuntime().MouChenLinRules.ValidInvitation(f) || f.MouChenLinInvitation is not { Stage: MouChenLinInvitationStage.MovementChildren, Closed: false, PendingCard: { } card } r || !r.Eligible.Contains(r.PendingGiver) || r.Answered.Contains(r.PendingGiver) || !createMouChenLinFacts().He(r.PendingGiver).Contains(card) || !matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Yaozuo needs this actual participant's current original HE entity.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new("skill-program.mou:yaozuo.give"), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Yaozuo lost its original native Give payment movement.");
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouChenLinInvitation = parent.MouChenLinInvitation!with { After = m.Sequence } });
            if (m.To == CardLocation.Hand(f.OwnerSeat))
                getCharacterChenLinMouRuntime().RecordMouChenLinGift(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id), r.PendingGiver, card.Id, r.Before, moves);
            getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, r.PendingGiver, f.OwnerSeat, [card.Id], r.Before);
        });
    }

    public void CloseInvitation(ProgramSkillFrame f) => getCharacterChenLinMouRuntime().IssueMouChenLinNonHandDonorTickets(f);
    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterChenLinMouRuntime().ValidMouChenLin(f) || f.MouChenLin is not { Stage: MouChenLinStage.GiveChildren or MouChenLinStage.ReturningChildren } r)
            throw new InvalidOperationException("Zhuanwen needs its own real public-card transfer segment.");
        var give = r.Stage == MouChenLinStage.GiveChildren;
        var ids = r.Cards.Where(c => getCardZoneStore().GetLocation(c.Id) == CardLocation.Processing && (!give || !MouChenLinRuntime.DamageCard(c.Kind))).Select(c => c.Id).ToArray();
        if (ids.Length == 0)
            return;
        if (give && !matchParticipantsState.ReferenceToPlayers[r.Target].IsAlive)
            return;
        var destination = give ? CardLocation.Hand(r.Target) : CardLocation.DrawPile;
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, new(give ? MouChenLinRuntime.GiveReason : MouChenLinRuntime.ReturnReason), (_, moves) =>
        {
            if (moves.Any(m => m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhuanwen changed the destination of its retained physical entities.");
            if (!give)
                getCardZoneStore().PlaceDrawPileCardsAtTop(moves.Where(m => m.To == CardLocation.DrawPile).Select(m => m.CardId).ToArray());
            getCharacterChenLinMouRuntime().MouChenLinRules.MovesPaid(f.Id, moves);
            if (give && r.Target != f.OwnerSeat)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, ids, r.Before);
        }, disclosure: CardMovementDisclosure.FaceUp);
    }
}
