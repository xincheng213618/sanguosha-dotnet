#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellActivityProgramsEntries
{
    internal bool AllowsHellActivityTier(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsHellActivityTier(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteHellActivity(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ExecuteHellActivity(supplied, operation);
    internal void ReturnHellChongshengDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHellChongshengDraw(frame, outcome);
}
