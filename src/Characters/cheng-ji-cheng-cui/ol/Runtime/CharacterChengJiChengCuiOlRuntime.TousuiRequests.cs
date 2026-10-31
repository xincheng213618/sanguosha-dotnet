#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengJiChengCuiOlRuntime
{
    internal const string TousuiRequestOpen = "tousui-request-open", TousuiRequestPick = "tousui-request-pick";
    internal IEnumerable<CardConversionSource> TousuiSources(CharacterState actor) => _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Where(i => i.SkillId == ChengJiChengCuiComposition.Tousui).SelectMany(i => i.Program.ViewAs.Where(r => r.Id == "bottom-cost-slash" && r.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor))).Select(r => new CardConversionSource(i.SkillId, r.Id, actor.Seat, i.SkillInstanceId)));
    internal IReadOnlyList<Card> TousuiRequestCards(CardConversionSource source)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) || _corePlayCardHistoryOperations.ViewAsRule(source)is not { CostDestination: SkillProgramCardDestination.DrawPileBottom } r || _coreCardUseModulesOperations.IsCardUseForbidden(source.OwnerSeat, CardKind.Slash, CardActionType.Use))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        return TousuiPhysicalCards(actor).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [c.Id])).DistinctBy(c => c.Id).OrderBy(c => c.Id).ToArray();
    }

    internal IReadOnlyList<PromptChoice> TousuiProgramSlashChoices(ProgramSkillFrame f, int actorSeat, IReadOnlyList<int> targets, string? resultBind = null)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect || !TousuiProgramActor(f, effect.Op, actorSeat))
            return[];
        var exact = TousuiProgramTargets(f, actorSeat, effect.Op);
        var result = new List<PromptChoice>();
        foreach (var source in TousuiSources(_coreMatchParticipantsPlayersAccess._players[actorSeat]).Where(s => TousuiRequestCards(s).Count >= 1))
            foreach (var target in targets.Where(exact.Contains).Distinct())
            {
                if (target == actorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    continue;
                var p = new Dictionary<string, string>
                {
                    {
                        "program-action",
                        TousuiRequestOpen
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
                result.Add(new(new($"tousui.request.{(f.Id)}.{(source.SkillInstanceId)}.{(source.BindingId)}.{(actorSeat)}.{(target)}"), "发动【透髓】：私下选择至少一张手牌或装备牌当【杀】使用", [], [target], p));
            }

        return Array.AsReadOnly(result.ToArray());
    }

    internal bool HasTousuiProgramSlash(CharacterState actor, IEnumerable<int> targets) => actor.IsAlive && targets.Any(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != actor.Seat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive) && TousuiSources(actor).Any(s => TousuiRequestCards(s).Count >= 1);
    internal bool TousuiProgramActor(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => op == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment ? f.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice } r && r.ActorSeat == actor && _characterKeBiNengOlOperations.ValidKeBiNeng(f) : _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, op, actor);
    internal int[] TousuiProgramTargets(ProgramSkillFrame f, int actor, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment && TousuiProgramActor(f, op, actor) ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor).Select(p => p.Seat).ToArray() : _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(f, actor, op);
    internal IReadOnlyList<PromptChoice> TousuiBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(pending.WeaponOwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat].IsAlive)
            return[];
        return Array.AsReadOnly(TousuiSources(_coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat]).Where(s => TousuiRequestCards(s).Count >= 1).Select(s =>
        {
            var p = new Dictionary<string, string>
            {
                {
                    "program-action",
                    TousuiRequestOpen
                },
                {
                    "response",
                    TousuiRequestOpen
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
            return new PromptChoice(new($"tousui.borrowed.{(pending.ResolutionId)}.{(s.SkillInstanceId)}.{(s.BindingId)}"), "发动【透髓】：将至少一张手牌或装备牌当【杀】使用", [], [pending.SlashTargetSeat], p);
        }).ToArray());
    }

    internal bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("response") != TousuiRequestOpen)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } original || !pending.AwaitingSlashChoice || original.PlayerSeat != pending.WeaponOwnerSeat || !TousuiBorrowedSlashChoices(pending).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is not { TousuiRequest: null } parent || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ResponseWindowFrame response || response.ParentFrameId != parent.Id)
            throw new InvalidOperationException("Tousui lost its original Borrowed Sword Slash-use request.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new TousuiRequestReceipt(parent.Id, 0, null, pending.WeaponOwnerSeat, pending.SlashTargetSeat, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent.Id, u => u with { TousuiRequest = r });
        BeginTousuiRequest(r);
        return true;
    }

    internal IReadOnlyList<PromptChoice> TousuiQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending)
    {
        var attack = pending.Attack;
        var actor = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || !_coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target))
            return[];
        return Array.AsReadOnly(TousuiSources(actor).Where(s => TousuiRequestCards(s).Count >= 1).Select(s =>
        {
            var p = new Dictionary<string, string>
            {
                {
                    "program-action",
                    TousuiRequestOpen
                },
                {
                    "action",
                    TousuiRequestOpen
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
            return new PromptChoice(new($"tousui.qinglong.{(attack.ResolutionId)}.{(s.SkillInstanceId)}.{(s.BindingId)}"), "发动【透髓】：选择至少一张牌追杀", [], [target.Seat], p);
        }).ToArray());
    }

    internal bool TryBeginTousuiQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("action") != TousuiRequestOpen)
            return false;
        var attack = pending.Attack;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } original || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, pending) || original.PlayerSeat != attack.SourceSeat || !TousuiQinglongSlashChoices(pending).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { TousuiRequest: null } parent || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id)
            throw new InvalidOperationException("Tousui lost its exact native Qinglong Slash-use request.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new TousuiRequestReceipt(parent.Id, 0, null, attack.SourceSeat, attack.TargetSeat, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent.Id, u => u with { TousuiRequest = r });
        BeginTousuiRequest(r);
        return true;
    }

    [ProgramAnswer(TousuiRequestOpen)]
    internal void OpenTousuiProgramRequest(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TousuiRequest: null } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } original || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect || selected.Targets is not [var target] || !TousuiProgramSlashChoices(f, original.PlayerSeat, TousuiProgramTargets(f, original.PlayerSeat, effect.Op)).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || !original.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Tousui lost its exact program-request actor, instruction or legal published frontier.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var r = new TousuiRequestReceipt(f.Id, f.InstructionIndex, effect.Op, original.PlayerSeat, target, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, original)
        {
            RequestSource = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            RequestGameplayHash = f.GameplayHash
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TousuiRequest = r });
        BeginTousuiRequest(r);
    }

    internal void BeginTousuiRequest(TousuiRequestReceipt r)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TousuiRequestStartedEvent(r.OwnerFrameId, r.InstructionIndex, r.RequestOp, r.ActorSeat, r.TargetSeat, r.Source, r.GameplayHash, r.OriginalDecision.PromptId, r.OriginalDecision.Revision, TousuiOriginalDecisionHash(r.OriginalDecision)));
        PublishTousuiRequest(r);
    }

    internal static string TousuiOriginalDecisionHash(PendingDecision decision) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(decision))));
    internal TousuiRequestReceipt? CurrentTousuiRequest() => _resolutionStackOperations._resolutionStack.LastOrDefault() switch
    {
        ProgramSkillFrame { TousuiRequest: { } r } => r,
        CardUseFrame { TousuiRequest: { } r } => r,
        ResponseWindowFrame response when _coreCardUseLifecycleOperations.LifecycleCardUse(response.ParentFrameId)is { TousuiRequest: { } r } => r,
        _ => null
    };
    internal bool ValidTousuiRequest(TousuiRequestReceipt r)
    {
        if (r.OwnerFrameId <= 0 || r.Source.OwnerSeat != r.ActorSeat || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash || _corePlayCardHistoryOperations.ViewAsRule(r.Source)is not { CostDestination: SkillProgramCardDestination.DrawPileBottom } || r.OriginalDecision.PlayerSeat != r.ActorSeat || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => !TousuiRequestCards(r.Source).Any(c => c.Id == id)) || _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestStartedEvent>().Count(e => e.OwnerFrameId == r.OwnerFrameId && e.InstructionIndex == r.InstructionIndex && e.RequestOp == r.RequestOp && e.ActorSeat == r.ActorSeat && e.TargetSeat == r.TargetSeat && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.OriginalPromptId == r.OriginalDecision.PromptId && e.OriginalRevision == r.OriginalDecision.Revision && e.OriginalDecisionHash == TousuiOriginalDecisionHash(r.OriginalDecision)) != 1 || !r.OriginalDecision.ValidTargetSeats.Contains(r.TargetSeat) || !r.OriginalDecision.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == TousuiRequestOpen && c.Parameters.GetValueOrDefault("frame-id") == r.OwnerFrameId.ToString(CultureInfo.InvariantCulture) && c.Parameters.GetValueOrDefault("actor-seat") == r.ActorSeat.ToString(CultureInfo.InvariantCulture) && c.Targets.SequenceEqual([r.TargetSeat]) && global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(c.Parameters, out var s) && s == r.Source))
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestMaterialSelectedEvent>().Where(e => e.OwnerFrameId == r.OwnerFrameId && e.OriginalPromptId == r.OriginalDecision.PromptId).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Selected.Count || !selected.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, selected.Length)) || !selected.Select(e => e.CardId).SequenceEqual(r.Selected) || selected.Any(e => e.ActorSeat != r.ActorSeat || e.From != _cardZonesOperations._cardZones.GetLocation(e.CardId)))
            return false;
        if (r.RequestOp is { } op)
            return r.OriginalDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TousuiRequest: { } stored } f && stored == r && f.Id == r.OwnerFrameId && f.InstructionIndex == r.InstructionIndex && f.GameplayHash == r.RequestGameplayHash && r.OriginalDecision.SourceSeat == f.OwnerSeat && r.OriginalDecision.TargetSeat == r.ActorSeat && r.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && r.RequestSource == new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == op && TousuiProgramActor(f, op, r.ActorSeat) && TousuiProgramTargets(f, r.ActorSeat, op).Contains(r.TargetSeat);
        if (r.OriginalDecision.Kind == DecisionKind.QinglongCrescentBlade)
            return r.OriginalDecision.IsPrivate && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack) && q.Attack.ResolutionId == r.OwnerFrameId && q.Attack.SourceSeat == r.ActorSeat && q.Attack.TargetSeat == r.TargetSeat && r.OriginalDecision.SourceSeat == r.ActorSeat && r.OriginalDecision.TargetSeat == r.TargetSeat && r.OriginalDecision.IncomingCard == q.Attack.EffectiveCardKind && r.OriginalDecision.SkillPrompt is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { TousuiRequest: { } native } u && u.Id == r.OwnerFrameId && native == r;
        return r.OriginalDecision is { Kind: DecisionKind.RespondSlash, RequiredCardKind: CardKind.Slash, IncomingCard: CardKind.BorrowedSword } && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } borrowed && borrowed.ResolutionId == r.OwnerFrameId && borrowed.WeaponOwnerSeat == r.ActorSeat && borrowed.SlashTargetSeat == r.TargetSeat && r.OriginalDecision.SourceSeat == borrowed.SourceSeat && r.OriginalDecision.TargetSeat == r.TargetSeat && r.OriginalDecision.SkillPrompt is null && _coreCardUseLifecycleOperations.LifecycleCardUse(r.OwnerFrameId)?.TousuiRequest == r && _resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == r.OwnerFrameId;
    }

    internal bool TousuiSelectedRequestLegal(TousuiRequestReceipt r)
    {
        if (r.Selected.Count < 1 || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], r.Selected, CardKind.Slash, false, r.Source)is not { } selection)
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

    internal IReadOnlyList<PromptChoice> TousuiRequestPickChoices(TousuiRequestReceipt r)
    {
        if (!ValidTousuiRequest(r))
            throw new InvalidOperationException("Tousui private material selection lost its exact unchanged request.");
        var choices = new List<PromptChoice>();
        foreach (var c in TousuiRequestCards(r.Source).Where(c => !r.Selected.Contains(c.Id)))
            choices.Add(new(new($"tousui.{(r.OwnerFrameId)}.card.{(c.Id)}"), "选择【" + c.DisplayName + "】", [c.Id], [], new Dictionary<string, string> { { "program-action", TousuiRequestPick }, { "branch", "card" } }));
        if (TousuiSelectedRequestLegal(r))
            choices.Add(new(new($"tousui.{(r.OwnerFrameId)}.finish"), "将所选的至少一张牌当【杀】使用", [], [r.TargetSeat], new Dictionary<string, string> { { "program-action", TousuiRequestPick }, { "branch", "finish" } }));
        choices.Add(new(new($"tousui.{(r.OwnerFrameId)}.cancel"), "返回原用牌请求", [], [], new Dictionary<string, string> { { "program-action", TousuiRequestPick }, { "branch", "cancel" } }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishTousuiRequest(TousuiRequestReceipt r)
    {
        var choices = TousuiRequestPickChoices(r);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.ActorSeat, "透髓：私下选择至少一张手牌或装备牌，然后确认使用；不会提前移动或公开未支付的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.TargetSeat], r.ActorSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(r.Source.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void StoreTousuiRequest(TousuiRequestReceipt? r, long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.Id == id)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { TousuiRequest = r });
        else
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { TousuiRequest = r });
    }

    [ProgramAnswer(TousuiRequestPick)]
    internal void AnswerTousuiRequest(PromptChoice selected)
    {
        if (CurrentTousuiRequest()is not { } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != r.ActorSeat || !TousuiRequestPickChoices(r).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Tousui private selection requires its real published actor and exact card identity.");
        var branch = selected.Parameters["branch"];
        if (branch == "card")
        {
            var id = selected.Cards.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TousuiRequestMaterialSelectedEvent(r.OwnerFrameId, r.OriginalDecision.PromptId, r.ActorSeat, r.Selected.Count, id, _cardZonesOperations._cardZones.GetLocation(id)));
            r = r with
            {
                Selected = r.Selected.Append(id).ToArray()
            };
            StoreTousuiRequest(r, r.OwnerFrameId);
            PublishTousuiRequest(r);
            return;
        }

        if (branch == "finish" && !TousuiSelectedRequestLegal(r))
            throw new InvalidOperationException("Tousui's final original target or complete physical payment became illegal.");
        StoreTousuiRequest(null, r.OwnerFrameId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = branch == "cancel" ? r.OriginalDecision with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
        }

        : r.OriginalDecision;
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TousuiRequestFinishedEvent(r.OwnerFrameId, r.ActorSeat, branch == "finish", branch == "finish" ? r.Selected.Count : 0));
        if (branch == "cancel")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], r.Selected, CardKind.Slash, false, r.Source) ?? throw new InvalidOperationException("Tousui lost its exact selected native payment.");
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
            var action = new LegalAction(LegalActionKind.Slash, selection.Cards[0].Id, r.TargetSeat, "透髓：使用【杀】", CardKind.Slash, TargetSeats: [r.TargetSeat], MinCardCount: r.Selected.Count, MaxCardCount: r.Selected.Count)
            {
                ConversionSource = r.Source,
                SelectableCardIds = r.Selected
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TousuiKeActionAcceptedEvent(f.Id, f.InstructionIndex, r.RequestSource!, r.RequestGameplayHash!, r.ActorSeat, r.TargetSeat, r.Source, r.GameplayHash, r.OriginalDecision.PromptId, r.Selected.Count));
            var k = f.KeBiNengPingduan!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, KeBiNengPingduan = k with { BasicActions = k.BasicActions.Append(action).ToArray(), SelectedBasicAction = action, Stage = KeBiNengPingduanStage.BasicIssuing } });
            _characterKeBiNengOlOperations.BeginKeBiNengPingduanBasicUse(f, action);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.RequestOp == SkillProgramEffectOp.RequestSlashByTarget)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, r.ActorSeat, "透髓：使用【杀】。");
        else if (r.RequestOp == SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, "used-slash", r.ActorSeat, "透髓：使用【杀】。");
        }
        else if (r.RequestOp is SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedTargetSlash = f.FixedTargetSlash!with { Issued = true } });
        else if (r.RequestOp == SkillProgramEffectOp.RequestLegalSlashByNearest)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NearestLegalSlashRequest = null });
        if (r.RequestOp == SkillProgramEffectOp.UseHejiAtCompletedSingleTarget)
        {
            PrepareTousuiWuJingNative(f, selection);
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

    internal void PrepareTousuiWuJingNative(ProgramSkillFrame f, ProgramMultiCardViewAsSelection selection)
    {
        if (!_characterWuJingOlOperations.IsWuJingHejiRequest(f))
            return;
        if (selection.Cards.Count < 1 || !IsTousuiSource(selection.Source) || !_characterWuJingOlOperations.WuJingCanUse(f.OwnerSeat, f.WuJing!.TargetSeat, selection.Cards[0], CardKind.Slash, selection.Source, selection.Cards.Select(c => c.Id).ToArray()) || _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestFinishedEvent>().Count(e => e.OwnerFrameId == f.Id && e.ActorSeat == f.OwnerSeat && e.Accepted && e.MaterialCount == selection.Cards.Count) != 1)
            throw new InvalidOperationException("Heji Tousui requires its actual accepted original material request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuJing = f.WuJing with { Stage = WuJingStage.IssuingUse, Selection = new(CardKind.Slash, false, false) { CardIds = selection.Cards.Select(c => c.Id).ToArray(), Conversions = [selection.Source] }, Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
    }

    internal bool HasTousuiRequestPrompt() => CurrentTousuiRequest()is { } r && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && p.PlayerSeat == r.ActorSeat && p.SkillPrompt?.SkillId == r.Source.SkillId && p.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") == TousuiRequestPick) && ValidTousuiRequest(r);
    internal PendingDecision? TousuiNativeOriginalDecision(long ownerId) => HasTousuiRequestPrompt() && CurrentTousuiRequest()is { RequestOp: null } r && r.OwnerFrameId == ownerId ? r.OriginalDecision : null;
    internal PendingDecision TousuiProgramNativeDecision(PendingDecision p) => global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime.ZonghuNativeDecision(HasTousuiRequestPrompt() && CurrentTousuiRequest()is { RequestOp: not null } r && p.PlayerSeat == r.ActorSeat ? r.OriginalDecision : p);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32203)]
    internal void AssertTousuiRequestState(ProgramSkillFrame f)
    {
        if (f.TousuiRequest is { } r && (!ValidTousuiRequest(r) || !HasTousuiRequestPrompt() || r.OwnerFrameId != f.Id))
            throw new InvalidOperationException("Tousui program request lost its owning instruction, original private frontier or ordered private selections.");
    }

    internal void AssertTousuiRequests()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
            AssertTousuiRequestState(f);
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            if (f.TousuiRequest is { } r && (!ValidTousuiRequest(r) || !HasTousuiRequestPrompt() || r.OwnerFrameId != f.Id || r.RequestOp is not null))
                throw new InvalidOperationException("Tousui native request lost its exact Borrowed Sword/Qinglong owner, original frontier or selected entity invoices.");
            if (f.Tousui is not null && !TousuiUseProof(f))
                throw new InvalidOperationException("Tousui native use lost its original material count, conversion source or final actor chain.");
        }
    }

    internal bool HasAcceptedTousuiKeAction(ProgramSkillFrame f, LegalAction action)
    {
        if (f.KeBiNengPingduan is not { } r || action is not { Kind: LegalActionKind.Slash, PlayedCardKind: CardKind.Slash, ConversionSource: { } source } || !IsTousuiSource(source) || source.OwnerSeat != r.ActorSeat || action.TargetSeats is not [var target] || action.MinCardCount < 1 || action.MaxCardCount != action.MinCardCount || action.SelectableCardIds.Count != action.MinCardCount || action.SelectableCardIds.Distinct().Count() != action.MinCardCount || action.CardId != action.SelectableCardIds[0] || _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiKeActionAcceptedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray()is not [var accepted] || accepted.InstructionIndex != f.InstructionIndex || accepted.RequestSource != r.Source || accepted.RequestGameplayHash != f.GameplayHash || accepted.ActorSeat != r.ActorSeat || accepted.TargetSeat != target || accepted.Source != source || accepted.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash || accepted.MaterialCount != action.MinCardCount || _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestStartedEvent>().Count(e => e.OwnerFrameId == f.Id && e.RequestOp == SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment && e.ActorSeat == r.ActorSeat && e.TargetSeat == target && e.Source == source && e.OriginalPromptId == accepted.OriginalPromptId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestFinishedEvent>().Count(e => e.OwnerFrameId == f.Id && e.ActorSeat == r.ActorSeat && e.Accepted && e.MaterialCount == accepted.MaterialCount) != 1)
            return false;
        var materials = _coreProgramEventHistoryOperations.ProgramEventHistory<TousuiRequestMaterialSelectedEvent>().Where(e => e.OwnerFrameId == f.Id && e.OriginalPromptId == accepted.OriginalPromptId).OrderBy(e => e.Index).ToArray();
        return materials.Length == accepted.MaterialCount && materials.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, materials.Length)) && materials.Select(e => e.CardId).SequenceEqual(action.SelectableCardIds) && materials.All(e => e.ActorSeat == r.ActorSeat && e.From.OwnerSeat == r.ActorSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain);
    }

    internal bool TryChooseTousuiAi(PendingDecision p, out PromptChoice choice)
    {
        choice = null !;
        if (!HasTousuiRequestPrompt() || CurrentTousuiRequest()is not { } r || p.PlayerSeat != r.ActorSeat)
            return false;
        choice = p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "finish") ?? p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[r.ActorSeat])).ThenBy(c => c.Cards[0]).FirstOrDefault() ?? p.Choices.Single(c => c.Parameters["branch"] == "cancel");
        return true;
    }
}
