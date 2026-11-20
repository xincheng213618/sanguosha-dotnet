using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

internal sealed record YijinIssuedGold(PlayerMarkerKind Marker, int IssuedTurn, int GiverSeat, string GrantSourceId);

public sealed partial class CharacterState
{
    internal YijinIssuedGold? YijinGold { get; set; }
}

internal sealed class DistributeYijinGoldDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeYijinGold;
    public override ISkillProgramEffectHandler Handler { get; } = new DistributeYijinGoldHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(3));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting)];
}

internal sealed class DistributeYijinGoldHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeYijinGold;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat,
        ISkillProgramEffectHost host) => host.GetCapability<IMobileYijin>().OfferGold(frame);
}

internal interface IMobileYijin : IProgramRuntimeOperations
{
    SkillProgramStepOutcome OfferGold(ProgramSkillFrame frame);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class MobileYijin(IProgramRuntimeFrames frames, IMatchParticipantStateAccess participants,
    IProgramTurnQueries turn, IProgramRuntimePrompts prompts, IProgramRuntimeEvents events, CoreParticipantHandProgramsEntries choicePrompts,
    CoreGameEngineStrategicProgramsEntries markers, CoreGameEngineProgramSkillChangesEntries changes,
    IProgramSkillChangeOperations skillChanges, ISkillGrantRevisionObservations observations) : IMobileYijin
{
    private const string Action = "distribute-yijin-gold";

    private sealed record Gold(PlayerMarkerKind Marker, string SkillId);

    private static class Catalog
    {
        internal static IReadOnlyList<Gold> Kinds { get; } = Array.AsReadOnly<Gold>(
        [
            new(PlayerMarkerKind.YijinWushi, "mobile:yijin-wushi"),
            new(PlayerMarkerKind.YijinHouren, "mobile:yijin-houren"),
            new(PlayerMarkerKind.YijinTongshen, "mobile:yijin-tongshen"),
            new(PlayerMarkerKind.YijinJinmi, "mobile:yijin-jinmi"),
            new(PlayerMarkerKind.YijinJiaxiong, "mobile:yijin-jiaxiong"),
            new(PlayerMarkerKind.YijinYongbi, "mobile:yijin-yongbi")
        ]);
    }

    private static bool HasGold(CharacterState player) => Catalog.Kinds.Any(gold => player.Markers.ContainsKey(gold.Marker));
    private static IEnumerable<Gold> Stock(CharacterState owner) =>
        Catalog.Kinds.Where(gold => owner.YijinGold?.Marker != gold.Marker &&
            owner.MarkerSourceCounts.ContainsKey((gold.Marker, owner.Seat)));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62212, SkillProgramEffectOp.DistributeYijinGold)]
    private bool CanOfferGold(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        Stock(participants.At(candidate.OwnerSeat)).Any() &&
        participants.Players.Any(player => player.IsAlive && player.Seat != candidate.OwnerSeat && !HasGold(player));

    public SkillProgramStepOutcome OfferGold(ProgramSkillFrame source)
    {
        var frame = frames.ActiveProgram(source.Id);
        if (!IsGoldChoiceInstruction(frame))
            throw new InvalidOperationException("亿金 lost its actual play phase distribution instruction.");
        var choices = Choices(frame);
        if (choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        choicePrompts.PublishParticipantHandChoice(frame, frame.OwnerSeat, choices, "亿金，选择一种金和接收角色。", frame.OwnerSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> Choices(ProgramSkillFrame frame) =>
        (from gold in Stock(participants.At(frame.OwnerSeat))
         from target in participants.Players
         where target.IsAlive && target.Seat != frame.OwnerSeat && !HasGold(target)
         select new PromptChoice(new($"yijin.{frame.Id}.{gold.Marker}.{target.Seat}"),
             $"交给{target.Name}【{PlayerMarkerCatalog.GetDisplayName(gold.Marker)}】", [], [target.Seat],
             new Dictionary<string, string> { ["program-action"] = Action, ["gold"] = gold.SkillId })).ToArray();

    [ProgramAnswer(Action)]
    private void GiveGold(PromptChoice choice)
    {
        if (frames.Top is not ProgramSkillFrame frame || !IsGoldChoiceInstruction(frame) ||
            prompts.Current?.PlayerSeat != frame.OwnerSeat ||
            Choices(frame).SingleOrDefault(available => available.Id == choice.Id) is not { } accepted)
            throw new InvalidOperationException("亿金 lost its live gold and recipient choice.");

        var gold = Catalog.Kinds.Single(kind => kind.SkillId == accepted.Parameters["gold"]);
        var recipient = accepted.Targets.Single();
        var holder = participants.At(recipient);
        var grant = $"rule:yijin:{frame.OwnerSeat}:{frame.Id}";
        prompts.Clear();
        markers.SetStrategicMarker(frame.OwnerSeat, gold.Marker, frame.OwnerSeat, 0, frame);
        holder.YijinGold = new(gold.Marker, turn.TurnNumber, frame.OwnerSeat, grant);
        markers.SetStrategicMarker(recipient, gold.Marker, recipient, 1, frame);
        changes.CommitProgramParticipantSkillGrants(frame, recipient, [gold.SkillId], grant, null, null);
        frames.AdvanceProgram(frame.Id);
    }

    private static bool IsGoldChoiceInstruction(ProgramSkillFrame frame) =>
        frame.SkillId == "mobile:yijin" && frame.TriggerId == "give-gold" && frame.InstructionIndex == 1 &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.PlayPhaseStarting };

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 62212)]
    private void ExpireGold(IGameEvent fact)
    {
        var seat = fact switch
        {
            TurnEndedEvent ended => ended.ActorSeat,
            PlayerDiedEvent death => death.VictimSeat,
            _ => -1
        };
        if (seat < 0 || participants.At(seat) is not { YijinGold: { } issued } holder ||
            fact is TurnEndedEvent end && end.TurnNumber <= issued.IssuedTurn)
            return;

        observations.Capture(participants.Players);
        var skillGrant = holder.SkillGrants.Grants.SingleOrDefault(grant => grant.SourceId == issued.GrantSourceId);
        if (skillGrant is not null)
            skillChanges.RemoveGrant(holder.Seat, skillGrant.GrantId);
        holder.YijinGold = null;
        holder.MarkerSourceCounts.Remove((issued.Marker, holder.Seat));
        if (holder.Markers.Remove(issued.Marker, out var count))
            events.AdvanceFact(new PlayerMarkerChangedEvent(frames.NextId(), holder.Seat, issued.Marker, -count, 0,
                issued.GiverSeat, "skill-program.mobile.yijin.expired"));
    }
}
