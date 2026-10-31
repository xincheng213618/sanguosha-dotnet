#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHanSongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries> getCharacterHanSongAnniversaryRuntime) : IAnniversaryHanSongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHanSongAnniversaryRuntime().BeginAnniversaryHanSong(frame);
}
