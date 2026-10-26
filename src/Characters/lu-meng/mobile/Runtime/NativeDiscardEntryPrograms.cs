using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeDiscardEntryStage { Choosing, Skipping, Entering }

public sealed record NativeDiscardEntryFrame(long Id, CardConversionSource Source, string GameplayHash,
    int TurnNumber, int TurnOwnerSeat, TurnPhase BeforePhase, int PlayPhaseInstanceId, PromptId PromptId,
    NativeDiscardEntryStage Stage = NativeDiscardEntryStage.Choosing)
    : ResolutionFrame(Id, ResolutionFrameKind.DiscardPhaseEntry, ResolutionFrameStep.ResolvingEffect);

[JsonDerivedType(typeof(NativeDiscardEntryFrame), "native-discard-entry")]
public abstract partial record ResolutionFrame;

public sealed record NativeDiscardEntryPolicyChosenEvent(long FrameId, CardConversionSource Source,
    string GameplayHash, int TurnNumber, int TurnOwnerSeat, TurnPhase BeforePhase, int PlayPhaseInstanceId, bool Applied) : IGameEvent;

public sealed partial class GameEngine
{
    private bool TryBeginPreEntryDiscardPolicy(CharacterState current)
    {
        if (_usedOrPlayedSlashDuringPlayPhase || _pendingTurnDelayedEffects.HasFlag(DelayedTurnEffects.SkipDiscardPhase) ||
            !current.IsAlive) return false;
        var policies = CardPolicies(current, SkillProgramCardPolicyKind.OfferSkipDiscard)
            .Where(item => item.Policy.BeforePhaseEntry == true).ToArray();
        if (policies.Length == 0) return false;
        if (_pendingDecision is not null || _resolutionStack.Count != 0 || current.Seat != _currentSeat ||
            _turnProgression.OwnerSeat != current.Seat || _turnProgression.TurnNumber != _turnNumber ||
            _phase is not (TurnPhase.Draw or TurnPhase.Play))
            throw new InvalidOperationException("A before-entry discard policy requires its clean actual turn boundary.");
        var (source, policy) = policies[0];
        var frame = new NativeDiscardEntryFrame(++_resolutionSequence,
            new(source.SkillId, policy.Id, current.Seat, source.SkillInstanceId), source.Definition.Program!.GameplayHash,
            _turnNumber, _turnProgression.OwnerSeat, _phase, _cardUseDebitPhaseInstanceId, CreatePromptId());
        PushRuntimeFrame(frame);
        _pendingDecision = new PendingDecision(DecisionKind.SkipDiscardPolicy, current.Seat,
            $"是否发动【{source.Definition.Name}】，在弃牌阶段开始前跳过此阶段？", [], [])
        {
            PromptId = frame.PromptId,
            Choices = [DiscardEntryPolicyChoice(frame, true), DiscardEntryPolicyChoice(frame, false)]
        };
        _status = current.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    private PromptChoice DiscardEntryPolicyChoice(NativeDiscardEntryFrame frame, bool apply)
    {
        var action = apply ? "discard-policy-use" : "discard-policy-skip";
        return new(new($"discard-entry.{frame.Id}.{(apply ? "use" : "skip")}"),
            apply ? $"发动【{_contentRegistry.GetSkill(frame.Source.SkillId).Name}】，跳过弃牌阶段。" : "不发动，进入弃牌阶段。",
            [], [], new Dictionary<string, string>
            {
                ["action"] = action, ["skill-id"] = frame.Source.SkillId, ["policy-id"] = frame.Source.BindingId
            });
    }

    private void AssertNativeDiscardEntry(NativeDiscardEntryFrame frame)
    {
        if (_resolutionStack.FirstOrDefault()?.Id != frame.Id || frame.TurnNumber != _turnNumber ||
            frame.TurnOwnerSeat != _turnProgression.OwnerSeat || frame.TurnOwnerSeat != _currentSeat ||
            frame.Source.OwnerSeat != _currentSeat || _turnProgression.TurnNumber != _turnNumber ||
            frame.BeforePhase is not (TurnPhase.Draw or TurnPhase.Play) || frame.BeforePhase != _phase ||
            frame.PlayPhaseInstanceId != _cardUseDebitPhaseInstanceId || !frame.PromptId.IsValid ||
            _contentRegistry.GetSkill(frame.Source.SkillId).Program?.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("A discard entry lost its original phase, actual turn or content hash.");
        if (frame.Stage == NativeDiscardEntryStage.Choosing)
        {
            if (!_players[frame.Source.OwnerSeat].IsAlive || _usedOrPlayedSlashDuringPlayPhase ||
                !CardPolicies(_players[frame.Source.OwnerSeat], SkillProgramCardPolicyKind.OfferSkipDiscard).Any(item =>
                    item.Policy.BeforePhaseEntry == true && item.Policy.Id == frame.Source.BindingId &&
                    item.Source.SkillId == frame.Source.SkillId && item.Source.SkillInstanceId == frame.Source.SkillInstanceId))
                throw new InvalidOperationException("An unpaid discard entry lost its actual Slash qualification or source.");
            return;
        }
        if (frame.Stage is not (NativeDiscardEntryStage.Skipping or NativeDiscardEntryStage.Entering) ||
            ProgramEventHistory<NativeDiscardEntryPolicyChosenEvent>().Count(e => e == new NativeDiscardEntryPolicyChosenEvent(
                frame.Id, frame.Source, frame.GameplayHash, frame.TurnNumber, frame.TurnOwnerSeat, frame.BeforePhase,
                frame.PlayPhaseInstanceId, frame.Stage == NativeDiscardEntryStage.Skipping)) != 1)
            throw new InvalidOperationException("A discard entry lost its singular accepted native choice.");
    }

    private bool AssertPreEntryDiscardPolicyChoice(PendingDecision pending)
    {
        if (_resolutionStack.FirstOrDefault() is not NativeDiscardEntryFrame frame) return false;
        AssertNativeDiscardEntry(frame);
        if (_resolutionStack.Count != 1 || frame.Stage != NativeDiscardEntryStage.Choosing ||
            pending.Kind != DecisionKind.SkipDiscardPolicy || pending.PlayerSeat != frame.Source.OwnerSeat ||
            pending.PromptId != frame.PromptId || pending.Choices.Count != 2 ||
            !pending.Choices.Select(c => c.Id).SequenceEqual(new[]
                { DiscardEntryPolicyChoice(frame, true).Id, DiscardEntryPolicyChoice(frame, false).Id }) ||
            pending.Choices.Any(c => c.Cards.Count != 0 || c.Targets.Count != 0 ||
                c.Parameters.GetValueOrDefault("skill-id") != frame.Source.SkillId ||
                c.Parameters.GetValueOrDefault("policy-id") != frame.Source.BindingId) ||
            pending.Choices[0].Parameters.GetValueOrDefault("action") != "discard-policy-use" ||
            pending.Choices[1].Parameters.GetValueOrDefault("action") != "discard-policy-skip" ||
            _status != (_players[frame.Source.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running))
            throw new InvalidOperationException("A before-entry discard policy lost its exact owning prompt.");
        return true;
    }

    private bool TryResolvePreEntryDiscardPolicy(bool useSkill)
    {
        if (_resolutionStack.FirstOrDefault() is not NativeDiscardEntryFrame frame) return false;
        if (_pendingDecision is not { } pending || !AssertPreEntryDiscardPolicyChoice(pending))
            throw new InvalidOperationException("There is no original before-entry discard choice to resolve.");
        ClearPendingDecision();
        ReplaceRuntimeTop(frame = frame with { Stage = useSkill ? NativeDiscardEntryStage.Skipping : NativeDiscardEntryStage.Entering });
        AdvanceEventRulesAndQueueFact(new NativeDiscardEntryPolicyChosenEvent(frame.Id, frame.Source, frame.GameplayHash,
            frame.TurnNumber, frame.TurnOwnerSeat, frame.BeforePhase, frame.PlayPhaseInstanceId, useSkill));
        AdvanceEventRulesAndQueueFact(new ProgramCardPolicyResolvedEvent(frame.Source.OwnerSeat,
            frame.Source.SkillId, frame.Source.BindingId, useSkill));
        AddLog(useSkill ? "SkillTriggered" : "SkillSkipped", useSkill
            ? $"{_players[frame.Source.OwnerSeat].Name} 发动【{_contentRegistry.GetSkill(frame.Source.SkillId).Name}】，跳过弃牌阶段。"
            : $"{_players[frame.Source.OwnerSeat].Name} 未发动【{_contentRegistry.GetSkill(frame.Source.SkillId).Name}】。", frame.Source.OwnerSeat);
        if (useSkill && TryBeginSkillInvocation(frame.Id, frame.Source.OwnerSeat, frame.Source.SkillId,
                $"discard-entry:{frame.Id}:{frame.Source.BindingId}", frame.Source.SkillInstanceId)) return true;
        ContinueNativeDiscardEntry(frame.Id);
        return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 84700,
        NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(NativeDiscardEntryFrame), 84700)]
    private void ContinueNativeDiscardEntry(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not NativeDiscardEntryFrame frame || frame.Id != frameId ||
            frame.Stage == NativeDiscardEntryStage.Choosing || _pendingDecision is not null)
            throw new InvalidOperationException("A discard entry cannot advance before its exact choice or invocation child returned.");
        AssertNativeDiscardEntry(frame);
        PopResolutionFrame(frame.Id, ResolutionFrameKind.DiscardPhaseEntry);
        if (_winner != Winner.None) return;
        if (frame.Stage == NativeDiscardEntryStage.Skipping) EndTurn();
        else BeginDiscardPhase(beforeEntryPolicyCompleted: true);
    }

    private bool HasNativeDiscardEntryBoundary()
    {
        if (_resolutionStack.FirstOrDefault() is not NativeDiscardEntryFrame frame) return false;
        AssertNativeDiscardEntry(frame);
        return true;
    }
}
