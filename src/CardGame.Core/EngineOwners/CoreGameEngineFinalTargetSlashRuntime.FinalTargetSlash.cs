#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFinalTargetSlashRuntime
{
    internal static bool HasFinalTargetSlashEffects(IEnumerable<SkillProgramEffect> effects) => RuntimeSkills.RuntimeRules_Core_FinalTargetSlash.HasFinalTargetSlashEffects(effects);
    internal static bool MatchesFinalTargetComparison(ProgramFinalTargetComparison comparison, SkillProgramTriggerFacts facts) => RuntimeSkills.RuntimeRules_Core_FinalTargetSlash.MatchesFinalTargetComparison(comparison, facts);
    internal void IssueFinalTargetSlashReceipt(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context, Facts: { } facts, TargetSeat: { } target } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == context.ParentCardUseFrameId)is not { Action: { } action } owner || action.ActionId != context.CardActionId || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || frame.OwnerSeat != context.ActorSeat || frame.OwnerSeat != action.ActorSeat || !action.EffectiveDesignatedTargetSeats.Contains(target) || !HasFinalTargetSlashEffects(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!.Triggers.Single(t => t.Id == frame.TriggerId).Effects) || effect.FinalTargetComparison is not { } comparison)
            throw new InvalidOperationException("A final target Slash fact lost its exact owning action and frozen target.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !MatchesFinalTargetComparison(comparison, facts))
            return;
        var entries = owner.FinalTargetSlashReceipts ?? [];
        var index = frame.InstructionIndex - 1;
        if (entries.Any(item => item.ProducerFrameId == frame.Id && item.EffectIndex == index))
            return;
        var receipt = new ProgramTargetSlashReceipt(action.ActionId, action.ActorSeat, target, owner.Id, frame.Id, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, index, effect.Op == SkillProgramEffectOp.PreventCurrentTargetSlashCancellation, effect.Op == SkillProgramEffectOp.AddCurrentTargetSlashDamage ? effect.Amount : 0, effect.Op == SkillProgramEffectOp.ZhuiLieEscalateTargetDamage);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { FinalTargetSlashReceipts = Array.AsReadOnly(entries.Append(receipt).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTargetSlashReceiptIssuedEvent(owner.Id, receipt));
    }

    internal IEnumerable<ProgramTargetSlashReceipt> FinalTargetSlashReceipts(long frameId, int target) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == frameId)is { Action: { } action, FinalTargetSlashReceipts: { } entries } ? entries.Where(item => item.ActionId == action.ActionId && item.CardUseFrameId == frameId && item.ActorSeat == action.ActorSeat && item.TargetSeat == target && action.EffectiveDesignatedTargetSeats.Contains(target)) : [];
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 30)]
    internal int FinalTargetSlashDamage(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) ? FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Sum(item => item.DamageBonus) : 0;
    internal bool HasFinalTargetSlashHpDamage(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive && FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Any(item => item.EscalateToTargetHp);
    internal int FinalTargetSlashEscalationBonus(GameEngine.IDamageAttempt attack, int damageWithoutEscalation)
    {
        if (attack.IsChainPropagation || attack is not NativeOwners.CardAttackHandle || !CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive)
            return 0;
        var escalation = FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).FirstOrDefault(item => item.EscalateToTargetHp);
        if (escalation is null)
            return 0;
        var targetHp = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Hp;
        return targetHp - damageWithoutEscalation;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 920)]
    internal bool CanOfferFinalTargetSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext window)
    {
        if (!HasFinalTargetSlashEffects(trigger.Effects) || trigger.Window == SkillProgramTriggerWindow.SlashBeforeResponse && trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ProhibitCurrentResponse))
            return true;
        return window is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: { } target, CardUse: { } context, Facts: not null } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == context.ParentCardUseFrameId)?.Action is { Type: CardActionType.Use } action && action.ActionId == context.CardActionId && action.ActorSeat == candidate.OwnerSeat && context.ActorSeat == candidate.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && action.EffectiveDesignatedTargetSeats.Contains(target) && (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RevealRecordedSuitSlash) || action.EffectiveDesignatedTargetSeats.Count == 1);
    }

    internal bool HasRankSlashRange(CharacterState actor, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (_coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.SlashRangeFromEffectiveRank, kind) || _coreJudgedRankSlashPoliciesOperations.CurrentJudgedRankSlashPolicy(actor.Seat)is not null || _characterLuKangWumiaoOperations.HasWuLuKangDixianGrant(actor.Seat));
    internal bool IsWithinSpecificSlashRange(CharacterState actor, CharacterState target, CardKind kind, int? rank, long? existingUseFrameId = null, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null) => _characterHuangZhongBoundaryMobileOperations.Value.IsBoundaryHuangZhongOrdinaryRange(actor, target, kind, excludedEquipmentId, excludedEquipmentSlot) || _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.SpecificSlashExemption, new(actor, target, kind, ExistingUseFrameId: existingUseFrameId, EffectiveRank: rank)) || (_coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.SlashRangeFromEffectiveRank, kind) && rank is> 0 ? (excludedEquipmentId is null && excludedEquipmentSlot is null ? _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, target.Seat) : CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, target, excludedEquipmentId, excludedEquipmentSlot))) <= rank.Value : excludedEquipmentId is null && excludedEquipmentSlot is null ? _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actor.Seat, target.Seat) : _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(actor.Seat, target.Seat) || CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, target, excludedEquipmentId, excludedEquipmentSlot)) <= CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(actor, excludedEquipmentId, excludedEquipmentSlot)));
    internal bool HasPotentialRankSlashRange(CharacterState actor, CharacterState target, CardKind kind) => (_characterLuKangWumiaoOperations.HasWuLuKangDixianGrant(actor.Seat) || _coreJudgedRankSlashPoliciesOperations.CurrentJudgedRankSlashPolicy(actor.Seat)is { Rank: > 1 } || HasRankSlashRange(actor, kind) && (_coreActionQueriesOperations.GetSlashUseCards(actor).Any(card => IsWithinSpecificSlashRange(actor, target, kind, SpecificSlashRank(actor, card, kind))) || _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Any(pair => IsWithinSpecificSlashRange(actor, target, kind, ZhangbaSpecificSlashRank(actor, pair)))));
    internal int? ProvidedSpecificSlashRank(CharacterState actor, CharacterState provider, IReadOnlyList<Card> cards, bool isTrueZhangba, CardKind kind = CardKind.Slash) => cards.Count == 1 ? HasRankSlashRange(actor, kind) ? _characterGuanYuWumiaoOperations.JuewuForeignHandRank(provider, cards[0]) ?? _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholIdentityRank(provider, cards[0]) ?? SpecificSlashRank(actor, cards[0], kind) : null : isTrueZhangba ? ZhangbaSpecificSlashRank(actor, cards, provider) : null;
    internal bool IsSpecificRankProviderPaymentLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false)
    {
        if (!CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) || !HasRankSlashRange(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat], kind) || pending.TargetSeat is not { } target)
            return true;
        var actor = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var rank = ProvidedSpecificSlashRank(actor, _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat], cards, isTrueZhangba, kind);
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards);
        return pending.IsBorrowedSwordUse ? _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, suit, false, color, rank, physicalCardIds: cards.Select(c => c.Id).ToArray()) : pending.IsQinglongCrescentBladeUse ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, color, false, suit, rank, physicalCardIds: cards.Select(c => c.Id).ToArray()) : pending.IsAssistedProgramUse ? _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(actor.Seat, target, kind, rank, allowPotentialRank: false, physicalCardIds: cards.Select(c => c.Id).ToArray()) : _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, suit, false, color, rank, physicalCardIds: cards.Select(c => c.Id).ToArray());
    }

    internal int? SpecificSlashRank(CharacterState actor, Card card, CardKind kind, long? useId = null)
    {
        if (!HasRankSlashRange(actor, kind))
            return null;
        if (useId is { } id && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == id)?.Action is { } action)
            return action.EffectiveRank is> 0 ? action.EffectiveRank : null;
        var rank = _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(actor, card);
        return rank > 0 ? rank : null;
    }

    internal int? ZhangbaSpecificSlashRank(CharacterState actor, IReadOnlyList<Card> cards, CharacterState? provider = null) => (_coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.SlashRangeFromEffectiveRank, CardKind.Slash) || _characterLuKangWumiaoOperations.HasWuLuKangDixianGrant(actor.Seat) || _characterHuangfuSongWumiaoOperations.HasWuHuangFuSongCapability) && _coreNativeDamageHandlesOperations.Value.UsesFormalZhangbaSerpentSpear && _coreAdvancedWeaponsOperations.HasWeaponAbility(provider ?? actor, CardKind.ZhangbaSerpentSpear) && cards.Count == 2 ? Math.Min(13, cards.Sum(card => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(provider ?? actor, card) ?? card.Rank)) : null;
    void IFinalTargetSlashProgramHost.IssueFinalTargetSlashReceipt(ProgramSkillFrame frame, SkillProgramEffect effect) => IssueFinalTargetSlashReceipt(frame, effect);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 260)]
    internal bool DistancePhysicalSlash260IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => IsWithinSpecificSlashRange(q.Actor, q.Target, q.Kind, q.SpecificEffectiveRank ?? (q.NoEffectiveRank ? null : SpecificSlashRank(q.Actor, q.PhysicalCard!, q.Kind, q.ExistingUseFrameId)), q.ExistingUseFrameId, q.ExcludedEquipmentId, q.ExcludedEquipmentSlot);
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 120)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 50)]
    internal bool DistanceVirtualSlash120IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => IsWithinSpecificSlashRange(q.Actor, q.Target, q.Kind, q.EffectiveRank);
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 110)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 170)]
    internal bool DistanceProvidedSlash110HasPotentialRankSlashRange(ProgramSlashDistanceQuery q) => q.AllowAnyPhysicalSuit && HasPotentialRankSlashRange(q.Actor, q.Target, q.Kind);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 90)]
    internal bool DistanceBorrowedPhysicalSlash90IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => IsWithinSpecificSlashRange(q.Actor, q.Target, q.Kind, SpecificSlashRank(q.Actor, q.PhysicalCard!, q.Kind));
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 50)]
    internal bool DistanceBorrowedPaidWeaponSlash50HasRankSlashRange(ProgramSlashDistanceQuery q) => HasRankSlashRange(q.Actor, CardKind.Slash) && q.PhysicalCard!.Rank > 0;
}
