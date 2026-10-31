#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanSlashCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanSlashHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunQuanOlMagicRuntime().BeginMoQuanyuSlash(frame);
}
