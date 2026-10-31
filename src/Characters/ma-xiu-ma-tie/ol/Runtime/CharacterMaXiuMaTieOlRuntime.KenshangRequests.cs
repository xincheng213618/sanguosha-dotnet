#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaXiuMaTieOlRuntime
{
    internal const string KenshangRequestOpen = "kenshang-request-open", KenshangRequestPick = "kenshang-request-pick";
    internal IEnumerable<CardConversionSource> KenshangSources(CharacterState actor) => _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.SelectMany(i => i.Program.ViewAs.Where(r => r.KenshangUse == true && r.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor))).Select(r => new CardConversionSource(i.SkillId, r.Id, actor.Seat, i.SkillInstanceId)));
    internal IReadOnlyList<Card> KenshangRequestCards(CardConversionSource source)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) || _corePlayCardHistoryOperations.ViewAsRule(source)is not { KenshangUse: true } r || _coreCardUseModulesOperations.IsCardUseForbidden(source.OwnerSeat, CardKind.Slash, CardActionType.Use))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        return r.SourceZones.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, actor.Seat))).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [c.Id])).DistinctBy(c => c.Id).OrderBy(c => c.Id).ToArray();
    }

    internal IReadOnlyList<PromptChoice> KenshangProgramSlashChoices(ProgramSkillFrame f, int actorSeat, IReadOnlyList<int> targets, string? resultBind = null)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect || !KenshangProgramActor(f, effect.Op, actorSeat))
            return[];
        var exact = KenshangProgramTargets(f, actorSeat, effect.Op);
        var result = new List<PromptChoice>();
        foreach (var source in KenshangSources(_coreMatchParticipantsPlayersAccess._players[actorSeat]).Where(s => KenshangRequestCards(s).Count >= 2))
            foreach (var target in targets.Where(exact.Contains).Distinct())
            {
                if (target == actorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    continue;
                var p = new Dictionary<string, string>
                {
                    {
                        "program-action",
                        KenshangRequestOpen
                    },
                    {
                        "frame-id",
                        f.Id.ToString(CultureInfo.InvariantCulture)
                    },
                    {
                        "actor-seat",
                        actorSeat.ToString(CultureInfo.InvariantCulture)
                    },
                    {
                        "target-seat",
                        target.ToString(CultureInfo.InvariantCulture)
                    }
                };
                global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
                result.Add(new(new($"kenshang.request.{(f.Id)}.{(source.SkillInstanceId)}.{(source.BindingId)}.{(actorSeat)}.{(target)}"), "发动【垦伤】：私下选择至少两张手牌或装备牌当【杀】使用", [], [target], p));
            }

        return Array.AsReadOnly(result.ToArray());
    }

    internal bool HasKenshangProgramSlash(CharacterState actor, IEnumerable<int> targets) => actor.IsAlive && targets.Any(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != actor.Seat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive) && KenshangSources(actor).Any(s => KenshangRequestCards(s).Count >= 2);
    internal bool KenshangProgramActor(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => op == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment ? f.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice } r && r.ActorSeat == actor && _characterKeBiNengOlOperations.ValidKeBiNeng(f) : _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, op, actor);
    internal int[] KenshangProgramTargets(ProgramSkillFrame f, int actor, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment && KenshangProgramActor(f, op, actor) ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor).Select(p => p.Seat).ToArray() : _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(f, actor, op);
    internal IReadOnlyList<PromptChoice> KenshangBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(pending.WeaponOwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat].IsAlive)
            return[];
        return Array.AsReadOnly(KenshangSources(_coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat]).Where(s => KenshangRequestCards(s).Count >= 2).Select(s =>
        {
            var p = new Dictionary<string, string>
            {
                {
                    "program-action",
                    KenshangRequestOpen
                },
                {
                    "response",
                    KenshangRequestOpen
                },
                {
                    "frame-id",
                    pending.ResolutionId.ToString(CultureInfo.InvariantCulture)
                },
                {
                    "actor-seat",
                    pending.WeaponOwnerSeat.ToString(CultureInfo.InvariantCulture)
                },
                {
                    "target-seat",
                    pending.SlashTargetSeat.ToString(CultureInfo.InvariantCulture)
                }
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, s);
            return new PromptChoice(new($"kenshang.borrowed.{(pending.ResolutionId)}.{(s.SkillInstanceId)}.{(s.BindingId)}"), "发动【垦伤】：将至少两张手牌或装备牌当【杀】使用", [], [pending.SlashTargetSeat], p);
        }).ToArray());
    }

    internal bool TryBeginKenshangBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("response") != KenshangRequestOpen)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } original || !pending.AwaitingSlashChoice || original.PlayerSeat != pending.WeaponOwnerSeat || !KenshangBorrowedSlashChoices(pending).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is not { KenshangRequest: null } parent || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ResponseWindowFrame response || response.ParentFrameId != parent.Id)
            throw new InvalidOperationException("Kenshang lost its original Borrowed Sword Slash-use request.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new KenshangRequestReceipt(parent.Id, 0, null, pending.WeaponOwnerSeat, pending.SlashTargetSeat, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent.Id, u => u with { KenshangRequest = r });
        BeginKenshangRequest(r);
        return true;
    }

    internal IReadOnlyList<PromptChoice> KenshangQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending)
    {
        var attack = pending.Attack;
        var actor = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || !_coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target))
            return[];
        return Array.AsReadOnly(KenshangSources(actor).Where(s => KenshangRequestCards(s).Count >= 2).Select(s =>
        {
            var p = new Dictionary<string, string>
            {
                {
                    "program-action",
                    KenshangRequestOpen
                },
                {
                    "action",
                    KenshangRequestOpen
                },
                {
                    "frame-id",
                    attack.ResolutionId.ToString(CultureInfo.InvariantCulture)
                },
                {
                    "actor-seat",
                    actor.Seat.ToString(CultureInfo.InvariantCulture)
                },
                {
                    "target-seat",
                    target.Seat.ToString(CultureInfo.InvariantCulture)
                }
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, s);
            return new PromptChoice(new($"kenshang.qinglong.{(attack.ResolutionId)}.{(s.SkillInstanceId)}.{(s.BindingId)}"), "发动【垦伤】：选择至少两张牌追杀", [], [target.Seat], p);
        }).ToArray());
    }

    internal bool TryBeginKenshangQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("action") != KenshangRequestOpen)
            return false;
        var attack = pending.Attack;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } original || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, pending) || original.PlayerSeat != attack.SourceSeat || !KenshangQinglongSlashChoices(pending).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { KenshangRequest: null } parent || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id)
            throw new InvalidOperationException("Kenshang lost its exact native Qinglong Slash-use request.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new KenshangRequestReceipt(parent.Id, 0, null, attack.SourceSeat, attack.TargetSeat, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent.Id, u => u with { KenshangRequest = r });
        BeginKenshangRequest(r);
        return true;
    }

    [ProgramAnswer(KenshangRequestOpen)]
    internal void OpenKenshangProgramRequest(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KenshangRequest: null } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } original || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect || selected.Targets is not [var target] || !KenshangProgramSlashChoices(f, original.PlayerSeat, KenshangProgramTargets(f, original.PlayerSeat, effect.Op)).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || !original.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Kenshang lost its exact program-request actor, instruction or legal published frontier.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new KenshangRequestReceipt(f.Id, f.InstructionIndex, effect.Op, original.PlayerSeat, target, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original)
        {
            RequestSource = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            RequestGameplayHash = f.GameplayHash
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { KenshangRequest = r });
        BeginKenshangRequest(r);
    }

    internal void BeginKenshangRequest(KenshangRequestReceipt r)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangRequestStartedEvent(r.OwnerFrameId, r.InstructionIndex, r.RequestOp, r.ActorSeat, r.TargetSeat, r.Source, r.GameplayHash, r.OriginalDecision.PromptId, r.OriginalDecision.Revision, KenshangOriginalDecisionHash(r.OriginalDecision)));
        PublishKenshangRequest(r);
    }

    internal static string KenshangOriginalDecisionHash(PendingDecision decision) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ma_xiu_ma_tie_ol_KenshangRequests.KenshangOriginalDecisionHash(decision);
    internal KenshangRequestReceipt? CurrentKenshangRequest() => _resolutionStackOperations._resolutionStack.LastOrDefault() switch
    {
        ProgramSkillFrame { KenshangRequest: { } r } => r,
        CardUseFrame { KenshangRequest: { } r } => r,
        ResponseWindowFrame response when _coreCardUseLifecycleOperations.LifecycleCardUse(response.ParentFrameId)is { KenshangRequest: { } r } => r,
        _ => null
    };
    internal bool ValidKenshangRequest(KenshangRequestReceipt r)
    {
        if (r.OwnerFrameId <= 0 || r.Source.OwnerSeat != r.ActorSeat || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash || _corePlayCardHistoryOperations.ViewAsRule(r.Source)is not { KenshangUse: true } || r.OriginalDecision.PlayerSeat != r.ActorSeat || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => !KenshangRequestCards(r.Source).Any(c => c.Id == id)) || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRequestStartedEvent>().Count(e => e.OwnerFrameId == r.OwnerFrameId && e.InstructionIndex == r.InstructionIndex && e.RequestOp == r.RequestOp && e.ActorSeat == r.ActorSeat && e.TargetSeat == r.TargetSeat && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.OriginalPromptId == r.OriginalDecision.PromptId && e.OriginalRevision == r.OriginalDecision.Revision && e.OriginalDecisionHash == KenshangOriginalDecisionHash(r.OriginalDecision)) != 1 || !r.OriginalDecision.ValidTargetSeats.Contains(r.TargetSeat) || !r.OriginalDecision.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == KenshangRequestOpen && c.Parameters.GetValueOrDefault("frame-id") == r.OwnerFrameId.ToString(CultureInfo.InvariantCulture) && c.Parameters.GetValueOrDefault("actor-seat") == r.ActorSeat.ToString(CultureInfo.InvariantCulture) && c.Targets.SequenceEqual([r.TargetSeat]) && global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(c.Parameters, out var s) && s == r.Source))
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRequestMaterialSelectedEvent>().Where(e => e.OwnerFrameId == r.OwnerFrameId && e.OriginalPromptId == r.OriginalDecision.PromptId).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Selected.Count || !selected.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, selected.Length)) || !selected.Select(e => e.CardId).SequenceEqual(r.Selected) || selected.Any(e => e.ActorSeat != r.ActorSeat || e.From != _cardZonesOperations._cardZones.GetLocation(e.CardId)))
            return false;
        if (r.RequestOp is { } op)
            return r.OriginalDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { KenshangRequest: { } stored } f && stored == r && f.Id == r.OwnerFrameId && f.InstructionIndex == r.InstructionIndex && f.GameplayHash == r.RequestGameplayHash && r.OriginalDecision.SourceSeat == f.OwnerSeat && r.OriginalDecision.TargetSeat == r.ActorSeat && r.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && r.RequestSource == new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == op && KenshangProgramActor(f, op, r.ActorSeat) && KenshangProgramTargets(f, r.ActorSeat, op).Contains(r.TargetSeat);
        if (r.OriginalDecision.Kind == DecisionKind.QinglongCrescentBlade)
            return r.OriginalDecision.IsPrivate && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack) && q.Attack.ResolutionId == r.OwnerFrameId && q.Attack.SourceSeat == r.ActorSeat && q.Attack.TargetSeat == r.TargetSeat && r.OriginalDecision.SourceSeat == r.ActorSeat && r.OriginalDecision.TargetSeat == r.TargetSeat && r.OriginalDecision.IncomingCard == q.Attack.EffectiveCardKind && r.OriginalDecision.SkillPrompt is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { KenshangRequest: { } native } u && u.Id == r.OwnerFrameId && native == r;
        return r.OriginalDecision is { Kind: DecisionKind.RespondSlash, RequiredCardKind: CardKind.Slash, IncomingCard: CardKind.BorrowedSword } && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } borrowed && borrowed.ResolutionId == r.OwnerFrameId && borrowed.WeaponOwnerSeat == r.ActorSeat && borrowed.SlashTargetSeat == r.TargetSeat && r.OriginalDecision.SourceSeat == borrowed.SourceSeat && r.OriginalDecision.TargetSeat == r.TargetSeat && r.OriginalDecision.SkillPrompt is null && _coreCardUseLifecycleOperations.LifecycleCardUse(r.OwnerFrameId)?.KenshangRequest == r && _resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == r.OwnerFrameId;
    }

    internal bool KenshangSelectedRequestLegal(KenshangRequestReceipt r)
    {
        if (r.Selected.Count < 2 || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], r.Selected, CardKind.Slash, false, r.Source)is not { } selection)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var cards = selection.Cards;
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards);
        if (r.RequestOp is null)
            return r.OriginalDecision.Kind == DecisionKind.QinglongCrescentBlade ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, CardKind.Slash, color, false, suit, physicalCardIds: r.Selected) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, CardKind.Slash, suit, false, color, physicalCardIds: r.Selected);
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(r.OwnerFrameId);
        var ignore = r.RequestOp is SkillProgramEffectOp.UseHejiAtCompletedSingleTarget or SkillProgramEffectOp.RequestSlashByTarget or SkillProgramEffectOp.RequestSlashByNearest || r.RequestOp == SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner && f.FixedTargetSlash!.IgnoreDistance;
        if (r.RequestOp == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment && !_coreRuleQueriesOperations.CanSpendSlashUse(actor, target, _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, r.Source), CardKind.Slash, physicalCardIds: r.Selected))
            return false;
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, cards[0], r.Source, CardKind.Slash, ignoreDistance: ignore, noEffectiveRank: true, physicalCardIds: r.Selected) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, suit) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.Slash, suit, color) && (ignore || _coreAssistedPhysicalCardsOperations.AssistedSlashPaymentHasRange(actor, target.Seat, cards));
    }

    internal IReadOnlyList<PromptChoice> KenshangRequestPickChoices(KenshangRequestReceipt r)
    {
        if (!ValidKenshangRequest(r))
            throw new InvalidOperationException("Kenshang private material selection lost its exact unchanged request.");
        var choices = new List<PromptChoice>();
        foreach (var c in KenshangRequestCards(r.Source).Where(c => !r.Selected.Contains(c.Id)))
            choices.Add(new(new($"kenshang.{(r.OwnerFrameId)}.card.{(c.Id)}"), "选择【" + c.DisplayName + "】", [c.Id], [], new Dictionary<string, string> { { "program-action", KenshangRequestPick }, { "branch", "card" } }));
        if (KenshangSelectedRequestLegal(r))
            choices.Add(new(new($"kenshang.{(r.OwnerFrameId)}.finish"), "将所选的至少两张牌当【杀】使用", [], [r.TargetSeat], new Dictionary<string, string> { { "program-action", KenshangRequestPick }, { "branch", "finish" } }));
        choices.Add(new(new($"kenshang.{(r.OwnerFrameId)}.cancel"), "返回原用牌请求", [], [], new Dictionary<string, string> { { "program-action", KenshangRequestPick }, { "branch", "cancel" } }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishKenshangRequest(KenshangRequestReceipt r)
    {
        var choices = KenshangRequestPickChoices(r);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.ActorSeat, "垦伤：私下选择至少两张手牌或装备牌，然后确认使用；不会提前移动或公开未支付的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.TargetSeat], r.ActorSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(r.Source.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void StoreKenshangRequest(KenshangRequestReceipt? r, long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.Id == id)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { KenshangRequest = r });
        else
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { KenshangRequest = r });
    }

    [ProgramAnswer(KenshangRequestPick)]
    internal void AnswerKenshangRequest(PromptChoice selected)
    {
        if (CurrentKenshangRequest()is not { } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != r.ActorSeat || !KenshangRequestPickChoices(r).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Kenshang private selection requires its real published actor and exact card identity.");
        var branch = selected.Parameters["branch"];
        if (branch == "card")
        {
            var id = selected.Cards.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangRequestMaterialSelectedEvent(r.OwnerFrameId, r.OriginalDecision.PromptId, r.ActorSeat, r.Selected.Count, id, _cardZonesOperations._cardZones.GetLocation(id)));
            r = r with
            {
                Selected = r.Selected.Append(id).ToArray()
            };
            StoreKenshangRequest(r, r.OwnerFrameId);
            PublishKenshangRequest(r);
            return;
        }

        if (branch == "finish" && !KenshangSelectedRequestLegal(r))
            throw new InvalidOperationException("Kenshang's final original target or complete physical payment became illegal.");
        StoreKenshangRequest(null, r.OwnerFrameId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = branch == "cancel" ? r.OriginalDecision with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
        }

        : r.OriginalDecision;
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangRequestFinishedEvent(r.OwnerFrameId, r.ActorSeat, branch == "finish", branch == "finish" ? r.Selected.Count : 0));
        if (branch == "cancel")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], r.Selected, CardKind.Slash, false, r.Source) ?? throw new InvalidOperationException("Kenshang lost its exact selected native payment.");
        if (r.RequestOp is null)
        {
            if (r.OriginalDecision.Kind == DecisionKind.QinglongCrescentBlade)
                _coreNativeQinglongOperations.BeginQinglongCrescentBladeFollowup(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!, selection.Cards[0], CardKind.Slash, r.ActorSeat, selection.Cards, conversionSource: r.Source);
            else
                _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordProgramMultiCardSlashChoice(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, selection);
            return;
        }

        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(r.OwnerFrameId);
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)!;
        if (r.RequestOp == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment)
        {
            var action = new LegalAction(LegalActionKind.Slash, selection.Cards[0].Id, r.TargetSeat, "垦伤：使用【杀】", CardKind.Slash, TargetSeats: [r.TargetSeat], MinCardCount: r.Selected.Count, MaxCardCount: r.Selected.Count)
            {
                ConversionSource = r.Source,
                SelectableCardIds = r.Selected
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangKeActionAcceptedEvent(f.Id, f.InstructionIndex, r.RequestSource!, r.RequestGameplayHash!, r.ActorSeat, r.TargetSeat, r.Source, r.GameplayHash, r.OriginalDecision.PromptId, r.Selected.Count));
            var k = f.KeBiNengPingduan!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, KeBiNengPingduan = k with { BasicActions = k.BasicActions.Append(action).ToArray(), SelectedBasicAction = action, Stage = KeBiNengPingduanStage.BasicIssuing } });
            _characterKeBiNengOlOperations.BeginKeBiNengPingduanBasicUse(f, action);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.RequestOp == SkillProgramEffectOp.RequestSlashByTarget)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, r.ActorSeat, "垦伤：使用【杀】。");
        else if (r.RequestOp == SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, "used-slash", r.ActorSeat, "垦伤：使用【杀】。");
        }
        else if (r.RequestOp is SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedTargetSlash = f.FixedTargetSlash!with { Issued = true } });
        else if (r.RequestOp == SkillProgramEffectOp.RequestLegalSlashByNearest)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NearestLegalSlashRequest = null });
        if (r.RequestOp == SkillProgramEffectOp.UseHejiAtCompletedSingleTarget)
        {
            _characterWuJingOlOperations.PrepareWuJingKenshangNative(f, selection);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        if (_characterSunYuanOlOperations.IsSayingSlashRequest(f))
        {
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(f, r.ActorSeat, r.ActorSeat, r.TargetSeat, CardKind.Slash, selection.Cards, r.Source);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(f))
        {
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(f, r.ActorSeat, r.ActorSeat, r.TargetSeat, CardKind.Slash, selection.Cards, r.Source);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], _coreMatchParticipantsPlayersAccess._players[r.TargetSeat], selection.Cards[0], CardKind.Slash, r.ActorSeat, physicalCards: selection.Cards, countsTowardSlashLimit: false, conversionSource: r.Source, programSkillCardUseFrameId: f.Id);
    }

    internal bool HasKenshangRequestPrompt() => CurrentKenshangRequest()is { } r && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && p.PlayerSeat == r.ActorSeat && p.SkillPrompt?.SkillId == r.Source.SkillId && p.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") == KenshangRequestPick) && ValidKenshangRequest(r);

    internal bool HasAcceptedKenshangKeAction(ProgramSkillFrame f, LegalAction action)
    {
        if (f.KeBiNengPingduan is not { } r || action is not { Kind: LegalActionKind.Slash, PlayedCardKind: CardKind.Slash, ConversionSource: { } source } || _corePlayCardHistoryOperations.ViewAsRule(source)?.KenshangUse != true || source.OwnerSeat != r.ActorSeat || action.TargetSeats is not [var target] || action.MinCardCount < 2 || action.MaxCardCount != action.MinCardCount || action.SelectableCardIds.Count != action.MinCardCount || action.SelectableCardIds.Distinct().Count() != action.MinCardCount || action.CardId != action.SelectableCardIds[0] || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangKeActionAcceptedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray()is not [var accepted] || accepted.InstructionIndex != f.InstructionIndex || accepted.RequestSource != r.Source || accepted.RequestGameplayHash != f.GameplayHash || accepted.ActorSeat != r.ActorSeat || accepted.TargetSeat != target || accepted.Source != source || accepted.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash || accepted.MaterialCount != action.MinCardCount || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRequestStartedEvent>().Count(e => e.OwnerFrameId == f.Id && e.RequestOp == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment && e.ActorSeat == r.ActorSeat && e.TargetSeat == target && e.Source == source && e.OriginalPromptId == accepted.OriginalPromptId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRequestFinishedEvent>().Count(e => e.OwnerFrameId == f.Id && e.ActorSeat == r.ActorSeat && e.Accepted && e.MaterialCount == accepted.MaterialCount) != 1)
            return false;
        var materials = _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRequestMaterialSelectedEvent>().Where(e => e.OwnerFrameId == f.Id && e.OriginalPromptId == accepted.OriginalPromptId).OrderBy(e => e.Index).ToArray();
        return materials.Length == accepted.MaterialCount && materials.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, materials.Length)) && materials.Select(e => e.CardId).SequenceEqual(action.SelectableCardIds) && materials.All(e => e.ActorSeat == r.ActorSeat && e.From.OwnerSeat == r.ActorSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment);
    }

    internal bool TryChooseKenshangAi(PendingDecision p, out PromptChoice choice)
    {
        choice = null !;
        if (!HasKenshangRequestPrompt() || CurrentKenshangRequest()is not { } r || p.PlayerSeat != r.ActorSeat)
            return false;
        choice = p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "finish") ?? p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[r.ActorSeat])).ThenBy(c => c.Cards[0]).FirstOrDefault() ?? p.Choices.Single(c => c.Parameters["branch"] == "cancel");
        return true;
    }
}
