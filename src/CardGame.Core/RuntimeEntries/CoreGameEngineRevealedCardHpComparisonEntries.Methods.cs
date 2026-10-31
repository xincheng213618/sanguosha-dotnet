#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealedCardHpComparisonEntries
{
    internal bool IsRevealedHpComparisonProgramDying() => _resolve().IsRevealedHpComparisonProgramDying();
    internal global::CardGame.Core.SkillProgramStepOutcome LoseHpIfRevealedNonEquipmentDiffers(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().LoseHpIfRevealedNonEquipmentDiffers(frame, bind);
}
