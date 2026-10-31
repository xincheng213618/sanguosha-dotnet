#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeTaiHouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 76500)]
    internal bool CanOfferOwnerKillDeathDraw(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.ActualTurnDeathOwnerKillsOnly == true) || context.Facts?.OwnerActualTurnKillCount is> 0;
}
