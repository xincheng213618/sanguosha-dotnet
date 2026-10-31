#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoYuCapability(Func<RuntimeEntries.CharacterCaoYuOlEntries> getCharacterCaoYuOlRuntime) : ICaoYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoYuOlRuntime().BeginCaoYu(f, op);
}
