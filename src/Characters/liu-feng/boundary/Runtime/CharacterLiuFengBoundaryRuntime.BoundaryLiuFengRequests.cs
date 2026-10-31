#nullable enable

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuFengBoundaryRuntime
{
    internal const string BoundaryLiuFengOpen = "boundary-liu-feng-open";
    internal IEnumerable<CardConversionSource> BoundaryLiuFengSources(int actor)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber <= 0 || BoundaryLiuFengPaidThisTurn(actor))
            yield break;
        foreach (var i in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[actor]).ProgramInstances.Where(i => i.SkillId == BoundaryLiuFengComposition.Qinling))
            yield return new(i.SkillId, BoundaryLiuFengComposition.Activation, actor, i.SkillInstanceId);
    }

    internal bool BoundaryLiuFengHasCost(int actor) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[actor])).Any(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    internal BoundaryLiuFengNeed? CaptureBoundaryLiuFengNeed(int actor, DecisionKind kind, ResolutionFrame? top)
    {
        if (top is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || top is ProgramSkillFrame { BoundaryLiuFeng: not null })
            return null;
        var owner = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        BoundaryLiuFengNeed N(BoundaryLiuFengIntent intent, int[] targets, SkillProgramEffectOp? op = null, int instruction = 0) => new(intent, owner, top.Id, _coreCardUseLifecycleOperations.LifecycleCardUse(owner)?.Action?.ActionId, actor, instruction, op, targets);
        // A provider supplies a card for somebody else's action; Qinling instead
        // makes this skill owner the real user. Pure Played requests are excluded.
        if (kind == DecisionKind.RespondSlash && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is null && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == owner && b.WeaponOwnerSeat == actor)
            return N(BoundaryLiuFengIntent.BorrowedSword, [b.SlashTargetSeat]);
        if (kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == owner && q.Attack.SourceSeat == actor && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack))
            return N(BoundaryLiuFengIntent.Qinglong, [q.Attack.TargetSeat]);
        if (kind != DecisionKind.ProgramTrigger || top is not ProgramSkillFrame p || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)is not { } effect)
            return null;
        int[]? targets = null;
        var intent = BoundaryLiuFengIntent.AssistedSlash;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(p) && _characterSunYuanOlOperations.TryGetSayingSlashRequest(p, out var sayingActor, out var sayingTargets) && sayingActor == actor)
            targets = sayingTargets.ToArray();
        else if (_characterLiYiOlOperations.IsChanshuangSlashRequest(p) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(p, out var chanshuangActor, out var chanshuangTargets) && chanshuangActor == actor)
            targets = chanshuangTargets.ToArray();
        else if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(p) && p.LvKuangLvXiang!.Actor == actor)
            targets = [p.LvKuangLvXiang.Target];
        else if (_characterWuJingOlOperations.IsWuJingHejiRequest(p) && p.OwnerSeat == actor)
            targets = [p.WuJing!.TargetSeat];
        else if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(p) && p.OwnerSeat == actor)
            targets = _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != actor).Select(t => t.Seat).ToArray();
        else if (p.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice, ActorSeat: var keActor } && keActor == actor && _characterKeBiNengOlOperations.ValidKeBiNeng(p))
        {
            targets = _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != actor).Select(t => t.Seat).ToArray();
            intent = BoundaryLiuFengIntent.GenericBasic;
        }
        else if (_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(p, effect.Op, actor))
        {
            targets = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(p, actor, effect.Op);
            intent = effect.Op switch
            {
                SkillProgramEffectOp.RequestSlashByTarget => BoundaryLiuFengIntent.ProgramSlash,
                SkillProgramEffectOp.RequestSlashByNearest => BoundaryLiuFengIntent.ProgramNearestSlash,
                SkillProgramEffectOp.RequestLegalSlashByNearest => BoundaryLiuFengIntent.NearestLegalSlash,
                SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => BoundaryLiuFengIntent.FixedSlash,
                _ => BoundaryLiuFengIntent.AssistedSlash
            };
        }

        return targets is { Length: > 0 } ? N(intent, targets, effect.Op, p.InstructionIndex) : null;
    }

    internal bool BoundaryLiuFengNeedCurrent(BoundaryLiuFengReceipt r) => r.Need is { } need && r.OriginalDecision is { } p && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == need.RequestFrameId)is { } parent && CaptureBoundaryLiuFengNeed(need.ActorSeat, p.Kind, parent) == need;
    internal bool BoundaryLiuFengRequestTarget(BoundaryLiuFengNeed n, int target)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == n.ActorSeat || !n.Targets.Contains(target) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(n.ActorSeat, CardKind.Slash, [])) || _coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, CardKind.Slash, CardActionType.Use) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.ActorSeat, target, Suit.None) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(n.ActorSeat, target, CardKind.Slash) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        if (n.Intent == BoundaryLiuFengIntent.BorrowedSword)
            return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == n.OwnerFrameId && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, false, null, 0, []);
        if (n.Intent == BoundaryLiuFengIntent.Qinglong)
            return _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == n.OwnerFrameId && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, null, false, Suit.None, 0, []);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.OwnerFrameId);
        var ignore = n.Intent is BoundaryLiuFengIntent.ProgramSlash or BoundaryLiuFengIntent.ProgramNearestSlash || parent is not null && (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent) || _characterWuJingOlOperations.IsWuJingHejiRequest(parent) || n.Intent == BoundaryLiuFengIntent.FixedSlash && parent.FixedTargetSlash?.IgnoreDistance == true);
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], new(0, CardKind.Slash, Suit.None, 0), effectiveKind: CardKind.Slash, ignoreDistance: ignore, noEffectiveRank: true, physicalCardIds: []);
    }

    internal PendingDecision? AppendBoundaryLiuFengQinlingChoices(PendingDecision? p)
    {
        if (p is null || p.Choices.Any(c => c.Parameters.ContainsKey(BoundaryLiuFengOpen)))
            return p;
        var sources = BoundaryLiuFengSources(p.PlayerSeat).ToArray();
        if (sources.Length == 0 || !BoundaryLiuFengHasCost(p.PlayerSeat) || CaptureBoundaryLiuFengNeed(p.PlayerSeat, p.Kind, _resolutionStackOperations._resolutionStack.LastOrDefault())is not { } n || !n.Targets.Any(t => BoundaryLiuFengRequestTarget(n, t)))
            return p;
        var choices = p.Choices.ToList();
        foreach (var source in sources)
        {
            var args = new Dictionary<string, string>
            {
                [BoundaryLiuFengOpen] = "open"
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            choices.Add(new(new($"boundary-liu-feng.open.{(n.RequestFrameId)}.{(source.SkillInstanceId)}"), "发动侵凌：弃置一张牌，视为使用杀", [], [], args));
        }

        return p with
        {
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray())
        };
    }

    internal static PendingDecision BoundaryLiuFengNativeDecision(PendingDecision p) => !p.Choices.Any(c => c.Parameters.ContainsKey(BoundaryLiuFengOpen)) ? p : p with
    {
        Choices = Array.AsReadOnly(p.Choices.Where(c => !c.Parameters.ContainsKey(BoundaryLiuFengOpen)).ToArray())
    };
    internal static string BoundaryLiuFengDecisionHash(PendingDecision p) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(RequestedDeckBasicFrame.FreezeDecision(p)))));
    internal bool IsBoundaryLiuFengQinlingAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && c.Parameters.ContainsKey(BoundaryLiuFengOpen)) == true;
    internal CommandResult SubmitBoundaryLiuFengQinlingAnswer(int actor, PromptId prompt, ChoiceId id)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != actor || p.PromptId != prompt)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidPrompt, "侵凌必须回答当前真实使用请求。");
        var selected = p.Choices.SingleOrDefault(c => c.Id == id && c.Parameters.ContainsKey(BoundaryLiuFengOpen));
        if (selected is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidChoice, "侵凌选择未公布。");
        return _kernelCoreMainOperations2.Accept(() =>
        {
            OpenBoundaryLiuFengRequest(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void OpenBoundaryLiuFengRequest(PromptChoice selected)
    {
        var p = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("Qinling lost its actual native request.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var n = CaptureBoundaryLiuFengNeed(p.PlayerSeat, p.Kind, _resolutionStackOperations._resolutionStack.LastOrDefault()) ?? throw new InvalidOperationException("Qinling cannot replace a pure Played request.");
        if (!p.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || !BoundaryLiuFengSources(p.PlayerSeat).Contains(source) || !BoundaryLiuFengHasCost(p.PlayerSeat) || !n.Targets.Any(t => BoundaryLiuFengRequestTarget(n, t)))
            throw new InvalidOperationException("Qinling lost its original live actor, source, allowance or target.");
        var hash = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash;
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, p.PlayerSeat, source.SkillId, source.BindingId, hash, 0, [], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            BoundaryLiuFeng = new(source, hash, SkillProgramEffectOp.ResolveBoundaryLiuFengQinling, n.OwnerFrameId, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, BoundaryLiuFengStage.Targets)
            {
                Need = n,
                OriginalDecision = p
            }
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengStartedEvent(f.Id, source, hash, SkillProgramEffectOp.ResolveBoundaryLiuFengQinling, n.OwnerFrameId, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengRequestStartedEvent(f.Id, n.OwnerFrameId, n.RequestFrameId, n.ActorSeat, n.InstructionIndex, n.RequestOp, n.Intent, p.PromptId, p.Revision, BoundaryLiuFengDecisionHash(p)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(f.Id, host, host);
    }

    internal bool HasBoundaryLiuFengProgramSlash(CharacterState actor, IEnumerable<int> targets) => BoundaryLiuFengSources(actor.Seat).Any() && BoundaryLiuFengHasCost(actor.Seat) && CaptureBoundaryLiuFengNeed(actor.Seat, DecisionKind.ProgramTrigger, _resolutionStackOperations._resolutionStack.LastOrDefault())is { } n && n.Targets.Any(t => targets.Contains(t) && BoundaryLiuFengRequestTarget(n, t));
    internal bool HasBoundaryLiuFengGenericBasic(ProgramSkillFrame f) => f.KeBiNengPingduan is { } r && HasBoundaryLiuFengProgramSlash(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.ActorSeat).Select(p => p.Seat));
    internal bool HasBoundaryLiuFengForcedSlash(CharacterState actor, CharacterState target, NativeOwners.BorrowedSwordHandle pending) => CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == pending.ResolutionId && pending.ActiveAttack is null && pending.WeaponOwnerSeat == actor.Seat && pending.SlashTargetSeat == target.Seat && BoundaryLiuFengSources(actor.Seat).Any() && BoundaryLiuFengHasCost(actor.Seat) && actor.IsAlive && target.IsAlive && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Slash, CardActionType.Use) && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, CardKind.Slash, Suit.None, false, null, 0, []);
    internal bool HasBoundaryLiuFengForcedSlash(CharacterState actor, CharacterState target, NativeOwners.QinglongCrescentBladeHandle pending) => CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == pending.Attack.ResolutionId && pending.Attack.SourceSeat == actor.Seat && pending.Attack.TargetSeat == target.Seat && BoundaryLiuFengSources(actor.Seat).Any() && BoundaryLiuFengHasCost(actor.Seat) && actor.IsAlive && target.IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Slash, CardActionType.Use) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, CardKind.Slash, null, false, Suit.None, 0, []);
    internal bool TryAdvanceBoundaryLiuFengAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].IsHuman)
            return false;
        if (p.Choices.FirstOrDefault(c => c.Parameters.ContainsKey(BoundaryLiuFengOpen))is not { } selected)
            return false;
        OpenBoundaryLiuFengRequest(selected);
        return true;
    }
}
