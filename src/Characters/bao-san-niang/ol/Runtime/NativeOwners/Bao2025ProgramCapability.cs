#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class Bao2025ProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBaoSanNiangOlEntries> getCharacterBaoSanNiangOlRuntime) : IBao2025ProgramHost
{
    public SkillProgramStepOutcome BeginBao2025(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterBaoSanNiangOlRuntime().BeginBao2025(f, e);
}
