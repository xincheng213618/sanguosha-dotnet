#nullable enable
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.NativeOwners;
internal sealed class WangYunProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangYunOlEntries> getCharacterWangYunOlRuntime) : IWangYunProgramHost
{
    public SkillProgramStepOutcome ResolveLianjiPaidWeaponAndSlash(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().ResolveLianjiPaidWeaponAndSlash(f);
    public SkillProgramStepOutcome AwakenFromIssuedLianjiSlashDamage(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().AwakenFromIssuedLianjiSlashDamage(f);
    public SkillProgramStepOutcome UseThreeRandomTricksFromSelectedMaterial(ProgramSkillFrame f) => getCharacterWangYunOlRuntime().UseThreeRandomTricksFromSelectedMaterial(f);
}
