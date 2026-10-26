namespace CardGame.Core;

internal interface IShanheDeathEquipmentProgramHost
{
    void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame);
}

internal sealed class AbolishPublicTeamEnemyEquipmentSlotsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.AbolishPublicTeamEnemyEquipmentSlots;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        context.Frame.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });
    public override ISkillProgramEffectHandler Handler { get; } = new AbolishPublicTeamEnemyEquipmentSlotsHandler();
}

internal sealed class AbolishPublicTeamEnemyEquipmentSlotsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.AbolishPublicTeamEnemyEquipmentSlots;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheDeathEquipmentProgramHost>().AbolishPublicTeamEnemyEquipmentSlots(frame);
        return SkillProgramStepOutcome.Continue;
    }
}

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheDeathEquipmentProgramHost(GameEngine engine) : IShanheDeathEquipmentProgramHost
    {
        public void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame) => engine.AbolishPublicTeamEnemyEquipmentSlots(frame);
    }

    private void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied, OwnerSeat: var owner, TargetSeat: var target } ||
            owner != frame.OwnerSeat || target != owner || _players[owner].IsAlive)
            throw new InvalidOperationException("Enemy equipment abolition requires its original owner's completed death.");
        var enemies = _players.Where(player => player.IsAlive && PublicTeamEnemy(owner, player)).ToArray();
        foreach (var enemy in enemies)
        foreach (var slot in Enum.GetValues<EquipmentSlot>())
            if (enemy.EquipmentSlotCapacity(slot) > 0)
                SetEquipmentSlotCapacity(enemy, slot, 0);
    }
}
