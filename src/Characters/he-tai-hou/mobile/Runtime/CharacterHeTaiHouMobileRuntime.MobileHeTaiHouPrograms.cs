#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeTaiHouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 76500)]
    internal bool CanOfferOwnerKillDeathDraw(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.ActualTurnDeathOwnerKillsOnly == true) || context.Facts?.OwnerActualTurnKillCount is> 0;
}
