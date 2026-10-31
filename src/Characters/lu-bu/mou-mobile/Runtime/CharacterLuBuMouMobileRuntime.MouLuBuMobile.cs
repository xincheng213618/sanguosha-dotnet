#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    internal bool HasMouLuBuMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuBuLiyu);

    internal static CardConversionSource MouLbSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_bu_mou_mobile_MouLuBuMobile.MouLbSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouLuBuMobileHost.Bonus(ProgramSkillFrame f) => ResolveMouLbBonus(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouLuBuMobileHost.Liyu(ProgramSkillFrame f) => BeginMouLbLiyu(f);
}
