#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MoDiaoChanCapability(Func<RuntimeEntries.CharacterDiaoChanOlMagicEntries> getCharacterDiaoChanOlMagicRuntime) : IMoDiaoChanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterDiaoChanOlMagicRuntime().BeginMoDc(frame, op);
}
