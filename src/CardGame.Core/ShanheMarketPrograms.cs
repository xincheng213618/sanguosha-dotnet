using System.Text.Json.Serialization;

namespace CardGame.Core;

public static class ShanheMarketPrograms
{
    public const string ExecutionSkillId = "shanhe:market-acquisition";
    public const string PurchaseTriggerId = "paid-purchase";
    internal const string ExecutionGrantId = "mode:shanhe-market:execution";
}

public sealed record ShanheMarketPurchaseCommittedEvent(long FrameId, long MarketRevision,
    string ChoiceId, string RewardId, int Price, int GoldAfter) : IGameEvent;
public sealed record ShanheMarketCardCreatedEvent(long PurchaseFrameId, string RewardId, int RecipeIndex,
    bool EquipmentResource, int CardId) : IGameEvent;
public sealed record ShanheMarketPurchaseAppliedEvent(long PurchaseFrameId, string RewardId) : IGameEvent;

public sealed record ShanheMarketPurchaseFrame(long Id, string ChoiceId, string RewardId, string? ReplaceSkillId,
    int Price, int HpCost, long MarketRevision, int GoldAfter, long? SuspendedFrameId, int SuspendedStackDepth,
    SuspendedDecisionState? Decision, EngineStatus Status)
    : ResolutionFrame(Id, ResolutionFrameKind.ShanheMarketPurchase, ResolutionFrameStep.ResolvingEffect)
{
    public long? ProgramFrameId { get; init; }
    public bool Applied { get; init; }
    public ShanheMarketDiscardQuote? DiscardQuote { get; init; }
}

public enum ShanheMarketApplicationStage { PayingHealth, Granting, Opening, Materials, Recovery, Children, Complete }
public sealed record ShanheMarketApplicationReceipt(long PurchaseFrameId, int InstructionIndex,
    string RewardId, ShanheMarketApplicationStage Stage)
{
    private readonly IReadOnlyList<string> _newSkillInstanceIds = Array.AsReadOnly(Array.Empty<string>());
    public IReadOnlyList<string> NewSkillInstanceIds
    { get => _newSkillInstanceIds; init => _newSkillInstanceIds = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheMarketApplicationReceipt? ShanheMarketApplication { get; init; }
}

public sealed record ShanheMarketOpeningOrigin(long PurchaseFrameId, long ExecutionFrameId,
    string RewardId);

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheMarketOpeningOrigin? ShanheMarketOpening { get; init; }
}

internal interface IShanheMarketPurchaseHost
{
    SkillProgramStepOutcome ApplyShanheMarketPurchase(ProgramSkillFrame frame);
}

internal sealed class ApplyShanheMarketPurchaseDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheMarketPurchase;
    public override ISkillProgramEffectHandler Handler { get; } = new ApplyShanheMarketPurchaseHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills,
        static (_, context) => context.PublicControlValue(0));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.SkillsChanged)];

    [ProgramCatalogValidation(62030)]
    internal static void ValidatePurchase(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ApplyShanheMarketPurchase)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ApplyShanheMarketPurchase))) return;
        if (program.Id != ShanheMarketPrograms.ExecutionSkillId || program.Activations.Count != 0 ||
            program.Triggers is not [{ Id: ShanheMarketPrograms.PurchaseTriggerId,
                Window: SkillProgramTriggerWindow.SkillsChanged, Subject: SkillProgramTriggerSubject.Owner,
                Optional: false, Effects: [{ Op: SkillProgramEffectOp.ApplyShanheMarketPurchase,
                    Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": market acquisition requires its single exact native paid-purchase binding.");
    }
}

public sealed class ApplyShanheMarketPurchaseHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheMarketPurchase;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheMarketPurchaseHost>().ApplyShanheMarketPurchase(frame);
}
