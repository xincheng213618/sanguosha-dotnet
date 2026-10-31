#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RevealedCardHpComparisonCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRevealedCardHpComparisonEntries> getCoreGameEngineRevealedCardHpComparisonRuntime) : IRevealedCardHpComparisonHost
{
    public SkillProgramStepOutcome LoseHpIfRevealedNonEquipmentDiffers(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineRevealedCardHpComparisonRuntime().LoseHpIfRevealedNonEquipmentDiffers(frame, sourceBind);
}
