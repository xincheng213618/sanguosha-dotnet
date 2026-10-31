#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhugeJinNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterZhugeJinMouEntries> getCharacterZhugeJinMouRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<DeterministicRandom> getDeterministicRandom, MouZhugeJinFactsFactory createMouZhugeJinFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries> getCoreGameEngineProgramCardTargetEffectsRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineCharacterHostEntries> getCoreGameEngineCharacterHostRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IMouZhugeJinNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Give(ProgramSkillFrame f)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.GiveChildren, Committed: true } r || r.Cards.Count == 0 || r.Cards.Any(id => getCardZoneStore().GetLocation(id) != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Taozhou requires the giver's committed current Hand entities.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(f.OwnerSeat), new(MouZhugeJinRuntime.GiveReason), getCharacterHuangQuanMouRuntime().NativeGiveCompletionRecords(f.Id, r.Target, f.OwnerSeat, r.Cards, cardMovementJournalState.ReferenceToMovementSequence, (_, moves) =>
        {
            var current = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = current.MouZhugeJin!;
            if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(current) || moves.Count != r.Cards.Count || moves.Any(m => !r.Cards.Contains(m.CardId) || m.From != CardLocation.Hand(r.Target) || m.Sequence <= paid.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Taozhou cannot substitute a primary real gift receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, current with { MouZhugeJin = paid with { Given = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)), After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves.Where(m => m.To == CardLocation.Hand(f.OwnerSeat)))
                advanceEventRulesAndQueueFact(new MouZhugeJinGivenEvent(f.Id, r.Target, f.OwnerSeat, m.CardId, m.Sequence));
        }));
    }

    public MouZhugeJinCard RandomHand(ProgramSkillFrame f, IReadOnlyList<MouZhugeJinCard> cards)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinHoude, Red: false } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Houde's hidden Hand choice requires the original other actor's actual Hand.");
        var before = getDeterministicRandom().State;
        var card = cards[getDeterministicRandom().Next(cards.Count)];
        advanceEventRulesAndQueueFact(new MouZhugeJinHandChosenEvent(f.Id, card.Id, before, getDeterministicRandom().State, cards.Count));
        return card;
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.CostChildren, Paying: { } card } r || !createMouZhugeJinFacts().Costs(f.OwnerSeat, card.From.OwnerSeat!.Value).Contains(card))
            throw new InvalidOperationException("Houde/Zijin requires its currently payable actual HE cost.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(MouZhugeJinRuntime.DiscardReason), (_, moves) =>
        {
            var current = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = current.MouZhugeJin!;
            if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(current) || paid.Paying != card || moves is not [var m] || !cardMovementJournalState.ReferenceToCardMovements.Contains(m) || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Houde/Zijin lost its original primary logical discard receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, current with { MouZhugeJin = paid with { CostPaid = true, After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouZhugeJinCostPaidEvent(f.Id, f.OwnerSeat, card.From.OwnerSeat!.Value, card.Id, m.Sequence));
        });
    }

    public void Lose(ProgramSkillFrame f)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinZijin, Stage: MouZhugeJinStage.LossChildren })
            throw new InvalidOperationException("Zijin requires the accepted native undamaging-use loss branch.");
        getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1);
    }

    public void Nullify(ProgramSkillFrame f)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinHoude, CostPaid: true })
            throw new InvalidOperationException("Houde cannot nullify before paying its true discard.");
        getCoreGameEngineProgramCardTargetEffectsRuntime().NullifyCurrentProgramCardEffect(f);
    }

    public void Grant(ProgramSkillFrame f)
    {
        if (!getCharacterZhugeJinMouRuntime().MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.GrantChildren } r || r.SecretNumber - r.Given < 2 || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouZhugeJinZijinGrantedEvent>().Any(p => p.FrameId == f.Id))
            throw new InvalidOperationException("Taozhou grants derived Zijin only after its exact committed actual-gift shortfall.");
        getCoreGameEngineCharacterHostRuntime().AcquireRuntimeSkills(matchParticipantsState.ReferenceToPlayers[r.Target], $"taozhou:{f.OwnerSeat}:{f.SkillInstanceId}:{f.Id}", [MouZhugeJinComposition.Zijin]);
        advanceEventRulesAndQueueFact(new MouZhugeJinZijinGrantedEvent(f.Id, r.Target));
    }
}
