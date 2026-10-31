#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ActionCategoryGiftProgramCapability(Func<RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries> getCoreGameEngineActionCategoryGiftsRuntime) : IActionCategoryGiftProgramHost
{
    public SkillProgramStepOutcome ChooseDifferentActionCategoryGift(ProgramSkillFrame frame, int targetSeat, string resultBind, IReadOnlyList<CardZoneKind> zones) => getCoreGameEngineActionCategoryGiftsRuntime().BeginDifferentActionCategoryGift(frame, targetSeat, resultBind, zones);
}
