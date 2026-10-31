#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineParticipantPreventionEntries
{
    internal bool IsParticipantPreventionDraw(ProgramSkillFrame f) => _resolve().IsParticipantPreventionDraw(f);
    internal SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(ProgramSkillFrame supplied) => _resolve().PreventParticipantDamageDrawActualSource(supplied);
}
