#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengYuMouOlEntries> getCharacterChengYuMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouChengYuNative
{
    public void FreezeLidan(CardUseFrame use, MouLidanUseReceipt receipt) => getCoreGameEngineCardUseLifecycleRuntime().UpdateLifecycleCardUse(use.Id, u => u with { MouLidan = receipt });
    public void ApplyTargets(ProgramSkillFrame f, MouLidanSelection selection) => getCharacterChengYuMouOlRuntime().ApplyMouLidanTargets(f, selection);
    public bool IssueReward(ProgramSkillFrame f, CardKind kind, int target) => getCharacterChengYuMouOlRuntime().IssueMouDanchiReward(f, kind, target);
    public bool DrainChildren(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id);
    public void Finish(ProgramSkillFrame f)
    {
        getCharacterChengYuMouOlRuntime().RestoreMouDanchiAttack(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id));
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(f);
        getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    }

    public void PublishState() => advanceRulesAndPublishState();
}
