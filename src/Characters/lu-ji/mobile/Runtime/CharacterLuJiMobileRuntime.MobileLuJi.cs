#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuJiMobileRuntime
{
    // This qualifies new invocations. An already-paid Yili continues on its
    // original program frame even if its HP-loss child removes Huaiju.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 80500)]
    internal bool MobileLuJiRequiresHuaiju(ProgramTriggerLegalityContext gate) => gate.SkillId is not ("mobile:yili" or "mobile:zhenglun") || _coreNationalSkillsOperations.GetSkillBindingShard(gate.Owner).HasSkill("classic:huaiju");
}
