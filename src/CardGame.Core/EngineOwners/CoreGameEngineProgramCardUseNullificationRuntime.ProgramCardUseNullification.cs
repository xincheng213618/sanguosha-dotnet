#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCardUseNullificationRuntime
{
    void IWholeCardUseNullificationHost.NullifyCurrentCardUse(ProgramSkillFrame frame) => NullifyCurrentProgramCardUse(frame);
    internal void NullifyCurrentProgramCardUse(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } context } windowContext || windowContext.TargetSeat != active.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != windowContext.ParentFrameId || window.ParentFrameId != context.ParentCardUseFrameId || window.Action.Type != CardActionType.Use || window.Action.ActionId != context.CardActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } action } use || action.ActionId != context.CardActionId || action.ActorSeat != context.ActorSeat || action.EffectiveKind != context.EffectiveKind || !use.TargetSeats.Contains(active.OwnerSeat) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } candidate || candidate.OwnerSeat != active.OwnerSeat || candidate.SkillId != active.SkillId || candidate.SkillInstanceId != active.SkillInstanceId || candidate.TriggerId != CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active) || candidate.GameplayHash != active.GameplayHash)
            throw new InvalidOperationException("Whole-use nullification lost its exact target-owner use window.");
        // Ineffectiveness is a state, not a return or a replacement of the
        // remaining candidates. A second owner may still commit a conversion.
        if (use.ProgramUseNullification is not null)
        {
            return;
        }

        var proof = new ProgramCardUseNullifiedEvent(active.Id, use.Id, window.Id, action.ActionId, active.SkillId, active.SkillInstanceId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.GameplayHash, active.InstructionIndex, active.OwnerSeat, action.ActorSeat, action.EffectiveKind);
        _invalidations.IssueUse(use.Id, action, new());
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ProgramUseNullification = proof });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(proof);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，" + $"令本次使用的【{(CardCatalog.Get(action.EffectiveKind).DisplayName)}】无效。", active.OwnerSeat, action.ActorSeat);
    }

    internal bool HasBeforeTargetCardProgram(CardKind kind)
    {
        var category = CardCatalog.Get(kind).CategoryName switch
        {
            "基本牌" => SkillProgramCardCategory.Basic,
            "装备牌" => SkillProgramCardCategory.Equipment,
            _ => SkillProgramCardCategory.Trick
        };
        return _contentRegistryOperations._contentRegistry.ProgramDependencies.HasBeforeTargetCardTrigger(kind, category);
    }

    internal bool TryBeginSimpleBeforeTargetPrograms(long useId, ProgramSimpleCardContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("A simple target window lost its use.");
        if (continuation.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol) || use.BeforeSimpleTargetEffectsStarted || !HasBeforeTargetCardProgram(use.CardKind))
            return false;
        var action = use.Action ?? throw new InvalidOperationException("A simple target window lost its accepted action.");
        if (action.Type != CardActionType.Use || !action.TargetSeats.SequenceEqual(use.TargetSeats))
            throw new InvalidOperationException("A simple target window changed its actual use targets.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, current => current with { BeforeSimpleTargetEffectsStarted = true });
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, use.TargetSeats, ProgramCardContinuation.BeforeSimpleTargetEffects, simpleContinuation: continuation);
    }
    internal double GetWholeCardUseNullificationAiScore(int ownerSeat, ProgramCardTriggerWindowFrame window)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId) ?? throw new InvalidOperationException("A use-nullification estimate lost its actual use.");
        if (_invalidations.IsUseIneffective(use))
            return 0d;
        var kind = window.Action.EffectiveKind;
        // This is public utility of the card's effects, not an assumption about
        // the actor's private skill or the value of changing conversion state.
        var equivalent = kind switch
        {
            CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash or CardKind.BorrowedSword => CardKind.Duel,
            CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning => CardKind.Dismantlement,
            CardKind.Peach => CardKind.PeachGarden,
            CardKind.Alcohol or CardKind.DrawTwo => CardKind.FiveGrains,
            _ => kind
        };
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(ownerSeat);
        // Humans have no controlling AI in the engine. Their public hint uses
        // an ephemeral scorer only; it neither chooses nor consumes randomness.
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains.GetValueOrDefault(ownerSeat) ?? new SimpleAiBrain(ownerSeat, unchecked(_coreMatchDefinitionOptionsAccess._options.Seed * 397) ^ (ownerSeat + 1), _coreMatchDefinitionOptionsAccess._options.AiPolicyVersion);
        var targets = window.Action.TargetSeats.Count > 0 ? window.Action.TargetSeats : [window.Action.ActorSeat];
        return targets.Distinct().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Sum(seat => brain.ScoreCardEffectIntervention(view, equivalent, seat)) * (kind == CardKind.DrawTwo ? 2d : 1d);
    }
}
