#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuixinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoCaoMobileGodRuntime>> getCharacterCaoCaoMobileGodRuntime) : IMobileGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoMobileGodRuntime().Value.BeginMobileGuixin(f);
}
