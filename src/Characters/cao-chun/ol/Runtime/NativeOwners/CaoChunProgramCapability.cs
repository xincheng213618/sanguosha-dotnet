#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoChunProgramCapability(Func<RuntimeEntries.CharacterCaoChunOlEntries> getCharacterCaoChunOlRuntime) : ICaoChunProgramHost
{
    public SkillProgramStepOutcome ExecuteCaoChun(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoChunOlRuntime().ExecuteCaoChun(f, op);
}
