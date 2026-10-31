#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoChunProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries> getCharacterCaoChunOlRuntime) : ICaoChunProgramHost
{
    public SkillProgramStepOutcome ExecuteCaoChun(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoChunOlRuntime().ExecuteCaoChun(f, op);
}
