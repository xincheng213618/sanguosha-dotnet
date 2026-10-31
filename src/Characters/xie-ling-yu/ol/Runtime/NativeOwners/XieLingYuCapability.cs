#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class XieLingYuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXieLingYuOlEntries> getCharacterXieLingYuOlRuntime) : IXieLingYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterXieLingYuOlRuntime().BeginXieLingYu(frame, op);
}
