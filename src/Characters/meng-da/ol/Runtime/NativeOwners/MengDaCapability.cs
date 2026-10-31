#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MengDaCapability(Func<RuntimeEntries.CharacterMengDaOlEntries> getCharacterMengDaOlRuntime) : IMengDaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterMengDaOlRuntime().BeginGoude(f);
}
