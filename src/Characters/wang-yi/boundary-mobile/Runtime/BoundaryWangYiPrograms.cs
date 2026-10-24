using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryWangYiHost { SkillProgramStepOutcome ChooseMijiCount(ProgramSkillFrame frame); }
internal sealed class ChooseBoundaryWangYiMijiCountDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryWangYiMijiCountHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)];
}
internal sealed class BoundaryWangYiMijiCountHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundaryWangYiHost>().ChooseMijiCount(f);
}
internal static class BoundaryWangYiMobile
{
    internal const string Zhenlie = "boundary:zhenlie-mobile", Miji = "boundary:miji-mobile";
    [ProgramCatalogValidation(57400)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op == SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount || e.NumberExpression == SkillProgramNumberExpression.BoundaryWangYiMijiCount)) return;
        if (p.Id != Miji || p.Activations.Count != 0 || p.Triggers is not [{ Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
            TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Effects: [
                { Op: SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount, Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, NumberExpression: SkillProgramNumberExpression.BoundaryWangYiMijiCount, ResultBind: "drawn", Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.DistributeOwnedCards, Target: SkillProgramEffectTarget.Owner, Zones: [CardZoneKind.Hand], SourceBind: "drawn", TargetKind: SkillProgramTargetKind.OtherLiving, AllowDeclineBeforeFirst: true, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Miji requires one own Ending count choice, native counted draw and optional equal-count hand distribution.");
    }
}

public sealed record BoundaryWangYiMijiSelection(int Maximum, int? Count = null);
public sealed record BoundaryWangYiMijiCountIssuedEvent(long FrameId, CardConversionSource Source, string GameplayHash, int Maximum) : IGameEvent;
public sealed record BoundaryWangYiMijiCountChosenEvent(long FrameId, int OwnerSeat, int Count) : IGameEvent;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryWangYiMijiSelection? BoundaryWangYiMijiCount { get; init; } }
