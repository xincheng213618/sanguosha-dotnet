#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuixinCapability(Func<RuntimeEntries.CharacterCaoCaoMobileGodEntries> getCharacterCaoCaoMobileGodRuntime) : IMobileGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoMobileGodRuntime().BeginMobileGuixin(f);
}
