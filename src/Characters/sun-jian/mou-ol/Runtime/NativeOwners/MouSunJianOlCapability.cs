#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouSunJianOlCapability(Func<RuntimeEntries.CharacterSunJianMouOlEntries> getCharacterSunJianMouOlRuntime) : IMouSunJianOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouSunJianYipo ? getCharacterSunJianMouOlRuntime().BeginMouSunJianOlYipo(f) : getCharacterSunJianMouOlRuntime().BeginMouSunJianOlHulie(f, op);
}
