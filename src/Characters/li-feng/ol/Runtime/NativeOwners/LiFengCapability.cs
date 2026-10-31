#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LiFengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries> getCharacterLiFengOlRuntime) : ILiFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiFengOlRuntime().BeginLiFeng(f, op);
}
