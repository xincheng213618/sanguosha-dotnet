#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LiYiCapability(Func<RuntimeEntries.CharacterLiYiOlEntries> getCharacterLiYiOlRuntime) : ILiYiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiYiOlRuntime().BeginChanshuang(f, op);
}
