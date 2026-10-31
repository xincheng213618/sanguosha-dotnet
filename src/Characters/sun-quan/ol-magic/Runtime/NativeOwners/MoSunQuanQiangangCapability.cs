#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanQiangangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanQiangangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoQiangang(f);
}
