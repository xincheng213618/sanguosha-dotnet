#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanSlashCapability(Func<RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanSlashHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunQuanOlMagicRuntime().BeginMoQuanyuSlash(frame);
}
