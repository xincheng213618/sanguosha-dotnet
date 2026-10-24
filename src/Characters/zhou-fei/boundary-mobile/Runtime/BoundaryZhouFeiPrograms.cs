using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum BoundaryZhouFeiStage { Store, StoreChildren, Target, Discard, DiscardChildren, Drawing, DrawChildren, RoundChoice, EffectChildren, UseChoice, IssuingUse, NativeUse, Used, ReturnChildren }
public sealed record BoundaryZhouFeiMaterial(int CardId, CardKind Kind, CardLocation From);
public sealed record BoundaryZhouFeiNative(long ParentId, CardConversionSource Source, string Hash, long DepositId, int CardId, CardLocation From, LegalAction Action, long UseId, long ActionId);
public sealed record BoundaryZhouFeiReceipt(CardConversionSource Source, string Hash, SkillProgramEffectOp Op, long WindowId, int Round, int Holder, int Direction)
{
    public BoundaryZhouFeiStage Stage { get; init; }
    public int Target { get; init; } = -1;
    public int Cursor { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public CardLocation? Location { get; init; }
    public NativeDrawOutcome? Draw { get; init; }
    public BoundaryZhouFeiNative? Native { get; init; }
    private IReadOnlyList<int> _selected = [];
    private IReadOnlyList<long> _deposits = [];
    private IReadOnlyList<BoundaryZhouFeiMaterial> _payment = [];
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<long> Deposits { get => _deposits; init => _deposits = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<BoundaryZhouFeiMaterial> Payment { get => _payment; init => _payment = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame { [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryZhouFeiReceipt? BoundaryZhouFei { get; init; } }
public sealed partial record CardUseFrame { [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryZhouFeiNative? BoundaryZhouFeiUse { get; init; } }
public sealed record BoundaryZhouFeiStartedEvent(long FrameId, CardConversionSource Source, string Hash, SkillProgramEffectOp Op, long WindowId, int Round, int Holder, int Direction) : IGameEvent;
// Deposits name a source/location/count, never concealed physical identities.
public sealed record BoundaryZhouFeiStoredEvent(long FrameId, CardConversionSource Source, string Hash, int Holder, CardLocation Location, int Count, int Before, int After) : IGameEvent;
public sealed record BoundaryZhouFeiTargetEvent(long FrameId, int Owner, int Round, int Target, bool Draw) : IGameEvent;
public sealed record BoundaryZhouFeiPaidEvent(long FrameId, string Branch, int Target, int Count, int Before, int After) : IGameEvent;
public sealed record BoundaryZhouFeiDrawEvent(long FrameId, int Target, long? AttemptId, int Actual, long Before, long After) : IGameEvent;
public sealed record BoundaryZhouFeiNativeEvent(long FrameId, long DepositId, long UseId, long ActionId, int CardId) : IGameEvent;
public sealed record BoundaryZhouFeiReturnedUseEvent(long FrameId, long UseId, long ActionId) : IGameEvent;
public sealed record BoundaryZhouFeiDepositReturnedEvent(long FrameId, long DepositId, int Holder, int Count, int Before, int After) : IGameEvent;

internal interface IBoundaryZhouFeiHost { SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation); }
internal sealed class BoundaryZhouFeiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{ public SkillProgramEffectOp Op => op; public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryZhouFeiHost>().Execute(frame, op); }
internal abstract class BoundaryZhouFeiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new BoundaryZhouFeiHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);
    public override ProgramDependencyKinds TracksActualTurnCursorReturn => ProgramDependencyKinds.Trigger;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(2));
}
internal sealed class ResolveBoundaryZhouFeiLiangyinDescriptor : BoundaryZhouFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin; }
internal sealed class StoreBoundaryZhouFeiKongshengDescriptor : BoundaryZhouFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng; }
internal sealed class ReturnBoundaryZhouFeiKongshengDescriptor : BoundaryZhouFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng; }

internal static class BoundaryZhouFeiComposition
{
    [ProgramCatalogValidation(54700)]
    private static void Validate(string path, SkillProgram program)
    {
        foreach (var effect in program.Activations.SelectMany(activation => activation.Effects).Concat(program.Triggers.SelectMany(trigger => trigger.Effects)))
            if (effect.Op == SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin && program.Id != "boundary:liangyin-mobile" ||
                effect.Op is SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng or SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng && program.Id != "boundary:kongsheng-mobile")
                throw new InvalidOperationException(path + ": mobile Zhou Fei operations belong only to the corresponding printed edition skill.");
        if (program.Id == "boundary:liangyin-mobile" && (program.Activations.Count != 0 || program.Triggers is not [
            { Id: "global-general-pile", Window: SkillProgramTriggerWindow.CardsMoved, Subject: SkillProgramTriggerSubject.Owner, Optional: true, MovementOccurrence: SkillProgramMovementOccurrence.PerBatch, SourceZones: [CardZoneKind.Hand], Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin }] },
            { Id: "round-unique", Window: SkillProgramTriggerWindow.RoundEnding, Subject: SkillProgramTriggerSubject.Owner, Optional: true, Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin }] }]))
            throw new InvalidOperationException(path + ": mobile Liangyin requires the global native movement and distinct round-target contracts.");
        if (program.Id == "boundary:kongsheng-mobile" && (program.Activations.Count != 0 || program.Triggers is not [
            { Id: "prepare-store", Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Subject: SkillProgramTriggerSubject.Owner, Optional: true, Effects: [{ Op: SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng }] },
            { Id: "health-store", Window: SkillProgramTriggerWindow.AfterHealthChanged, Subject: SkillProgramTriggerSubject.Owner, Optional: true, Effects: [{ Op: SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng }] },
            { Id: "return-held", Window: SkillProgramTriggerWindow.TurnEnding, TurnOwnerScope: SkillProgramTurnOwnerScope.AnyLiving, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng }] }]))
            throw new InvalidOperationException(path + ": mobile Kongsheng requires both actual storage windows and the original holder's issued ending return.");
    }
}
