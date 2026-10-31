#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuPanCapability(Func<RuntimeEntries.CharacterLiuPanOlEntries> getCharacterLiuPanOlRuntime) : ILiuPanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuPanOlRuntime().BeginPijing(f, op);
}
