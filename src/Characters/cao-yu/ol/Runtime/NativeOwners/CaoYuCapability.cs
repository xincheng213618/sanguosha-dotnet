#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CaoYuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoYuOlEntries> getCharacterCaoYuOlRuntime) : ICaoYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoYuOlRuntime().BeginCaoYu(f, op);
}
