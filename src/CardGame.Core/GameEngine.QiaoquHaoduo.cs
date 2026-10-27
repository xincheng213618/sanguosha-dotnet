namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int? BorrowedSwordSlashDamageModifierOwner(IDamageAttempt attack)
    {
        if (attack is not CardAttackHandle || attack.IsSourceLess || attack.IsChainPropagation ||
            attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage ||
            attack.EffectiveCardKind is not { } kind || !IsSlashCard(kind) ||
            LifecycleCardUse(attack.ResolutionId) is not { Action: { Type: CardActionType.Use } action } child ||
            !IsSlashCard(child.CardKind) || child.SourceSeat != action.ActorSeat ||
            attack.CardUserSeat != action.ActorSeat)
            return null;

        // The native continuation owns the exact forced child. RequesterSeat
        // instead describes card supply, and cannot identify the trick user.
        var parents = _resolutionStack.OfType<CardUseFrame>().Where(parent =>
            parent.Id != child.Id && parent.CardKind == CardKind.BorrowedSword &&
            parent.Action is { Type: CardActionType.Use } original &&
            parent.Continuations.BorrowedSword is { Active: true } borrowed &&
            borrowed.ActiveAttackOwnerId == child.Id &&
            parent.SourceSeat == borrowed.SourceSeat && original.ActorSeat == borrowed.SourceSeat &&
            action.ParentActionId == original.ActionId && action.ActorSeat == borrowed.WeaponOwnerSeat &&
            IsValidPlayerSeat(borrowed.SourceSeat)).Take(2).ToArray();

        // Redirection may change the victim without changing this paid Slash.
        return parents is [var owner] ? owner.SourceSeat : null;
    }
}
