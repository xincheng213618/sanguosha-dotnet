#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYuanYinHost(Func<RuntimeEntries.CharacterYuanYinAnniversaryEntries> getCharacterYuanYinAnniversaryRuntime) : IAnniversaryYuanYinHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveAnniversaryYuanYinMoshou ? getCharacterYuanYinAnniversaryRuntime().BeginYuanYinMoshou(f) : getCharacterYuanYinAnniversaryRuntime().BeginYuanYinYunjiu(f);
}
