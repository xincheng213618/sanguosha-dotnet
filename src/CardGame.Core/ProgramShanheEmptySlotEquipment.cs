namespace CardGame.Core;

internal interface IShanheEmptySlotEquipmentProgramHost
{
    SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly = false);
}

internal sealed class PlaceRandomEmptySlotDeckEquipmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PlaceRandomEmptySlotDeckEquipment;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(12));
    public override ISkillProgramEffectHandler Handler { get; } = new PlaceRandomEmptySlotDeckEquipmentHandler();
}

internal sealed class PlaceRandomEmptySlotDeckEquipmentHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PlaceRandomEmptySlotDeckEquipment;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheEmptySlotEquipmentProgramHost>().PlaceRandomDeckEquipment(frame);
}

internal sealed class PlaceRandomMissingArmorDeckEquipmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(12));
    public override ISkillProgramEffectHandler Handler { get; } = new PlaceRandomMissingArmorDeckEquipmentHandler();
}

internal sealed class PlaceRandomMissingArmorDeckEquipmentHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheEmptySlotEquipmentProgramHost>().PlaceRandomDeckEquipment(frame, armorOnly: true);
}
