#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal bool HasProspectiveMouXqQinglong(CharacterState actor, CharacterState target, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade && attack.SourceSeat == actor.Seat && attack.TargetSeat == target.Seat && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card)) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && MouXiaoQiaoOl.JueyanAvailable(actor.Seat, card.Id) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, effectiveKind: _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card), effectiveColor: null, allowAnyColor: false, physicalSuit: Suit.None, effectiveRank: 0, physicalCardIds: [])) && _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(actor, SkillProgramTriggerWindow.PublicHandRevealed).Any(c => c.SkillId == MouXiaoQiaoOlRuntime.Jueyan);
    internal PendingDecision? AddMouXqJueyanRequestChoices(PendingDecision? p)
    {
        if (p is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.PlayerSeat) || !_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].IsAlive)
            return p;
        IReadOnlyList<PromptChoice> choices = [];
        var actor = _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat];
        if (p.Kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } blade)
            choices = MouXqForcedSlashChoices(actor, _coreMatchParticipantsPlayersAccess._players[blade.Attack.TargetSeat], blade.Attack.ResolutionId, true);
        else if (p.Kind == DecisionKind.RespondSlash && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } b)
            choices = MouXqForcedSlashChoices(actor, _coreMatchParticipantsPlayersAccess._players[b.SlashTargetSeat], b.ResolutionId, false);
        else if (p.Kind == DecisionKind.ProgramTrigger && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is { } effect)
            choices = MouXqProgramSlashChoices(f, p.PlayerSeat, _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(f, p.PlayerSeat, effect.Op), effect.ResultBind);
        return choices.Count == 0 ? p : p with
        {
            Choices = Array.AsReadOnly(p.Choices.Concat(choices).DistinctBy(c => c.Id).ToArray())
        };
    }

    internal IReadOnlyList<PromptChoice> MouXqForcedSlashChoices(CharacterState actor, CharacterState target, long parentId, bool qinglong)
    {
        if (qinglong ? _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || blade.Attack.ResolutionId != parentId || blade.Attack.SourceSeat != actor.Seat || blade.Attack.TargetSeat != target.Seat || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack) : _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not { } borrowed || borrowed.ResolutionId != parentId || borrowed.WeaponOwnerSeat != actor.Seat || borrowed.SlashTargetSeat != target.Seat || borrowed.ActiveAttack is not null || _coreNativeBorrowedSwordOperations.GetWeapon(actor)is null)
            return[];
        var c = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(actor, SkillProgramTriggerWindow.PublicHandRevealed).FirstOrDefault(c => c.SkillId == MouXiaoQiaoOlRuntime.Jueyan);
        if (c is null)
            return[];
        var source = new CardConversionSource(c.SkillId, "retained-hand-use", actor.Seat, c.SkillInstanceId);
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card)) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && MouXiaoQiaoOl.JueyanAvailable(actor.Seat, card.Id)).Where(card => qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, effectiveKind: _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card), effectiveColor: null, allowAnyColor: false, physicalSuit: Suit.None, effectiveRank: 0, physicalCardIds: []) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card), Suit.None, false, null, 0, [])).Select(card =>
        {
            var kind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card);
            var p = new Dictionary<string, string>
            {
                [qinglong ? "action" : "response"] = "mou-xiao-qiao-forced-slash",
                ["output-kind"] = kind.ToString(),
                ["original-frame-id"] = parentId.ToString(CultureInfo.InvariantCulture),
                ["forced-use"] = qinglong ? "qinglong" : "borrowed-sword"
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
            return new PromptChoice(new($"jueyan.forced.{(parentId)}.{(actor.Seat)}.{(card.Id)}.{(kind)}"), $"绝颜：展示并保留【{(card.DisplayName)}】，视为使用之。", [card.Id], [target.Seat], p);
        }).ToArray());
    }

    internal bool BeginMouXqForcedSlash(PromptChoice choice)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || choice.Cards is not [var cardId] || choice.Targets is not [var target] || !long.TryParse(choice.Parameters.GetValueOrDefault("original-frame-id"), out var parent) || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("Jueyan lost its native forced Slash choice.");
        var qinglong = choice.Parameters.GetValueOrDefault("forced-use") == "qinglong";
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        if (prompt.Kind != (qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash) || !prompt.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || !MouXqForcedSlashChoices(actor, _coreMatchParticipantsPlayersAccess._players[target], parent, qinglong).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jueyan cannot replace another native target or a pure Played response.");
        StartMouXqRequestedReveal(prompt, global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice), cardId, new(parent, prompt.Kind, kind, actor.Seat, target, _resolutionStackOperations._resolutionStack[^1].Id));
        return true;
    }

    internal void StartMouXqRequestedReveal(PendingDecision prompt, CardConversionSource source, int cardId, MouXiaoQiaoJueyanResponseRequest request)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, source.OwnerSeat, source.SkillId, "retained-hand-use", program.GameplayHash, 0, [cardId], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            ResponseDecision = prompt,
            MouXiaoQiaoJueyanResponse = request
        };
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool IsMouXqForcedRequest(MouXiaoQiaoJueyanResponseRequest r, ResolutionFrame parent) => r.DecisionKind switch
    {
        DecisionKind.RespondSlash => parent is ResponseWindowFrame w && w.ParentFrameId == r.ParentFrameId && w.ResponderSeat == r.ActorSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(r.ParentFrameId)?.Continuations.BorrowedSword is { Active: true, AwaitingSlashChoice: true, ActiveAttackOwnerId: null } b && b.WeaponOwnerSeat == r.ActorSeat && b.SlashTargetSeat == r.TargetSeat,
        DecisionKind.QinglongCrescentBlade => parent is CardUseFrame use && use.Id == r.ParentFrameId && use.SourceSeat == r.ActorSeat && use.Continuations.QinglongCrescentBlade is { Active: true } && use.CardAttack is { } a && a.SourceSeat == r.ActorSeat && a.TargetSeat == r.TargetSeat,
        DecisionKind.ProgramTrigger => parent is ProgramSkillFrame f && IsMouXqProgramRequest(f, r),
        _ => false
    };
    internal void IssueMouXqRequestedSlash(MouXiaoQiaoJueyanResponseGrant grant)
    {
        var r = grant.Request;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed = null;
        if (r.DecisionKind == DecisionKind.RespondSlash)
        {
            borrowed = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword ?? throw new InvalidOperationException("Jueyan lost its original borrowed weapon request.");
            _coreNativeResponseWindowsOperations.PopResponseWindow(borrowed.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(borrowed.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            borrowed.AwaitingSlashChoice = false;
            borrowed.SlashCardId = 0;
            borrowed.EffectiveSlashKind = r.OutputKind;
        }
        else if (r.DecisionKind == DecisionKind.QinglongCrescentBlade)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, actor.Seat, target.Seat, true, [], r.OutputKind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
        }
        else
            PrepareMouXqProgramSlash(r);
        var card = new Card(0, r.OutputKind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [target.Seat], r.OutputKind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, r.OutputKind), physicalCardIds: [], conversionSource: grant.Source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: r.OutputKind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, r.OutputKind), programSkillCardUseFrameId: r.DecisionKind == DecisionKind.ProgramTrigger ? r.ParentFrameId : null);
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (r.DecisionKind == DecisionKind.RespondSlash && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, grant.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, r.OutputKind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, r.OutputKind, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
}
