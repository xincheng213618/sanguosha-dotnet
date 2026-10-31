#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MengDaCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMengDaOlEntries> getCharacterMengDaOlRuntime) : IMengDaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterMengDaOlRuntime().BeginGoude(f);
}
