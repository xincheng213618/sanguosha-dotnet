#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangXiuAnniversaryNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterZhangXiuMouEntries> getCharacterZhangXiuMouRuntime, Func<DeterministicRandom> getDeterministicRandom, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<CardZoneStore> getCardZoneStore, MouZhangXiuAnniversaryFactsFactory createMouZhangXiuAnniversaryFacts, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : IMouZhangXiuAnniversaryNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public bool CanSlash(ProgramSkillFrame f) => getCharacterZhangXiuMouRuntime().CanMouZhangXiuAnniversarySlash(f);
    public void Slash(ProgramSkillFrame f) => getCharacterZhangXiuMouRuntime().IssueMouZhangXiuAnniversarySlash(f);
    public MouZhangXiuAnniversaryCard RandomHand(ProgramSkillFrame f, IReadOnlyList<MouZhangXiuAnniversaryCard> cards)
    {
        if (!getCharacterZhangXiuMouRuntime().ValidMouZhangXiuAnniversary(f) || f.MouZhangXiuAnniversary is not { Stage: MouZhangXiuAnniversaryStage.DiscardCard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Fuxi requires the exact opposing anonymous-Hand selection.");
        var before = getDeterministicRandom().State;
        var card = cards[getDeterministicRandom().Next(cards.Count)];
        advanceEventRulesAndQueueFact(new MouZhangXiuAnniversaryHandChosenEvent(f.Id, r.Target, card.Id, before, getDeterministicRandom().State));
        return card;
    }

    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterZhangXiuMouRuntime().ValidMouZhangXiuAnniversary(f) || f.MouZhangXiuAnniversary is not { Cards.Count: > 0 } r || r.Stage is not (MouZhangXiuAnniversaryStage.GiveChildren or MouZhangXiuAnniversaryStage.DiscardChildren or MouZhangXiuAnniversaryStage.ObtainChildren or MouZhangXiuAnniversaryStage.DistributionChildren))
            throw new InvalidOperationException("Mou Zhang Xiu requires his exact issued physical movement segment.");
        var discard = r.Stage == MouZhangXiuAnniversaryStage.DiscardChildren;
        var obtain = r.Stage == MouZhangXiuAnniversaryStage.ObtainChildren;
        var distribute = r.Stage == MouZhangXiuAnniversaryStage.DistributionChildren;
        var recipient = distribute ? r.Recipient : obtain ? f.OwnerSeat : r.Target;
        var destination = discard ? CardLocation.DiscardPile : CardLocation.Hand(recipient);
        if (r.Cards.Any(c => getCardZoneStore().GetLocation(c.Id) != c.From) || obtain && r.Cards.Any(c => c.From != CardLocation.DiscardPile) || distribute && (r.Cards.Count != 1 || r.Cards.Any(c => c.From != CardLocation.Hand(f.OwnerSeat) || !r.Gained.Contains(c.Id) || r.Given.Contains(c.Id))) || !obtain && !distribute && r.Cards.Any(c => !createMouZhangXiuAnniversaryFacts().Cards(f.OwnerSeat, discard ? r.Target : f.OwnerSeat, discard).Contains(c)))
            throw new InvalidOperationException("Mou Zhang Xiu cannot pay with another entity, unacquired card or different region.");
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            if (moves.Any(m => m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Zhang Xiu native movement changed its actual destination.");
            getCharacterZhangXiuMouRuntime().MouZhangXiuAnniversaryRules.MovesPaid(f.Id, moves);
            if (!obtain && !discard)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, f.OwnerSeat, recipient, ids, r.Before);
        }

        var reason = new CardMoveReason(discard ? MouZhangXiuAnniversaryRuntime.DiscardReason : obtain ? MouZhangXiuAnniversaryRuntime.ClaimReason : distribute ? MouZhangXiuAnniversaryRuntime.DistributionReason : MouZhangXiuAnniversaryRuntime.GiveReason);
        if (discard)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Target, ids, reason, Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, reason, Paid);
    }
}
