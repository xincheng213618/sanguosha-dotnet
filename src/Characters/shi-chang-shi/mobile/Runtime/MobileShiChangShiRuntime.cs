using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record AttendantPairRevealedEvent(int OwnerSeat, string Primary, string Partner, int Remaining) : IGameEvent;

public sealed record AttendantPairOffer(int PrimaryIndex, IReadOnlyList<int> PartnerIndexes, int? RefusedIndex);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AttendantPairOffer? AttendantPair { get; init; }
}

public sealed partial class CharacterState
{
    // Null means this seat has never received its game-start reserve. Only real
    // setup can issue it. The value records which of the original ten remain.
    internal int? AttendantReserve { get; set; }
    internal IReadOnlyList<int>? AttendantRefusalMasks { get; set; }
}

internal interface IAttendantPairResolution
{
    SkillProgramStepOutcome OfferPair(ProgramSkillFrame frame);
}

internal sealed class RevealAttendantPairDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RevealAttendantPair;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new RevealPair();
    public override ProgramOperationAiPolicy AiPolicy { get; } =
        new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition());
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.GameStarting, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow])];

    [ProgramCatalogValidation(610900)]
    private static void Validate(string path, SkillProgram program)
    {
        var triggers = program.Triggers.Where(trigger =>
            trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.RevealAttendantPair)).ToArray();
        if (triggers.Length == 0) return;
        if (program.Id != MobileShiChangShiRuntime.Danggu || triggers is not
            [{ Id: "opening-pair", Window: SkillProgramTriggerWindow.GameStarting, Optional: false,
                Effects: [{ Op: SkillProgramEffectOp.RevealAttendantPair }] },
             { Id: "return-pair", Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Optional: false,
                Effects: [{ Op: SkillProgramEffectOp.RevealAttendantPair }, { Op: SkillProgramEffectOp.Draw, Amount: 1 }] }])
            throw new InvalidOperationException($"{path}: attendant pairing requires the original opening and rest-return bindings.");
    }

    private sealed class RevealPair : ISkillProgramEffectHandler
    {
        public SkillProgramEffectOp Op => SkillProgramEffectOp.RevealAttendantPair;

        public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target,
            ISkillProgramEffectHost host) => host.GetCapability<IAttendantPairResolution>().OfferPair(frame);
    }
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class MobileShiChangShiRuntime(
    IMatchParticipantStateAccess participants,
    IMatchRandomSource random,
    IProgramRuntimeFrames frames,
    IProgramRuntimePrompts prompts,
    IProgramRuntimeEvents events,
    IProgramSkillGrantQueries skills,
    IProgramSkillAcquisitions acquisitions,
    IProgramContentQueries content,
    ISkillGrantRevisionObservations grantRevisions,
    RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries playUses) : IAttendantPairResolution
{
    internal const string Danggu = "mobile:danggu";
    internal const string Mowang = "mobile:mowang";
    private const string SourcePrefix = "acquired:mobile:danggu:attendant:";
    private const string ChoosePartner = "choose-attendant-partner";

    private IReadOnlyList<(string Name, string SkillId)> Attendants => MobileAttendantRoster.Cards;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 610900)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 610900)]
    private bool CanOffer(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != Danggu) return true;
        var owner = participants.At(gate.OwnerSeat);
        return Eligible(owner, gate.Candidate.BindingId);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 610902, SkillProgramEffectOp.ObtainDeckCards)]
    private bool IsZimouUseOrdinal(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "mobile:zimou") return true;
        var ordinal = gate.Candidate.BindingId switch
        {
            "second-use" => 2,
            "fourth-use" => 4,
            "sixth-use" => 6,
            _ => 0
        };
        return ordinal > 0 && gate.Context.CardUse is { } use && use.ActorSeat == gate.OwnerSeat &&
            playUses.IsCurrentPlayPhaseUse(gate.OwnerSeat, use.CardActionId, ordinal);
    }

    private static bool Eligible(CharacterState owner, string? binding) => binding switch
    {
        "opening-pair" => owner.AttendantReserve is null,
        "return-pair" => owner.ReturnedFromRestThisTurn && owner.AttendantReserve is > 0,
        _ => false
    };

    private bool MayRest(CharacterState owner, DeathFrame death) =>
        death.VictimSeat == owner.Seat && owner.AttendantReserve is > 0 &&
        skills.OwnedGrants(owner.Seat).Any(grant => grant.SkillId == Danggu) &&
        HasActiveSkill(owner.Seat, Mowang);

    private bool HasActiveSkill(int ownerSeat, string skillId) =>
        skills.ActiveGrants(ownerSeat).Any(grant => grant.SkillId == skillId);

    public SkillProgramStepOutcome OfferPair(ProgramSkillFrame supplied)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        var owner = participants.At(frame.OwnerSeat);
        if (frame.SkillId != Danggu || frame.InstructionIndex != 1 || frame.AttendantPair is not null ||
            !owner.IsAlive || !Eligible(owner, frame.TriggerId) || !HasActiveSkill(owner.Seat, Danggu) ||
            frame.WindowContext is not { } context ||
            frames.Find(context.ParentFrameId) is not ProgramLifecycleTriggerWindowFrame parent ||
            parent.Window != context.Window ||
            (frame.TriggerId == "opening-pair"
                ? parent.Window != SkillProgramTriggerWindow.GameStarting
                : parent.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || parent.OwnerSeat != owner.Seat))
            throw new InvalidOperationException("结党必须由开局或原休整回归的结算发起。");

        if (owner.AttendantReserve is null)
        {
            owner.AttendantReserve = (1 << Attendants.Count) - 1;
            owner.AttendantRefusalMasks = GenerateRefusals();
            owner.DeathRestRule = death => MayRest(owner, death);
        }

        var remaining = Enumerable.Range(0, Attendants.Count)
            .Where(index => (owner.AttendantReserve.Value & (1 << index)) != 0).ToArray();
        if (remaining.Length < 2 || remaining.Length % 2 != 0)
            throw new InvalidOperationException("常侍储备必须成对消耗。");
        random.Shuffle(remaining);
        var primary = remaining[0];
        var offered = remaining.Skip(1).Take(4).ToArray();
        var refusal = FindRefusal(owner, primary, offered);
        var partners = offered.Where(index => index != refusal).ToArray();

        var offer = new AttendantPairOffer(primary, Array.AsReadOnly(partners), refusal);
        frames.ReplaceTop(frame = frame with { AttendantPair = offer, Step = ResolutionFrameStep.AwaitingResponse });
        var skill = content.Skill(Danggu);
        var choices = partners.Select(index => PartnerChoice(frame, index)).ToArray();
        var refusalText = refusal is { } rejected ? $"{Attendants[rejected].Name}拒绝结党。" : "";
        prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, owner.Seat,
            $"【党锢】主将为{Attendants[primary].Name}。{refusalText}请选择一名副将。", [], [], owner.Seat)
        {
            PromptId = prompts.NextId(),
            IsPrivate = true,
            TargetSeat = owner.Seat,
            SkillPrompt = new(Danggu, skill.Name, "党锢 · 结党", skill.Description),
            Choices = Array.AsReadOnly(choices)
        }, owner.IsHuman);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private PromptChoice PartnerChoice(ProgramSkillFrame frame, int index)
    {
        var (name, skillId) = Attendants[index];
        return new(new ChoiceId($"attendant-partner.{frame.Id}.{index}"),
            $"{name} · 【{content.SkillName(skillId)}】", [], [], new Dictionary<string, string>
            {
                ["program-action"] = ChoosePartner,
                ["attendant-index"] = index.ToString(CultureInfo.InvariantCulture)
            });
    }

    [ProgramAnswer(ChoosePartner)]
    private void CommitPair(PromptChoice chosen)
    {
        if (frames.Top is not ProgramSkillFrame { SkillId: Danggu, AttendantPair: { } offer } frame ||
            prompts.Current is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat ||
            !int.TryParse(chosen.Parameters.GetValueOrDefault("attendant-index"), NumberStyles.None,
                CultureInfo.InvariantCulture, out var partner) || !offer.PartnerIndexes.Contains(partner) ||
            chosen.Id != PartnerChoice(frame, partner).Id || chosen.Cards.Count != 0 || chosen.Targets.Count != 0)
            throw new InvalidOperationException("结党选择不属于当前常侍窗口。");

        var owner = participants.At(frame.OwnerSeat);
        var pair = (1 << offer.PrimaryIndex) | (1 << partner);
        if (!owner.IsAlive || owner.AttendantReserve is not { } remaining || (remaining & pair) != pair ||
            !HasActiveSkill(owner.Seat, Danggu))
            throw new InvalidOperationException("结党选择已失效。");

        prompts.Clear();
        frames.ReplaceTop(frame = frame with { AttendantPair = null, Step = ResolutionFrameStep.ResolvingEffect });
        owner.AttendantReserve = remaining & ~pair;
        var (primaryName, primarySkill) = Attendants[offer.PrimaryIndex];
        var (partnerName, partnerSkill) = Attendants[partner];
        var sourceId = SourcePrefix + frame.Id.ToString(CultureInfo.InvariantCulture);
        acquisitions.GrantOwnedSource(frame, sourceId, primarySkill);
        acquisitions.GrantOwnedSource(frame, sourceId, partnerSkill);
        events.AdvanceFact(new AttendantPairRevealedEvent(owner.Seat, primaryName, partnerName,
            System.Numerics.BitOperations.PopCount((uint)owner.AttendantReserve.Value)));
        frames.AdvanceProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RevealAttendantPair)]
    private static PromptChoice ChooseAiPartner(PendingDecision decision, ProgramSkillFrame frame) =>
        decision.PlayerSeat == frame.OwnerSeat && frame.AttendantPair is not null
            ? decision.Choices[0]
            : throw new InvalidOperationException("结党选择没有对应的原结算。");

    private IReadOnlyList<int> GenerateRefusals()
    {
        var masks = new int[Attendants.Count];
        for (var primary = 0; primary < masks.Length; primary++)
        {
            var candidates = Enumerable.Range(0, Attendants.Count).Where(index => index != primary).ToArray();
            random.Shuffle(candidates);
            var refusalCount = random.Next(candidates.Length + 1);
            foreach (var candidate in candidates.Take(refusalCount))
                masks[primary] |= 1 << candidate;
        }
        return Array.AsReadOnly(masks);
    }

    private int? FindRefusal(CharacterState owner, int primary, IReadOnlyList<int> offered)
    {
        if (offered.Count <= 1) return null;
        var fixedConflict = primary switch { 3 => 5, 5 => 3, 7 => 8, 8 => 7, _ => -1 };
        if (offered.Contains(fixedConflict)) return fixedConflict;

        var refusals = owner.AttendantRefusalMasks ?? throw new InvalidOperationException("缺少本局的常侍认可关系。");
        var rejected = offered.Where(index => (refusals[primary] & (1 << index)) != 0).ToArray();
        return rejected.Length == 0 ? null : rejected[random.Next(rejected.Length)];
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 610900)]
    private void ClearPairOnDeath(PlayerDiedEvent died)
    {
        var owner = participants.At(died.VictimSeat);
        if (owner.AttendantReserve is null) return;
        ClearPair(owner);
        if (!died.IsResting) owner.AttendantReserve = 0;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 610901)]
    private void ClearReserveOnLoss(CharacterSkillsLostEvent lost)
    {
        var owner = participants.At(lost.Seat);
        if (owner.AttendantReserve is null || !lost.SkillIds.Contains(Danggu, StringComparer.Ordinal)) return;
        owner.AttendantReserve = 0;
        ClearPair(owner);
    }

    private void ClearPair(CharacterState owner)
    {
        var grants = owner.SkillGrants.Grants.Where(grant => grant.SourceId.StartsWith(SourcePrefix, StringComparison.Ordinal)).ToArray();
        if (grants.Length == 0) return;
        grantRevisions.Capture(participants.Players);
        foreach (var grant in grants)
            owner.SkillGrants.RemoveGrant(grant.GrantId);
    }
}

internal static class MobileAttendantRoster
{
    internal static IReadOnlyList<(string Name, string SkillId)> Cards { get; } =
        Array.AsReadOnly<(string Name, string SkillId)>(
        [
            ("张让", "mobile:attendant-taoluan"),
            ("赵忠", "mobile:chiyan"),
            ("孙璋", "mobile:zimou"),
            ("毕岚", "mobile:picai"),
            ("夏恽", "mobile:yaozhuo"),
            ("韩悝", "mobile:xiaolu"),
            ("栗嵩", "mobile:kuiji"),
            ("段珪", "mobile:chixia"),
            ("郭胜", "mobile:niqu"),
            ("高望", "mobile:miaoyu")
        ]);
}
