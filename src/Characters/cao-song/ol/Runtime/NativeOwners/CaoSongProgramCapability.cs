#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CaoSongProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoSongOlEntries> getCharacterCaoSongOlRuntime) : ICaoSongProgramHost
{
    public SkillProgramStepOutcome ReplaceDrawWithLiluGift(ProgramSkillFrame frame) => getCharacterCaoSongOlRuntime().BeginCaoSongLilu(frame);
    public void GrantYizhengSupport(ProgramSkillFrame frame) => getCharacterCaoSongOlRuntime().IssueCaoSongYizheng(frame);
}
