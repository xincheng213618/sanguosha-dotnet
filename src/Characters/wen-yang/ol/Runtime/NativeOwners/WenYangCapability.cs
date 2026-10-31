#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WenYangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries> getCharacterWenYangOlRuntime) : IWenYangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWenYangOlRuntime().BeginWenYang(f, op);
}
