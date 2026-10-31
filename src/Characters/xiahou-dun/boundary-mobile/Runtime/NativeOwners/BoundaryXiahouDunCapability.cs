#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXiahouDunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouDunBoundaryMobileEntries> getCharacterXiahouDunBoundaryMobileRuntime) : IBoundaryXiahouDunHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunBoundaryMobileRuntime().BeginBoundaryXiahouDun(f, op);
}
