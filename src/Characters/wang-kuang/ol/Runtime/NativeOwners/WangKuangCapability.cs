#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WangKuangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangKuangOlEntries> getCharacterWangKuangOlRuntime) : IWangKuangProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool ending) => getCharacterWangKuangOlRuntime().BeginWangKuang(f, ending);
}
