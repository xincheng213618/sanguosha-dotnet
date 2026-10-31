#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceGrainRuntime
{
    internal bool IsShanheSourceGrainContext(string skillId, SkillProgramEffectOp operation) => _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && (skillId == "shanhe:source-cangchu-1182" && operation == SkillProgramEffectOp.ResolveGuanduCangchu || skillId == "shanhe:source-sushou-1183" && operation == SkillProgramEffectOp.ResolveGuanduSushou);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62064, SkillProgramEffectOp.ResolveGuanduCangchu, SkillProgramEffectOp.ResolveGuanduSushou)]
    internal bool CanOfferShanheSourceGrain(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId is not ("shanhe:source-cangchu-1182" or "shanhe:source-sushou-1183"))
            return true;
        var cangchu = gate.SkillId == "shanhe:source-cangchu-1182";
        if (!IsShanheSourceGrainContext(gate.SkillId, cangchu ? SkillProgramEffectOp.ResolveGuanduCangchu : SkillProgramEffectOp.ResolveGuanduSushou))
            return false;
        var context = gate.Context;
        return cangchu ? context.Window == SkillProgramTriggerWindow.GameStarting || context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } && context.TargetSeat == gate.OwnerSeat && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && _coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire : context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting && gate.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat;
    }
}
