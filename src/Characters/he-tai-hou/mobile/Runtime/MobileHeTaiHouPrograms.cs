using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ActualTurnDeathOwnerKillsOnly { get; internal init; }
}
public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 76500)]
    private bool CanOfferOwnerKillDeathDraw(SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(e => e.ActualTurnDeathOwnerKillsOnly == true) ||
        context.Facts?.OwnerActualTurnKillCount is > 0;
}
