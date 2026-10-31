#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YaDanCapability(Func<RuntimeEntries.CharacterYaDanOlEntries> getCharacterYaDanOlRuntime) : IYaDanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYaDanOlRuntime().BeginYaDan(f, op);
}
