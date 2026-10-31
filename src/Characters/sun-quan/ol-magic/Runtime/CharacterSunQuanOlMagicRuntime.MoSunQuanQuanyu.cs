#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal const string MoQuanyuDrawReason = "skill-program.mo-sun-quan.quanyu.draw";
    internal const int MoQuanyuAllEffects = 63;
    internal bool HasMoSunQuanQuanyuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoSunQuanQuanyuRoundChoices);

    internal static int MoQuanyuBit(MoQuanyuEffect effect) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_quan_ol_magic_MoSunQuanQuanyu.MoQuanyuBit(effect);
    internal static CardConversionSource MoQuanyuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_quan_ol_magic_MoSunQuanQuanyu.MoQuanyuSource(f);
    internal MoQuanyuEffect MoQuanyuCurrentChoice(int issuer, int participant) => !HasMoSunQuanQuanyuCapability ? MoQuanyuEffect.None : _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuChoiceRevealedEvent>().LastOrDefault(e => e.IssuerSeat == issuer && e.ParticipantSeat == participant && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber)?.Effect ?? MoQuanyuEffect.None;
    internal int MoQuanyuHistoryMask(int issuer, int participant) => !HasMoSunQuanQuanyuCapability ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuChoiceRevealedEvent>().Where(e => e.IssuerSeat == issuer && e.ParticipantSeat == participant).Aggregate(0, (mask, e) => mask | MoQuanyuBit(e.Effect));
    internal bool TryGetMoQuanyuSource(int issuer, out CardConversionSource source, out string gameplayHash)
    {
        source = null !;
        gameplayHash = "";
        if (!HasMoSunQuanQuanyuCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(issuer) || !_coreMatchParticipantsPlayersAccess._players[issuer].IsAlive)
            return false;
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[issuer]).ProgramInstances.Where(i => i.SkillId == "ol:mo-quanyu").OrderBy(i => i.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        if (instance is null)
            return false;
        source = new(instance.SkillId, "target-only-choices", issuer, instance.SkillInstanceId);
        gameplayHash = instance.Program.GameplayHash;
        return true;
    }

    internal IReadOnlyList<MoQuanyuPublicChoice> GetMoQuanyuPublicChoices() => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuChoicesStartedEvent>().Select(e => e.Source.OwnerSeat).Distinct().Order().SelectMany(issuer => _coreMatchParticipantsPlayersAccess._players.Select(p => new MoQuanyuPublicChoice(issuer, p.Seat, _coreActualRoundProgressRoundNumberAccess._roundNumber, MoQuanyuCurrentChoice(issuer, p.Seat), MoQuanyuHistoryMask(issuer, p.Seat)))).ToArray());
    internal bool CanOfferMoQuanyuRound(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HasMoSunQuanQuanyuCapability && c.SkillId == "ol:mo-quanyu" && c.BindingId == "round-choices" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && context.OwnerSeat == c.OwnerSeat && context.Window == SkillProgramTriggerWindow.RoundStarting && context.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<RoundStartedEvent>().LastOrDefault()?.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<RoundStartedEvent>().Last().ActorSeat == context.SourceSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.RoundStarting, Continuation: ProgramLifecycleContinuation.RoundProgramsTurnStart } mother && mother.CandidateIndex >= 0 && mother.CandidateIndex < mother.Candidates.Count && mother.Candidates[mother.CandidateIndex] == c && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuChoicesStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Kind == MoQuanyuInvocationKind.Round && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28910, SkillProgramEffectOp.ResolveMoSunQuanQuanyuRoundChoices)]
    internal bool CanOfferMoQuanyuRoundGate(ProgramTriggerLegalityContext g) => CanOfferMoQuanyuRound(g.Candidate, g.Context);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28911, SkillProgramEffectOp.ResolveMoSunQuanQuanyuTargetChoice)]
    internal bool HideInternalMoQuanyuTargetChoice(ProgramActivationLegalityContext g) => false;
    SkillProgramStepOutcome global::CardGame.Core.IMoSunQuanQuanyuHost.BeginRound(ProgramSkillFrame f) => BeginMoQuanyuRound(f);
    SkillProgramStepOutcome global::CardGame.Core.IMoSunQuanQuanyuHost.BeginTarget(ProgramSkillFrame f) => throw new InvalidOperationException("Quanyu target-only choices require the exact paid native Tianen factory.");
    internal SkillProgramStepOutcome BeginMoQuanyuRound(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MoQuanyu is not null || f.TriggerId != "round-choices" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferMoQuanyuRound(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context))
            throw new InvalidOperationException("Quanyu requires one genuine first normal turn boundary for this actual round.");
        var participants = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, p.Seat)).Select((p, i) => FreezeMoQuanyuParticipant(f.OwnerSeat, p.Seat, i)).ToArray();
        var r = new MoQuanyuReceipt(MoQuanyuSource(f), f.GameplayHash, MoQuanyuInvocationKind.Round, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, MoQuanyuEffect.None)
        {
            Participants = participants,
            Stage = MoQuanyuStage.Choosing
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyu = r });
        RecordMoQuanyuStart(f);
        ContinueMoQuanyu(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal MoQuanyuParticipant FreezeMoQuanyuParticipant(int issuer, int participant, int index)
    {
        var history = MoQuanyuHistoryMask(issuer, participant);
        return new(index, participant, history, MoQuanyuAllEffects & ~history);
    }

    internal void BeginMoQuanyuTargetChoice(ProgramSkillFrame parent, MoQuanyuTargetRequest request)
    {
        parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id);
        if (request.ParentFrameId != parent.Id || request.ParentInstructionIndex != parent.InstructionIndex || request.ParentSource != new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || request.ParentGameplayHash != parent.GameplayHash || request.QuanyuSource.SkillId != "ol:mo-quanyu" || request.QuanyuSource.BindingId != "target-only-choices" || request.QuanyuSource.OwnerSeat != parent.OwnerSeat || request.QuanyuSource.SkillInstanceId != "issued:tianen:" + parent.Id || _contentRegistryOperations._contentRegistry.GetSkill(request.QuanyuSource.SkillId).Program?.GameplayHash != request.QuanyuGameplayHash || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(request.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[request.TargetSeat].IsAlive || !IsMoSunQuanTianenQuanyuRequest(parent, request) || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuTargetRequestIssuedEvent>().Any(e => e.Request.ParentFrameId == parent.Id))
            throw new InvalidOperationException("Quanyu target-only choices require the one signed paid Tianen parent request.");
        var r = new MoQuanyuReceipt(request.QuanyuSource, request.QuanyuGameplayHash, MoQuanyuInvocationKind.TargetOnly, request.RoundNumber, request.ActualTurn, request.TurnOwner, MoQuanyuCurrentChoice(parent.OwnerSeat, parent.OwnerSeat))
        {
            Stage = MoQuanyuStage.Choosing,
            Participants = [FreezeMoQuanyuParticipant(parent.OwnerSeat, request.TargetSeat, 0)],
            TargetRequest = request
        };
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.OwnerSeat, "ol:mo-quanyu", "target-only-choices", request.QuanyuGameplayHash, 1, [], [])
        {
            SkillInstanceId = request.QuanyuSource.SkillInstanceId,
            MoQuanyu = r
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuTargetRequestIssuedEvent(f.Id, request));
        _coreRuntimeOperations.PushRuntimeFrame(f);
        RecordMoQuanyuStart(f);
        ContinueMoQuanyu(f.Id);
    }

    internal void RecordMoQuanyuStart(ProgramSkillFrame f)
    {
        var r = f.MoQuanyu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuChoicesStartedEvent(f.Id, r.Source, r.GameplayHash, r.Kind, r.RoundNumber, r.ActualTurn, r.TurnOwner, r.OwnerChoiceBefore, r.Participants.Count));
        foreach (var p in r.Participants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuParticipantFrozenEvent(f.Id, p.Index, p.Seat, p.HistoryMask, p.AvailableMask));
    }

    internal static string MoQuanyuEffectName(MoQuanyuEffect e) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_quan_ol_magic_MoSunQuanQuanyu.MoQuanyuEffectName(e);
    internal IReadOnlyList<PromptChoice> MoQuanyuChoices(ProgramSkillFrame f)
    {
        var r = f.MoQuanyu!;
        if (r.Stage != MoQuanyuStage.Choosing || r.Votes.Count >= r.Participants.Count)
            return[];
        var p = r.Participants[r.Votes.Count];
        return Array.AsReadOnly(Enum.GetValues<MoQuanyuEffect>().Where(e => e != MoQuanyuEffect.None && (p.AvailableMask & MoQuanyuBit(e)) != 0).Select(e => new PromptChoice(new($"mo-quanyu.{(f.Id)}.{(p.Index)}.{((int)e)}"), MoQuanyuEffectName(e), [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mo-quanyu-choice", ["effect"] = ((int)e).ToString(CultureInfo.InvariantCulture), ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["participant"] = p.Seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishMoQuanyu(ProgramSkillFrame f)
    {
        var r = f.MoQuanyu!;
        var chooser = r.Participants[r.Votes.Count].Seat;
        var choices = MoQuanyuChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("An exhausted Quanyu participant must be recorded without an empty prompt.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, "权御：密选一项尚未选择过的效果。", [], [chooser], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mo-quanyu-choice")]
    internal void AnswerMoQuanyu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQuanyu: { Stage: MoQuanyuStage.Choosing } r } f || r.Votes.Count >= r.Participants.Count || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.Participants[r.Votes.Count].Seat || !MoQuanyuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || !int.TryParse(choice.Parameters.GetValueOrDefault("effect"), out var number) || number is < 1 or > 6)
            throw new InvalidOperationException("Quanyu requires the current participant's exact published private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, MoQuanyu = r with { Votes = r.Votes.Append(new(r.Participants[r.Votes.Count].Seat, (MoQuanyuEffect)number)).ToArray() } });
        ContinueMoQuanyu(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMoSunQuanQuanyuRoundChoices, SkillProgramEffectOp.ResolveMoSunQuanQuanyuTargetChoice)]
    internal PromptChoice ChooseMoQuanyuAi(PendingDecision prompt) => prompt.Choices.OrderBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
