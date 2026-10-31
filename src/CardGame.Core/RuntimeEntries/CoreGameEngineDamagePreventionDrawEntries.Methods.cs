#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionDrawEntries
{
    internal SkillProgramStepOutcome PreventDamageAndDrawMultiple(ProgramSkillFrame frame, int multiplier) => _resolve().PreventDamageAndDrawMultiple(frame, multiplier);
}
