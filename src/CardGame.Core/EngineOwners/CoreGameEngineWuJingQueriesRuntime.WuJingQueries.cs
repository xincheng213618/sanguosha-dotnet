#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWuJingQueriesRuntime
{

    internal bool IsTurnSlashChoiceUseForbidden(int actorSeat, PromptChoice choice)
    {
        if (!_characterWuJingOlOperations.HasWuJingLiubing || choice.Cards is not [var id])
            return _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actorSeat, choice.Cards) || _coreCuiYanNativeQueriesOperations.IsCuiYanSlashChoiceForbidden(actorSeat, choice);
        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var conversion);
        var kind = choice.Parameters.TryGetValue("effective-kind", out var name) && Enum.TryParse<CardKind>(name, out var parsed) ? parsed : CardKind.Slash;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var suit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, card, kind, [id], conversion is null ? [] : [conversion], _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, [id], card));
        return _coreTurnHandCategoryRestrictionsOperations.IsHandCategoryMaterialRestricted(choice.Cards) || suit is { } value && _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actorSeat, value) || _coreCuiYanNativeQueriesOperations.IsCuiYanSlashChoiceForbidden(actorSeat, choice);
    }
}
