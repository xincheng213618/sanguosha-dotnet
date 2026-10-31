#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YanRouCapability(Func<RuntimeEntries.CharacterYanRouOlEntries> getCharacterYanRouOlRuntime) : IYanRouHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanRouOlRuntime().BeginYanRou(f, op);
}
