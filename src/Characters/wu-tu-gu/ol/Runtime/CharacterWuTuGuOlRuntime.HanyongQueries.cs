#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuTuGuOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27320)]
    internal void CaptureWuTuGuHanyongUse(IGameEvent payload)
    {
        if (!HasWuTuGuCapability || payload is not CardUseDeclaredEvent declared || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { Action: { Type: CardActionType.Use } action } use || use.CardId != declared.CardId || use.SourceSeat != declared.SourceSeat || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || use.CardKind is not (CardKind.BarbarianAssault or CardKind.ArrowBarrage) || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive)
            return;
        foreach (var binding in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).Where(b => b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ArmHanyongNativeGroupDamage }]))
        {
            var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, use.SourceSeat, binding.SkillInstanceId);
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuTuGuHanyongQualifiedEvent>().Any(e => e.UseId == use.Id && e.Source == source))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuHanyongQualifiedEvent(use.Id, action.ActionId, source, binding.Program.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].Hp, action.ProviderSeat, use.CardKind));
        }
    }

    internal WuTuGuHanyongQualifiedEvent? WuTuGuHanyongQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } card || context.OwnerSeat != c.OwnerSeat || card.ActorSeat != c.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != card.CardActionId)
            return null;
        var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuTuGuHanyongQualifiedEvent>().SingleOrDefault(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.Source == source && e.GameplayHash == c.GameplayHash && e.Kind == use.CardKind && e.ProviderSeat == a.ProviderSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Round > 0 && e.HpAtUse < e.Round);
    }

    internal bool ValidWuTuGuHanyongUse(CardUseFrame use) => use.WuTuGuHanyong is { Grants.Count: > 0 } r && use.Action is { Type: CardActionType.Use } a && r.Grants.Select(g => g.Source).Distinct().Count() == r.Grants.Count && r.Grants.All(g => g.UseId == use.Id && g.ActionId == a.ActionId && g.Kind == use.CardKind && a.EffectiveKind == g.Kind && g.Kind is (CardKind.BarbarianAssault or CardKind.ArrowBarrage) && g.ProviderSeat == a.ProviderSeat && g.Round > 0 && g.HpAtUse < g.Round && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, g.Source.OwnerSeat, g.ProviderSeat) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuTuGuHanyongArmedEvent>().Count(e => e.Grant == g) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuTuGuHanyongQualifiedEvent>().Count(e => e.UseId == g.UseId && e.ActionId == g.ActionId && e.Source == g.Source && e.GameplayHash == g.GameplayHash && e.ActualTurn == g.ActualTurn && e.Round == g.Round && e.HpAtUse == g.HpAtUse && e.ProviderSeat == g.ProviderSeat && e.Kind == g.Kind) == 1);
    [NativeUseValidation(27320)]
    internal void AssertWuTuGuNativeUse(CardUseFrame use)
    {
        if (use.WuTuGuHanyong is not null && !ValidWuTuGuHanyongUse(use))
            throw new InvalidOperationException("Hanyong lost its exact once-accepted native use, original source or frozen HP/round.");
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 170)]
    internal int WuTuGuHanyongDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasWuTuGuCapability || attack.IsChainPropagation || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { WuTuGuHanyong: not null } use)
            return 0;
        if (!ValidWuTuGuHanyongUse(use) || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not { } group || group.ResolutionId != use.Id || group.Effect != GroupCardEffect.ResponseAttack || attack.EffectiveCardKind != use.CardKind || !group.TargetSeats.Contains(attack.TargetSeat))
            throw new InvalidOperationException("Hanyong damage must remain on the original native group-use target cursor.");
        return use.WuTuGuHanyong!.Grants.Count;
    }
}
