#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    internal bool HasMouLuBuMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuBuLiyu);

    internal static CardConversionSource MouLbSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_bu_mou_mobile_MouLuBuMobile.MouLbSource(f);
    SkillProgramStepOutcome IMouLuBuMobileHost.Bonus(ProgramSkillFrame f) => ResolveMouLbBonus(f);
    SkillProgramStepOutcome IMouLuBuMobileHost.Liyu(ProgramSkillFrame f) => BeginMouLbLiyu(f);
}
