namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Each target owns one native attack receipt. Later purchases affect the
    // next issued target, never an already requested Dodge quantity.
    private void FreezeRequiredGroupDodgeResponses(GroupCardHandle group, CardAttackHandle attack)
    {
        if (group.RequiredCardKind != CardKind.Dodge) return;
        attack.SetRequiredDodgeResponses(GetRequiredResponseCount(_players[group.SourceSeat],
            group.SourceSeat, attack.TargetSeat, group.Card.Kind, CardKind.Dodge));
    }

    private bool TryFinishRequiredGroupDodge(CardAttackHandle attack)
    {
        if (ActiveGroupCard is not { Effect: GroupCardEffect.ResponseAttack, RequiredCardKind: CardKind.Dodge } group ||
            !SameAttackOwner(group.CurrentAttack, attack)) return false;
        var completed = attack.RegisterDodgeResponse();
        if (attack.RequiredDodgeResponses > 1)
            AdvanceEventRulesAndQueueFact(new RequiredResponseProgressEvent(group.ResolutionId,
                group.SourceSeat, attack.TargetSeat, group.Card.Kind, CardKind.Dodge,
                attack.SuccessfulDodgeResponses, attack.RequiredDodgeResponses));
        if (completed) CompleteAttack(attack);
        else
        {
            SetCardUseStep(group.ResolutionId, ResolutionFrameStep.AwaitingResponse);
            ContinueGroupAttackResponseAfterEffect(group);
        }
        return true;
    }
}
