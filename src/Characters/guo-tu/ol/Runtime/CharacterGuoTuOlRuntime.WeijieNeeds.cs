#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal const string WeijieOpen = RuntimeSkills.RuntimeRules_guo_tu_ol_WeijieNeeds.WeijieOpen, WeijieChoice = "weijie-choice", WeijieDiscardReason = "skill-program.guo-tu.weijie.discard";
    internal bool HasWeijieCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWeijieRequiredBasicRequest);
    internal bool WeijieTurnOpen(int seat) => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && seat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal bool WeijieSpent(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().Any(e => e.Source.OwnerSeat == seat && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal IEnumerable<(CardConversionSource Source, string Hash)> WeijieSources(CharacterState actor)
    {
        if (!HasWeijieCapability || !actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !WeijieTurnOpen(actor.Seat) || WeijieSpent(actor.Seat))
            yield break;
        foreach (var i in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var a in i.Program.Activations.Where(a => a.Effects is [{ Op: SkillProgramEffectOp.ResolveWeijieRequiredBasicRequest }]))
                yield return (new(i.SkillId, a.Id, actor.Seat, i.SkillInstanceId), i.Program.GameplayHash);
    }

    internal IEnumerable<int> WeijieVictims(int actor) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor && _coreNativeRuleQueriesOperations.GetCombatDistance(actor, p.Seat) == 1 && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Any(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, CardLocation.Hand(p.Seat), OwnedCardMoveIntent.Discard))).Select(p => p.Seat);
    internal WeijieNeed? CaptureWeijieNeed(PendingDecision original, ResolutionFrame? suppliedNativeTop = null) => WeijieCurrentNeed(original.PlayerSeat, original.Kind, suppliedNativeTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault());
    internal WeijieNeed? WeijieCurrentNeed(int provider, DecisionKind kind, ResolutionFrame? top)
    {
        if (!HasWeijieCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(provider) || top is null || top is ProgramSkillFrame { Weijie: not null })
            return null;
        var owner = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        long? action = _coreCardUseLifecycleOperations.LifecycleCardUse(owner)?.Action?.ActionId;
        WeijieNeed N(WeijieIntent intent, int actor, IReadOnlyList<int> targets, int cursor = 0, int? requester = null, int? opponent = null, int index = 0, int round = 0, SkillProgramEffectOp? op = null, int instruction = 0, long? ownerId = null) => new(intent, ownerId ?? owner, top.Id, action, provider, actor, requester, opponent, cursor, index, round, instruction, op, targets);
        if (kind == DecisionKind.RescueDying && top is DyingFrame d && d.ResponderSeat == provider)
            return N(WeijieIntent.Dying, provider, [d.VictimSeat], d.ResponderIndex, ownerId: d.Id);
        if (kind == DecisionKind.RespondDodge)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } fd && fd.CurrentCandidateSeat == provider && CoreNativeDamageHandlesRuntime.SameAttackOwner(fd.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack))
                return N(WeijieIntent.FactionDodge, fd.OwnerSeat, [], fd.CandidateIndex, fd.OwnerSeat, fd.Attack.SourceSeat, ownerId: fd.Attack.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } ga } g && ga.TargetSeat == provider && g.RequiredCardKind == CardKind.Dodge)
                return N(WeijieIntent.Group, provider, [], ga.SuccessfulDodgeResponses, opponent: g.SourceSeat, index: g.TargetIndex, ownerId: g.ResolutionId);
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.TargetSeat == provider && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveCardKind ?? CardKind.Dodge))
                return N(WeijieIntent.Dodge, provider, [], a.SuccessfulDodgeResponses, opponent: a.SourceSeat, ownerId: a.ResolutionId);
        }

        if (kind == DecisionKind.RespondSlash)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } f && f.CurrentCandidateSeat == provider)
            {
                var played = !CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(f);
                var opponent = f.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.OpponentSeat : f.ResponseAttack?.SourceSeat;
                var program = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == f.ProgramSkillFrameId);
                return N(played ? WeijieIntent.FactionSlashPlayed : WeijieIntent.FactionSlashUse, f.OwnerSeat, f.TargetSeat is { } t ? [t] : [], f.CandidateIndex, f.OwnerSeat, opponent, op: program is null ? null : _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(program)?.Op, instruction: program?.InstructionIndex ?? 0, ownerId: f.OwnerFrameId);
            }

            if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.WeaponOwnerSeat == provider)
                return N(WeijieIntent.BorrowedSword, provider, [b.SlashTargetSeat], opponent: b.SourceSeat, ownerId: b.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && duel.ResponderSeat == provider)
                return N(WeijieIntent.Duel, provider, [], duel.SuccessfulSlashResponses, opponent: duel.OpponentSeat, ownerId: duel.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } ga } g && ga.TargetSeat == provider && g.RequiredCardKind == CardKind.Slash)
                return N(WeijieIntent.Group, provider, [], opponent: g.SourceSeat, index: g.TargetIndex, ownerId: g.ResolutionId);
        }

        if (kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.SourceSeat == provider && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack))
            return N(WeijieIntent.Qinglong, provider, [q.Attack.TargetSeat], opponent: q.Attack.TargetSeat, ownerId: q.Attack.ResolutionId);
        if (kind != DecisionKind.ProgramTrigger || top is not ProgramSkillFrame p || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)is not { } effect)
            return null;
        if (_coreWenyiPeachSuppliesOperations.TryGetWenyiWeijiePeachNeed(p, provider, out var wenyiTarget))
            return N(WeijieIntent.WenyiPeach, provider, [wenyiTarget], op: effect.Op, instruction: p.InstructionIndex);
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(p) && _characterSunYuanOlOperations.TryGetSayingSlashRequest(p, out var sayingActor, out var sayingTargets) && provider == sayingActor)
            return N(WeijieIntent.AssistedSlash, provider, sayingTargets, op: effect.Op, instruction: p.InstructionIndex);
        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(p) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(p, out var requestedActor, out var requestedTargets) && provider == requestedActor)
            return N(WeijieIntent.AssistedSlash, provider, requestedTargets, op: effect.Op, instruction: p.InstructionIndex);
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(p) && p.LvKuangLvXiang!.Actor == provider)
            return N(WeijieIntent.AssistedSlash, provider, [p.LvKuangLvXiang.Target], op: effect.Op, instruction: p.InstructionIndex);
        if (_characterWuJingOlOperations.IsWuJingHejiRequest(p) && p.OwnerSeat == provider)
            return N(WeijieIntent.AssistedSlash, provider, [p.WuJing!.TargetSeat], op: effect.Op, instruction: p.InstructionIndex);
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(p) && p.OwnerSeat == provider)
            return N(WeijieIntent.AssistedSlash, provider, _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != provider).Select(t => t.Seat).ToArray(), op: effect.Op, instruction: p.InstructionIndex);
        if (p.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice, ActorSeat: var actor } && actor == provider && _characterKeBiNengOlOperations.ValidKeBiNeng(p))
            return N(WeijieIntent.GenericBasic, provider, _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive).Select(t => t.Seat).ToArray(), op: effect.Op, instruction: p.InstructionIndex);
        if (!_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(p, effect.Op, provider))
            return null;
        var targets = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(p, provider, effect.Op);
        if (targets.Length == 0)
            return null;
        return N(effect.Op switch
        {
            SkillProgramEffectOp.RequestSlashByTarget => WeijieIntent.ProgramSlash,
            SkillProgramEffectOp.RequestSlashByNearest => WeijieIntent.ProgramNearestSlash,
            SkillProgramEffectOp.RequestLegalSlashByNearest => WeijieIntent.NearestLegalSlash,
            SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => WeijieIntent.FixedSlash,
            _ => WeijieIntent.AssistedSlash
        }, provider, targets, op: effect.Op, instruction: p.InstructionIndex);
    }

    internal static bool WeijieIsDying(WeijieNeed n) => RuntimeSkills.RuntimeRules_guo_tu_ol_WeijieNeeds.WeijieIsDying(n);
    internal static bool WeijieIsPlayed(WeijieNeed n) => RuntimeSkills.RuntimeRules_guo_tu_ol_WeijieNeeds.WeijieIsPlayed(n);
    internal IEnumerable<(CardKind Kind, int Target)> WeijieOutputs(WeijieNeed n)
    {
        var kinds = n.Intent == WeijieIntent.WenyiPeach ? new[]
        {
            CardKind.Peach
        }

        : WeijieIsDying(n) ? new[]
        {
            CardKind.Peach,
            CardKind.Alcohol
        }

        : n.Intent is WeijieIntent.Dodge or WeijieIntent.FactionDodge || n.Intent == WeijieIntent.Group && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!.RequiredCardKind == CardKind.Dodge ? new[]
        {
            CardKind.Dodge
        }

        : n.Intent == WeijieIntent.GenericBasic ? new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.Peach,
            CardKind.Alcohol
        }

        : new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash
        };
        foreach (var kind in kinds)
        {
            var targets = WeijieIsDying(n) || n.Intent == WeijieIntent.WenyiPeach ? n.Targets : WeijieIsPlayed(n) || n.Intent == WeijieIntent.Dodge ? new[]
            {
                n.OpponentSeat ?? n.ActorSeat
            }

            : kind is CardKind.Peach or CardKind.Alcohol ? new[]
            {
                n.ActorSeat
            }

            : n.Targets;
            foreach (var t in targets.Distinct())
                if (WeijieNativeLegal(n, kind, t))
                    yield return (kind, t);
        }
    }

    internal bool WeijieNativeLegal(WeijieNeed n, CardKind kind, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(n.ProviderSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(n.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[n.ProviderSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        var provider = _coreMatchParticipantsPlayersAccess._players[n.ProviderSeat];
        var action = WeijieIsPlayed(n) || n.Intent == WeijieIntent.Dodge ? CardActionType.Response : CardActionType.Use;
        if (!WeijieIsPlayed(n) && !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(n.ActorSeat, kind, [])))
            return false;
        if (_coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, kind, action) || n.ProviderSeat != n.ActorSeat && _coreCardUseModulesOperations.IsCardUseForbidden(n.ProviderSeat, CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? CardKind.Slash : kind, action) || CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && action == CardActionType.Response && _coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, CardKind.Slash, action))
            return false;
        if (n.Intent == WeijieIntent.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) && _coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(actor.Seat))
            return false;
        if (WeijieIsDying(n))
            return _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.Id == n.OwnerFrameId && d.ResponderIndex == n.Cursor && d.ResponderSeat == n.ProviderSeat && d.VictimSeat == target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].Hp <= 0 && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.ActorSeat, target, Suit.None) && (kind == CardKind.Peach && _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(n.ActorSeat, target) || kind == CardKind.Alcohol && n.ActorSeat == target && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(target));
        if (n.Intent is WeijieIntent.Dodge or WeijieIntent.Duel or WeijieIntent.Group or WeijieIntent.FactionDodge or WeijieIntent.FactionSlashPlayed)
        {
            var attack = n.Intent == WeijieIntent.FactionDodge ? _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.Attack : n.Intent == WeijieIntent.Duel ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.Attack : n.Intent is WeijieIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.CurrentAttack : n.Intent == WeijieIntent.FactionSlashPlayed ? _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.ResponseAttack : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
            if (attack is null || attack.ResolutionId != n.OwnerFrameId || _characterLuMengMouOperations.IsYingboUnrespondable(attack.ResolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(attack.ResolutionId) || _characterLiuQiOlOperations.IsWenjiResponseForbidden2(attack.ResolutionId, n.ActorSeat) || _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(attack.CardUserSeat, n.ActorSeat, attack.EffectiveCardKind ?? CardKind.Slash, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.TargetSeats ?? [n.ActorSeat]))
                return false;
            if (kind == CardKind.Dodge && (_coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is not null || !_coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(provider, [], kind)))
                return false;
            return n.Intent == WeijieIntent.Dodge ? kind == CardKind.Dodge && attack.TargetSeat == n.ActorSeat && attack.SuccessfulDodgeResponses == n.Cursor : n.Intent == WeijieIntent.Duel ? CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResponderSeat == n.ActorSeat && _coreNativeCardContinuationCursorsOperations.ActiveDuel.SuccessfulSlashResponses == n.Cursor : n.Intent == WeijieIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.TargetIndex == n.TargetIndex && CoreNativeCardClassificationsRuntime.MatchesRequiredCard(kind, _coreNativeCardContinuationCursorsOperations.ActiveGroupCard.RequiredCardKind!.Value) : n.Intent == WeijieIntent.FactionDodge ? kind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat == n.ProviderSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense.CandidateIndex == n.Cursor : _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.CurrentCandidateSeat == n.ProviderSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.CandidateIndex == n.Cursor && CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.ActorSeat, target, Suit.None) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(n.ActorSeat, target, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None))
            return false;
        if (kind == CardKind.Peach)
            return n.Intent == WeijieIntent.WenyiPeach ? _coreWenyiPeachSuppliesOperations.WenyiWeijiePeachLegal(n, target) : n.Intent == WeijieIntent.GenericBasic && target == n.ActorSeat && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(n.ActorSeat);
        if (kind == CardKind.Alcohol)
            return n.Intent == WeijieIntent.GenericBasic && target == n.ActorSeat && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(n.ActorSeat) && _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || target == n.ActorSeat || !n.Targets.Contains(target))
            return false;
        if (n.Intent == WeijieIntent.BorrowedSword)
            return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == n.OwnerFrameId && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []);
        if (n.Intent == WeijieIntent.Qinglong)
            return _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == n.OwnerFrameId && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, null, false, Suit.None, 0, []);
        if (n.Intent == WeijieIntent.FactionSlashUse && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } fr)
            return fr.CurrentCandidateSeat == n.ProviderSeat && fr.CandidateIndex == n.Cursor && _coreTurnRedSlashBenefitsOperations.IsRedSlashProviderPaymentLegal(fr, kind, []) && (fr.IsBorrowedSwordUse ? _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []) : fr.IsQinglongCrescentBladeUse ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, null, false, Suit.None, 0, []) : _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []));
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.OwnerFrameId);
        var ignore = n.Intent is WeijieIntent.ProgramSlash or WeijieIntent.ProgramNearestSlash || parent is not null && (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent) || _characterWuJingOlOperations.IsWuJingHejiRequest(parent) || n.Intent == WeijieIntent.FixedSlash && parent.FixedTargetSlash?.IgnoreDistance == true);
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], new(0, kind, Suit.None, 0), effectiveKind: kind, ignoreDistance: ignore, noEffectiveRank: true, physicalCardIds: []);
    }

    internal bool HasWeijieRequiredBasic(CharacterState actor, CardKind required)
    {
        if (!WeijieSources(actor).Any() || !WeijieVictims(actor.Seat).Any())
            return false;
        var kind = _coreDyingCursorRuntimeOperations.ActiveDying?.ResponderSeat == actor.Seat ? DecisionKind.RescueDying : required == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.RespondSlash;
        var n = WeijieCurrentNeed(actor.Seat, kind, _resolutionStackOperations._resolutionStack.LastOrDefault());
        return n is not null && WeijieOutputs(n).Any(o => CoreNativeCardClassificationsRuntime.MatchesRequiredCard(o.Kind, required) || WeijieIsDying(n) && required == CardKind.Peach);
    }

    internal bool HasWeijieForcedSlash(CharacterState actor, CharacterState target, bool qinglong) => WeijieSources(actor).Any() && WeijieVictims(actor.Seat).Any() && WeijieCurrentNeed(actor.Seat, qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash, _resolutionStackOperations._resolutionStack.LastOrDefault())is { } n && n.Targets.Contains(target.Seat) && WeijieOutputs(n).Any();
    internal bool HasWeijieForcedSlash(CharacterState actor, CharacterState target, NativeOwners.BorrowedSwordHandle pending) => HasWeijieCapability && CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.ActiveAttack is null && pending.WeaponOwnerSeat == actor.Seat && pending.SlashTargetSeat == target.Seat && actor.IsAlive && target.IsAlive && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && WeijieSources(actor).Any() && WeijieVictims(actor.Seat).Any() && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, kind, Suit.None, false, null, 0, []));
    internal bool HasWeijieForcedSlash(CharacterState actor, CharacterState target, NativeOwners.QinglongCrescentBladeHandle pending) => HasWeijieCapability && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.Attack.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.Attack.SourceSeat == actor.Seat && pending.Attack.TargetSeat == target.Seat && actor.IsAlive && target.IsAlive && WeijieSources(actor).Any() && WeijieVictims(actor.Seat).Any() && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, kind, null, false, Suit.None, 0, []));
    internal bool HasWeijieProgramSlash(CharacterState actor, IEnumerable<int> targets) => WeijieSources(actor).Any() && WeijieVictims(actor.Seat).Any() && WeijieCurrentNeed(actor.Seat, DecisionKind.ProgramTrigger, _resolutionStackOperations._resolutionStack.LastOrDefault())is { } n && WeijieOutputs(n).Any(o => CoreNativeCardClassificationsRuntime.IsSlashCard(o.Kind) && targets.Contains(o.Target));
    internal bool HasWeijieGenericBasic(ProgramSkillFrame f) => f.KeBiNengPingduan is { } r && WeijieSources(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat]).Any() && WeijieVictims(r.ActorSeat).Any() && WeijieCurrentNeed(r.ActorSeat, DecisionKind.ProgramTrigger, f)is { } n && WeijieOutputs(n).Any();
    internal PendingDecision? AddWeijieRequestChoices(PendingDecision? p)
    {
        if (p is null || !HasWeijieCapability || p.Choices.Any(c => c.Parameters.ContainsKey(WeijieOpen)) || CaptureWeijieNeed(p)is not { } n || !WeijieSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]).Any() || !WeijieVictims(p.PlayerSeat).Any() || !WeijieOutputs(n).Any())
            return p;
        if (p.PromptId == default)
            p = p with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
            };
        var choices = p.Choices.ToList();
        foreach (var(source, _)in WeijieSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]))
        {
            var args = new Dictionary<string, string>
            {
                {
                    WeijieOpen,
                    "open"
                }
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            choices.Add(new(new($"weijie.{(n.RequestFrameId)}.{(source.SkillInstanceId)}"), "发动【诿解】：弃置距离1的他人一张暗手牌尝试提供基本牌", [], [], args));
        }

        return p with
        {
            Choices = Array.AsReadOnly(choices.ToArray())
        };
    }

    internal static PendingDecision WeijieNativeDecision(PendingDecision p) => CharacterLiuFengBoundaryRuntime.BoundaryLiuFengNativeDecision(RuntimeSkills.RuntimeRules_guo_tu_ol_WeijieNeeds.WeijieNativeDecision(p));
}
