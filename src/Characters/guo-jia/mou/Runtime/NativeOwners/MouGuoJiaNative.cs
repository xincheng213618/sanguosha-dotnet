#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoJiaNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterGuoJiaMouEntries> getCharacterGuoJiaMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, Func<CardZoneStore> getCardZoneStore, MouGuoJiaFactsFactory createMouGuoJiaFacts, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineCharacterHostEntries> getCoreGameEngineCharacterHostRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime) : IMouGuoJiaNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void State(ProgramSkillFrame f, bool yin)
    {
        if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(f) || f.SkillId != MouGuoJiaRuntime.Xianmou)
            throw new InvalidOperationException("Xianmou state requires its issued opening or paid original branch.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, f.SkillId, f.SkillInstanceId, "yin", yin);
    }

    public IReadOnlyList<MouGjEntity> Peek(ProgramSkillFrame f, int count)
    {
        if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(f) || f.MouGuoJia is not { } r || !(r.Stage == MouGuoJiaStage.RemoveGrantChildren && count == 5 || r.Operation == SkillProgramEffectOp.ResolveMouGuoJiaYiji && r.Stage == MouGuoJiaStage.YijiCard && count == 2))
            throw new InvalidOperationException("Mou Guo Jia can only privately view the exact current top five or inherited Yiji top two.");
        _ = getCorePhysicalDrawFacadesRuntime().EnsureDrawPile();
        return Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DrawPile).TakeLast(count).Reverse().Select(c => new MouGjEntity(c.Id, c.Kind, CardLocation.DrawPile, c.DisplayName)).ToArray());
    }

    public void Move(ProgramSkillFrame f)
    {
        if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(f) || f.MouGuoJia is not { Stage: MouGuoJiaStage.MovementChildren or MouGuoJiaStage.YijiChildren } r || r.Selected.Any(id => !r.Viewed.Any(c => c.Id == id && getCardZoneStore().GetLocation(id) == c.From)) || r.Yin && r.Selected.Any(id => !createMouGuoJiaFacts().Discardable(r.Target, id)))
            throw new InvalidOperationException("Mou Guo Jia lost his original privately viewed payable entity.");
        var yiji = r.Stage == MouGuoJiaStage.YijiChildren;
        var to = r.Yin ? CardLocation.DiscardPile : CardLocation.Hand(yiji ? r.Target : f.OwnerSeat);
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouGuoJia!;
            if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Viewed.Any(c => r.Selected.Contains(c.Id) && c.Id == m.CardId && c.From == m.From) || m.Sequence <= r.Before || m.To != to && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Guo Jia lost his actual selected gain, native logical discard or top-card Give receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouGuoJia = paid with { Actual = moves.Count(m => m.To == to), After = moves.Max(m => m.Sequence) } });
            advanceEventRulesAndQueueFact(new MouGjMovementPaidEvent(f.Id, r.Cursor, moves.Count(m => m.To == to), r.Before, moves.Max(m => m.Sequence)));
            if (yiji)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, r.Selected, r.Before);
        }

        if (r.Yin)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Target, r.Selected, new("skill-program.mou:xianmou.discard"), Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(r.Selected, to, new(yiji ? "skill-program.mou-guo-jia:yiji.give" : "skill-program.mou:xianmou.obtain"), Paid);
    }

    public void YijiGrant(ProgramSkillFrame f, bool acquire)
    {
        if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(f) || f.MouGuoJia is not { Yin: false } r || !(acquire ? r.Stage == MouGuoJiaStage.GrantChildren && r.Actual < r.X : r.Stage == MouGuoJiaStage.RemoveGrantChildren))
            throw new InvalidOperationException("Only another actual Yang activation may replace its own earlier Yiji source.");
        var key = MouGuoJiaRuntime.GrantSource(f);
        var count = 0;
        if (acquire)
            count = getCoreGameEngineCharacterHostRuntime().AcquireRuntimeSkills(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], key, [MouGuoJiaRuntime.Yiji]).Count;
        else
            foreach (var grant in matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == MouGuoJiaRuntime.Yiji && g.SourceId == "acquired:" + key).ToArray())
                if (matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId))
                    count++;
        advanceEventRulesAndQueueFact(new MouGjYijiGrantEvent(f.Id, r.Source, key, acquire, count));
    }

    public void Judge(ProgramSkillFrame f) => getCharacterGuoJiaMouRuntime().BeginMouGjJudgment(f);
    public void Damage(ProgramSkillFrame f)
    {
        if (!getCharacterGuoJiaMouRuntime().MouGuoJia.Valid(f) || f.MouGuoJia is not { Stage: MouGuoJiaStage.DamageChildren, LightningHit: true } r || getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<MouGjLightningIssuedEvent>().Count(p => p.FrameId == f.Id && p.JudgmentId == r.JudgmentId && p.Target == f.OwnerSeat && p.Amount == 3) != 1)
            throw new InvalidOperationException("Xianmou requires his exact finalized Lightning hit once.");
        getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true);
    }
}
