#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTargetHandVisibilityRuntime
{
    internal bool HasProgramVisibleTargetHandDuringTrick(int source, CardKind kind) => _coreMatchDefinitionOptionsAccess._options.ShanheBattle is not null && _coreMatchDefinitionOptionsAccess._options.ModeId == ShanheBattleSetup.BattleModeId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && (kind is CardKind.Dismantlement or CardKind.Snatch) && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[source], SkillProgramCardPolicyKind.VisibleTargetHandDuringTrick, kind);
    internal PendingDecision ProjectVisibleTargetHandDecision(PendingDecision decision, int viewerSeat)
    {
        if (decision.Kind != DecisionKind.SelectTargetCard || decision.PlayerSeat != viewerSeat || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not { } selection || selection.SourceSeat != viewerSeat || decision.TargetSeat != selection.TargetSeat || !(_coreWushuangCombatTacticsOperations.WushuangHasVisibleTargetHand(viewerSeat, selection.EffectCardKind) || _coreProgramShanheSourceZhaoranOperations.HasShanheSourceZhaoranPublicHand(selection.TargetSeat)))
            return decision;
        return decision with
        {
            Choices = VisibleTargetHandChoices(selection, decision.Choices, includeMetadata: true)
        };
    }

    internal IReadOnlyList<PromptChoice> VisibleTargetHandChoices(TargetCardSelectionFrame selection, IReadOnlyList<PromptChoice> choices, bool includeMetadata)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[selection.TargetSeat]);
        return Array.AsReadOnly(choices.Select(choice =>
        {
            if (choice.Cards.Count != 0 || !choice.Targets.SequenceEqual([selection.TargetSeat]) || choice.Parameters.GetValueOrDefault("action") != "target-card-slot" || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), NumberStyles.None, CultureInfo.InvariantCulture, out var slot) || slot < 0 || slot >= hand.Count || !selection.CandidateSlots.Contains(slot))
                return choice;
            var card = hand[slot];
            var parameters = new Dictionary<string, string>(choice.Parameters);
            if (includeMetadata)
                parameters["visible-target-hand-card-id"] = card.Id.ToString(CultureInfo.InvariantCulture);
            return choice with
            {
                Description = $"选择【{(card.DisplayName)}】（{(card.RankText)}）",
                Parameters = parameters
            };
        }).ToArray());
    }

    internal bool RefreshVisibleTargetHandSelectionPrompt()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SelectTargetCard, PlayerSeat: 0 } pending || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not { SourceSeat: 0 } selection || pending.TargetSeat != selection.TargetSeat || !(_coreWushuangCombatTacticsOperations.WushuangHasVisibleTargetHand(selection.SourceSeat, selection.EffectCardKind) || _coreProgramShanheSourceZhaoranOperations.HasShanheSourceZhaoranPublicHand(selection.TargetSeat)))
            return false;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Prompt = $"请选择 {(_coreMatchParticipantsPlayersAccess._players[selection.TargetSeat].Name)} 的一张牌。",
            Choices = VisibleTargetHandChoices(selection, pending.Choices, includeMetadata: false)
        };
        return true;
    }
}
