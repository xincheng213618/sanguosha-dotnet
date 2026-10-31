#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramShanheDeathEquipmentRuntime
{
    void IShanheDeathEquipmentProgramHost.AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame) => AbolishPublicTeamEnemyEquipmentSlots(frame);
    internal void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied, OwnerSeat: var owner, TargetSeat: var target } || owner != frame.OwnerSeat || target != owner || _coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            throw new InvalidOperationException("Enemy equipment abolition requires its original owner's completed death.");
        var enemies = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _coreTeamActivityProgramsOperations.PublicTeamEnemy(owner, player)).ToArray();
        foreach (var enemy in enemies)
            foreach (var slot in Enum.GetValues<EquipmentSlot>())
                if (enemy.EquipmentSlotCapacity(slot) > 0)
                    _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(enemy, slot, 0);
    }
}
