using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheSourceTiepaoReceipt(long DamageWindowId, long DamageId, int TargetSeat,
    int? MaterialCardId = null, CardLocation? MaterialFrom = null, long? UseId = null, long? ActionId = null,
    bool Issuing = false, bool Returned = false, int MovementBefore = 0);
public sealed record ShanheSourceTiepaoIssuedEvent(long ProgramFrameId, long DamageId, long UseId,
    long ActionId, int OwnerSeat, int TargetSeat, int CardId, CardLocation From) : IGameEvent;
public sealed record ShanheSourceTiepaoReturnedEvent(long ProgramFrameId, long UseId, int OwnerSeat,
    bool CausedDamage) : IGameEvent;
public sealed record ShanheSourceTiepaoDistanceGrantedEvent(long ProgramFrameId, long UseId,
    int OwnerSeat, int AffectedSeat, int ActualTurn) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceTiepaoReceipt? ShanheSourceTiepao { get; init; }
}
public interface IShanheSourceTiepaoHost
{
    SkillProgramStepOutcome Begin(ProgramSkillFrame frame);
}
internal sealed class ResolveShanheSourceTiepaoRetaliationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation;
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheSourceTiepaoHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.RequestSlashByTarget,
        static (e, c) => c.DeckPrograms(e));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition());
        RequireAlways(e, r.Path); return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class ShanheSourceTiepaoHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceTiepaoHost>().Begin(f);
}
internal static class ShanheSourceTiepaoComposition
{
    internal const string SkillId = "shanhe:source-tiepao-14075";
    [ProgramCatalogValidation(24780)]
    internal static void Validate(string path, SkillProgram p)
    {
        foreach (var t in p.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation)))
            if (p.Id != SkillId || t.Window != SkillProgramTriggerWindow.AfterDamageApplied ||
                t.Subject != SkillProgramTriggerSubject.Owner || !t.Optional ||
                t.DamageOccurrence != SkillProgramDamageOccurrence.PerDamage ||
                t.Effects is not [{ Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
                t.Condition.Kind != SkillProgramTriggerConditionKind.Always)
                throw new InvalidOperationException(path + ": source14075 requires its original optional actual damage-target window.");
    }
}
