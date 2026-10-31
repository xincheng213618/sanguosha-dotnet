#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionCategoryGiftsEntries
{
    internal void AssertDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertDifferentActionCategoryGift(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, int provider, string resultBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones) => _resolve().BeginDifferentActionCategoryGift(frame, provider, resultBind, zones);
    internal void ResolveDifferentActionCategoryGift(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDifferentActionCategoryGift(choice);
    internal global::CardGame.Core.PromptChoice SelectAiDifferentActionCategoryGift(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiDifferentActionCategoryGift(decision, frame);
}
