#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownBoundGiftsEntries
{
    internal void AssertRegisteredShownGiftReceipt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertRegisteredShownGiftReceipt(frame, plan);
    internal void AssertShownGiftReceipt(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> instructions) => _resolve().AssertShownGiftReceipt(frame, instructions);
    internal global::CardGame.Core.SkillProgramStepOutcome GiveProgramShownBoundGift(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().GiveProgramShownBoundGift(frame, bind);
}
