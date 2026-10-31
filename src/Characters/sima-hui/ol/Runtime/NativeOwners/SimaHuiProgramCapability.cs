#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SimaHuiProgramCapability(Func<RuntimeEntries.CharacterSimaHuiOlEntries> getCharacterSimaHuiOlRuntime) : ISimaHuiProgramHost
{
    public SkillProgramStepOutcome BeginSimaHuiOperation(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterSimaHuiOlRuntime().BeginSimaHuiOperation(frame, op);
}
