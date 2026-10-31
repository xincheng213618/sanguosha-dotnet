#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DuanJiongCapability(Func<RuntimeEntries.CharacterDuanJiongOlEntries> getCharacterDuanJiongOlRuntime) : IDuanJiongProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterDuanJiongOlRuntime().BeginDuanJiongSaogu(f);
}
