#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuHuangFuSongCapability(Func<RuntimeEntries.CharacterHuangfuSongWumiaoEntries> getCharacterHuangfuSongWumiaoRuntime) : IWuHuangFuSongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangfuSongWumiaoRuntime().BeginWuHuangFuSong(f, op);
}
