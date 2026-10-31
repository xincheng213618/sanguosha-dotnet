#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiAnniversaryGodRuntime
{
    internal CardUseFrame? ShenDaCuixinOriginal(ProgramSkillWindowContext w, int owner, string skillId) => w.CardUse is { } c && w.Window == SkillProgramTriggerWindow.CardUseCompleted && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } a } use && use.ShenDengAiCuixin is null && a.ActionId == c.CardActionId && a.ActorSeat == owner && (skillId == "happy:cuixin" || skillId == ShenDaCuixin && a.ProviderSeat == owner && a.PhysicalCards.Count > 0 && a.PhysicalCards.All(p => p.From == CardLocation.Hand(owner))) ? use : null;
    internal IReadOnlyList<GameEngine.ShenDaRepeatPlan> ShenDaRepeatPlans(int owner, CardKind kind, IReadOnlyList<int> originalTargets)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(owner, kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner, kind, [])))
            return[];
        var prev = _coreTableOrderOperations.GetAdjacentLivingSeat(owner, -1);
        var next = _coreTableOrderOperations.GetAdjacentLivingSeat(owner, 1);
        var targets = new List<int>();
        if (prev is { } p && next is { } n && p != owner && n != owner)
        {
            if (originalTargets.Contains(p))
                targets.Add(n);
            if (originalTargets.Contains(n))
                targets.Add(p);
        }

        var result = new List<GameEngine.ShenDaRepeatPlan>();
        foreach (var target in targets.Distinct())
        {
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            {
                if (_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: [], countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None))
                    result.Add(new("slash:" + target, kind, LegalActionKind.Slash, [target], "摧心：视为对“" + _coreMatchParticipantsPlayersAccess._players[target].Name + "”使用【" + CardCatalog.Get(kind).DisplayName + "】"));
            }
            else if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
            {
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []))
                {
                    var group = kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains or CardKind.AllianceFeast or CardKind.ReturnSoul or CardKind.StriveUpstream;
                    var eligible = group ? option.TargetSeats.Contains(target) : option.TargetSeats.Count > 0 && option.TargetSeats[0] == target;
                    if (!eligible || option.ActionKind == LegalActionKind.Recast)
                        continue;
                    var seats = group || kind == CardKind.IronChain ? new[]
                    {
                        target
                    }

                    : option.TargetSeats.ToArray();
                    result.Add(new("trick:" + target + ":" + option.Id.Value, kind, option.ActionKind, seats, "摧心：视为对“" + _coreMatchParticipantsPlayersAccess._players[target].Name + "”使用【" + CardCatalog.Get(kind).DisplayName + "】", option.TargetCardId, option.RequiredCardKind));
                }
            }
            else if (CoreNativeCardClassificationsRuntime.IsDelayedCard(kind) && kind != CardKind.Lightning)
            {
                var t = _coreMatchParticipantsPlayersAccess._players[target];
                if (!t.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(t, kind) && !_coreProgramCardPoliciesOperations.HasCardPolicy(t, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && (kind != CardKind.SupplyShortage || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, kind) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, t, kind) || _coreNativeRuleQueriesOperations.GetCombatDistance(owner, target) <= _coreRuleQueriesOperations.GetCardUseDistanceLimit(actor, kind)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, Suit.None) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(owner, target, kind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner, target, Suit.None))
                    result.Add(new("delayed:" + target, kind, CoreGameEngineDelayedPlacementTargetProgramsRuntime.DelayedPlacementActionKind(kind), [target], "摧心：视为对“" + t.Name + "”使用【" + CardCatalog.Get(kind).DisplayName + "】"));
            }
        }

        return result.GroupBy(p => p.Id, StringComparer.Ordinal).Select(g => g.First()).ToArray();
    }

    internal void IssueShenDaCuixin(ProgramSkillFrame f, string option)
    {
        var r = f.ShenDengAi!;
        var original = ShenDaCuixinOriginal(f.WindowContext!, f.OwnerSeat, f.SkillId) ?? throw new InvalidOperationException("Cuixin lost its edition-qualified completed source.");
        var p = ShenDaRepeatPlans(f.OwnerSeat, r.OutputKind!.Value, original.TargetSeats).Single(o => o.Id == option);
        if (r.OriginalUseId != original.Id || r.OriginalActionId != original.Action!.ActionId || r.Stage != ShenDengAiStage.RepeatChoice)
            throw new InvalidOperationException("Cuixin cannot repeat a different original use.");
        r = r with
        {
            Stage = ShenDengAiStage.NativeUse,
            TargetSeat = p.TargetSeats[0],
            OriginalAttackActive = original.CardAttack?.Active == true,
            OriginalDuelActive = original.Continuations.Duel?.Active == true,
            OriginalGroupActive = original.Continuations.GroupCard?.Active == true
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(original.Id, u => u with { CardAttack = r.OriginalAttackActive ? u.CardAttack!with { Active = false } : u.CardAttack, Continuations = u.Continuations with { Duel = r.OriginalDuelActive ? u.Continuations.Duel!with { Active = false } : u.Continuations.Duel, GroupCard = r.OriginalGroupActive ? u.Continuations.GroupCard!with { Active = false } : u.Continuations.GroupCard } });
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenDengAi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiCursorSuspendedEvent(f.Id, original.Id, r.OriginalActionId));
        Card card;
        if (CoreNativeCardClassificationsRuntime.IsDelayedCard(p.Kind))
        {
            var generated = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1);
            card = new(generated, p.Kind, Suit.None, 0);
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(generated);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenDengAi = r = r with { CarrierId = generated } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiCarrierCreatedEvent(f.Id, r.Source, f.GameplayHash, generated, p.Kind));
        }
        else
            card = new(0, p.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, p.TargetSeats, p.Kind, physicalCardIds: []);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.ShenDengAiCuixin is null)
            throw new InvalidOperationException("Cuixin must sign its genuine native Use before effects.");
        if (card.Id > 0)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Processing, new("skill-program.shen-deng-ai.cuixin.carrier"));
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, f.OwnerSeat, targets[0], null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], p.Kind), programSkillCardUseFrameId: f.Id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
            _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal CardActionContext PrepareShenDaCuixinAction(CardActionContext a)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenDengAi: { Stage: ShenDengAiStage.NativeUse, UseId: 0 } r } f)
            return a;
        if (!CharacterDengAiHappyGodRuntime.IsShenDaCuixin(f.SkillId) || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.PhysicalCards.Count != 0 || a.ConversionChain.Count != 0 || a.EffectiveKind != r.OutputKind)
            throw new InvalidOperationException("Cuixin cannot normalize another producer's action.");
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, a.PhysicalCards, a.ConversionChain, a.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 38501)]
    internal void BindShenDaCuixin(CardUseDeclaredEvent e)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { } use)
            return;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { ShenDengAi: { Stage: ShenDengAiStage.NativeUse, UseId: 0 } r } f)
            return;
        if (use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || use.CardKind != r.OutputKind || use.CardId != r.CarrierId || use.PhysicalCardIds is not { Count: 0 } || use.TargetSeats.Count == 0 || use.TargetSeats[0] != r.TargetSeat)
            throw new InvalidOperationException("Cuixin must issue exactly its accepted neutral native card and opposite-neighbor target.");
        var origin = new ShenDengAiNativeOrigin(f.Id, r.Source, f.GameplayHash, r.OriginalUseId, r.OriginalActionId, use.Id, a.ActionId, f.OwnerSeat, r.TargetSeat, use.CardKind, r.CarrierId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShenDengAi = r with { UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ShenDengAiCuixin = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiNativeIssuedEvent(origin));
    }

    internal bool IsShenDaCuixinUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ShenDengAiCuixin: { } o, Action: { } a } use && id == o.UseId && a.ActionId == o.ActionId && use.CardKind == o.Kind && use.CardId == o.CarrierId && use.PhysicalCardIds is { Count: 0 } && a.Type == CardActionType.Use && a.EffectiveKind == o.Kind && a.PhysicalCards.Count == 0 && a.ConversionChain.Count == 0 && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.ProviderSeat == o.ActorSeat && a.RequesterSeat is null && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.FrameId)is ProgramSkillFrame { ShenDengAi: { Stage: ShenDengAiStage.NativeUse } r } f && r.UseId == id && r.ActionId == a.ActionId && r.Source == o.Source && r.GameplayHash == o.GameplayHash && CharacterDengAiHappyGodRuntime.IsShenDaCuixin(f.SkillId) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiNativeIssuedEvent>().Count(e => e.Origin == o) == 1;
    internal bool IsShenDaCuixinTrick(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.CardKind is { } kind && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && IsShenDaCuixinUse(id);
    internal Card ReadShenDaCuixinAppearance(long id, CardAppearanceReference a) => IsShenDaCuixinUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } u && a.Id == u.CardId && a.Kind == u.CardKind && a.Suit == Suit.None && a.Rank == 0 ? new(a.Id, a.Kind, Suit.None, 0) : throw new InvalidOperationException("Cuixin lost its exact native continuation appearance.");

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 38500)]
    internal bool FinishShenDaCuixinAttack(NativeOwners.CardAttackHandle a, out bool paused)
    {
        paused = false;
        if (a.Card is not null || !IsShenDaCuixinUse(a.ResolutionId) || _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)is not { } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.CardKind is not (CardKind.Duel or CardKind.FireAttack or CardKind.UnexpectedAssault))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, use.CardId, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(a, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: a.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(a);
        }

        return true;
    }

    [NativeUseValidation(38500)]
    internal void ValidateShenDaCuixin(CardUseFrame u)
    {
        if (u.ShenDengAiCuixin is not null && (!IsShenDaCuixinUse(u.Id) || u.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Cuixin must finish its exact whole native Use.");
    }

    [NativeUseReturn(38500)]
    internal void ReturnShenDaCuixin(CardUseFrame u)
    {
        if (u.ShenDengAiCuixin is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.ShenDengAi!;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || r.UseId != u.Id || r.ActionId != o.ActionId || r.Returned || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id) != 1)
            throw new InvalidOperationException("Cuixin cannot return or issue twice.");
        foreach (var b in _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == u.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiNativeBatchEvent(f.Id, u.Id, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenDengAi = r with { Stage = ShenDengAiStage.NativeChildren, Returned = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiNativeReturnedEvent(f.Id, u.Id, o.ActionId));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 38500)]
    internal bool ReturnShenDaCuixinAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenDengAi: { Stage: ShenDengAiStage.NativeChildren } r } f || r.UseId != completion.ResolutionId)
            return false;
        ContinueShenDa(f.Id);
        return true;
    }

    internal bool IsShenDaCuixinSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.ShenDengAi is { Stage: ShenDengAiStage.NativeChildren, Returned: true } r && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiNativeBatchEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.BatchId == b.Id) == 1;

    internal void RestoreShenDaCuixinCursor(ProgramSkillFrame f)
    {
        if (f.ShenDengAi is not { OriginalUseId: > 0 } r || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiCursorSuspendedEvent>().Any(e => e.FrameId == f.Id))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || !r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.OriginalUseId)is not { Action: { } a } original || a.ActionId != r.OriginalActionId)
            throw new InvalidOperationException("Cuixin lost its suspended original card cursor.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(original.Id, u => u with { CardAttack = r.OriginalAttackActive ? u.CardAttack!with { Active = true } : u.CardAttack, Continuations = u.Continuations with { Duel = r.OriginalDuelActive ? u.Continuations.Duel!with { Active = true } : u.Continuations.Duel, GroupCard = r.OriginalGroupActive ? u.Continuations.GroupCard!with { Active = true } : u.Continuations.GroupCard } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiCursorRestoredEvent(f.Id, original.Id, a.ActionId));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1100)]
    internal bool WindowSettlement_IsShenDaCuixinSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsShenDaCuixinSettlementBatch(frame, batch);
}
