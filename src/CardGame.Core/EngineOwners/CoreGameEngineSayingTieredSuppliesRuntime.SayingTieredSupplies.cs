#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSayingTieredSuppliesRuntime
{
    internal const string SayingTieredSlashAction = "saying-tiered-zero-slash";
    internal IReadOnlyList<PromptChoice> SayingTieredProgramSlashChoices(ProgramSkillFrame f)
    {
        if (!_characterSunYuanOlOperations.IsSayingSlashRequest(f) || !_characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var actorSeat, out var targets) || targets is not [var targetSeat])
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!actor.IsAlive || !target.IsAlive)
            return[];
        var choices = new List<PromptChoice>();
        foreach (var(source, rule, _)in _coreTieredRoundZeroUsesOperations.TieredRoundZeroRules(actor).Where(r => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.Rule.OutputKind)))
        {
            var kind = rule.OutputKind;
            if (_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actorSeat, targetSeat, Suit.None) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new(0, kind, Suit.None, 0), source, kind, noEffectiveRank: true, physicalCardIds: []) || _characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId) && _characterWoLongFengChuOlOperations.UnconsumedEquipmentSlotPayment(source)is null && !Enum.GetValues<EquipmentSlot>().Any(slot => _characterWoLongFengChuOlOperations.EquipmentSlotNamedChoiceIsPayable(actor, source, kind, EquipmentSlotNamedUseIntent.SayingSlash, targets, slot)))
                continue;
            var args = new Dictionary<string, string>
            {
                ["program-action"] = SayingTieredSlashAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["actor-seat"] = actorSeat.ToString(CultureInfo.InvariantCulture),
                ["output-kind"] = kind.ToString()
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            var choice = new PromptChoice(new($"saying-tiered.{(f.Id)}.{(source.SkillInstanceId)}.{(source.BindingId)}.{(kind)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", [], [targetSeat], args);
            if (_characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId) && _characterWoLongFengChuOlOperations.UnconsumedEquipmentSlotPayment(source)is { } paid && (paid.Intent != EquipmentSlotNamedUseIntent.SayingSlash || paid.OriginalParentId != f.Id || paid.Source != source || paid.EffectiveKind != kind || paid.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash || paid.TargetSignature != global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime.EquipmentSlotNamedTargetSignature(null, choice)))
                continue;
            choices.Add(choice);
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAnswer(SayingTieredSlashAction)]
    internal void ResolveSayingTieredSlash(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !_characterSunYuanOlOperations.IsSayingSlashRequest(f) || !_characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var actorSeat, out var targets) || targets is not [var target] || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != actorSeat || prompt.SkillPrompt?.SkillId != f.SkillId || selected.Cards.Count != 0 || !selected.Targets.SequenceEqual(targets) || !prompt.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || !SayingTieredProgramSlashChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || !Enum.TryParse<CardKind>(selected.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("Saying's tiered supplier lost the exact original private request, native name or target.");
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        // The real equipment-slot program owns all cost children before any
        // global selected-use conversion or Saying native child is created.
        if (_characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId) && _characterWoLongFengChuOlOperations.UnconsumedEquipmentSlotPayment(source)is null)
        {
            _characterWoLongFengChuOlOperations.BeginEquipmentSlotNamedUse(actor, source, kind, EquipmentSlotNamedUseIntent.SayingSlash, prompt, null, selected);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(f, actorSeat, actorSeat, target, kind, [], source);
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, kind, Suit.None, 0), actorSeat, targets, kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], kind), physicalCardIds: [], conversionSource: source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actorSeat, target, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], kind), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actorSeat, target, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[target]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool SayingEquipmentSlotPaymentRidesOn(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!_characterSunYuanOlOperations.IsSayingSlashRequest(f) || !_characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var actor, out var targets) || child is not ProgramSkillFrame { EquipmentSlotNamedUse: { Intent: EquipmentSlotNamedUseIntent.SayingSlash, ResponseChoice: { } choice } draft } payment || payment.ParentFrameId != f.Id || draft.OriginalParentId != f.Id || payment.OwnerSeat != actor || draft.Source.OwnerSeat != actor || payment.SkillId != draft.Source.SkillId || payment.SkillInstanceId != draft.Source.SkillInstanceId || payment.GameplayHash != draft.GameplayHash || !_characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(payment.SkillId) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(draft.EffectiveKind) || !choice.Targets.SequenceEqual(targets) || global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice) != draft.Source || choice.Parameters.GetValueOrDefault("program-action") != SayingTieredSlashAction || draft.OriginalDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } original || original.PlayerSeat != actor || original.SourceSeat != f.OwnerSeat || original.TargetSeat != actor || original.SkillPrompt?.SkillId != f.SkillId || !original.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || draft.OriginalDecisionSignature != global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime.EquipmentSlotNamedDecisionSignature(original) || draft.TargetSignature != global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime.EquipmentSlotNamedTargetSignature(null, choice))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentSlotNamedUseStartedEvent>().Count(e => e.FrameId == payment.Id && e.Source == draft.Source && e.GameplayHash == draft.GameplayHash && e.Intent == draft.Intent && e.EffectiveKind == draft.EffectiveKind && e.OriginalParentId == f.Id && e.OriginalPromptId == original.PromptId && e.OriginalRevision == original.Revision && e.OriginalDecisionSignature == draft.OriginalDecisionSignature && e.TargetSignature == draft.TargetSignature && e.ActualTurn == draft.ActualTurn && e.ActualTurnOwner == draft.ActualTurnOwner && e.PhaseInstance == draft.PhaseInstance) == 1;
    }

    internal bool SayingEquipmentSlotUseStillLegal(CharacterState actor, EquipmentSlotNamedUseDraft draft) => draft.Intent == EquipmentSlotNamedUseIntent.SayingSlash && draft.OriginalParentId is { } parentId && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.Id == parentId && draft.ResponseChoice is { } choice && _characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var originalActor, out _) && actor.Seat == originalActor && SayingTieredProgramSlashChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice]));
}
