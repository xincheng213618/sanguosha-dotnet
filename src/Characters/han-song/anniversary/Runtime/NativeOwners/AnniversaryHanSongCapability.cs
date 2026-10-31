#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHanSongCapability(Func<RuntimeEntries.CharacterHanSongAnniversaryEntries> getCharacterHanSongAnniversaryRuntime) : IAnniversaryHanSongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHanSongAnniversaryRuntime().BeginAnniversaryHanSong(frame);
}
