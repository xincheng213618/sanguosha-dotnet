namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_mi_heng_le_LeMiHengJigu
{
    internal static CardConversionSource JiguSource(ProgramSkillFrame f) =>
        new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static bool IsJiguDamageTrigger(SkillProgramTrigger t) =>
        t.Effects is [{ Op: SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw }];

    internal static JiguActivationPaidEvent JiguPayment(long id, JiguReceipt r) => new(id, r.Source,
        r.GameplayHash, r.DamageFrameId, r.DamageWindowId, r.RoundNumber, r.ActualTurn, r.TurnOwner,
        r.MarkedHandCount, r.EquipmentCount, r.EmptySlotCount, r.StartedSeatCount, r.UseNumber, r.DisabledForRound);
}
