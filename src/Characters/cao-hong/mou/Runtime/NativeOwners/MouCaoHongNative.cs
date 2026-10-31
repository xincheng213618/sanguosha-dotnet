#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoHongNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoHongMouEntries> getCharacterCaoHongMouRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::CardGame.Core.NativeOwners.MouCaoHongFactsFactory createMouCaoHongFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : IMouCaoHongNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public MouCaoHongCard RandomHand(ProgramSkillFrame f, IReadOnlyList<MouCaoHongCard> cards)
    {
        if (!getCharacterCaoHongMouRuntime().MouCaoHong.Valid(f) || f.MouCaoHong is not { Stage: MouCaoHongStage.ObtainCard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Xianju requires the original actor's blind current opposing-Hand choice.");
        var before = getDeterministicRandom().State;
        var card = cards[getDeterministicRandom().Next(cards.Count)];
        advanceEventRulesAndQueueFact(new MouCaoHongHandChosenEvent(f.Id, r.Target, card.Id, before, getDeterministicRandom().State, cards.Count));
        return card;
    }

    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterCaoHongMouRuntime().MouCaoHong.Valid(f) || f.MouCaoHong is not { } r || r.Cards.Count == 0 || r.Stage is not (MouCaoHongStage.ExchangeTakeChildren or MouCaoHongStage.ReturnChildren or MouCaoHongStage.ObtainChildren or MouCaoHongStage.DiscardChildren))
            throw new InvalidOperationException("Mou Cao Hong movement requires its exact current paid segment.");
        var discard = r.Stage == MouCaoHongStage.DiscardChildren;
        var give = r.Stage == MouCaoHongStage.ReturnChildren;
        var holder = give || discard ? f.OwnerSeat : r.Target;
        if (r.Cards.Any(c => !createMouCaoHongFacts().Cards(f.OwnerSeat, holder, discard).Contains(c)) || r.Stage == MouCaoHongStage.ExchangeTakeChildren && r.Cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Mou Cao Hong cannot replace its current native HE material with a different entity or region.");
        var destination = discard ? CardLocation.DiscardPile : CardLocation.Hand(give ? r.Target : f.OwnerSeat);
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouCaoHong!;
            if (!getCharacterCaoHongMouRuntime().MouCaoHong.Valid(parent) || moves.Count != r.Cards.Count || moves.Any(m => !r.Cards.Any(c => c.Id == m.CardId && c.Kind == m.CardKind && c.From == m.From) || m.Sequence <= r.Before || m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Cao Hong lost its original primary physical movement receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouCaoHong = paid with { After = moves.Max(m => m.Sequence), Actual = moves.Count(m => discard || m.To == destination) } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouCaoHongMovementPaidEvent(f.Id, r.Cursor, m.CardId, m.From, m.To, m.Sequence));
            if (give)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, ids, r.Before);
        }

        if (discard)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(MouCaoHongRuntime.DiscardReason), Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, new(give ? MouCaoHongRuntime.GiveReason : MouCaoHongRuntime.TakeReason), Paid);
    }
}
