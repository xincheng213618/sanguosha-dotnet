#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuNative(Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CharacterChengYuMouOlEntries> getCharacterChengYuMouOlRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouChengYuNative
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
