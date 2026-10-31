#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DuanJiongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuanJiongOlEntries> getCharacterDuanJiongOlRuntime) : IDuanJiongProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterDuanJiongOlRuntime().BeginDuanJiongSaogu(f);
}
