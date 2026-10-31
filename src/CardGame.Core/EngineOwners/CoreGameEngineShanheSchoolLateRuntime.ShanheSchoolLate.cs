#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolLateRuntime
{
    internal int ProgramDamageModifierAmount(SkillProgramDamageModifier modifier) => modifier.AmountExpression == SkillProgramDamageAmountExpression.CompletedRoundCount // The first round has not yet elapsed. Global rounds, rather than
 // personal or extra turns, determine this school's growth.
    ? checked(modifier.Amount * Math.Max(0, _coreActualRoundProgressRoundNumberAccess._roundNumber - 1)) : _coreShanheS5QuyiOperations.ShanheS5JiaoziModifierAmount(modifier);
}
