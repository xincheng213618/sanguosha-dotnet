#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiuFengHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries> getCharacterLiuFengBoundaryRuntime) : IBoundaryLiuFengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuFengBoundaryRuntime().BeginBoundaryLiuFeng(f, op);
}
