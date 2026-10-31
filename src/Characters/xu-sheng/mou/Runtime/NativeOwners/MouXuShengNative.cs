#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXuShengNative(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuShengMouEntries> getCharacterXuShengMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::CardGame.Core.NativeOwners.MouXuShengFactsFactory createMouXuShengFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : IMouXuShengNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Boost(ProgramSkillFrame f)
    {
        if (!getCharacterXuShengMouRuntime().MouXuSheng.Valid(f) || f.MouXuSheng is not { Accepted: true, Operation: SkillProgramEffectOp.ResolveMouXuShengQinqiang } r)
            throw new InvalidOperationException("Qinqiang requires its accepted original true Hand Use.");
        // A true response Use owns its response window, never the incoming
        // card's damage. Such a non-damaging card may still spend this item.
        if (r.UseId == 0)
            return;
        if (getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(r.UseId)is not { Action: { } action } use || action.ActionId != r.ActionId || action.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Qinqiang must attach only to its own accepted card.");
        var boost = new MouXuShengBoost(f.Id, r.Source, f.GameplayHash, r.UseId, r.ActionId, r.X);
        getCoreGameEngineCardUseLifecycleRuntime().UpdateLifecycleCardUse(use.Id, u => u with { MouXuShengBoosts = (u.MouXuShengBoosts ?? []).Append(boost).ToArray() });
        advanceEventRulesAndQueueFact(new MouXuShengBoostIssuedEvent(boost));
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterXuShengMouRuntime().MouXuSheng.Valid(f) || f.MouXuSheng is not { Stage: MouXuShengStage.DiscardChildren, Paying: { } selected } r || !createMouXuShengFacts().Discardable(selected.Chooser, selected.Card))
            throw new InvalidOperationException("Yizhen requires the selected opposing real Hand entity.");
        var card = selected.Card with
        {
            IsRed = global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[selected.Card.Holder], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(selected.Card.CardId)))
        };
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, selected.Chooser, [card.CardId], new(MouXuShengRuntime.DiscardReason), (_, records) =>
        {
            var parent = getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouXuSheng is not { Stage: MouXuShengStage.DiscardChildren } paid || !getCharacterXuShengMouRuntime().MouXuSheng.Valid(parent) || paid.Paying != selected || records is not [var moved] || !cardMovementJournalState.ReferenceToCardMovements.Contains(moved) || moved.CardId != card.CardId || moved.From != CardLocation.Hand(card.Holder) || moved.Sequence <= paid.Before || moved.Reason.Value != MouXuShengRuntime.DiscardReason || moved.To != CardLocation.DiscardPile && moved.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Yizhen's primary discard must retain its chooser, original holder and exact material.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouXuSheng = paid with { After = moved.Sequence, Paid = paid.Paid.Append(new(selected.Chooser, card, moved.Sequence)).ToArray() } });
            advanceEventRulesAndQueueFact(new MouXuShengDiscardPaidEvent(f.Id, selected.Chooser, card.Holder, card.CardId, card.IsRed, moved.Sequence));
        });
    }

    public void Claim(ProgramSkillFrame f, IReadOnlyList<int> ids)
    {
        if (!getCharacterXuShengMouRuntime().MouXuSheng.Valid(f) || f.MouXuSheng is not { Stage: MouXuShengStage.ClaimChildren, Paid: [var first, var second] } r || first.Card.IsRed is not { } color || second.Card.IsRed != color || ids.Any(id => !r.Paid.Any(p => p.Card.CardId == id) || getCardZoneStore().GetLocation(id) != CardLocation.DiscardPile))
            throw new InvalidOperationException("Yizhen may only recover its matching-color actual discarded entities still in the discard pile.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouXuShengRuntime.ClaimReason), (_, records) =>
        {
            var parent = getFrameStore().OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouXuSheng = parent.MouXuSheng!with { After = records.Max(m => m.Sequence) } });
            foreach (var moved in records.Where(m => m.To == CardLocation.Hand(f.OwnerSeat)))
                advanceEventRulesAndQueueFact(new MouXuShengClaimedEvent(f.Id, moved.CardId, moved.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
    }
}
