#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangJiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangJiOlEntries> getCharacterZhangJiOlRuntime) : IZhangJiProgramHost
{
    public SkillProgramStepOutcome BeginZhangJiOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangJiOlRuntime().BeginZhangJiOperation(f, op);
}
