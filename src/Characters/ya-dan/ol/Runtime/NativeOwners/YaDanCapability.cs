#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class YaDanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries> getCharacterYaDanOlRuntime) : IYaDanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYaDanOlRuntime().BeginYaDan(f, op);
}
