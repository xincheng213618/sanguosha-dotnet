#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellActivityProgramsEntries
{
    internal bool AllowsHellActivityTier(ProgramTriggerLegalityContext gate) => _resolve().AllowsHellActivityTier(gate);
    internal SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ExecuteHellActivity(supplied, operation);
    internal void ReturnHellChongshengDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnHellChongshengDraw(frame, outcome);
}
