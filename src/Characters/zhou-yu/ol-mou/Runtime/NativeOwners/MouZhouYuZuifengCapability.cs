#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuZuifengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries> getCharacterZhouYuOlMouRuntime) : IMouZhouYuZuifengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhouYuOlMouRuntime().BeginMouZuifeng(f);
}
