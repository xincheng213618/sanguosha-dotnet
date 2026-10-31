#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OlGuixinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoCaoOlGodEntries> getCharacterCaoCaoOlGodRuntime) : IOlGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoOlGodRuntime().BeginOlGuixin(f);
}
