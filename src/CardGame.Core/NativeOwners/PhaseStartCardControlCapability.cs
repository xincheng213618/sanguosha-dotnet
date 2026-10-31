#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PhaseStartCardControlCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePhaseStartCardControlRuntime>> getCoreGameEnginePhaseStartCardControlRuntime) : IPhaseStartCardControlHost
{
    public SkillProgramStepOutcome GrantOtherPlayUseRestriction(ProgramSkillFrame frame, string skillId) => getCoreGameEnginePhaseStartCardControlRuntime().Value.BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.OtherPlayRestriction, skillId);
    public SkillProgramStepOutcome ChooseStartingEquipmentControl(ProgramSkillFrame frame) => getCoreGameEnginePhaseStartCardControlRuntime().Value.BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.StartingEquipment, null);
}
