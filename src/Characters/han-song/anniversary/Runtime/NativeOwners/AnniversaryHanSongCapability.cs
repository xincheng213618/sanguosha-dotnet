#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHanSongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHanSongAnniversaryRuntime>> getCharacterHanSongAnniversaryRuntime) : IAnniversaryHanSongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHanSongAnniversaryRuntime().Value.BeginAnniversaryHanSong(frame);
}
