#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCardUseRolesRuntime
{
    internal CardUseFrame GetProgramRoleCardUse(ProgramSkillFrame frame, bool ownerMayBeTarget = false)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == context.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != context.CardActionId || action.Type != CardActionType.Use || action.EffectiveDesignatedTargetSeats.Count != 1 || !IsProgramCardUseRoleOwner(frame.OwnerSeat, action, ownerMayBeTarget))
            throw new InvalidOperationException("A card-use role change requires the owner's unique-target designation window.");
        return use;
    }

    internal static bool IsProgramCardUseRoleOwner(int ownerSeat, CardActionContext action, bool ownerMayBeTarget) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardUseRoles.IsProgramCardUseRoleOwner(ownerSeat, action, ownerMayBeTarget);
    internal IReadOnlyList<int> GetProgramCardUseRoleTargets(ProgramSkillFrame frame, bool ownerMayBeTarget = false)
    {
        var use = GetProgramRoleCardUse(frame, ownerMayBeTarget);
        var action = use.Action!;
        return _coreMatchParticipantsPlayersAccess._players.Where(target => !action.EffectiveDesignatedTargetSeats.Contains(target.Seat) && CanBeProgramCardUseRoleTarget(use, target)).Select(target => target.Seat).ToArray();
    }

    internal IReadOnlyList<int> GetProgramCardUseRoleTargets(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } cardUse } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == cardUse.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != cardUse.CardActionId || !IsProgramCardUseRoleOwner(ownerSeat, action, ownerMayBeTarget: true) || action.EffectiveDesignatedTargetSeats.Count != 1)
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(target => !action.EffectiveDesignatedTargetSeats.Contains(target.Seat) && CanBeProgramCardUseRoleTarget(use, target)).Select(target => target.Seat).ToArray();
    }

    internal bool CanBeProgramCardUseRoleTarget(CardUseFrame use, CharacterState target)
    {
        var action = use.Action!;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var kind = action.EffectiveKind;
        if (!target.IsAlive || target.Seat == actor.Seat || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind, use.Id) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, action.EffectiveSuit ?? Suit.None, global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, action.EffectiveSuit))
            return false;
        var physical = action.PhysicalCards.Count > 0 ? _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(action.PhysicalCards[0].CardId)).Single(card => card.Id == action.PhysicalCards[0].CardId) : new Card(-1, kind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0);
        return kind switch
        {
            CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, physical, action.ConversionChain.FirstOrDefault(), kind, _coreIssuedPlayUsePoliciesOperations.HasIssuedFirstPlayUseDistance(use.Id), use.Id, physicalCardIds: action.PhysicalCards.Select(c => c.CardId).ToArray()),
            CardKind.Duel => true,
            CardKind.Snatch => _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target) && (_coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(use.Id, actor.Seat) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, target, kind, use.Id) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, kind) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, target.Seat) == 1),
            CardKind.Dismantlement => _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target),
            CardKind.FireAttack or CardKind.UnexpectedAssault => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.DrawTwo => _characterSunLuBanBoundaryMobileOperations.AllowsBoundarySunLuBanDrawTwoRoleTarget(use),
            CardKind.IronChain => true,
            CardKind.BorrowedSword => _coreNativeBorrowedSwordOperations.GetWeapon(target)is not null && _coreMatchParticipantsPlayersAccess._players.Any(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, victim)),
            _ => false
        };
    }

    internal void ReplaceCurrentProgramCardUseActor(ProgramSkillFrame frame, int actorSeat)
    {
        var use = GetProgramRoleCardUse(frame);
        // A legal invitee may give away its final card (or weapon). The paid
        // transfer cannot revoke the already selected role-change option.
        if (!_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || actorSeat == frame.OwnerSeat || use.Action!.EffectiveDesignatedTargetSeats.Contains(actorSeat) || !frame.SelectedTargetSeats.SequenceEqual([actorSeat]))
            throw new InvalidOperationException("The replacement actor lost its selected role-change participant.");
        var previous = use.SourceSeat;
        var action = _coreProgramFactionActionRewardOperations.CaptureReplacedFactionActor(CloneRoleAction(use.Action!, actorSeat, use.TargetSeats), actorSeat);
        UpdateProgramRoleCardUse(use with { SourceSeat = actorSeat, Action = action }, action);
        _coreProvenanceCardPoliciesOperations.ClearProvenanceUseOnActorChange(use.Id, actorSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseActorReplacedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, previous, actorSeat, action.ProviderSeat));
    }

    internal SkillProgramStepOutcome AddCurrentProgramCardUseTarget(ProgramSkillFrame frame, int targetSeat)
    {
        var use = GetProgramRoleCardUse(frame, ownerMayBeTarget: true);
        if (!GetProgramCardUseRoleTargets(frame, ownerMayBeTarget: true).Contains(targetSeat))
            throw new InvalidOperationException("The additional target is not legal for the current card.");
        if (use.CardKind == CardKind.BorrowedSword)
            return _coreProgramCompoundCardTargetsOperations.SelectProgramAddedBorrowedSwordVictim(frame, use, targetSeat);
        CompleteProgramCardUseTargetAddition(frame, use, use.TargetSeats.Append(targetSeat).ToArray(), targetSeat);
        return SkillProgramStepOutcome.Continue;
    }

    internal void CompleteProgramCardUseTargetAddition(ProgramSkillFrame frame, CardUseFrame use, IReadOnlyList<int> targets, int targetSeat)
    {
        var action = CloneRoleAction(use.Action!, use.SourceSeat, targets);
        UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, targetSeat));
        if (use.CardKind == CardKind.BorrowedSword && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveSameCategoryFromDeck))
        {
            var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
            var effect = ProgramInstructionResolver.Default.Resolve(frame, program).GetPausedInstruction(frame.InstructionIndex).Effect;
            if (frame.GameplayHash != program.GameplayHash || effect.Op != SkillProgramEffectOp.AddCurrentCardUseTarget || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } || context.ParentCardUseFrameId != use.Id || context.CardActionId != action.ActionId || targets.Count != use.TargetSeats.Count + 2 || !targets.Take(use.TargetSeats.Count).SequenceEqual(use.TargetSeats) || targets[^2] != targetSeat || !frame.SelectedTargetSeats.SequenceEqual([targetSeat]) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedCategoryCompoundTargetsIssuedEvent>().Any(e => e.ProgramFrameId == frame.Id && e.CardUseFrameId == use.Id && e.InstructionIndex == frame.InstructionIndex))
                throw new InvalidOperationException("A compound addition lost its original prefix, paused producer or exact new pair.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedCategoryCompoundTargetsIssuedEvent(frame.Id, use.Id, action.ActionId, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, frame.InstructionIndex, use.TargetSeats, targets));
        }
    }

    internal CardActionContext CloneRoleAction(CardActionContext action, int actorSeat, IReadOnlyList<int> targets) => new(action.ActionId, action.ParentActionId, action.Type, actorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, targets, action.PhysicalCards, action.ConversionChain, action.EffectiveKind == CardKind.BorrowedSword ? targets.Where((_, index) => index % 2 == 0).ToArray() : targets, action.EffectiveSuit, action.EffectiveRank, _contentRegistryOperations._contentRegistry.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeActionAppearance) || action.ConversionChain.Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is not null) || _characterLiuPanOlOperations.HasLiuPanCapability || _characterQuHuangOlOperations.HasQuHuangCapability || _characterRuanYuOlOperations.HasRuanYuCapability || _characterWuJingOlOperations.HasWuJingLiubing || _characterWuJingOlOperations.HasWuJingHeji || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveKenshangTargetRewrite) || _coreDamageTargetBudgetOperations.TracksDamageTargetBudgetTargets || _coreAdjacentRedTargetsOperations.HasAdjacentRedTargetCapability || _coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability || _coreDyingWindowSkillPoliciesOperations.HasBlackTrickTargetPolicy || action.FactionOrigin is not null || (_coreComparedBlackSlashPoliciesOperations.HasComparedBlackSlashPolicies || _coreDirectedCardPolicyQueriesOperations.HasYuanchouCardPolicy) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) ? action.EffectiveIsRed : null, action.FactionOrigin);
    internal void UpdateProgramRoleCardUse(CardUseFrame use, CardActionContext action)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index < 0)
            throw new InvalidOperationException("A card-use role change lost its parent.");
        var previous = (CardUseFrame)_resolutionStackOperations._resolutionStack[index];
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use);
        if (_characterHuaManOlOperations.HasHuaManCapability)
            _characterHuaManOlOperations.CaptureHuaManCompletedUse(use.Id, action);
        for (var cursor = index + 1; cursor < _resolutionStackOperations._resolutionStack.Count; cursor++)
            if (_resolutionStackOperations._resolutionStack[cursor] is ProgramCardTriggerWindowFrame window && window.ParentFrameId == use.Id)
                _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[cursor].Id, window with { Action = action });
        _coreOwnPublicPileConversionsOperations.RefreshOwnPublicPileAddedTargetCandidates(use, previous);
    }
}
