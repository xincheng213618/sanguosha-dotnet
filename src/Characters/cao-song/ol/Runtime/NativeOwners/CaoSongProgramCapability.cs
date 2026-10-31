#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoSongProgramCapability(Func<RuntimeEntries.CharacterCaoSongOlEntries> getCharacterCaoSongOlRuntime) : ICaoSongProgramHost
{
    public SkillProgramStepOutcome ReplaceDrawWithLiluGift(ProgramSkillFrame frame) => getCharacterCaoSongOlRuntime().BeginCaoSongLilu(frame);
    public void GrantYizhengSupport(ProgramSkillFrame frame) => getCharacterCaoSongOlRuntime().IssueCaoSongYizheng(frame);
}
