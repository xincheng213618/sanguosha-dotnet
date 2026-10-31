#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientCategorySlashTargetsRuntime
{
    internal bool TryGetRecipientCategorySlashUse(int owner, ProgramSkillWindowContext c, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        if (!_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(owner, c, out use, out window) || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || window.Continuation != ProgramCardContinuation.Slash)
            return false;
        var id = use.Id;
        // Native TargetsConfirmed is the immutable original designation. An earlier
        // extra-target skill may already have extended the current action by now.
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Where(e => e.ResolutionId == id).ToArray()is [{ TargetSeats.Count: 1 }];
    }

    internal bool CanBeRecipientCategorySlashTarget(CardUseFrame use, CharacterState target)
    {
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.Action is not { Type: CardActionType.Use } action || !target.IsAlive || use.TargetSeats.Contains(target.Seat) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(action.ActorSeat, target.Seat, use.CardKind, use.Id) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(action.ActorSeat, target.Seat, action.EffectiveSuit) || _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], use.CardKind, [target.Seat]))
            return false;
        // No current primary FAQ establishes distance-free designation. This
        // generic contract therefore applies the native Slash target range.
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], target, new(-1, use.CardKind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0), action.ConversionChain.FirstOrDefault(), use.CardKind, ignoreDistance: false, existingUseFrameId: use.Id, specificEffectiveRank: action.EffectiveRank, physicalCardIds: action.PhysicalCards.Select(c => c.CardId).ToArray());
    }

    internal void CommitRecipientCategorySlashTargets(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark!;
        if (!TryGetRecipientCategorySlashUse(f.OwnerSeat, f.WindowContext!, out var use, out _) || use.Id != r.CardUseFrameId || use.Action!.ActionId != r.ActionId || !use.TargetSeats.SequenceEqual(r.BaseTargetSeats) || r.AddedTargetSeats.Any(s => !CanBeRecipientCategorySlashTarget(use, _coreMatchParticipantsPlayersAccess._players[s])))
            throw new InvalidOperationException("A category mark cannot replace its frozen original Slash or add an illegal target.");
        var result = Array.AsReadOnly(r.BaseTargetSeats.Concat(r.AddedTargetSeats).ToArray());
        if (r.AddedTargetSeats.Count > 0)
        {
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, result);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = result, Action = action, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            foreach (var target in r.AddedTargetSeats)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(f.Id, f.SkillId, f.OwnerSeat, use.Id, target));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategorySlashTargetsResolvedEvent { FrameId = f.Id, CardUseFrameId = use.Id, ActionId = r.ActionId!.Value, Source = r.Source, GameplayHash = r.GameplayHash, TokenId = r.Token!.TokenId, BeforeTargets = r.BaseTargetSeats, AddedTargets = r.AddedTargetSeats, ResultTargets = result });
        _coreRecipientCategoryMarksOperations.CompleteRecipientCategoryMark(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal bool IsRecipientCategorySlashTargetFact(CardUseFrame use, RecipientCategorySlashTargetsResolvedEvent fact, IReadOnlyList<int> before)
    {
        if (use.Action is not { Type: CardActionType.Use } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.Source.OwnerSeat != action.ActorSeat || !fact.BeforeTargets.SequenceEqual(before) || fact.AddedTargets.Count > 2 || fact.AddedTargets.Distinct().Count() != fact.AddedTargets.Count || fact.AddedTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || before.Contains(s)) || !fact.ResultTargets.SequenceEqual(before.Concat(fact.AddedTargets)) || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } p || p.GameplayHash != fact.GameplayHash || p.Triggers.SingleOrDefault(t => t.Id == fact.Source.BindingId)is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.SpendCategoryMarkForSlashTargets } effect])
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<TargetsConfirmedEvent>().Where(e => e.ResolutionId == use.Id).ToArray()is [{ TargetSeats.Count: 1 }] && history.OfType<RecipientCategorySlashTargetsResolvedEvent>().Count(e => e.FrameId == fact.FrameId) == 1 && history.OfType<RecipientCategoryMarkStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.Source == fact.Source && e.GameplayHash == fact.GameplayHash && e.SourceSkillId == effect.SourceBind && e.StateId == effect.StateId && e.Operation == effect.Op && e.TokenId == fact.TokenId && e.HolderSeat == action.ActorSeat) == 1 && history.OfType<RecipientCategoryMarkConsumedEvent>().Count(e => e.FrameId == fact.FrameId && e.TokenId == fact.TokenId && e.HolderSeat == action.ActorSeat && e.SourceSkillId == effect.SourceBind && e.StateId == effect.StateId && e.Kind == RecipientCategoryMarkKind.ExtraTargets) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.OwnerSeat == action.ActorSeat && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == trigger.Window) == 1 && history.OfType<ProgramCardUseTargetAddedEvent>().Where(e => e.FrameId == fact.FrameId && e.CardUseFrameId == use.Id).Select(e => e.TargetSeat).SequenceEqual(fact.AddedTargets);
    }

    internal bool HasRecipientCategorySlashTargetTail(CardUseFrame use) => _coreRecipientCategoryMarksOperations.TracksRecipientCategoryMarks && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientCategorySlashTargetsResolvedEvent>().Any(e => e.AddedTargets.Count > 0 && IsRecipientCategorySlashTargetFact(use, e, e.BeforeTargets) && use.TargetSeats.Count == use.Action!.TargetSeats.Count && use.Action.TargetSeats.Count >= e.ResultTargets.Count && use.Action.TargetSeats.Take(e.ResultTargets.Count).SequenceEqual(e.ResultTargets));
}
