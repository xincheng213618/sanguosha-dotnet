#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineParticipantPreventionEntries
{
    internal bool IsParticipantPreventionDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsParticipantPreventionDraw(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PreventParticipantDamageDrawActualSource(supplied);
}
