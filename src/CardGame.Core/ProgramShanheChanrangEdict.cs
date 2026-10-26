using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheChanrangFirstGainEvent(int ActualTurn, int TurnOwnerSeat, int RecipientSeat,
    long BatchId, long FirstMovementSequence) : IGameEvent;
public sealed record ShanheChanrangPaidEvent(long FrameId, int OwnerSeat, int GiverSeat, int RecipientSeat,
    int CardId, int MovementSequence) : IGameEvent;

public sealed record ShanheChanrangDraft
{
    private IReadOnlyList<int> _recipients = [];
    private IReadOnlyList<PromptChoice> _choices = [];
    public long ParentWindowId { get; init; }
    public long BatchId { get; init; }
    public int ActualTurn { get; init; }
    public int EquipmentId { get; init; }
    public int Cursor { get; init; }
    public string Stage { get; init; } = "branch";
    public int? GiverSeat { get; init; }
    public int? RecipientSeat { get; init; }
    public int? PaidCardId { get; init; }
    public int? PaidMovementSequence { get; init; }
    public IReadOnlyList<int> Recipients { get => _recipients; init => _recipients = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<PromptChoice> Choices { get => _choices; init => _choices = Array.AsReadOnly(value.Select(PromptChoiceProjection.Freeze).ToArray()); }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ShanheChanrangDraft? ShanheChanrang { get; init; }
}

internal interface IShanheChanrangHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame); }
internal sealed class ResolveShanheChanrangEdictDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheChanrangEdict;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheChanrangHandler();
}
internal sealed class ShanheChanrangHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheChanrangEdict;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheChanrangHost>().Resolve(frame);
}

internal static class ShanheChanrangComposition
{
    [ProgramCatalogValidation(5434)]
    internal static void Validate(string path, SkillProgram program)
    {
        var entries = program.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheChanrangEdict)).ToArray();
        if (entries.Length == 0) return;
        if (program.Id != "shanhe:chanrang-edict-1510" || program.Activations.Count != 0 || program.Triggers.Count != 1 ||
            entries is not [{ Optional: false, Window: SkillProgramTriggerWindow.CardsMoved, MovementOccurrence: SkillProgramMovementOccurrence.PerBatch,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShanheChanrangEdict }] }])
            throw new InvalidOperationException(path + ": the edict requires its exact equipment-owned first-gain movement entry.");
    }
}
