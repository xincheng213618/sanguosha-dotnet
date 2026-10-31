#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWangYiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangYiBoundaryMobileRuntime>> getCharacterWangYiBoundaryMobileRuntime) : IBoundaryWangYiHost
{
    public SkillProgramStepOutcome ChooseMijiCount(ProgramSkillFrame frame) => getCharacterWangYiBoundaryMobileRuntime().Value.ChooseBoundaryWangYiMijiCount(frame);
}
