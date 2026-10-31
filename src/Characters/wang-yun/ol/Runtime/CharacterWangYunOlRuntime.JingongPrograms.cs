#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYunOlRuntime
{
    internal bool HasWangYunTrickCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial);

    internal CardKind[] WangYunRandomTrickPool() => _contentRegistryOperations._contentRegistry.Cards.Values.Where(c => c.LegacyKind is { } kind && CardCatalog.Get(kind).CategoryName == "锦囊牌" && kind != CardKind.Nullification && (kind == CardKind.Lightning || _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(_contentRegistryOperations._contentRegistry.GetSkill("ol:jingong").Program!).ViewAs.Any(r => r.OutputKind == kind))).Select(c => c.LegacyKind!.Value).Distinct().Order().ToArray();
    internal bool IsWangYunRandomConversionAvailable(CharacterState owner, Card card, CardConversionSource source, bool forResponse)
    {
        if (!HasWangYunTrickCapability || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial) || !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(source.SkillId, out var definition) || definition.Program is not { } p || !p.Activations.Any(a => ProgramInstructionResolver.Default.Features(a).HasOperation(SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial)))
            return true;
        return !forResponse && source.OwnerSeat == owner.Seat && WangYunMaterial(card) && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { WangYunTrick: { Stage: "choice" or "issuing-trick" } r } f && f.OwnerSeat == owner.Seat && f.SkillId == source.SkillId && f.SkillInstanceId == source.SkillInstanceId && r.CardId == card.Id && r.From == _cardZonesOperations._cardZones.GetLocation(card.Id) && _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(p).ViewAs.SingleOrDefault(v => v.Id == source.BindingId)is { } rule && r.Samples.Contains(rule.OutputKind);
    }

    internal static string WangYunSamplesFingerprint(IReadOnlyList<CardKind> samples) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wang_yun_ol_Jingong_Programs.WangYunSamplesFingerprint(samples);
    internal SkillProgramStepOutcome UseThreeRandomTricksFromSelectedMaterial(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WangYunTrick is not null || !WangYunActivationMatches(f, SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial) || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Random tricks lost the original actual material activation.");
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        var pool = WangYunRandomTrickPool().ToList();
        if (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !WangYunMaterial(card) || pool.Count < 3)
            throw new InvalidOperationException("Random tricks require one real Slash/equipment entity and at least three real proactive native trick templates.");
        var samples = new List<CardKind>();
        for (var i = 0; i < 3; i++)
        {
            var at = _randomOperations._random.Next(pool.Count);
            samples.Add(pool[at]);
            pool.RemoveAt(at);
        }

        var r = new WangYunTrickReceipt
        {
            Source = WangYunSource(f),
            GameplayHash = f.GameplayHash,
            Turn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            PhaseInstance = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            InstructionIndex = 1,
            CardId = id,
            PrintedKind = card.Kind,
            From = from,
            Samples = samples,
            Stage = "choice"
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunTrick = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunRandomTricksStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.PhaseInstance, id, card.Kind, from, WangYunSamplesFingerprint(samples)));
        PublishWangYun(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<LegalAction> WangYunTrickActions(ProgramSkillFrame f) => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], includeProgramActions: false, selectedPhysicalCardId: f.WangYunTrick!.CardId).Where(a => a.ConversionSource is { } s && s.SkillId == f.SkillId && s.SkillInstanceId == f.SkillInstanceId && a.PlayedCardKind is { } k && f.WangYunTrick.Samples.Contains(k)).ToArray();
    internal bool CanWangYunLightning(ProgramSkillFrame f)
    {
        var r = f.WangYunTrick!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = _cardZonesOperations._cardZones.CardsAt(r.From).SingleOrDefault(c => c.Id == r.CardId);
        return r.Samples.Contains(CardKind.Lightning) && owner.IsAlive && card is not null && WangYunMaterial(card) && !owner.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(owner, CardKind.Lightning) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Lightning, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(owner, CardKind.Lightning, [owner.Seat]) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, [card.Id]) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(owner.Seat, card.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Lightning, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)));
    }

    internal IReadOnlyList<PromptChoice> WangYunTrickChoices(ProgramSkillFrame f)
    {
        var result = new List<PromptChoice>();
        var index = 0;
        foreach (var a in WangYunTrickActions(f))
        {
            var args = WangYunArgs(f, "trick");
            args["choice-index"] = (index++).ToString(CultureInfo.InvariantCulture);
            args["effective-kind"] = a.PlayedCardKind!.Value.ToString();
            result.Add(new(new($"wang-yun.{(f.Id)}.trick-{(index)}"), a.Description, [f.WangYunTrick!.CardId], a.TargetSeats, args));
        }

        if (CanWangYunLightning(f))
        {
            var args = WangYunArgs(f, "lightning");
            args["effective-kind"] = CardKind.Lightning.ToString();
            result.Add(new(new($"wang-yun.{(f.Id)}.lightning"), "将本次材料当【闪电】对自己使用", [f.WangYunTrick!.CardId], [f.OwnerSeat], args));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void IssueWangYunTrick(ProgramSkillFrame f, PromptChoice selected)
    {
        var r = f.WangYunTrick!;
        var kind = Enum.Parse<CardKind>(selected.Parameters["effective-kind"]);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = _cardZonesOperations._cardZones.CardsAt(r.From).Single(c => c.Id == r.CardId);
        LegalAction? action = null;
        if (kind != CardKind.Lightning)
            action = WangYunTrickActions(f)[int.Parse(selected.Parameters["choice-index"], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunTrick = r with { Stage = "issuing-trick", SelectedKind = kind } });
        if (action is not null)
        {
            _coreNativeActionDispatchOperations.ExecuteAction(owner, action);
            return;
        }

        if (!CanWangYunLightning(f))
            throw new InvalidOperationException("The sampled Lightning lost its actual native self-placement legality.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, owner.Seat, [owner.Seat], CardKind.Lightning, conversionSource: r.Source);
        _corePhysicalMovementFacadesOperations.MoveCard(card, r.From, CardLocation.Processing, CardMoveReasons.Use);
        CompleteWangYunNativePayment(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(id, f.SkillId, f.ActivationId, f.OwnerSeat, [card.Id], CardKind.Lightning, true, [owner.Seat]));
        if (!_coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability && _coreCardActionsOperations.TryBeginDelayedCardUsePrograms(id))
            return;
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, owner.Seat, [owner.Seat], LegalActionKind.Lightning, playedCardKind: CardKind.Lightning);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 30)]
    internal bool WangYunConversionUnavailable(ProgramViewAsSourceQuery query) => !IsWangYunRandomConversionAvailable(query.Owner, query.Card, query.Source, query.ForResponse);
}
