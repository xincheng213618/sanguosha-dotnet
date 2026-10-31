#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuAnGuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuAnGuoOlEntries> getCharacterWuAnGuoOlRuntime) : IWuAnGuoProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterWuAnGuoOlRuntime().BeginWuAnGuoLiyong(frame);
}
