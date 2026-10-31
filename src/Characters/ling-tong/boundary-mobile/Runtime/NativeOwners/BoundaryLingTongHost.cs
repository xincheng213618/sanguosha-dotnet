#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLingTongBoundaryMobileRuntime>> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongHost
{
    public SkillProgramStepOutcome SelectEquipmentPair(ProgramSkillFrame frame) => getCharacterLingTongBoundaryMobileRuntime().Value.SelectBoundaryLingTongEquipmentPair(frame);
}
