#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuAnGuoCapability(Func<RuntimeEntries.CharacterWuAnGuoOlEntries> getCharacterWuAnGuoOlRuntime) : IWuAnGuoProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterWuAnGuoOlRuntime().BeginWuAnGuoLiyong(frame);
}
