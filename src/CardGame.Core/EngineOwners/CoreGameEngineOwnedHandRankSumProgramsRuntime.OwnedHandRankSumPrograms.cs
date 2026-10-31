#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 10)]
    internal bool CanStartOwnedHandRankSum(CharacterState owner, ProgramExecutionPlan plan) => plan.Instructions.FirstOrDefault()is not { Op: SkillProgramEffectOp.SelectOwnedHandRankSum } selector || RankSubsetSearch.CanComplete(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Select(c => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(owner, c) ?? c.Rank), selector.ExactRankSum!.Value);
    internal SkillProgramStepOutcome BeginOwnedHandRankSum(ProgramSkillFrame frame, int exactRankSum, string resultBind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!);
        var effect = plan.GetPausedInstruction(active.InstructionIndex).Effect;
        if (active.InstructionIndex != 1 || active.WindowContext is not null || plan.SourceKind != ProgramInstructionSourceKind.Activation || effect.Op != SkillProgramEffectOp.SelectOwnedHandRankSum || effect.ExactRankSum != exactRankSum || effect.ResultBind != resultBind || active.OwnedHandRankSumSelection is not null || active.CardSetBindings.Any(b => b.Name == resultBind))
            throw new InvalidOperationException("Owned-hand rank-sum selection lost its exact activation instruction.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat]).ToArray();
        if (!RankSubsetSearch.CanComplete(hand.Select(c => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], c) ?? c.Rank), exactRankSum))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "自己的手牌中已没有精确点数和方案，未移动任何牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        active = active with
        {
            OwnedHandRankSumSelection = new(active.InstructionIndex, active.OwnerSeat, active.SkillId, active.ActivationId, active.SkillInstanceId, active.GameplayHash, resultBind, exactRankSum, Array.AsReadOnly(hand.Select(c => c.Id).ToArray()), Array.AsReadOnly(hand.Select(c => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], c) ?? c.Rank).ToArray()), Array.AsReadOnly(Array.Empty<int>()))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishOwnedHandRankSum(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static int OwnedHandSelectedRankSum(ProgramOwnedHandRankSumSelection draft) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OwnedHandRankSumPrograms.OwnedHandSelectedRankSum(draft);
    internal IReadOnlyList<PromptChoice> OwnedHandRankSumChoices(ProgramSkillFrame frame)
    {
        var draft = frame.OwnedHandRankSumSelection ?? throw new InvalidOperationException("Missing private rank-sum draft.");
        var remaining = draft.RequiredRankSum - OwnedHandSelectedRankSum(draft);
        var unused = draft.CandidateCardIds.Select((id, index) => (id, rank: draft.CandidateRanks[index])).Where(c => !draft.SelectedCardIds.Contains(c.id)).ToArray();
        var parameters = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ["program-action"] = "owned-hand-rank-sum", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind, ["selection-index"] = draft.SelectedCardIds.Count.ToString(CultureInfo.InvariantCulture) });
        if (remaining == 0)
            return Array.AsReadOnly(new[] { new PromptChoice(new ChoiceId($"owned-hand-rank-sum.frame-{(frame.Id)}.finish-{(draft.SelectedCardIds.Count)}"), $"完成选择（点数和 {(draft.RequiredRankSum)}）。", Array.AsReadOnly(Array.Empty<int>()), Array.AsReadOnly(Array.Empty<int>()), parameters) });
        return Array.AsReadOnly(unused.Where(c => c.rank <= remaining && RankSubsetSearch.CanComplete(unused.Where(other => other.id != c.id).Select(other => other.rank), remaining - c.rank)).Select(c =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(draft.OwnerSeat)).Single(card => card.Id == c.id);
            return new PromptChoice(new ChoiceId($"owned-hand-rank-sum.frame-{(frame.Id)}.pick-{(draft.SelectedCardIds.Count)}.card-{(c.id)}"), $"选择手牌【{(card.DisplayName)}】（{((c.rank == card.Rank ? card.RankText : (card with { Rank = c.rank }).RankText))}）；还需点数 {(remaining)}。", Array.AsReadOnly(new[] { c.id }), Array.AsReadOnly(Array.Empty<int>()), parameters);
        }).ToArray());
    }

    internal void PublishOwnedHandRankSum(ProgramSkillFrame frame)
    {
        var choices = OwnedHandRankSumChoices(frame);
        if (choices.Count == 0)
            throw new InvalidOperationException("An exact rank-sum draft must retain a completion path.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"选择自己的手牌，点数和须恰好为 {(frame.OwnedHandRankSumSelection!.RequiredRankSum)}（完成前不移动牌）。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).ToArray()), Array.AsReadOnly(Array.Empty<int>()), SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 精确点数和", skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("owned-hand-rank-sum")]
    internal void ResolveOwnedHandRankSumChoice(PromptChoice selected)
    {
        if (!long.TryParse(selected.Parameters.GetValueOrDefault("frame-id"), NumberStyles.None, CultureInfo.InvariantCulture, out var frameId))
            throw new InvalidOperationException("The rank-sum choice lost its owning frame.");
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        AssertOwnedHandRankSumSelection(frame, plan);
        var draft = frame.OwnedHandRankSumSelection!;
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], OwnedHandRankSumChoices(frame).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The rank-sum choice is not an exact published, completable selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "手牌选择的当事人或技能实例已失效，未移动任何牌。");
            return;
        }

        if (selected.Cards.Count == 1)
        {
            frame = frame with
            {
                OwnedHandRankSumSelection = draft with
                {
                    SelectedCardIds = Array.AsReadOnly(draft.SelectedCardIds.Append(selected.Cards[0]).ToArray())
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishOwnedHandRankSum(frame);
            return;
        }

        if (OwnedHandSelectedRankSum(draft) != draft.RequiredRankSum || draft.SelectedCardIds.Count == 0)
            throw new InvalidOperationException("A rank-sum selection cannot finish at a different sum.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OwnedHandRankSumSelection = null });
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, draft.ResultBind, draft.SelectedCardIds, SkillProgramCardSetVisibility.Private, Array.AsReadOnly(draft.SelectedCardIds.Select(_ => CardLocation.Hand(draft.OwnerSeat)).ToArray()), selectionActorSeat: draft.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 120)]
    internal void AssertOwnedHandRankSumSelection(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var paused = frame.InstructionIndex > 0 && frame.InstructionIndex <= plan.Instructions.Count ? plan.GetPausedInstruction(frame.InstructionIndex).Effect : null;
        if (frame.OwnedHandRankSumSelection is not { } draft)
        {
            if (paused?.Op == SkillProgramEffectOp.SelectOwnedHandRankSum)
                throw new InvalidOperationException("A suspended exact rank-sum instruction lost its private draft.");
            return;
        }

        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        if (plan.SourceKind != ProgramInstructionSourceKind.Activation || frame.WindowContext is not null || frame.InstructionIndex != 1 || draft.InstructionIndex != frame.InstructionIndex || paused is not { Op: SkillProgramEffectOp.SelectOwnedHandRankSum } || paused.ExactRankSum != draft.RequiredRankSum || paused.ResultBind != draft.ResultBind || draft.OwnerSeat != frame.OwnerSeat || draft.SkillId != frame.SkillId || draft.BindingId != frame.ActivationId || draft.SkillInstanceId != frame.SkillInstanceId || draft.GameplayHash != frame.GameplayHash || draft.RequiredRankSum is < 1 or > 208 || draft.CandidateCardIds.Count != draft.CandidateRanks.Count || draft.CandidateCardIds.Count != hand.Count || draft.CandidateCardIds.Any(id => id <= 0) || draft.CandidateCardIds.Distinct().Count() != draft.CandidateCardIds.Count || draft.CandidateRanks.Any(rank => rank is < 1 or > 13) || !draft.CandidateCardIds.Order().SequenceEqual(hand.Select(c => c.Id).Order()) || draft.CandidateCardIds.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat) || (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], hand.Single(c => c.Id == id)) ?? hand.Single(c => c.Id == id).Rank) != draft.CandidateRanks[index]).Any() || draft.SelectedCardIds.Distinct().Count() != draft.SelectedCardIds.Count || draft.SelectedCardIds.Any(id => !draft.CandidateCardIds.Contains(id)) || OwnedHandSelectedRankSum(draft) > draft.RequiredRankSum || !RankSubsetSearch.CanComplete(draft.CandidateCardIds.Select((id, index) => (id, draft.CandidateRanks[index])).Where(c => !draft.SelectedCardIds.Contains(c.id)).Select(c => c.Item2), draft.RequiredRankSum - OwnedHandSelectedRankSum(draft)) || frame.CardSetBindings.Any(b => b.Name == draft.ResultBind) || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || prompt.TargetSeat != frame.OwnerSeat || prompt.SkillPrompt?.SkillId != frame.SkillId || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, OwnedHandRankSumChoices(frame)))
            throw new InvalidOperationException("A private rank-sum draft lost its exact activation, complete physical hand, completion path or published prompt.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.SelectOwnedHandRankSum)]
    internal PromptChoice SelectAiOwnedHandRankSum(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (frame.OwnedHandRankSumSelection is null || decision.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("The native AI rank-sum chooser lost its own private hand.");
        // The private chooser uses only its own cards; every offered pick still has a feasible completion.
        return decision.Choices.OrderBy(c => c.Cards.Count == 0 ? 0d : CardCatalog.Get(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(card => card.Id == c.Cards[0]).Kind).HandKeepValue).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    SkillProgramStepOutcome global::CardGame.Core.IOwnedHandRankSumHost.BeginOwnedHandRankSum(ProgramSkillFrame frame, int exactRankSum, string resultBind) => BeginOwnedHandRankSum(frame, exactRankSum, resultBind);
}
