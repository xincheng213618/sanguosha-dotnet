using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramCardEffectEpisode? CardEffectEpisode { get; internal init; }
}

internal sealed class IssueCurrentCardEffectEpisodeDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueCurrentCardEffectEpisode;
    public override ISkillProgramEffectHandler Handler { get; } = new IssueCurrentCardEffectEpisodeHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(6));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "priority", "scope", "count", "targetRef", "targetIndex", "replay",
            "targetAdmission", "continueAfterActorDeath", "continueOriginalAfterActorDeath", "condition");
        var scope = r.Has("scope") ? r.RequiredEnum<NativeCardEffectEpisodeScope>("scope") : NativeCardEffectEpisodeScope.WholeUse;
        var count = r.Has("count") ? r.RequiredInt("count") : 1;
        if (count is < 1 or > 20) throw new InvalidOperationException("A declared effect episode count must be 1..20.");
        var target = r.Has("targetRef") ? r.RequiredParticipantReference("targetRef") : null;
        var index = r.Has("targetIndex") ? r.RequiredInt("targetIndex") : (int?)null;
        if (scope == NativeCardEffectEpisodeScope.WholeUse && (target is not null || index is not null) ||
            scope == NativeCardEffectEpisodeScope.FrozenTarget && (target is null || index is not null) ||
            scope is NativeCardEffectEpisodeScope.ExecutionOccurrence or NativeCardEffectEpisodeScope.ExecutionPair && (index is not >= 0 || target is not null) ||
            scope == NativeCardEffectEpisodeScope.ExecutionPair && index % 2 != 0 ||
            target is not null && target.Kind is not (ProgramParticipantRef.Owner or ProgramParticipantRef.SelectedTarget or ProgramParticipantRef.EventSource or ProgramParticipantRef.EventTarget))
            throw new InvalidOperationException("A native episode declares a whole use, a frozen participant, or an exact execution slot/pair.");
        var spec = new NativeCardEffectEpisodeSpec(r.Has("priority") ? r.RequiredInt("priority") : 0, scope, count,
            TargetIndex: index,
            Replay: r.Has("replay") ? r.RequiredEnum<NativeCardEffectEpisodeReplay>("replay") : NativeCardEffectEpisodeReplay.Original,
            TargetAdmission: r.Has("targetAdmission") ? r.RequiredEnum<NativeCardEffectEpisodeTargetAdmission>("targetAdmission") : NativeCardEffectEpisodeTargetAdmission.RetainIssued,
            ContinueAfterActorDeath: !r.Has("continueAfterActorDeath") || r.RequiredBool("continueAfterActorDeath"),
            ContinueOriginalAfterActorDeath: r.Has("continueOriginalAfterActorDeath") && r.RequiredBool("continueOriginalAfterActorDeath"));
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), count, r.Condition())
            { CardEffectEpisode = new(spec, target) };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireContext(ProgramContextCapability.CardAction), new RequireTriggerWindows(
            [SkillProgramTriggerWindow.CardUseCommitted, SkillProgramTriggerWindow.CardUseTargetsFinalized]),
            .. ParticipantResources(effect.CardEffectEpisode!.TargetRef)];
}
internal sealed class IssueCurrentCardEffectEpisodeHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.IssueCurrentCardEffectEpisode;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<INativeCardEffectEpisodes>().IssueCurrent(frame, effect.CardEffectEpisode!);
        return SkillProgramStepOutcome.Continue;
    }
}
