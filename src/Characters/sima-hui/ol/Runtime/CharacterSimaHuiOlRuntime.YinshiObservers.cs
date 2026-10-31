#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaHuiOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 420)]
    internal bool CanPreventSimaHuiDamage(int owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.PreventDamageWithoutSealsAndArmor))
            return true;
        if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || context.TargetSeat != owner || global::CardGame.Core.EngineOwners.CharacterSimaHuiOlDefinitions.SimaHuiSeals.Any(m => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(m) > 0) || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).Any(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.Armor) || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.TargetSeat != owner || window.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != (window.ContinuationAttackResolutionId ?? window.ParentFrameId) || attack.TargetSeat != owner)
            return false;
        return _coreNativeCardClassificationsOperations.GetDamageNature(attack) != DamageNature.Normal || attack.EffectiveCardKind is { } kind && (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) || kind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning);
    }
}
