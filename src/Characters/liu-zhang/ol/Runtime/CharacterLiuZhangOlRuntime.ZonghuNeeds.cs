#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal const string ZonghuOpen = RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuOpen, ZonghuChoice = "zonghu-choice", ZonghuGiftReason = "skill-program.liu-zhang.zonghu.give";
    internal bool HasZonghuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveZonghuRequiredUseRequest);

    internal static CardConversionSource ZonghuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuSource(f);
    internal bool ZonghuTurnOpen(int seat) => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal bool ZonghuSpent(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuCostPaidEvent>().Any(e => e.Source.OwnerSeat == seat && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal IEnumerable<(CardConversionSource Source, string Hash)> ZonghuSources(CharacterState actor)
    {
        if (!HasZonghuCapability || !actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !ZonghuTurnOpen(actor.Seat) || ZonghuSpent(actor.Seat))
            yield break;
        foreach (var i in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var a in i.Program.Activations.Where(a => a.Effects is [{ Op: SkillProgramEffectOp.ResolveZonghuRequiredUseRequest }]))
                yield return (new(i.SkillId, a.Id, actor.Seat, i.SkillInstanceId), i.Program.GameplayHash);
    }

    internal int ZonghuCost => Math.Min(3, Math.Max(1, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber));

    internal IReadOnlyList<Card> ZonghuGiftCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).OrderBy(c => c.Id).ToArray();
    internal IEnumerable<int> ZonghuVictims(int actor) => ZonghuGiftCards(actor).Count >= ZonghuCost ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor).Select(p => p.Seat) : [];
    internal bool ZonghuExcludedSource(ResolutionFrame? top) => top is ProgramSkillFrame f && f.SkillId is "classic:zhongyong" or "classic:luanwu" or "boundary:luanwu" or "ol:qiaowu" or "ol:nisiwohuo";
    internal ZonghuNeed ZonghuPlayNeed(int seat) => new(ZonghuIntent.Play, 0, 0, null, seat, seat, null, null, 0, 0, 0, 0, null, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != seat).Select(p => p.Seat).ToArray());
    internal ZonghuNeed? CaptureZonghuNeed(PendingDecision original, ResolutionFrame? suppliedNativeTop = null) => ZonghuCurrentNeed(original.PlayerSeat, original.Kind, suppliedNativeTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault());
    internal ZonghuNeed? ZonghuCurrentNeed(int provider, DecisionKind kind, ResolutionFrame? top)
    {
        if (!HasZonghuCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(provider) || top is ProgramSkillFrame { Zonghu: not null } || ZonghuExcludedSource(top))
            return null;
        if (kind == DecisionKind.PlayCard && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat == provider && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && (_resolutionStackOperations._resolutionStack.Count == 0 || top is null))
            return ZonghuPlayNeed(provider);
        if (top is null || kind == DecisionKind.RescueDying || kind == DecisionKind.RespondDodge && (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat == provider || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.CurrentAttack?.TargetSeat == provider) || kind == DecisionKind.RespondSlash && (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.CurrentCandidateSeat == provider || _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResponderSeat == provider || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.CurrentAttack?.TargetSeat == provider))
            return null;
        var owner = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        long? action = _coreCardUseLifecycleOperations.LifecycleCardUse(owner)?.Action?.ActionId;
        ZonghuNeed N(ZonghuIntent intent, int actor, IReadOnlyList<int> targets, int cursor = 0, int? requester = null, int? opponent = null, int index = 0, int round = 0, SkillProgramEffectOp? op = null, int instruction = 0, long? ownerId = null) => new(intent, ownerId ?? owner, top.Id, action, provider, actor, requester, opponent, cursor, index, round, instruction, op, targets);
        if (kind == DecisionKind.RescueDying && top is DyingFrame d && d.ResponderSeat == provider)
            return N(ZonghuIntent.Dying, provider, [d.VictimSeat], d.ResponderIndex, ownerId: d.Id);
        if (kind == DecisionKind.RespondDodge)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } fd && fd.CurrentCandidateSeat == provider && CoreNativeDamageHandlesRuntime.SameAttackOwner(fd.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack))
                return N(ZonghuIntent.FactionDodge, fd.OwnerSeat, [], fd.CandidateIndex, fd.OwnerSeat, fd.Attack.SourceSeat, ownerId: fd.Attack.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } ga } g && ga.TargetSeat == provider && g.RequiredCardKind == CardKind.Dodge)
                return N(ZonghuIntent.Group, provider, [], ga.SuccessfulDodgeResponses, opponent: g.SourceSeat, index: g.TargetIndex, ownerId: g.ResolutionId);
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.TargetSeat == provider && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveCardKind ?? CardKind.Dodge))
                return N(ZonghuIntent.Dodge, provider, [], a.SuccessfulDodgeResponses, opponent: a.SourceSeat, ownerId: a.ResolutionId);
        }

        if (kind == DecisionKind.RespondSlash)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } f && f.CurrentCandidateSeat == provider)
            {
                var played = !CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(f);
                var opponent = f.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.OpponentSeat : f.ResponseAttack?.SourceSeat;
                var program = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == f.ProgramSkillFrameId);
                return N(played ? ZonghuIntent.FactionSlashPlayed : ZonghuIntent.FactionSlashUse, f.OwnerSeat, f.TargetSeat is { } t ? [t] : [], f.CandidateIndex, f.OwnerSeat, opponent, op: program is null ? null : _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(program)?.Op, instruction: program?.InstructionIndex ?? 0, ownerId: f.OwnerFrameId);
            }

            if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.WeaponOwnerSeat == provider)
                return N(ZonghuIntent.BorrowedSword, provider, [b.SlashTargetSeat], opponent: b.SourceSeat, ownerId: b.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && duel.ResponderSeat == provider)
                return N(ZonghuIntent.Duel, provider, [], duel.SuccessfulSlashResponses, opponent: duel.OpponentSeat, ownerId: duel.ResolutionId);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } ga } g && ga.TargetSeat == provider && g.RequiredCardKind == CardKind.Slash)
                return N(ZonghuIntent.Group, provider, [], opponent: g.SourceSeat, index: g.TargetIndex, ownerId: g.ResolutionId);
        }

        if (kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.SourceSeat == provider && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack))
            return N(ZonghuIntent.Qinglong, provider, [q.Attack.TargetSeat], opponent: q.Attack.TargetSeat, ownerId: q.Attack.ResolutionId);
        if (kind != DecisionKind.ProgramTrigger || top is not ProgramSkillFrame p || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)is not { } effect)
            return null;
        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(p) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(p, out var requestedActor, out var requestedTargets) && provider == requestedActor)
            return N(ZonghuIntent.AssistedSlash, provider, requestedTargets, op: effect.Op, instruction: p.InstructionIndex);
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(p) && p.LvKuangLvXiang!.Actor == provider)
            return N(ZonghuIntent.AssistedSlash, provider, [p.LvKuangLvXiang.Target], op: effect.Op, instruction: p.InstructionIndex);
        if (_characterWuJingOlOperations.IsWuJingHejiRequest(p) && p.OwnerSeat == provider)
            return N(ZonghuIntent.AssistedSlash, provider, [p.WuJing!.TargetSeat], op: effect.Op, instruction: p.InstructionIndex);
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(p) && p.OwnerSeat == provider)
            return N(ZonghuIntent.AssistedSlash, provider, _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != provider).Select(t => t.Seat).ToArray(), op: effect.Op, instruction: p.InstructionIndex);
        if (p.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice, ActorSeat: var actor } && actor == provider && _characterKeBiNengOlOperations.ValidKeBiNeng(p))
            return N(ZonghuIntent.GenericBasic, provider, _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive).Select(t => t.Seat).ToArray(), op: effect.Op, instruction: p.InstructionIndex);
        if (!_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(p, effect.Op, provider))
            return null;
        var targets = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(p, provider, effect.Op);
        if (targets.Length == 0)
            return null;
        return N(effect.Op switch
        {
            SkillProgramEffectOp.RequestSlashByTarget => ZonghuIntent.ProgramSlash,
            SkillProgramEffectOp.RequestSlashByNearest => ZonghuIntent.ProgramNearestSlash,
            SkillProgramEffectOp.RequestLegalSlashByNearest => ZonghuIntent.NearestLegalSlash,
            SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => ZonghuIntent.FixedSlash,
            _ => ZonghuIntent.AssistedSlash
        }, provider, targets, op: effect.Op, instruction: p.InstructionIndex);
    }

    internal static bool ZonghuIsDying(ZonghuNeed n) => RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuIsDying(n);
    internal static bool ZonghuIsPlayed(ZonghuNeed n) => RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuIsPlayed(n);
    internal IEnumerable<(CardKind Kind, int Target)> ZonghuOutputs(ZonghuNeed n)
    {
        if (n.ProviderSeat != n.ActorSeat || n.RequesterSeat is not null || n.Intent is ZonghuIntent.Dying or ZonghuIntent.Group or ZonghuIntent.Duel or ZonghuIntent.FactionDodge or ZonghuIntent.FactionSlashPlayed or ZonghuIntent.FactionSlashUse)
            yield break;
        if (n.Intent == ZonghuIntent.Dodge)
        {
            if (ZonghuNativeLegal(n, CardKind.Dodge, n.OpponentSeat ?? n.ActorSeat))
                yield return (CardKind.Dodge, n.OpponentSeat ?? n.ActorSeat);
            yield break;
        }

        foreach (var target in n.Targets.Distinct())
            if (ZonghuNativeLegal(n, CardKind.Slash, target))
                yield return (CardKind.Slash, target);
    }

    internal bool ZonghuNativeLegal(ZonghuNeed n, CardKind kind, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(n.ProviderSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(n.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[n.ProviderSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (n.ProviderSeat != n.ActorSeat || kind is not (CardKind.Slash or CardKind.Dodge))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        var provider = _coreMatchParticipantsPlayersAccess._players[n.ProviderSeat];
        var action = ZonghuIsPlayed(n) || n.Intent == ZonghuIntent.Dodge ? CardActionType.Response : CardActionType.Use;
        if (_coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, kind, action) || n.ProviderSeat != n.ActorSeat && _coreCardUseModulesOperations.IsCardUseForbidden(n.ProviderSeat, CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? CardKind.Slash : kind, action) || CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && action == CardActionType.Response && _coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, CardKind.Slash, action))
            return false;
        if (n.Intent == ZonghuIntent.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) && _coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(actor.Seat))
            return false;
        if (ZonghuIsDying(n))
            return _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.Id == n.OwnerFrameId && d.ResponderIndex == n.Cursor && d.ResponderSeat == n.ProviderSeat && d.VictimSeat == target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].Hp <= 0 && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.ActorSeat, target, Suit.None) && (kind == CardKind.Peach && _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(n.ActorSeat, target) || kind == CardKind.Alcohol && n.ActorSeat == target && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(target));
        if (n.Intent is ZonghuIntent.Dodge or ZonghuIntent.Duel or ZonghuIntent.Group or ZonghuIntent.FactionDodge or ZonghuIntent.FactionSlashPlayed)
        {
            var attack = n.Intent == ZonghuIntent.FactionDodge ? _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.Attack : n.Intent == ZonghuIntent.Duel ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.Attack : n.Intent is ZonghuIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.CurrentAttack : n.Intent == ZonghuIntent.FactionSlashPlayed ? _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.ResponseAttack : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
            if (attack is null || attack.ResolutionId != n.OwnerFrameId || _characterLuMengMouOperations.IsYingboUnrespondable(attack.ResolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(attack.ResolutionId) || _characterLiuQiOlOperations.IsWenjiResponseForbidden2(attack.ResolutionId, n.ActorSeat) || _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(attack.CardUserSeat, n.ActorSeat, attack.EffectiveCardKind ?? CardKind.Slash, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.TargetSeats ?? [n.ActorSeat]))
                return false;
            if (kind == CardKind.Dodge && (_coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is not null || !_coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(provider, [], kind)))
                return false;
            return n.Intent == ZonghuIntent.Dodge ? kind == CardKind.Dodge && attack.TargetSeat == n.ActorSeat && attack.SuccessfulDodgeResponses == n.Cursor : n.Intent == ZonghuIntent.Duel ? CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResponderSeat == n.ActorSeat && _coreNativeCardContinuationCursorsOperations.ActiveDuel.SuccessfulSlashResponses == n.Cursor : n.Intent == ZonghuIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.TargetIndex == n.TargetIndex && CoreNativeCardClassificationsRuntime.MatchesRequiredCard(kind, _coreNativeCardContinuationCursorsOperations.ActiveGroupCard.RequiredCardKind!.Value) : n.Intent == ZonghuIntent.FactionDodge ? kind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat == n.ProviderSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense.CandidateIndex == n.Cursor : _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.CurrentCandidateSeat == n.ProviderSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.CandidateIndex == n.Cursor && CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.ActorSeat, target, Suit.None) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(n.ActorSeat, target, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None))
            return false;
        if (kind == CardKind.Peach)
            return n.Intent == ZonghuIntent.GenericBasic && target == n.ActorSeat && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(n.ActorSeat);
        if (kind == CardKind.Alcohol)
            return n.Intent == ZonghuIntent.GenericBasic && target == n.ActorSeat && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(n.ActorSeat) && _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || target == n.ActorSeat || !n.Targets.Contains(target))
            return false;
        if (n.Intent == ZonghuIntent.BorrowedSword)
            return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == n.OwnerFrameId && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []);
        if (n.Intent == ZonghuIntent.Qinglong)
            return _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == n.OwnerFrameId && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, null, false, Suit.None, 0, []);
        if (n.Intent == ZonghuIntent.FactionSlashUse && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } fr)
            return fr.CurrentCandidateSeat == n.ProviderSeat && fr.CandidateIndex == n.Cursor && _coreTurnRedSlashBenefitsOperations.IsRedSlashProviderPaymentLegal(fr, kind, []) && (fr.IsBorrowedSwordUse ? _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []) : fr.IsQinglongCrescentBladeUse ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, null, false, Suit.None, 0, []) : _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None, false, null, 0, []));
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.OwnerFrameId);
        var ignore = false;
        return _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[target], false, kind, physicalCardIds: []) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], new(0, kind, Suit.None, 0), effectiveKind: kind, ignoreDistance: ignore, noEffectiveRank: true, physicalCardIds: []);
    }

    internal bool HasZonghuRequiredBasic(CharacterState actor, CardKind required)
    {
        if (!ZonghuSources(actor).Any() || !ZonghuVictims(actor.Seat).Any())
            return false;
        var kind = _coreDyingCursorRuntimeOperations.ActiveDying?.ResponderSeat == actor.Seat ? DecisionKind.RescueDying : required == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.RespondSlash;
        var n = ZonghuCurrentNeed(actor.Seat, kind, _resolutionStackOperations._resolutionStack.LastOrDefault());
        return n is not null && ZonghuOutputs(n).Any(o => CoreNativeCardClassificationsRuntime.MatchesRequiredCard(o.Kind, required) || ZonghuIsDying(n) && required == CardKind.Peach);
    }

    internal bool HasZonghuForcedSlash(CharacterState actor, CharacterState target, bool qinglong) => ZonghuSources(actor).Any() && ZonghuVictims(actor.Seat).Any() && ZonghuCurrentNeed(actor.Seat, qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash, _resolutionStackOperations._resolutionStack.LastOrDefault())is { } n && n.Targets.Contains(target.Seat) && ZonghuOutputs(n).Any();
    internal bool HasZonghuForcedSlash(CharacterState actor, CharacterState target, NativeOwners.BorrowedSwordHandle pending) => HasZonghuCapability && CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.ActiveAttack is null && pending.WeaponOwnerSeat == actor.Seat && pending.SlashTargetSeat == target.Seat && actor.IsAlive && target.IsAlive && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && ZonghuSources(actor).Any() && ZonghuVictims(actor.Seat).Any() && new[]
    {
        CardKind.Slash
    }.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, kind, Suit.None, false, null, 0, []));
    internal bool HasZonghuForcedSlash(CharacterState actor, CharacterState target, NativeOwners.QinglongCrescentBladeHandle pending) => HasZonghuCapability && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.Attack.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.Attack.SourceSeat == actor.Seat && pending.Attack.TargetSeat == target.Seat && actor.IsAlive && target.IsAlive && ZonghuSources(actor).Any() && ZonghuVictims(actor.Seat).Any() && new[]
    {
        CardKind.Slash
    }.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, kind, null, false, Suit.None, 0, []));
    internal bool HasZonghuProgramSlash(CharacterState actor, IEnumerable<int> targets) => ZonghuSources(actor).Any() && ZonghuVictims(actor.Seat).Any() && ZonghuCurrentNeed(actor.Seat, DecisionKind.ProgramTrigger, _resolutionStackOperations._resolutionStack.LastOrDefault())is { } n && ZonghuOutputs(n).Any(o => CoreNativeCardClassificationsRuntime.IsSlashCard(o.Kind) && targets.Contains(o.Target));
    internal bool HasZonghuGenericBasic(ProgramSkillFrame f) => f.KeBiNengPingduan is { } r && ZonghuSources(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat]).Any() && ZonghuVictims(r.ActorSeat).Any() && ZonghuCurrentNeed(r.ActorSeat, DecisionKind.ProgramTrigger, f)is { } n && ZonghuOutputs(n).Any();
    internal PendingDecision? AddZonghuRequestChoices(PendingDecision? p)
    {
        if (p is null || !HasZonghuCapability || p.Choices.Any(c => c.Parameters.ContainsKey(ZonghuOpen)) || CaptureZonghuNeed(p)is not { } n || !ZonghuSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]).Any() || !ZonghuVictims(p.PlayerSeat).Any() || !ZonghuOutputs(n).Any())
            return p;
        if (p.PromptId == default)
            p = p with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
            };
        var choices = p.Choices.ToList();
        foreach (var(source, _)in ZonghuSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]))
        {
            var args = new Dictionary<string, string>
            {
                {
                    ZonghuOpen,
                    "open"
                }
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            choices.Add(new(new($"zonghu.{(n.RequestFrameId)}.{(source.SkillInstanceId)}"), "发动【宗护】：交给他人牌，视为本人使用杀或闪", [], [], args));
        }

        return p with
        {
            Choices = Array.AsReadOnly(choices.ToArray())
        };
    }

    internal static PendingDecision ZonghuNativeDecision(PendingDecision p) => RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuNativeDecision(p);
}
