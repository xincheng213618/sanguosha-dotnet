#nullable enable
using System.Globalization;
using System.Text;

namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiYongFeibaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries> getCharacterCaiYongLeRuntime) : ILeCaiYongFeibaiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaiYongLeRuntime().BeginLeCaiYongFeibai(f);
}
