#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanTianenCapability(Func<RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanTianenHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoTianen(f);
}
