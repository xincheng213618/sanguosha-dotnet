#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LeCaoZhiWenlanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries> getCharacterCaoZhiLeRuntime) : ILeCaoZhiWenlanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoZhiLeRuntime().BeginLeCaoZhiWenlan(f);
}
