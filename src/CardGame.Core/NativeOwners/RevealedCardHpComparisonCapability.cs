#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RevealedCardHpComparisonCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineRevealedCardHpComparisonRuntime>> getCoreGameEngineRevealedCardHpComparisonRuntime) : IRevealedCardHpComparisonHost
{
    public SkillProgramStepOutcome LoseHpIfRevealedNonEquipmentDiffers(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineRevealedCardHpComparisonRuntime().Value.LoseHpIfRevealedNonEquipmentDiffers(frame, sourceBind);
}
