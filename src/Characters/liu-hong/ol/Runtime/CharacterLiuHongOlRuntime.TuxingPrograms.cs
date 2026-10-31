#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuHongOlRuntime
{
    internal void ResolveTuxingAfterSlotAbolish(ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount)
    {
        if (abolishedSlotCount <= 0)
            return;
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[recipientSeat];
        if (!owner.IsAlive)
            return;
        if (!_coreNationalSkillsOperations.EnabledContentSkillIds(owner).Contains("ol:tuxing", StringComparer.Ordinal))
            return;
        var tuxingPresentation = _contentRegistryOperations._contentRegistry!.GetSkill("ol:tuxing");
        // Mutations and deferred recovery/HP observers belong to the original
        // abolishing program. Never substitute its owner for the affected seat.
        for (var index = 0; index < abolishedSlotCount; index++)
        {
            _coreProgramLifecycleOperations.ChangeProgramParticipantMaximumHp(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), recipientSeat, 1, "ol:tuxing");
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 的【{(tuxingPresentation.Name)}】生效：增加1点体力上限并回复1点体力。", recipientSeat);
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(active.Id, recipientSeat, recipientSeat, 1, null, null);
        }

        if (!IsTuxingDamageArmPending(recipientSeat))
            return;
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 的【{(tuxingPresentation.Name)}】生效：所有装备栏均已废除，减少4点体力上限，本局游戏接下来造成的伤害+1。", recipientSeat);
        _coreProgramLifecycleOperations.ChangeProgramParticipantMaximumHp(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), recipientSeat, -4, "ol:tuxing");
        ArmProgramGameDamageBonus(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "ol:tuxing", recipientSeat, 1);
    }

    internal SkillProgramStepOutcome TuxingProgramArmGameDamage(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || !IsTuxingDamageArmPending(active.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】生效：所有装备栏均已废除，本局游戏接下来造成的伤害+1。", active.OwnerSeat);
        ArmProgramGameDamageBonus(active, active.SkillId, active.OwnerSeat, 1);
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsTuxingDamageArmPending(int ownerSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        if (!global::CardGame.Core.EngineOwners.CharacterLiuHongOlDefinitions.YujueSlotOrder.All(slot => owner.EquipmentSlotCapacity(slot) == 0))
            return false;
        return !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramGameDamageBonusArmedEvent>().Any(fact => fact.OwnerSeat == ownerSeat && fact.SkillId == "ol:tuxing");
    }
}
