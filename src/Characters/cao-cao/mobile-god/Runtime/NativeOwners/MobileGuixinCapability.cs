#nullable enable
using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuixinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoCaoMobileGodEntries> getCharacterCaoCaoMobileGodRuntime) : IMobileGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoMobileGodRuntime().BeginMobileGuixin(f);
}
