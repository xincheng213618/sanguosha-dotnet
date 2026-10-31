#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class XieLingYuCapability(Func<RuntimeEntries.CharacterXieLingYuOlEntries> getCharacterXieLingYuOlRuntime) : IXieLingYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterXieLingYuOlRuntime().BeginXieLingYu(frame, op);
}
