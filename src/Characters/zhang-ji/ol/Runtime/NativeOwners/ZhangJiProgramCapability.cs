#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangJiProgramCapability(Func<RuntimeEntries.CharacterZhangJiOlEntries> getCharacterZhangJiOlRuntime) : IZhangJiProgramHost
{
    public SkillProgramStepOutcome BeginZhangJiOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangJiOlRuntime().BeginZhangJiOperation(f, op);
}
