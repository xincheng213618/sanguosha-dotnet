#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WangYunProgramCapability(Func<RuntimeEntries.CharacterWangYunOlEntries> getCharacterWangYunOlRuntime) : IWangYunProgramHost
{
    public SkillProgramStepOutcome ResolveLianjiPaidWeaponAndSlash(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().ResolveLianjiPaidWeaponAndSlash(f);
    public SkillProgramStepOutcome AwakenFromIssuedLianjiSlashDamage(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().AwakenFromIssuedLianjiSlashDamage(f);
    public SkillProgramStepOutcome UseThreeRandomTricksFromSelectedMaterial(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().UseThreeRandomTricksFromSelectedMaterial(f);
}
