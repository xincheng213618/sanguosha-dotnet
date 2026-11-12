using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramHandLimitExemption? HandLimitExemption { get; internal init; }
}

internal sealed class GrantHandLimitExemptionDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantHandLimitExemption;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantHandLimitExemptionHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, _) => { });
    public override IReadOnlyList<SkillStateBoundarySpec> LifetimeBoundaries(SkillProgramEffect effect) => effect.HandLimitExemption!.Spec.Until;
    public override ProgramFactRequirements FactRequirementsFor(SkillProgramEffect effect)
    {
        var returned = effect.HandLimitExemption!.Spec.TurnRequirements.HasFlag(HandLimitTurnRequirements.NoTurnReturned)
            ? ProgramTrackedFact.ActualTurnReturn : ProgramTrackedFact.None;
        return base.FactRequirementsFor(effect).Union(new(returned, returned));
    }
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "relationId", "cardsBind", "handLimitMatch", "expiry", "turnRequirements",
            "allowBeforeFirstTurn", "requireCurrentHandEntry", "until", "condition");
        var bind = r.OptionalIdentifier("cardsBind");
        var match = r.Has("handLimitMatch")
            ? SkillProgramCatalog.ParseHandLimitMatch(r.Node.GetProperty("handLimitMatch"), r.Path + ".handLimitMatch") : new HandLimitMatch();
        if (bind is null && match.Mark is null && match.CardKinds.Count == 0 && match.PrintedRank is null &&
            match.EffectiveSuits.Count == 0 && match.IsRed is null)
            throw new InvalidOperationException(r.Path + ": an unbound hand-limit relation requires an explicit card selector.");
        var expiry = r.Has("expiry") ? r.RequiredEnum<HandLimitExemptionExpiry>("expiry") : HandLimitExemptionExpiry.Permanent;
        var requirements = HandLimitTurnRequirements.None;
        if (r.Has("turnRequirements"))
            foreach (var requirement in r.RequiredEnumArray<HandLimitTurnRequirements>("turnRequirements")) requirements |= requirement;
        if (expiry == HandLimitExemptionExpiry.ActualTurnOpen && requirements == HandLimitTurnRequirements.None)
            throw new InvalidOperationException(r.Path + ": actualTurnOpen requires explicit non-empty turnRequirements.");
        var until = r.Has("until") ? SkillStateDescriptor.ParseUntil(r.Node.GetProperty("until"), r.Path + ".until") : [];
        var spec = new HandLimitExemptionSpec(match, bind is null ? null : [], expiry,
            RequireCurrentHandEntry: r.Has("requireCurrentHandEntry") && r.RequiredBool("requireCurrentHandEntry"), Until: until,
            AllowBeforeFirstTurn: r.Has("allowBeforeFirstTurn") && r.RequiredBool("allowBeforeFirstTurn"), TurnRequirements: requirements);
        return new(Op, r.Has("target") ? r.RequiredEnum<SkillProgramEffectTarget>("target") : SkillProgramEffectTarget.Owner,
            0, r.Condition(), sourceBind: bind)
        { HandLimitExemption = new(r.RequiredIdentifier("relationId"), spec, bind) };
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)
    {
        var resources = WithSelectedTarget(effect).ToList();
        if (effect.HandLimitExemption!.CardsBind is { } bind) resources.Add(new ReadCardSet(bind));
        return Array.AsReadOnly(resources.ToArray());
    }
}

internal sealed class GrantHandLimitExemptionHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantHandLimitExemption;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IHandLimitExemptions>().Apply(frame, targetSeat, effect.HandLimitExemption!);
        return SkillProgramStepOutcome.Continue;
    }
}
