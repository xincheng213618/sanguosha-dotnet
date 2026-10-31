#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OlGuixinCapability(Func<RuntimeEntries.CharacterCaoCaoOlGodEntries> getCharacterCaoCaoOlGodRuntime) : IOlGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoOlGodRuntime().BeginOlGuixin(f);
}
