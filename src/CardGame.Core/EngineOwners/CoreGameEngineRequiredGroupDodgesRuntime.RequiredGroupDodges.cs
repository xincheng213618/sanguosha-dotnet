#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRequiredGroupDodgesRuntime
{
    internal void FreezeRequiredGroupDodgeResponses(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (group.RequiredCardKind != CardKind.Dodge)
            return;
        attack.SetRequiredDodgeResponses(_coreNativeEquipmentAndFactionQueriesOperations.GetRequiredResponseCount(_coreMatchParticipantsPlayersAccess._players[group.SourceSeat], group.SourceSeat, attack.TargetSeat, group.Card.Kind, CardKind.Dodge));
    }

    internal bool TryFinishRequiredGroupDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not { Effect: GroupCardEffect.ResponseAttack, RequiredCardKind: CardKind.Dodge } group || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack))
            return false;
        var completed = attack.RegisterDodgeResponse();
        if (attack.RequiredDodgeResponses > 1)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RequiredResponseProgressEvent(group.ResolutionId, group.SourceSeat, attack.TargetSeat, group.Card.Kind, CardKind.Dodge, attack.SuccessfulDodgeResponses, attack.RequiredDodgeResponses));
        if (completed)
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        else
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.AwaitingResponse);
            _coreNativeGroupAttackOperations.ContinueGroupAttackResponseAfterEffect(group);
        }

        return true;
    }
}
