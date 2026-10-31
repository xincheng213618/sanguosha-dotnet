#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SunHaoCapability(Func<RuntimeEntries.CharacterSunHaoOlEntries> getCharacterSunHaoOlRuntime) : ISunHaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHaoOlRuntime().BeginSunHao(f, op);
}
