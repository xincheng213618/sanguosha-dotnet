#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappySunQuanCapability(Func<RuntimeEntries.CharacterSunQuanHappyGodEntries> getCharacterSunQuanHappyGodRuntime) : IHappySunQuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunQuanHappyGodRuntime().BeginHappySunQuan(f, op);
}
