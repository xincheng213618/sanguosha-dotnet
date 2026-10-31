#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouYuJinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuJinMouMobileRuntime>> getCharacterYuJinMouMobileRuntime) : IMouYuJinHost
{
    public SkillProgramStepOutcome Xiayuan(ProgramSkillFrame f) => getCharacterYuJinMouMobileRuntime().Value.ResolveMouXiayuan(f);
    public SkillProgramStepOutcome Jieyue(ProgramSkillFrame f) => getCharacterYuJinMouMobileRuntime().Value.ResolveMouJieyue(f);
}
