#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuanPingNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CharacterGuanPingMouEntries> getCharacterGuanPingMouRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries> getCoreGameEngineCurrentTurnSkillSuppressionsRuntime) : IMouGuanPingNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void SetMask(ProgramSkillFrame f, int mask)
    {
        if (f.MouGuanPing is not { } r || r.Mask != mask || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(r.Use.UseId)is not { MouGuanPing: { } receipt } use || receipt.ActionId != r.Use.ActionId || receipt != r.Use with
        {
            OptionMask = receipt.OptionMask
        }

        )
            throw new InvalidOperationException("Wuwei may only modify its own original issued Slash.");
        getCoreGameEngineCardUseLifecycleRuntime().UpdateLifecycleCardUse(use.Id, u => u with { MouGuanPing = receipt with { OptionMask = mask } });
    }

    public void Suppress(ProgramSkillFrame f, int target)
    {
        if (!getCharacterGuanPingMouRuntime().MouGuanPing.Valid(f) || f.MouGuanPing is not { Stage: MouGuanPingStage.Children } r || !r.Targets.Contains(target))
            throw new InvalidOperationException("Wuwei suppression requires an actual original Slash target.");
        getCoreGameEngineCurrentTurnSkillSuppressionsRuntime().IssueCurrentTurnNonLockedSkillSuppression(f, target);
    }
}
