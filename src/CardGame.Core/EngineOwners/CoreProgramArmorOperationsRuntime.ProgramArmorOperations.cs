#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramArmorOperationsRuntime
{
    internal void GrantProgramArmor(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var program = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!;
        var effect = ProgramInstructionResolver.Default.Resolve(active, program).GetPausedInstruction(active.InstructionIndex).Effect;
        var index = active.InstructionIndex - 1;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ArmorGrantedEvent>().Any(grant => grant.ProgramFrameId == active.Id && grant.InstructionIndex == index))
            throw new InvalidOperationException("An armor grant instruction cannot be paid twice.");
        IReadOnlyList<int> targets = effect.Target switch
        {
            SkillProgramEffectTarget.Owner => [active.OwnerSeat],
            SkillProgramEffectTarget.SelectedTarget when active.SelectedTargetSeats.Count == 1 => active.SelectedTargetSeats,
            SkillProgramEffectTarget.SelectedTargets => active.SelectedTargetSeats,
            _ => throw new InvalidOperationException("An armor grant lost its selected recipient set.")};
        if (targets.Distinct().Count() != targets.Count || targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)))
            throw new InvalidOperationException("An armor grant requires distinct actual recipients.");
        if (!_modeRulesOperations._modeRules.AllowsArmorGain)
            return;
        var payments = targets.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Select(seat => (Seat: seat, Before: _coreMatchParticipantsPlayersAccess._players[seat].Armor, After: checked(_coreMatchParticipantsPlayersAccess._players[seat].Armor + effect.Amount))).ToArray();
        var source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(active);
        foreach (var payment in payments)
        {
            _coreMatchParticipantsPlayersAccess._players[payment.Seat].Armor = payment.After;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(active.Id, index, source, payment.Seat, effect.Amount, payment.Before, payment.After));
            _coreMatchLogOperations.AddLog("ArmorGranted", $"{(_coreMatchParticipantsPlayersAccess._players[payment.Seat].Name)} 获得 {(effect.Amount)} 点护甲，现有 {(payment.After)} 点。", active.OwnerSeat, payment.Seat);
        }
    }

    void IArmorProgramHost.GrantArmor(ProgramSkillFrame frame) => GrantProgramArmor(frame);
}
