#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class NiuFuCapability(Func<RuntimeEntries.CharacterNiuFuOlEntries> getCharacterNiuFuOlRuntime) : INiuFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterNiuFuOlRuntime().BeginNiuFu(f, op);
}
