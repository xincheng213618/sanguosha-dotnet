#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuHuangFuSongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries> getCharacterHuangfuSongWumiaoRuntime) : IWuHuangFuSongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangfuSongWumiaoRuntime().BeginWuHuangFuSong(f, op);
}
