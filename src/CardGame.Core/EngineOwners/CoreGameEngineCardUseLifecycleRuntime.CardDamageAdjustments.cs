#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability]
internal sealed partial class CoreGameEngineCardUseLifecycleRuntime : ICurrentCardDamageProgramHost
{
    internal void IssueCardDamageAdjustment(long useId, int amount, int? targetSeat = null,
        IReadOnlyList<CardKind>? cardKinds = null, SkillProgramDamageLayer damageLayer = SkillProgramDamageLayer.AdditionalDamage)
    {
        if (amount == 0) return;
        var use = LifecycleCardUse(useId) ?? throw new InvalidOperationException("A card damage adjustment requires its owning native use.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with
        {
            CardDamageAdjustments = [.. use.CardDamageAdjustments ?? [], new(amount, targetSeat, cardKinds, damageLayer)]
        });
    }

    internal int CardDamageAdjustmentBonus(GameEngine.IDamageAttempt attack, bool cardDamageOnly = false) =>
        attack is NativeOwners.CardAttackHandle && !attack.IsChainPropagation && LifecycleCardUse(attack.ResolutionId)?.CardDamageAdjustments is { } rows
            ? rows.Where(row => (row.TargetSeat is null || row.TargetSeat == attack.TargetSeat) &&
                (row.CardKinds.Count == 0 || attack.EffectiveCardKind is { } kind && ProgramCardKindMatch.Contains(row.CardKinds, kind)) &&
                (!cardDamageOnly || row.DamageLayer == SkillProgramDamageLayer.CardDamage)).Sum(row => row.Amount) : 0;

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 25)]
    internal int IssuedCardDamageBonus(GameEngine.IDamageAttempt attack) => CardDamageAdjustmentBonus(attack);

    void ICurrentCardDamageProgramHost.AddCurrentCardDamage(ProgramSkillFrame frame, SkillProgramEffect effect, int amount)
    {
        if (frame.WindowContext?.CardUse is not { } context || LifecycleCardUse(context.ParentCardUseFrameId) is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != context.CardActionId)
            throw new InvalidOperationException("A current-card damage adjustment requires the original accepted card action.");
        int? target = effect.CardDamageTargetScope == ProgramCardDamageTargetScope.CurrentTarget
            ? frame.WindowContext.TargetSeat ?? throw new InvalidOperationException("A target damage adjustment requires its frozen current target.") : null;
        IssueCardDamageAdjustment(use.Id, amount, target, effect.CardKinds, effect.CardDamageLayer);
    }
}
