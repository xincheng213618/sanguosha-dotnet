#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramCapabilitiesRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IConfiguredConversionProgramHost.DeclareBoundCardName(ProgramSkillFrame frame, string sourceBind, string stateId) => _coreConfiguredConversionsOperations.BeginConfiguredCardDeclaration(frame, sourceBind, stateId);
    void global::CardGame.Core.IConfiguredConversionProgramHost.UpgradeConversionTier(ProgramSkillFrame frame, string stateId) => _coreProgramSkillChangesOperations.UpgradeConfiguredConversionTier(frame, stateId);
    SkillProgramStepOutcome global::CardGame.Core.IActionCategoryGiftProgramHost.ChooseDifferentActionCategoryGift(ProgramSkillFrame frame, int targetSeat, string resultBind, IReadOnlyList<CardZoneKind> zones) => _coreActionCategoryGiftsOperations.BeginDifferentActionCategoryGift(frame, targetSeat, resultBind, zones);
}
