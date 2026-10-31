#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineColdDamageRuntime
{
    internal bool TryBeginColdDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, DamageNature nature)
    {
        if (nature != DamageNature.Cold || attack.IsChainPropagation || attack.DamageRedirected || attack.IsSourceLess || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || _coreNativeIceSwordOperations.GetIceSwordTargetCardCount(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], attack.SourceSeat) == 0)
            return false;
        if (attack is global::CardGame.Core.NativeOwners.CardAttackHandle)
        {
            if (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ColdDamageReplacementAttempted)
                return false;
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { ColdDamageReplacementAttempted = true });
        }
        else
        {
            if (_coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ColdDamageReplacementAttempted)
                return false;
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { ColdDamageReplacementAttempted = true });
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != attack.ResolutionId)
            throw new InvalidOperationException("Cold damage must be offered on its original direct attack frame.");
        var frame = new ColdDamageReplacementFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, attack.ResolutionId, attack is global::CardGame.Core.NativeOwners.CardAttackHandle, attack.SourceSeat, attack.TargetSeat, amount);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ColdDamageOfferedEvent(frame.Id, frame.ParentFrameId, frame.SourceSeat, frame.TargetSeat, frame.Amount));
        ContinueColdDamageReplacement(frame.Id);
        return true;
    }

    internal global::CardGame.Core.GameEngine.IDamageAttempt ColdDamageAttack(ColdDamageReplacementFrame frame) => frame.CardAttack ? _kernelCoreMainOperations.CreateCardAttackHandle(frame.ParentFrameId) : _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(frame.ParentFrameId);
    internal bool HasColdDamagePrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is ColdDamageReplacementFrame f && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } p && p.PlayerSeat == f.SourceSeat && p.Choices.Count > 0 && p.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") == "native-cold-damage" && c.Parameters.GetValueOrDefault("frame-id") == f.Id.ToString(CultureInfo.InvariantCulture));
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.BeforeOpening, 5770)]
    [NativeFrameRoute(typeof(ColdDamageReplacementFrame), 5770)]
    internal void ContinueColdDamageReplacement(long frameId)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ColdDamageReplacementFrame frame && frame.Id == frameId)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.ColdDamageReplacement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
                return;
            // The previous discard is already paid. A child can change the next
            // available card, but cannot cause that first card to be charged twice.
            if (frame.ChoiceMade && (!frame.Activated || frame.Discards.Count == 2 || !_coreMatchParticipantsPlayersAccess._players[frame.TargetSeat].IsAlive || _coreNativeIceSwordOperations.GetIceSwordTargetCardCount(_coreMatchParticipantsPlayersAccess._players[frame.TargetSeat], frame.SourceSeat) == 0))
            {
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(frame.LogicalDiscard);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ColdDamageCompletedEvent(frame.Id, frame.ParentFrameId, frame.Activated, frame.Activated ? frame.Amount : 0, frame.Discards.Count));
                var attack = ColdDamageAttack(frame);
                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ColdDamageReplacement);
                if (frame.Activated)
                    _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
                else if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                    _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
                return;
            }

            PublishColdDamageChoice(frame);
            return;
        }
    }

    internal IReadOnlyList<PromptChoice> ColdDamageChoices(ColdDamageReplacementFrame frame)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[frame.TargetSeat];
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Parameters(string action) => new()
        {
            ["program-action"] = "native-cold-damage",
            ["action"] = action,
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture)
        };
        PromptChoice Choice(string key, string text, int[] cards, int[] targets, Dictionary<string, string> parameters) => new(new ChoiceId($"cold.{(frame.Id)}.{(frame.Discards.Count)}.{(key)}"), text, cards, targets, parameters);
        if (!frame.ChoiceMade)
        {
            choices.Add(Choice("replace", "防止此次冰伤害，改为依次弃置目标两张牌。", [], [], Parameters("cold-replace")));
            choices.Add(Choice("damage", $"保留对{(target.Name)}的{(frame.Amount)}点冰伤害。", [], [], Parameters("cold-damage")));
        }
        else
        {
            for (var slot = 0; slot < _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count; slot++)
            {
                var p = Parameters("cold-discard");
                p["target-zone"] = "hand";
                p["slot-index"] = slot.ToString(CultureInfo.InvariantCulture);
                choices.Add(Choice($"hand-{(slot)}", $"弃置{(target.Name)}的第{(slot + 1)}个暗手牌位。", [], [target.Seat], p));
            }

            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.SourceSeat, c, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard)))
            {
                var p = Parameters("cold-discard");
                p["target-zone"] = "equipment";
                choices.Add(Choice($"equipment-{(card.Id)}", $"弃置{(target.Name)}装备区的【{(card.DisplayName)}】。", [card.Id], [target.Seat], p));
            }
        }

        return choices;
    }

    internal void PublishColdDamageChoice(ColdDamageReplacementFrame frame)
    {
        var choices = ColdDamageChoices(frame);
        if (choices.Count == 0)
            throw new InvalidOperationException("Cold replacement lost its available next discard.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.SourceSeat, frame.Activated ? $"冰伤害已防止，请选择依次弃置的第{(frame.Discards.Count + 1)}张牌。" : "是否用弃牌替代此次冰伤害？", frame.Activated ? choices.SelectMany(c => c.Cards).ToArray() : [], frame.Activated ? [frame.TargetSeat] : [], SourceSeat: frame.SourceSeat, IncomingCard: ColdDamageAttack(frame).EffectiveCardKind)
        {
            PromptId = _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat].IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = frame.TargetSeat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("native-cold-damage")]
    internal void ResolveColdDamageChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ColdDamageReplacementFrame frame || !HasColdDamagePrompt() || !ColdDamageChoices(frame).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime.SameNameHandChoicesEqual(c, choice)))
            throw new InvalidOperationException("Cold damage lost its exact private source choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var action = choice.Parameters["action"];
        if (!frame.ChoiceMade)
        {
            if (action is not ("cold-replace" or "cold-damage"))
                throw new InvalidOperationException("Cold replacement must be chosen before its paid discards.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ChoiceMade = true, Activated = action == "cold-replace" });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ColdDamageChoiceEvent(frame.Id, frame.Activated));
            ContinueColdDamageReplacement(frame.Id);
            return;
        }

        var target = _coreMatchParticipantsPlayersAccess._players[frame.TargetSeat];
        Card material;
        if (action != "cold-discard" || !frame.Activated || frame.Discards.Count >= 2)
            throw new InvalidOperationException("Cold damage lost its unpaid sequential discard slot.");
        if (choice.Parameters["target-zone"] == "hand" && choice.Cards.Count == 0 && int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), NumberStyles.None, CultureInfo.InvariantCulture, out var slot) && slot >= 0 && slot < _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count)
            material = _corePhysicalOwnedCardQueriesOperations.GetHand(target)[slot];
        else if (choice.Parameters["target-zone"] == "equipment" && choice.Cards.Count == 1 && _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).SingleOrDefault(c => c.Id == choice.Cards[0])is { } equipment)
            material = equipment;
        else
            throw new InvalidOperationException("Cold damage selected an unavailable target entity.");
        var from = _cardZonesOperations._cardZones.GetLocation(material.Id);
        if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.SourceSeat, material, from, OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Cold damage cannot discard protected equipment.");
        var draft = frame.LogicalDiscard;
        if (_coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards)
            draft = draft is null ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.SourceSeat, frame.SourceSeat, [material.Id], global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageDefinitions.ColdDamageDiscardReason, frame.Id) : draft with
            {
                Materials = draft.Materials.Append(new(material.Id, from)).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LogicalDiscard = draft });
        var sequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count;
        _corePhysicalMovementFacadesOperations.MoveCard(material, from, CardLocation.DiscardPile, global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageDefinitions.ColdDamageDiscardReason);
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Skip(sequence).Single(m => m.CardId == material.Id && m.From == from && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageDefinitions.ColdDamageDiscardReason);
        var receipt = new ColdDamageDiscardedEvent(frame.Id, frame.Discards.Count + 1, material.Id, from, movement.Sequence);
        // Keep any recovery attempts enqueued by removing Silver Lion on this frame.
        var current = (ColdDamageReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { Discards = frame.Discards.Append(receipt).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(receipt);
        ContinueColdDamageReplacement(frame.Id);
    }

    internal bool TryChooseColdDamageAi(PendingDecision decision, out PromptChoice selected)
    {
        selected = null !;
        if (!HasColdDamagePrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ColdDamageReplacementFrame frame)
            return false;
        var(id, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseColdDamageChoice(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), frame.TargetSeat, frame.Amount, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++, _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(ColdDamageAttack(frame), decision.PlayerSeat, damageAmount: frame.Amount));
        _coreNativeAiDecisionsOperations.AddThought(thought);
        selected = decision.Choices.Single(c => c.Id == id);
        return true;
    }
}
