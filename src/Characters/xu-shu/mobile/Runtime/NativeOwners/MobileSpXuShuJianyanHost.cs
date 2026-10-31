#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileSpXuShuJianyanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXuShuMobileRuntime>> getCharacterXuShuMobileRuntime) : IMobileSpXuShuJianyanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXuShuMobileRuntime().Value.BeginMobileSpXuShuJianyan(f);
}
