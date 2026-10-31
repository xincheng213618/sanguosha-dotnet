#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SimaHuiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaHuiOlEntries> getCharacterSimaHuiOlRuntime) : ISimaHuiProgramHost
{
    public SkillProgramStepOutcome BeginSimaHuiOperation(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterSimaHuiOlRuntime().BeginSimaHuiOperation(frame, op);
}
