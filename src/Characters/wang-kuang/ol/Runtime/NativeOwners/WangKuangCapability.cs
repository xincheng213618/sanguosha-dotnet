#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WangKuangCapability(Func<RuntimeEntries.CharacterWangKuangOlEntries> getCharacterWangKuangOlRuntime) : IWangKuangProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool ending) => getCharacterWangKuangOlRuntime().BeginWangKuang(f, ending);
}
