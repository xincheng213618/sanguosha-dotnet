#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYuanYinHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanYinAnniversaryRuntime>> getCharacterYuanYinAnniversaryRuntime) : IAnniversaryYuanYinHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveAnniversaryYuanYinMoshou ? getCharacterYuanYinAnniversaryRuntime().Value.BeginYuanYinMoshou(f) : getCharacterYuanYinAnniversaryRuntime().Value.BeginYuanYinYunjiu(f);
}
