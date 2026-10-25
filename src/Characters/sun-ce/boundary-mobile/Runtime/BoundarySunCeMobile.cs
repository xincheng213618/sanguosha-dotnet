using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundarySunCeChallengeReceipt(int ProviderSeat, int ProviderCardId, bool Awakened, string Stage, long? PindianFrameId = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundarySunCeChallengeReceipt? BoundarySunCeChallenge { get; init; }
}
public sealed record BoundarySunCeChallengeStartedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int ProviderSeat, int ProviderCardId, bool Awakened, int TurnNumber) : IGameEvent;
public sealed record BoundarySunCeChallengeAnsweredEvent(long FrameId, bool Accepted) : IGameEvent;
internal interface IBoundarySunCeMobileHost { SkillProgramStepOutcome Challenge(ProgramSkillFrame f); }
internal sealed class ResolveBoundarySunCeZhibaDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundarySunCeZhiba;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundarySunCeChallengeHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Pindian, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget()];
}
internal sealed class BoundarySunCeChallengeHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundarySunCeZhiba;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundarySunCeMobileHost>().Challenge(f);
}
// Zhiba is supplied to another Wu player's native Play phase, never an action
// the Lord may initiate against that player.
[ProgramSelectionModule(ProgramSelectionStage.Availability, 66300, SkillProgramEffectOp.ResolveBoundarySunCeZhiba)]
internal sealed class BoundarySunCeOwnChallengeUnavailable : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext c, ProgramSelectionCandidates p, IProgramSelectionQueries q) => false;
}
public sealed partial class GameEngine
{
    private const string BoundarySunCeMobileZhiba = "boundary:zhiba-mobile", BoundarySunCeMobileHunzi = "boundary:hunzi-mobile", BoundarySunCeMobileChallenge = "accept-wu-challenge";
    private bool BoundarySunCeLordQualified(CharacterState lord) => lord.IsAlive &&
        EnabledSkillPrograms(lord).Any(p => p.Id == BoundarySunCeMobileZhiba) &&
        HasSkillRoleQualification(lord, BoundarySunCeMobileZhiba, GetRuntimeSkillInstanceId(lord, BoundarySunCeMobileZhiba), Role.Lord);
    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 66300)]
    private IEnumerable<LegalAction> BuildBoundarySunCeMobileChallenges(CharacterState provider)
    {
        if (GetEffectiveFactionId(provider) != "wu" || GetHand(provider).Count == 0) yield break;
        foreach (var lord in _players.Where(p => p.Seat != provider.Seat && BoundarySunCeLordQualified(p) && CanBePindianTarget(provider.Seat, p.Seat)))
        {
            if (_programContributionUses.GetValueOrDefault((provider.Seat, lord.Seat, BoundarySunCeMobileZhiba, BoundarySunCeMobileChallenge)) != 0) continue;
            yield return new(LegalActionKind.UseProgramSkill, null, lord.Seat, $"制霸：与主公 {lord.Name} 拼点。", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 1, MaxTargetCount: 1)
            { ProgramSkillId = BoundarySunCeMobileZhiba, ProgramActivationId = BoundarySunCeMobileChallenge, ProgramSkillOwnerSeat = lord.Seat,
                SelectableCardIds = GetHand(provider).Select(c => c.Id).ToArray(), SelectableTargetSeats = [lord.Seat], ProgramAiHint = new(1, 0, 0, 0, 0, 0, true, false) };
        }
    }
    [ProgramActionHandler(ProgramActionHandlerKind.Contribution, 66300)]
    private bool ExecuteBoundarySunCeMobileChallenge(CharacterState provider, CharacterState lord, LegalAction a, int card)
    {
        if (a.ProgramSkillId != BoundarySunCeMobileZhiba) return false;
        var key = (provider.Seat, lord.Seat, BoundarySunCeMobileZhiba, BoundarySunCeMobileChallenge);
        if (a.ProgramActivationId != BoundarySunCeMobileChallenge || !BoundarySunCeLordQualified(lord) ||
            provider.Seat == lord.Seat || GetEffectiveFactionId(provider) != "wu" || !CanBePindianTarget(provider.Seat, lord.Seat) ||
            !GetHand(provider).Any(c => c.Id == card) || _programContributionUses.GetValueOrDefault(key) != 0)
            throw new InvalidOperationException("Mobile Zhiba requires another Wu player's unused native Play-phase challenge and actual hand card.");
        var p = GetEnabledSkillProgram(lord, BoundarySunCeMobileZhiba);
        var awake = ProgramEventHistory<ProgramBindingResolvedEvent>().Any(e => e.OwnerSeat == lord.Seat && e.SkillId == BoundarySunCeMobileHunzi && e.Activated && e.Completed);
        _programContributionUses[key] = 1;
        var f = new ProgramSkillFrame(++_resolutionSequence, lord.Seat, p.Id, BoundarySunCeMobileChallenge, p.GameplayHash, 0, [], [provider.Seat])
        { SkillInstanceId = GetRuntimeSkillInstanceId(lord, p.Id), BoundarySunCeChallenge = new(provider.Seat, card, awake, "request") };
        PushRuntimeFrame(f);
        AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, lord.Seat, p.Id, f.ActivationId));
        AdvanceEventRulesAndQueueFact(new BoundarySunCeChallengeStartedEvent(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, provider.Seat, card, awake, _turnNumber));
        AdvanceRuntimeProgram(f.Id); return true;
    }
    [ProgramCapability]
    private sealed class BoundarySunCeMobileHost(GameEngine e) : IBoundarySunCeMobileHost
    { public SkillProgramStepOutcome Challenge(ProgramSkillFrame f) => e.BeginBoundarySunCeMobileChallenge(f); }
    private SkillProgramStepOutcome BeginBoundarySunCeMobileChallenge(ProgramSkillFrame f)
    {
        if (f.BoundarySunCeChallenge is not { Stage: "request" } r) throw new InvalidOperationException("Zhiba requires its issued original challenger frame.");
        if (!r.Awakened) { BeginBoundarySunCeMobileContest(f); return SkillProgramStepOutcome.AwaitChild; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "制霸：你已觉醒，是否接受本次拼点？", [], [], r.ProviderSeat)
        { PromptId = CreatePromptId(), Choices = BoundarySunCeChallengeChoices(f), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState(); return SkillProgramStepOutcome.AwaitChoice;
    }
    private static IReadOnlyList<PromptChoice> BoundarySunCeChallengeChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { true, false }.Select(accept =>
        new PromptChoice(new($"mobile-zhiba.{f.Id}.{accept}"), accept ? "接受拼点" : "拒绝拼点", [], [], new Dictionary<string, string>
        { ["program-action"] = "boundary-sun-ce-challenge", ["accept"] = accept.ToString() })).ToArray());
    [ProgramAnswer("boundary-sun-ce-challenge")]
    private void AnswerBoundarySunCeChallenge(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundarySunCeChallenge: { Stage: "request", Awakened: true } } f ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundarySunCeChallengeChoices(f).Any(c => c.Id == choice.Id))
            throw new InvalidOperationException("Zhiba refusal belongs only to its already awakened Lord and original challenge.");
        ClearPendingDecision(); var accept = bool.Parse(choice.Parameters["accept"]);
        AdvanceEventRulesAndQueueFact(new BoundarySunCeChallengeAnsweredEvent(f.Id, accept));
        if (!accept) { FinishProgramSkill(f, true); return; }
        BeginBoundarySunCeMobileContest(f);
    }
    private void BeginBoundarySunCeMobileContest(ProgramSkillFrame f)
    {
        var r = f.BoundarySunCeChallenge!;
        if (!BoundarySunCeLordQualified(_players[f.OwnerSeat]) || !_players[r.ProviderSeat].IsAlive ||
            !GetHand(_players[r.ProviderSeat]).Any(c => c.Id == r.ProviderCardId) || !CanBePindianTarget(r.ProviderSeat, f.OwnerSeat))
        { FinishProgramSkill(f, false); return; }
        ReplaceRuntimeTop(f = f with { BoundarySunCeChallenge = r with { Stage = "contest" } });
        var skill = _contentRegistry.GetSkill(f.SkillId);
        BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name + " · 拼点", skill.Description), r.ProviderSeat, f.OwnerSeat, r.ProviderCardId, "mobile-zhiba-result");
        var pair = (PindianFrame)_resolutionStack[^1];
        ReplaceRuntimeFrame(f.Id, f with { BoundarySunCeChallenge = f.BoundarySunCeChallenge! with { PindianFrameId = pair.Id } });
    }
    // The native claim prompt and typed Pindian return retain the original
    // challenger as source. A tie is a non-win and also permits the Lord's claim.
    private bool TryBeginBoundarySunCeZhibaClaim(PindianFrame pair)
    {
        if (pair.SkillId != BoundarySunCeMobileZhiba || pair.ClaimSeats is not null || pair.Result is not { SourceWon: false } result ||
            _resolutionStack.Count < 2 || _resolutionStack[^2] is not ProgramSkillFrame { BoundarySunCeChallenge: { Stage: "contest" } r } f ||
            r.PindianFrameId != pair.Id || pair.ParentFrameId != f.Id || result.SourceSeat != r.ProviderSeat || result.OpponentSeat != f.OwnerSeat || !_players[f.OwnerSeat].IsAlive) return false;
        var ids = AvailablePindianCards(result); if (ids.Length == 0) return false;
        pair = pair with { ClaimSeats = new[] { f.OwnerSeat }, ClaimIndex = 0, PindianStep = PindianStep.ClaimResult };
        ReplaceRuntimeTop(pair);
        var choices = new[] {
            new PromptChoice(new($"mobile-zhiba.{pair.Id}.claim"), "获得本次拼点的牌", ids, [], new Dictionary<string,string> { ["action"] = "pindian-claim", ["take"] = "true" }),
            new PromptChoice(new($"mobile-zhiba.{pair.Id}.decline"), "不获得拼点牌", [], [], new Dictionary<string,string> { ["action"] = "pindian-claim", ["take"] = "false" }) };
        SetPindianPrompt(pair, f.OwnerSeat, DecisionKind.SkillModule, "制霸：对方没赢，是否获得拼点牌？", choices, ids, [], pair.Presentation); return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66300)]
    private bool KeepBoundarySunCeMobileContest(ProgramSkillFrame f) => f.SkillId == BoundarySunCeMobileZhiba && f.BoundarySunCeChallenge is { Stage: "contest" } &&
        ProgramEventHistory<BoundarySunCeChallengeStartedEvent>().Count(e => e.FrameId == f.Id && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId) == 1;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundarySunCeZhiba)]
    private static PromptChoice BoundarySunCeChallengeAi(ProgramSkillFrame f) => BoundarySunCeChallengeChoices(f).First();
}
