#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanQiangangCapability(Func<RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanQiangangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoQiangang(f);
}
