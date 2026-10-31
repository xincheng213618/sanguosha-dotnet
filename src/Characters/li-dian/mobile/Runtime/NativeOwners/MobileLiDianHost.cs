#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileLiDianHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiDianMobileRuntime>> getCharacterLiDianMobileRuntime) : IMobileLiDianWangxiHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame f) => getCharacterLiDianMobileRuntime().Value.BeginMobileLiDianWangxi(f);
}
