#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouEntries> getCharacterZhouYuMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::CardGame.Core.NativeOwners.MouZhouYuFactsFactory createMouZhouYuFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IMouZhouYuNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void State(ProgramSkillFrame f, bool yin)
    {
        if (!getCharacterZhouYuMouRuntime().MouZyAnniversary.Valid(f))
            throw new InvalidOperationException("Yingmou state lost its original accepted branch.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, MouZhouYuRuntime.Yingmou, f.SkillInstanceId, "yin", yin);
    }

    public void Use(ProgramSkillFrame f) => getCharacterZhouYuMouRuntime().IssueMouZyUse(f);
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterZhouYuMouRuntime().MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { Stage: MouZhouYuStage.DiscardChildren, Frozen.Count: 0 } r || r.Selected.Count == 0 || r.Selected.Any(id => !createMouZhouYuFacts().DiscardableHand(r.Actor).Any(c => c.Id == id)))
            throw new InvalidOperationException("Yingmou discard requires no related Hand cards and the actor's real discardable Hand.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Actor, r.Selected, new(MouZhouYuRuntime.DiscardReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouZhouYuAnniversary!;
            if (!getCharacterZhouYuMouRuntime().MouZyAnniversary.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From != CardLocation.Hand(r.Actor) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Yingmou lost its primary native Hand discard receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouZhouYuAnniversary = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouZhouYuDiscardPaidEvent(f.Id, r.Actor, m.CardId, m.Sequence));
        });
    }

    public void Damage(ProgramSkillFrame f)
    {
        if (!getCharacterZhouYuMouRuntime().MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { Stage: MouZhouYuStage.DamageChildren, Damaged: false } r)
            throw new InvalidOperationException("Yingmou final fire damage requires its genuine undamaged child chain.");
        getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, r.Target, 1, nature: DamageNature.Fire, explicitSourceSeat: f.OwnerSeat);
    }
}
