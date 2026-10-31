#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePindianRandomSelectionsRuntime
{
    internal bool HasPindianRandomPolicy(int seat) => _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.PindianOpponentRandomHand).Any();
    internal bool PindianPolicySourceLive(int seat, CardConversionSource source, SkillProgramCardPolicyKind kind) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[seat], kind).Any(b => b.Source.SkillId == source.SkillId && b.Source.SkillInstanceId == source.SkillInstanceId && b.Policy.Id == source.BindingId);
    internal IReadOnlyList<PindianRandomCandidate> CollectPindianRandomPolicies(int source, int opponent)
    {
        var candidates = new[]
        {
            (Owner: source, Target: opponent),
            (Owner: opponent, Target: source)
        }.SelectMany(p => _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[p.Owner], SkillProgramCardPolicyKind.PindianOpponentRandomHand).Take(1).Select(b => new PindianRandomCandidate(p.Owner, p.Target, new(b.Source.SkillId, b.Policy.Id, p.Owner, b.Source.SkillInstanceId)))).ToArray();
        return Array.AsReadOnly(candidates);
    }

    internal PindianRandomSelection CreatePindianRandomSelection(PindianFrame frame, bool deferredTop = false, int? deferredCard = null)
    {
        return new(CollectPindianRandomPolicies(frame.SourceSeat, frame.OpponentSeat!.Value), Array.AsReadOnly(Array.Empty<PindianRandomReceipt>()), DeferredSourceTop: deferredTop, DeferredSourceCardId: deferredCard);
    }

    internal bool TryContinuePindianRandomSelection(PindianFrame frame)
    {
        if (frame.OpponentSeat is not { } opponent || frame.Result is not null)
            return false;
        if (frame.RandomSelection is null)
        {
            if (!HasPindianRandomPolicy(frame.SourceSeat) && !HasPindianRandomPolicy(opponent))
                return false;
            frame = frame with
            {
                RandomSelection = CreatePindianRandomSelection(frame)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        }

        var state = frame.RandomSelection!;
        while (state.Index < state.Candidates.Count)
        {
            var candidate = state.Candidates[state.Index];
            if (!PindianPolicySourceLive(candidate.OwnerSeat, candidate.Source, SkillProgramCardPolicyKind.PindianOpponentRandomHand) || !_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat]).Count == 0)
            {
                state = state with
                {
                    Index = state.Index + 1
                };
                frame = frame with
                {
                    RandomSelection = state
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame);
                continue;
            }

            var skill = _contentRegistryOperations._contentRegistry.GetSkill(candidate.Source.SkillId);
            var choices = new[]
            {
                new PromptChoice(new ChoiceId($"pindian.{(frame.Id)}.random-{(state.Index)}"), "令对方使用随机手牌拼点", [], [], new Dictionary<string, string> { ["action"] = "pindian-random", ["take"] = "true" }),
                new PromptChoice(new ChoiceId($"pindian.{(frame.Id)}.random-decline-{(state.Index)}"), "不发动", [], [], new Dictionary<string, string> { ["action"] = "pindian-random", ["take"] = "false" })
            };
            _corePindianOperations.SetPindianPrompt(frame, candidate.OwnerSeat, DecisionKind.SkillModule, $"【{(skill.Name)}】：是否令 {(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat].Name)} 使用随机手牌拼点？", choices, [], [], new(candidate.Source.SkillId, skill.Name, skill.Name + " · 随机拼点", skill.Description));
            return true;
        }

        if (state.DeferredSourceTop || state.DeferredSourceCardId is not null)
        {
            var forced = state.Receipts.Any(r => r.TargetSeat == frame.SourceSeat);
            if (!forced)
                frame = frame with
                {
                    SourceCardId = state.DeferredSourceTop ? _corePindianPoliciesOperations.ReserveOrReadPindianTop(_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat], true) : state.DeferredSourceCardId,
                    SourceUsesDrawPileTop = state.DeferredSourceTop
                };
            state = state with
            {
                DeferredSourceTop = false,
                DeferredSourceCardId = null
            };
            frame = frame with
            {
                RandomSelection = state
            };
        }

        frame = frame with
        {
            PindianStep = frame.SourceCardId is null ? PindianStep.ChooseSourceCard : PindianStep.ChooseOpponentCard
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        if (frame.SourceCardId is not null && state.ForcedOpponentCardId is { } forcedOpponent)
        {
            _corePindianOperations.RevealPindian(frame, forcedOpponent);
            return true;
        }

        return false;
    }

    internal bool TryResolvePindianRandomChoice(PindianFrame frame, PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("action") == "pindian-random")
        {
            var state = frame.RandomSelection ?? throw new InvalidOperationException("Random Pindian lost its owning receipt.");
            var candidate = state.Candidates[state.Index];
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (selected.Parameters["take"] == "true" && PindianPolicySourceLive(candidate.OwnerSeat, candidate.Source, SkillProgramCardPolicyKind.PindianOpponentRandomHand) && _coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat]).Count > 0)
            {
                if (candidate.TargetSeat == frame.SourceSeat && frame.SourceUsesDrawPileTop)
                    throw new InvalidOperationException("Random Pindian cannot replace an already reserved top source.");
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat]).OrderBy(card => card.Id).ToArray();
                var before = _randomOperations._random.State;
                var cardId = hand[_randomOperations._random.Next(hand.Length)].Id;
                var receipt = new PindianRandomReceipt(candidate.OwnerSeat, candidate.TargetSeat, candidate.Source, cardId, before, _randomOperations._random.State);
                state = state with
                {
                    Receipts = Array.AsReadOnly(state.Receipts.Append(receipt).ToArray())
                };
                if (candidate.TargetSeat == frame.SourceSeat)
                {
                    frame = frame with
                    {
                        SourceCardId = cardId,
                        SourceUsesDrawPileTop = false
                    };
                }
                else
                    state = state with
                    {
                        ForcedOpponentCardId = cardId
                    };
                // Commit the identity and RNG state on the original frame before publishing the public activation.
                frame = frame with
                {
                    RandomSelection = state
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianRandomHandCommittedEvent(frame.Id, candidate.OwnerSeat, candidate.TargetSeat, candidate.Source));
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(candidate.Source.SkillId).Name)}】，令对方使用随机手牌拼点。", candidate.OwnerSeat, candidate.TargetSeat);
            }

            frame = frame with
            {
                RandomSelection = state with
                {
                    Index = state.Index + 1
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            _corePindianOperations.PublishPindianSelection(frame);
            return true;
        }

        if (frame.PindianStep != PindianStep.ChooseParticipants || frame.RandomSelection is not null)
            return false;
        var opponent = selected.Targets.Single();
        if (!HasPindianRandomPolicy(frame.SourceSeat) && !HasPindianRandomPolicy(opponent))
            return false;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        frame = frame with
        {
            OpponentSeat = opponent,
            PindianStep = PindianStep.ChooseSourceCard
        };
        frame = frame with
        {
            RandomSelection = CreatePindianRandomSelection(frame, selected.Parameters.GetValueOrDefault("action") == "pindian-top", selected.Cards.Count == 1 ? selected.Cards[0] : null)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        _corePindianOperations.PublishPindianSelection(frame);
        return true;
    }

    internal bool IsPindianRandomPrompt(PindianFrame frame) => frame.RandomSelection is { } state && state.Index < state.Candidates.Count;
    internal bool AssertPindianRandomSelection(PindianFrame frame, PendingDecision decision)
    {
        if (frame.RandomSelection is not { } state)
            return false;
        if (state.Candidates is not System.Collections.ObjectModel.ReadOnlyCollection<PindianRandomCandidate> || state.Receipts is not System.Collections.ObjectModel.ReadOnlyCollection<PindianRandomReceipt> || state.Index < 0 || state.Index > state.Candidates.Count || state.Receipts.GroupBy(r => r.TargetSeat).Any(g => g.Count() != 1) || state.Receipts.Any(r => r.Source.OwnerSeat != r.OwnerSeat || r.OwnerSeat == r.TargetSeat || r.RandomBefore == r.RandomAfter || frame.Result is null && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Any(c => c.Id == r.CardId)))
            throw new InvalidOperationException("Random Pindian lost its frozen identities or RNG receipt.");
        if (!IsPindianRandomPrompt(frame))
            return false;
        var candidate = state.Candidates[state.Index];
        if (decision.PlayerSeat != candidate.OwnerSeat || decision.SkillPrompt!.SkillId != candidate.Source.SkillId || decision.ValidCardIds.Count != 0 || decision.ValidTargetSeats.Count != 0 || decision.Choices.Count != 2 || decision.Choices.Any(c => c.Cards.Count != 0 || c.Targets.Count != 0 || c.Parameters.GetValueOrDefault("action") != "pindian-random"))
            throw new InvalidOperationException("Random Pindian selection leaked a hand identity or lost its owner.");
        return true;
    }
}
