#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWangYiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangYiBoundaryMobileEntries> getCharacterWangYiBoundaryMobileRuntime) : IBoundaryWangYiHost
{
    public SkillProgramStepOutcome ChooseMijiCount(ProgramSkillFrame frame) => getCharacterWangYiBoundaryMobileRuntime().ChooseBoundaryWangYiMijiCount(frame);
}
