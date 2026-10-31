#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunCeBoundaryMobileRuntime
{
    internal const string BoundarySunCeMobileHunzi = "boundary:hunzi-mobile";
    internal bool BoundarySunCeLordQualified(CharacterState lord, string skillId) => lord.IsAlive && _coreSkillProgramsOperations.EnabledSkillPrograms(lord).Any(p => p.Id == skillId) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(lord, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(lord, skillId), Role.Lord);
    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 66300)]
    internal IEnumerable<LegalAction> BuildBoundarySunCeMobileChallenges(CharacterState provider)
    {
        if (_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) != "wu" || _corePhysicalOwnedCardQueriesOperations.GetHand(provider).Count == 0)
            yield break;
        foreach (var lord in _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != provider.Seat && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(provider.Seat, p.Seat)))
            foreach (var program in _coreSkillProgramsOperations.EnabledSkillPrograms(lord))
                foreach (var activation in program.Activations.Where(a => ProgramInstructionResolver.Default.Features(a).HasOperation(SkillProgramEffectOp.ResolveBoundarySunCeZhiba)))
                {
                    if (!BoundarySunCeLordQualified(lord, program.Id) || _coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses.GetValueOrDefault((provider.Seat, lord.Seat, program.Id, activation.Id)) != 0)
                        continue;
                    yield return new(LegalActionKind.UseProgramSkill, null, lord.Seat, $"制霸：与主公 {(lord.Name)} 拼点。", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 1, MaxTargetCount: 1)
                    {
                        ProgramSkillId = program.Id,
                        ProgramActivationId = activation.Id,
                        ProgramSkillOwnerSeat = lord.Seat,
                        SelectableCardIds = _corePhysicalOwnedCardQueriesOperations.GetHand(provider).Select(c => c.Id).ToArray(),
                        SelectableTargetSeats = [lord.Seat],
                        ProgramAiHint = new(1, 0, 0, 0, 0, 0, true, false)
                    };
                }
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Contribution, 66300)]
    internal bool ExecuteBoundarySunCeMobileChallenge(CharacterState provider, CharacterState lord, LegalAction a, int card)
    {
        var p = _coreSkillProgramsOperations.EnabledSkillPrograms(lord).SingleOrDefault(p => p.Id == a.ProgramSkillId);
        var plan = p is null ? null : ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Activation, a.ProgramActivationId);
        if (plan?.Features.HasOperation(SkillProgramEffectOp.ResolveBoundarySunCeZhiba) != true)
            return false;
        var key = (provider.Seat, lord.Seat, p!.Id, plan.BindingId);
        if (!BoundarySunCeLordQualified(lord, p.Id) || provider.Seat == lord.Seat || _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) != "wu" || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(provider.Seat, lord.Seat) || !_corePhysicalOwnedCardQueriesOperations.GetHand(provider).Any(c => c.Id == card) || _coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses.GetValueOrDefault(key) != 0)
            throw new InvalidOperationException("Mobile Zhiba requires another Wu player's unused native Play-phase challenge and actual hand card.");
        var awakening = plan.Instructions[0].SkillIds.SingleOrDefault() ?? BoundarySunCeMobileHunzi;
        var awake = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingResolvedEvent>().Any(e => e.OwnerSeat == lord.Seat && e.SkillId == awakening && e.Activated && e.Completed);
        _coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses[key] = 1;
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, lord.Seat, p.Id, plan.BindingId, p.GameplayHash, 0, [], [provider.Seat])
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(lord, p.Id),
            BoundarySunCeChallenge = new(provider.Seat, card, awake, "request")
        };
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, lord.Seat, p.Id, f.ActivationId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunCeChallengeStartedEvent(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, provider.Seat, card, awake, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IBoundarySunCeMobileHost.Challenge(ProgramSkillFrame f) => BeginBoundarySunCeMobileChallenge(f);
    internal SkillProgramStepOutcome BeginBoundarySunCeMobileChallenge(ProgramSkillFrame f)
    {
        if (f.BoundarySunCeChallenge is not { Stage: "request" } r)
            throw new InvalidOperationException("Zhiba requires its issued original challenger frame.");
        if (!r.Awakened)
        {
            BeginBoundarySunCeMobileContest(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "制霸：你已觉醒，是否接受本次拼点？", [], [], r.ProviderSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = BoundarySunCeChallengeChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static IReadOnlyList<PromptChoice> BoundarySunCeChallengeChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { true, false }.Select(accept => new PromptChoice(new($"mobile-zhiba.{(f.Id)}.{(accept)}"), accept ? "接受拼点" : "拒绝拼点", [], [], new Dictionary<string, string> { ["program-action"] = "boundary-sun-ce-challenge", ["accept"] = accept.ToString() })).ToArray());
    [ProgramAnswer("boundary-sun-ce-challenge")]
    internal void AnswerBoundarySunCeChallenge(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundarySunCeChallenge: { Stage: "request", Awakened: true } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundarySunCeChallengeChoices(f).Any(c => c.Id == choice.Id))
            throw new InvalidOperationException("Zhiba refusal belongs only to its already awakened Lord and original challenge.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var accept = bool.Parse(choice.Parameters["accept"]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunCeChallengeAnsweredEvent(f.Id, accept));
        if (!accept)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        BeginBoundarySunCeMobileContest(f);
    }

    internal void BeginBoundarySunCeMobileContest(ProgramSkillFrame f)
    {
        var r = f.BoundarySunCeChallenge!;
        if (!BoundarySunCeLordQualified(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId) || !_coreMatchParticipantsPlayersAccess._players[r.ProviderSeat].IsAlive || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.ProviderSeat]).Any(c => c.Id == r.ProviderCardId) || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(r.ProviderSeat, f.OwnerSeat))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundarySunCeChallenge = r with { Stage = "contest" } });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name + " · 拼点", skill.Description), r.ProviderSeat, f.OwnerSeat, r.ProviderCardId, "mobile-zhiba-result");
        var pair = (PindianFrame)_resolutionStackOperations._resolutionStack[^1];
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundarySunCeChallenge = f.BoundarySunCeChallenge!with { PindianFrameId = pair.Id } });
    }

    internal bool TryBeginBoundarySunCeZhibaClaim(PindianFrame pair)
    {
        if (pair.ClaimSeats is not null || pair.Result is not { SourceWon: false } result || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { BoundarySunCeChallenge: { Stage: "contest" } r } f || pair.SkillId != f.SkillId || r.PindianFrameId != pair.Id || pair.ParentFrameId != f.Id || result.SourceSeat != r.ProviderSeat || result.OpponentSeat != f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return false;
        var ids = _corePindianOperations.AvailablePindianCards(result);
        if (ids.Length == 0)
            return false;
        pair = pair with
        {
            ClaimSeats = new[]
            {
                f.OwnerSeat
            },
            ClaimIndex = 0,
            PindianStep = PindianStep.ClaimResult
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(pair);
        var choices = new[]
        {
            new PromptChoice(new($"mobile-zhiba.{(pair.Id)}.claim"), "获得本次拼点的牌", ids, [], new Dictionary<string, string> { ["action"] = "pindian-claim", ["take"] = "true" }),
            new PromptChoice(new($"mobile-zhiba.{(pair.Id)}.decline"), "不获得拼点牌", [], [], new Dictionary<string, string> { ["action"] = "pindian-claim", ["take"] = "false" })
        };
        _corePindianOperations.SetPindianPrompt(pair, f.OwnerSeat, DecisionKind.SkillModule, "制霸：对方没赢，是否获得拼点牌？", choices, ids, [], pair.Presentation);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66300)]
    internal bool KeepBoundarySunCeMobileContest(ProgramSkillFrame f) => f.BoundarySunCeChallenge is { Stage: "contest" } && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunCeChallengeStartedEvent>().Count(e => e.FrameId == f.Id && e.Source.SkillId == f.SkillId && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash) == 1;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundarySunCeZhiba)]
    internal static PromptChoice BoundarySunCeChallengeAi(ProgramSkillFrame f) => BoundarySunCeChallengeChoices(f).First();
}
