#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShaoMobileShiRuntime
{
    internal bool HasShiSsCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiSsGanjue);

    internal bool IsShiSsGanjueStrikeUse(long useFrameId) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiSsGanjueIssuedEvent>().Any(entry => entry.UseId == useFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 68400)]
    internal bool ShiSsGanjueUseDistance(ProgramCardDistanceQuery query) => query.CardUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 68400)]
    internal bool ShiSsGanjueSlashDistance(ProgramSlashDistanceQuery query) => query.ExistingUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);
}
