#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuanPingNative(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanPingMouEntries> getCharacterGuanPingMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries> getCoreGameEngineCurrentTurnSkillSuppressionsRuntime) : IMouGuanPingNative
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
