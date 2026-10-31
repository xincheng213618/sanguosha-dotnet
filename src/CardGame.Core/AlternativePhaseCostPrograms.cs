namespace CardGame.Core;

public enum ProgramAlternativePhaseCostKind { DiscardCard, RemoveMarker }
public sealed record ProgramAlternativePhaseCostReceipt(int InstructionIndex,
    ProgramAlternativePhaseCostKind Kind, PlayerMarkerKind Marker,
    int? CardId = null, CardLocation? SourceLocation = null);
public sealed record ProgramAlternativePhaseCostPaidEvent(long FrameId, string SkillId,
    string BindingId, string SkillInstanceId, int OwnerSeat,
    ProgramAlternativePhaseCostKind Kind, PlayerMarkerKind Marker, int? CardId,
    CardLocation? SourceLocation) : IGameEvent;
public sealed record ProgramTurnPhaseSubstitutedEvent(long FrameId, string SkillId,
    string BindingId, string SkillInstanceId, int OwnerSeat, int ActualTurnNumber,
    SkillProgramTriggerWindow Window, SkillProgramTurnPhase SkippedPhase) : IGameEvent;
public sealed record ProgramActualPhaseSubstitution(long ParentFrameId, long ProducerFrameId,
    int OwnerSeat, string SkillId, string SkillInstanceId, int ActualTurnNumber,
    SkillProgramTurnPhase SkippedPhase);

public interface IAlternativePhaseCostProgramHost
{
    SkillProgramStepOutcome PayOwnedCardOrMarker(ProgramSkillFrame frame,
        IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker);
}
public sealed class PayOwnedCardOrMarkerHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PayOwnedCardOrMarker;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IAlternativePhaseCostProgramHost>().PayOwnedCardOrMarker(frame, effect.Zones, effect.Marker!.Value);
}
internal sealed class PayOwnedCardOrMarkerDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PayOwnedCardOrMarker;
    public override ISkillProgramEffectHandler Handler { get; } = new PayOwnedCardOrMarkerHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.DiscardSelected,
        static (_, context) => context.PublicControlValue(-4d));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "zones", "marker", "condition");
        var owner = FilterBoundCardsProgramOperationDescriptor.Owner(r);
        var zones = r.RequiredEnumArray<CardZoneKind>("zones");
        if (zones.Count == 0 || zones.Distinct().Count() != zones.Count ||
            zones.Any(zone => zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.zones: one-card alternative costs accept distinct owned hand/equipment areas.");
        var effect = new SkillProgramEffect(Op, owner, 1, r.Condition(), zones: zones,
            marker: r.RequiredEnum<PlayerMarkerKind>("marker"));
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.JudgmentPhaseStarting,
            SkillProgramTriggerWindow.DrawPhaseStarting, SkillProgramTriggerWindow.AfterNormalDraw,
            SkillProgramTriggerWindow.DiscardPhaseStarting]), new RequireOwnTurnBoundary()];
}

internal static class AlternativePhaseCostCompositionContract
{
    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow window, SkillProgramTriggerSubject? subject,
        SkillProgramTurnOwnerScope turnOwnerScope)
    {
        var payment = effects.Select((effect, index) => (effect, index)).Where(item => item.effect.Op == SkillProgramEffectOp.PayOwnedCardOrMarker).ToArray();
        if (window is SkillProgramTriggerWindow.JudgmentPhaseStarting or SkillProgramTriggerWindow.DrawPhaseStarting &&
            effects.Any(e => e.Op == SkillProgramEffectOp.SkipTurnPhases) && payment.Length == 0 &&
            !IsRevealedDrawChoice(effects, window, subject, turnOwnerScope))
            throw new InvalidOperationException($"Invalid skill program at {path}: actual starting substitution requires an explicit alternative paid producer or an own revealed Draw choice.");
        if (payment.Length > 0)
        {
            var phase = window switch
            {
                SkillProgramTriggerWindow.JudgmentPhaseStarting => SkillProgramTurnPhase.Judgment,
                SkillProgramTriggerWindow.DrawPhaseStarting => SkillProgramTurnPhase.Draw,
                SkillProgramTriggerWindow.AfterNormalDraw => SkillProgramTurnPhase.Play,
                SkillProgramTriggerWindow.DiscardPhaseStarting => SkillProgramTurnPhase.Discard,
                _ => throw new InvalidOperationException($"Invalid skill program at {path}: alternative payment has no supported actual phase boundary.")
            };
            if (payment.Length != 1 || payment[0].index != 0 || effects.Count < 2 ||
                effects[1].Op != SkillProgramEffectOp.SkipTurnPhases || !effects[1].SkippedPhases.SequenceEqual([phase]) ||
                subject != SkillProgramTriggerSubject.Owner || turnOwnerScope != SkillProgramTurnOwnerScope.Own)
                throw new InvalidOperationException($"Invalid skill program at {path}: one initial alternative payment must precede its exact own phase skip.");
        }
        if (effects.Any(e => e.Op == SkillProgramEffectOp.RecordEndHandCountAndGrantMarker) &&
            (window != SkillProgramTriggerWindow.TurnEnding || subject != SkillProgramTriggerSubject.Owner || turnOwnerScope != SkillProgramTurnOwnerScope.Own))
            throw new InvalidOperationException($"Invalid skill program at {path}: ending history requires its own ending boundary.");
    }

    private static bool IsRevealedDrawChoice(IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow window, SkillProgramTriggerSubject? subject, SkillProgramTurnOwnerScope turnOwnerScope)
    {
        if (window != SkillProgramTriggerWindow.DrawPhaseStarting || subject != SkillProgramTriggerSubject.Owner ||
            turnOwnerScope != SkillProgramTurnOwnerScope.Own) return false;
        var skips = effects.Select((effect, index) => (effect, index))
            .Where(item => item.effect.Op == SkillProgramEffectOp.SkipTurnPhases).ToArray();
        if (skips.Length != 1 || !skips[0].effect.SkippedPhases.SequenceEqual([SkillProgramTurnPhase.Draw]) ||
            skips[0].effect.Condition is not { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: { } bind, OptionId: { } option }) return false;
        var choice = effects.Take(skips[0].index).Select((effect, index) => (effect, index))
            .SingleOrDefault(item => item.effect.Op == SkillProgramEffectOp.ChooseOption && item.effect.ResultBind == bind);
        return choice.effect is not null && choice.effect.Condition.Kind == SkillProgramConditionKind.Always &&
            choice.effect.Options.Count >= 2 && choice.effect.Options.Any(candidate => candidate.Id == option) &&
            effects.Take(choice.index).Any(effect => effect.Op == SkillProgramEffectOp.RevealTopCards &&
                effect.Visibility == SkillProgramCardSetVisibility.Public && effect.Condition.Kind == SkillProgramConditionKind.Always);
    }
}
