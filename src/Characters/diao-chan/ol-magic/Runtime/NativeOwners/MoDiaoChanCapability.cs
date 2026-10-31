#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoDiaoChanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanOlMagicEntries> getCharacterDiaoChanOlMagicRuntime) : IMoDiaoChanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterDiaoChanOlMagicRuntime().BeginMoDc(frame, op);
}
