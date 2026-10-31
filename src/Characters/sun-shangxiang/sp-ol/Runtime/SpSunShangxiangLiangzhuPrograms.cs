using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SpSunShangxiangFanxiangReceipt? SpSunShangxiangFanxiang { get; init; }
}
public sealed record SpSunShangxiangRecoveryQualifiedEvent(CardConversionSource Source, string GameplayHash, long ChangeId, long? RecoveryParentId,
    int RecoveredSeat, int HpBefore, int HpAfter, ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
public sealed record SpSunShangxiangHelpedEvent(long FrameId, int OwnerSeat, int HelpedSeat, long ChangeId) : IGameEvent;
public sealed record SpSunShangxiangFanxiangReceipt(CardConversionSource Source, string GameplayHash, long LifecycleId, int QualifiedSeat, int MaximumBefore);
public sealed record SpSunShangxiangFanxiangAcceptedEvent(long FrameId, SpSunShangxiangFanxiangReceipt Receipt) : IGameEvent;
public sealed record SpSunShangxiangFanxiangCompletedEvent(long FrameId, int OwnerSeat, int MaximumHp) : IGameEvent;
internal interface ISpSunShangxiangOlHost
{ SkillProgramStepOutcome CompleteFanxiang(ProgramSkillFrame frame); }
internal sealed class SpSunShangxiangFanxiangDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CompleteSpSunShangxiangFanxiang;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new SpSunShangxiangFanxiangHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnSkills, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class SpSunShangxiangFanxiangHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.CompleteSpSunShangxiangFanxiang;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<ISpSunShangxiangOlHost>().CompleteFanxiang(f);
}
internal static class SpSunShangxiangOlComposition
{
    internal const string Liangzhu = "ol:liangzhu",Fanxiang = "ol:fanxiang",Xiaoji = "classic:xiaoji";
    internal const string LiangzhuChoice = "liangzhu-benefit";
    [ProgramCatalogValidation(1450)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (p.Id != Fanxiang && !p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e =>
            e.Op == SkillProgramEffectOp.CompleteSpSunShangxiangFanxiang)) return;
        var valid = p.Activations.Count == 0 && p.ViewAs.Count == 0 && p.CardPolicies.Count == 0 && p.Modifiers.Count == 0 && p.BooleanStates.Count == 0 && p.Triggers is [var t] &&
            !t.Optional && t.Subject == SkillProgramTriggerSubject.Owner && t.OwnerRelation is null && t.Condition.Kind == SkillProgramTriggerConditionKind.Always &&
            t.CardKinds.Count == 0 && t.CardCategories.Count == 0 && t.Suits.Count == 0 && t.SourceZones.Count == 0 && t.DestinationZones.Count == 0 &&
            p.Id == Fanxiang && t.Id == "assisted-wounded-awakening" && t.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow &&
                t.UsageScope == SkillUsageScope.Game && t.UsageLimit == 1 && t.Effects is [
                    { Op: SkillProgramEffectOp.ChangeMaximumHp, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always },
                    { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always },
                    { Op: SkillProgramEffectOp.CompleteSpSunShangxiangFanxiang, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }];
        if (!valid) throw new InvalidOperationException(path + ": SP Sun Shangxiang requires complete ordered Fanxiang.");
    }
}
