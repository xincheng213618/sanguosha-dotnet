using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasProgramVisibleTargetHandDuringTrick(int source, CardKind kind) =>
        _options.ShanheBattle is not null && _options.ModeId == ShanheBattleSetup.BattleModeId &&
        IsValidPlayerSeat(source) && (kind is CardKind.Dismantlement or CardKind.Snatch) &&
        HasCardPolicy(_players[source], SkillProgramCardPolicyKind.VisibleTargetHandDuringTrick, kind);

    // This is display metadata on the chooser's already-private snapshot. The
    // command continues to answer the original issued choice and opaque slot.
    private PendingDecision ProjectVisibleTargetHandDecision(PendingDecision decision, int viewerSeat)
    {
        if (decision.Kind != DecisionKind.SelectTargetCard || decision.PlayerSeat != viewerSeat ||
            ActiveTargetCardSelection is not { } selection || selection.SourceSeat != viewerSeat ||
            decision.TargetSeat != selection.TargetSeat ||
            !(WushuangHasVisibleTargetHand(viewerSeat, selection.EffectCardKind) ||
              HasShanheSourceZhaoranPublicHand(selection.TargetSeat))) return decision;
        return decision with { Choices = VisibleTargetHandChoices(selection, decision.Choices, includeMetadata: true) };
    }

    private IReadOnlyList<PromptChoice> VisibleTargetHandChoices(TargetCardSelectionFrame selection,
        IReadOnlyList<PromptChoice> choices, bool includeMetadata)
    {
        var hand = GetHand(_players[selection.TargetSeat]);
        return Array.AsReadOnly(choices.Select(choice =>
        {
            if (choice.Cards.Count != 0 || !choice.Targets.SequenceEqual([selection.TargetSeat]) ||
                choice.Parameters.GetValueOrDefault("action") != "target-card-slot" ||
                !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), NumberStyles.None,
                    CultureInfo.InvariantCulture, out var slot) || slot < 0 || slot >= hand.Count ||
                !selection.CandidateSlots.Contains(slot)) return choice;
            var card = hand[slot];
            var parameters = new Dictionary<string, string>(choice.Parameters);
            if (includeMetadata) parameters["visible-target-hand-card-id"] = card.Id.ToString(CultureInfo.InvariantCulture);
            return choice with
            {
                Description = $"选择【{card.DisplayName}】（{card.RankText}）",
                Parameters = parameters
            };
        }).ToArray());
    }

    private bool RefreshVisibleTargetHandSelectionPrompt()
    {
        if (_pendingDecision is not { Kind: DecisionKind.SelectTargetCard, PlayerSeat: 0 } pending ||
            ActiveTargetCardSelection is not { SourceSeat: 0 } selection ||
            pending.TargetSeat != selection.TargetSeat ||
            !(WushuangHasVisibleTargetHand(selection.SourceSeat, selection.EffectCardKind) ||
              HasShanheSourceZhaoranPublicHand(selection.TargetSeat))) return false;
        _pendingDecision = pending with
        {
            PromptId = CreatePromptId(),
            Prompt = $"请选择 {_players[selection.TargetSeat].Name} 的一张牌。",
            Choices = VisibleTargetHandChoices(selection, pending.Choices, includeMetadata: false)
        };
        return true;
    }
}
