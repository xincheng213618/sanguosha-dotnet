#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal CardUseFrame? ChijieDamageUse(int owner, ProgramSkillWindowContext w, out CompletedUndamagedUseDamageRecordedEvent damage)
    {
        damage = null !;
        if (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not DamageTriggerWindowFrame window || window.TargetSeat != owner || window.SourceSeat == owner || w.TargetSeat != owner || w.SourceSeat != window.SourceSeat || window.TriggerWindow != w.Window)
            return null;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedUndamagedUseDamageRecordedEvent>().Where(e => e.DamageFrameId == window.ParentFrameId && e.TargetSeat == owner && e.SourceSeat == window.SourceSeat && e.Amount > 0 && !e.SourceLess).ToArray();
        if (facts is not [var fact] || _coreCardUseLifecycleOperations.LifecycleCardUse(fact.CardUseFrameId)is not { Action: { Type: CardActionType.Use } a } use || fact.CardActionId != a.ActionId || fact.EffectiveKind != use.CardKind || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a))
            return null;
        damage = fact;
        return use;
    }

    internal IReadOnlyList<int> ChijieClaimable(CardUseFrame use)
    {
        if (use.XinPiOrigin is not { } o || use.Action is not { } a || o.UseId != use.Id || o.Action.ActionId != a.ActionId || !o.Action.PhysicalCards.SequenceEqual(a.PhysicalCards) || !_coreCompletedUndamagedTargetRevealOperations.HasExactAcceptedCompletedUndamagedUse(use, a))
            return[];
        return Array.AsReadOnly(o.Materials.Where(m => !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, m.CardId) && XinPiMaterialStillOriginal(use, m)).Select(m => m.CardId).Distinct().ToArray());
    }

    internal bool ChijieUndamagedIncoming(int owner, ProgramSkillWindowContext w, out CardUseFrame use)
    {
        var actual = ExactXinPiNativeWindow(w, true);
        use = actual!;
        return actual is { Action: { } a } && a.ActorSeat != owner && XinPiMainTargets(actual).Contains(owner) && !actual.CausedDamage && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.CardUseFrameId == actual.Id && e.Amount > 0) && ChijieClaimable(actual).Count > 0;
    }
}
