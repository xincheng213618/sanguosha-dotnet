#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ActionCategoryGiftProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineActionCategoryGiftsRuntime>> getCoreGameEngineActionCategoryGiftsRuntime) : IActionCategoryGiftProgramHost
{
    public SkillProgramStepOutcome ChooseDifferentActionCategoryGift(ProgramSkillFrame frame, int targetSeat, string resultBind, IReadOnlyList<CardZoneKind> zones) => getCoreGameEngineActionCategoryGiftsRuntime().Value.BeginDifferentActionCategoryGift(frame, targetSeat, resultBind, zones);
}
