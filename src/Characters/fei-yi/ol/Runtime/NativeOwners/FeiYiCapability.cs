#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FeiYiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries> getCharacterFeiYiOlRuntime) : IFeiYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFeiYiOlRuntime().BeginFeiYi(f, op);
}
