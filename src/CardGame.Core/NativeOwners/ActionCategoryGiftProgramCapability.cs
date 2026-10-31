#nullable enable
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core.NativeOwners;
internal sealed class ActionCategoryGiftProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries> getCoreGameEngineActionCategoryGiftsRuntime) : IActionCategoryGiftProgramHost
{
    public SkillProgramStepOutcome ChooseDifferentActionCategoryGift(ProgramSkillFrame frame, int targetSeat, string resultBind, IReadOnlyList<CardZoneKind> zones) => getCoreGameEngineActionCategoryGiftsRuntime().BeginDifferentActionCategoryGift(frame, targetSeat, resultBind, zones);
}
