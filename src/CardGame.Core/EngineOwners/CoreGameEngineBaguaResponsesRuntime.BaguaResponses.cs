#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBaguaResponsesRuntime
{
    internal static PromptChoice BaguaSkipChoice(bool factionDefense = false) => RuntimeSkills.RuntimeRules_Core_BaguaResponses.BaguaSkipChoice(factionDefense);
    internal void SkipBaguaResponseCore()
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondDodge);
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision!;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Prompt = "你未发动【八卦阵】，是否打出【闪】？",
            Choices = decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("response")is not ("bagua" or "faction-defense-bagua" or "bagua-skip")).ToArray()
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void PublishFailedBaguaResponse(NativeOwners.CardAttackHandle attack)
    {
        var defense = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense;
        var responder = _coreMatchParticipantsPlayersAccess._players[defense?.CurrentCandidateSeat ?? attack.TargetSeat];
        var cards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, CardKind.Dodge);
        if (defense is null && _coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is { } requireRed)
            cards = cards.Where(card => CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(responder, card)) == requireRed).ToList();
        if (defense is null)
            _coreNativeResponseWindowsOperations.PushResponseWindow(attack.ResolutionId, attack.SourceSeat, responder.Seat, attack.EffectiveCardKind ?? CardKind.Slash, CardKind.Dodge);
        var choices = defense is not null ? _coreNativeFactionDefenseOperations.CreateFactionDefenseResponseChoices(responder, defense, cards, includeBagua: false) : _coreNativePromptBoundaryOperations.CreateResponseChoices(cards, CardKind.Dodge, attack.EffectiveCardKind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, CardKind.Dodge), includeBagua: false, includeFactionDefense: _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionDefense(responder, attack), responder: responder);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondDodge, responder.Seat, "【八卦阵】判定失败，是否继续打出【闪】？", cards.Select(card => card.Id).ToArray(), [], attack.SourceSeat, attack.EffectiveCardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            RequiredCardKind = CardKind.Dodge,
            TargetSeat = defense?.OwnerSeat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
}
