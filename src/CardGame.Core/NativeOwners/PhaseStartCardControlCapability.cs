#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PhaseStartCardControlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseStartCardControlEntries> getCoreGameEnginePhaseStartCardControlRuntime) : IPhaseStartCardControlHost
{
    public SkillProgramStepOutcome GrantOtherPlayUseRestriction(ProgramSkillFrame frame, string skillId) => getCoreGameEnginePhaseStartCardControlRuntime().BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.OtherPlayRestriction, skillId);
    public SkillProgramStepOutcome ChooseStartingEquipmentControl(ProgramSkillFrame frame) => getCoreGameEnginePhaseStartCardControlRuntime().BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.StartingEquipment, null);
}
