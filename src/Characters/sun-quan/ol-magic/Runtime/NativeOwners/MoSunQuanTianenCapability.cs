#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanTianenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanTianenHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoTianen(f);
}
