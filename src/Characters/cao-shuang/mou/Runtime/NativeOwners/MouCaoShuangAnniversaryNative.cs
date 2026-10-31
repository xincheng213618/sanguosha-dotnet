#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoShuangAnniversaryNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoShuangMouRuntime>> getCharacterCaoShuangMouRuntime, MouCaoShuangAnniversaryFactsFactory createMouCaoShuangAnniversaryFacts, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineNativeRecastInvocationsEntries> getCoreGameEngineNativeRecastInvocationsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<DeterministicRandom> getDeterministicRandom, ProgramSkillHostFactory createProgramSkillHost, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<Dictionary<int, long>> getDictionary, Func<RuntimeEntries.CoreGameEngineCharacterHostEntries> getCoreGameEngineCharacterHostRuntime) : IMouCaoShuangAnniversaryNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void PublishState() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterCaoShuangMouRuntime().Value.MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Stage: MouCaoShuangAnniversaryStage.MoveChildren, Cards.Count: > 0 } r || r.Cards.Any(c => !createMouCaoShuangAnniversaryFacts().Payable(r.Payer, r.Option == 3).Contains(c)))
            throw new InvalidOperationException("Jianzhuan requires the actual chooser's frozen available entities.");
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> records)
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouCaoShuangAnniversary!;
            if (records.Count != r.Cards.Count || !records.Zip(r.Cards).All(pair => pair.First.CardId == pair.Second.Id && pair.First.CardKind == pair.Second.Kind && pair.First.From == pair.Second.From && pair.First.Sequence > r.Before && (pair.First.To == CardLocation.DiscardPile || pair.First.To == CardLocation.OutsideGame)))
                throw new InvalidOperationException("Jianzhuan lost its actual discard or recast entities.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouCaoShuangAnniversary = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var m in records)
                advanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryMaterialEvent(f.Id, r.Repeat, r.Payer, m.CardId, m.From, m.Sequence, r.Option == 3));
        }

        if (r.Option == 3)
        {
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(f = getCoreGameEngineNativeRecastInvocationsRuntime().BeginFrameNativeRecastInvocation(f, r.Payer, ids, r.Repeat));
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, Paid, disclosure: CardMovementDisclosure.FaceUp);
        }
        else
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Payer, ids, new(MouCaoShuangAnniversaryRuntime.DiscardReason), Paid);
    }

    public void CompleteRecast(ProgramSkillFrame f, int actual)
    {
        var r = f.MouCaoShuangAnniversary!;
        if (!getCharacterCaoShuangMouRuntime().Value.MouCaoShuangAnniversary.Valid(f) || r.Option != 3 || r.Stage != MouCaoShuangAnniversaryStage.DrawChildren || r.Draw?.ActualDrawIds.Count != actual)
            throw new InvalidOperationException("Jianzhuan recast requires its actual typed draw result.");
        for (var i = 0; i < r.Cards.Count; i++)
            advanceEventRulesAndQueueFact(new CardRecastEvent(r.Payer, r.Cards[i].Id, r.Cards[i].Kind, i < actual ? 1 : 0));
        getCoreGameEngineNativeRecastInvocationsRuntime().CompleteFrameNativeRecastInvocation(f, actual);
    }

    public void Delete(ProgramSkillFrame f)
    {
        if (!getCharacterCaoShuangMouRuntime().Value.MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Operation: SkillProgramEffectOp.DeleteMouCaoShuangJianzhuanOption } r)
            throw new InvalidOperationException("Jianzhuan deletion requires its actual completed Play.");
        var items = MouCaoShuangAnniversaryRuntime.Items(r.Remaining);
        var before = getDeterministicRandom().State;
        var item = items[getDeterministicRandom().Next(items.Length)];
        advanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryDeletedEvent(f.Id, f.OwnerSeat, r.Turn, r.PhaseId, item, r.Remaining, r.Remaining & ~(1 << (item - 1)), before, getDeterministicRandom().State));
    }

    public void Maximum(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(f, 2);
    public void Recover(ProgramSkillFrame f) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 2, null, null);
    public void Grant(ProgramSkillFrame f)
    {
        if (!getCharacterCaoShuangMouRuntime().Value.MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { Stage: MouCaoShuangAnniversaryStage.Grant })
            throw new InvalidOperationException("Fanshi grant requires its completed native repeats and recovery.");
        foreach (var p in matchParticipantsState.ReferenceToPlayers)
            getDictionary().TryAdd(p.Seat, p.SkillGrants.Revision);
        var owner = matchParticipantsState.ReferenceToPlayers[f.OwnerSeat];
        foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == MouCaoShuangAnniversaryRuntime.Jianzhuan).ToArray())
            owner.SkillGrants.RemoveGrant(grant.GrantId);
        getCoreGameEngineCharacterHostRuntime().AcquireRuntimeSkills(owner, f.SkillId, [MouCaoShuangAnniversaryRuntime.Fudou]);
        advanceEventRulesAndQueueFact(new MouCaoShuangAnniversaryGrantedEvent(f.Id, f.OwnerSeat));
    }

    public void LoseHp(ProgramSkillFrame f, int target) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
}
