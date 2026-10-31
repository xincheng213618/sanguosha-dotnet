#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionCategoryGiftsEntries
{
    internal SkillProgramStepOutcome BeginDifferentActionCategoryGift(ProgramSkillFrame frame, int provider, string resultBind, IReadOnlyList<CardZoneKind> zones) => _resolve().BeginDifferentActionCategoryGift(frame, provider, resultBind, zones);
    internal void ResolveDifferentActionCategoryGift(PromptChoice choice) => _resolve().ResolveDifferentActionCategoryGift(choice);
    internal PromptChoice SelectAiDifferentActionCategoryGift(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiDifferentActionCategoryGift(decision, frame);
}
