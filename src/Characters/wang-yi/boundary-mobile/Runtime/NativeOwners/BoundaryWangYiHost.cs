#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWangYiHost(Func<RuntimeEntries.CharacterWangYiBoundaryMobileEntries> getCharacterWangYiBoundaryMobileRuntime) : IBoundaryWangYiHost
{
    public SkillProgramStepOutcome ChooseMijiCount(ProgramSkillFrame frame) => getCharacterWangYiBoundaryMobileRuntime().ChooseBoundaryWangYiMijiCount(frame);
}
