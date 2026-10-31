#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramCapabilitiesRuntime
{
    SkillProgramStepOutcome IConfiguredConversionProgramHost.DeclareBoundCardName(ProgramSkillFrame frame, string sourceBind, string stateId) => _coreConfiguredConversionsOperations.Value.BeginConfiguredCardDeclaration(frame, sourceBind, stateId);
    void IConfiguredConversionProgramHost.UpgradeConversionTier(ProgramSkillFrame frame, string stateId) => _coreProgramSkillChangesOperations.UpgradeConfiguredConversionTier(frame, stateId);
    SkillProgramStepOutcome IActionCategoryGiftProgramHost.ChooseDifferentActionCategoryGift(ProgramSkillFrame frame, int targetSeat, string resultBind, IReadOnlyList<CardZoneKind> zones) => _coreActionCategoryGiftsOperations.Value.BeginDifferentActionCategoryGift(frame, targetSeat, resultBind, zones);
}
