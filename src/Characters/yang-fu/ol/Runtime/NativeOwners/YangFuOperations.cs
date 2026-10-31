#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangFuOperations(Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime>> getCoreGameEngineTriggeredCardProgramsRuntime, Func<RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries> getCoreGameEngineDeckEquipmentProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries> getCoreGameEngineProgramCardUseRolesRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineCardUseDebitsEntries> getCoreGameEngineCardUseDebitsRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IYangFuOperations
{
    public bool BeginCharacterStateChildren(long frameId) => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    public bool BeginHpChildren(long frameId) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    public bool BeginMovementChildren(long frameId) => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(frameId);
    public bool BeginSkillChildren(long frameId) => getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(frameId);
    public SkillProgramStepOutcome UseVirtualSlash(ProgramSkillFrame frame, int target) => getCoreGameEngineTriggeredCardProgramsRuntime().Value.UseTriggeredProgramVirtualSlash(frame, target);
    public SkillProgramStepOutcome UseDeckEquipment(ProgramSkillFrame frame, int target, string bind) => getCoreGameEngineDeckEquipmentProgramsRuntime().UseProgramRandomDeckEquipment(frame, target, bind);
    public void Finish(ProgramSkillFrame frame, bool completed) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
    public int DrawCards(int recipient, int amount, CardMoveReason reason) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[recipient], amount, true, reason).Count;
    public void PublishState() => advanceRulesAndPublishState();
    public void UpdateRoleUse(CardUseFrame use, CardActionContext action) => getCoreGameEngineProgramCardUseRolesRuntime().UpdateProgramRoleCardUse(use, action);
    public void SetNativeOrigin(long useId, YangFuNativeOrigin origin) => getCoreGameEngineCardUseLifecycleRuntime().UpdateLifecycleCardUse(useId, use => use with { SuyiOrigin = origin });
    public void RecordSlashDebit(long useId, int owner) => getCoreGameEngineCardUseDebitsRuntime().RecordSlashUseDebit(useId, owner);
    public bool AdvanceObserverSubtree() => getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(true);
}
