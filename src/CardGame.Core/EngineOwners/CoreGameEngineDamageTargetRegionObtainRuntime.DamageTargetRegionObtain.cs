#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetRegionObtainRuntime
{
    internal bool UsesDamageTargetRegionObtain(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions is [{ Op: SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory, ObtainMode: DamageTargetObtainMode.EntireSelectedZone, DuelDamageExpression: DamageTargetDuelDamageExpression.ObtainedCardCount }];
    internal static int DamageTargetObtainedCount(ProgramDamageTargetObtain receipt) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetRegionObtain.DamageTargetObtainedCount(receipt);
    internal static bool DamageTargetObtainedEquipment(ProgramDamageTargetObtain receipt) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetRegionObtain.DamageTargetObtainedEquipment(receipt);
    internal static long DamageTargetObtainAnchor(ProgramDamageTargetObtain receipt) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetRegionObtain.DamageTargetObtainAnchor(receipt);
    internal IReadOnlyList<PromptChoice> DamageTargetRegionObtainChoices(ProgramSkillFrame frame)
    {
        var victim = frame.DamageTargetObtain!.VictimSeat;
        return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Where(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, victim)).Count > 0).Select(zone => new PromptChoice(new ChoiceId($"damage-target-region.frame-{(frame.Id)}.{(zone)}"), $"获得其{((zone == CardZoneKind.Hand ? "手牌区" : zone == CardZoneKind.Equipment ? "装备区" : "判定区"))}的全部牌（{(_cardZonesOperations._cardZones.CardsAt(new(zone, victim)).Count)}张）。", [], [], new Dictionary<string, string> { ["program-action"] = "damage-target-obtain-region", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["source-zone"] = zone.ToString() })).ToArray();
    }

    internal void IssueDamageTargetRegionObtain(ProgramSkillFrame frame, PromptChoice choice)
    {
        var draft = frame.DamageTargetObtain!;
        if (!UsesDamageTargetRegionObtain(frame) || draft.Stage != ProgramDamageTargetObtainStage.SelectingCard || draft.RegionObtain is not null || !Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Contains(zone))
            throw new InvalidOperationException("A region obtain must select one exact original zone once.");
        var materials = _cardZonesOperations._cardZones.CardsAt(new(zone, draft.VictimSeat)).Select(card => new DamageTargetRegionMaterial(card.Id, card.Kind)).ToArray();
        if (materials.Length == 0)
            throw new InvalidOperationException("The selected obtain zone is empty.");
        var receipt = new ProgramDamageTargetRegionObtainReceipt(zone, draft.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0, false)
        {
            Materials = materials
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DamageTargetObtain = draft with { Stage = ProgramDamageTargetObtainStage.ObtainChildren, RegionObtain = receipt }, PendingMovementContinuation = new(draft.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(item => item.CardId).ToArray(), CardLocation.Hand(draft.OwnerSeat), new("skill-program.damage-target-obtain.obtain"), (batchId, actual) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = receipt with
            {
                SequenceAfter = actual.Max(movement => movement.Sequence),
                MovementBatchId = batchId,
                Paid = true,
                Movements = actual.Select(movement => new ProgramDamageTargetObtainReceipt(movement.Sequence, movement.CardId, movement.CardKind, movement.From, movement.To, movement.Reason.Value, _coreEquipmentContentOperations.EquipmentRules.IsEquipment(movement.CardKind))).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { DamageTargetObtain = current.DamageTargetObtain!with { RegionObtain = paid } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetRegionObtainPaidEvent(frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, frame.InstructionIndex, draft.DamageWindowId, draft.DamageFrameId, draft.OuterCardUseFrameId, draft.OuterActionId, draft.VictimSeat, paid));
        });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal static bool SameDamageTargetRegionReceipt(ProgramDamageTargetRegionObtainReceipt left, ProgramDamageTargetRegionObtainReceipt right) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetRegionObtain.SameDamageTargetRegionReceipt(left, right);
    internal bool IsValidDamageTargetRegionObtain(ProgramSkillFrame frame, ProgramDamageTargetObtain draft)
    {
        if (!UsesDamageTargetRegionObtain(frame) || draft.Receipt is not null || draft.ObtainMovementSequence is not null)
            return false;
        if (draft.Stage == ProgramDamageTargetObtainStage.SelectingCard)
            return draft.RegionObtain is null;
        if (draft.RegionObtain is not { } receipt || receipt.RecipientSeat != draft.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Contains(receipt.SelectedZone) || receipt.Materials.Count == 0 || receipt.Materials.Select(item => item.CardId).Distinct().Count() != receipt.Materials.Count)
            return false;
        if (!receipt.Paid)
            return draft.Stage == ProgramDamageTargetObtainStage.ObtainChildren && receipt.Movements.Count == 0 && receipt.MovementBatchId == 0 && receipt.SequenceAfter == receipt.SequenceBefore && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id && frame.PendingMovementContinuation is not null;
        if (receipt.MovementBatchId <= 0 || receipt.SequenceAfter <= receipt.SequenceBefore || receipt.Movements.Count != receipt.Materials.Count || receipt.Movements.Max(item => item.MovementSequence) != receipt.SequenceAfter || receipt.Movements.Select(item => item.MovementSequence).Distinct().Count() != receipt.Movements.Count || receipt.Movements.Select(item => new DamageTargetRegionMaterial(item.CardId, item.PrintedKind)).SequenceEqual(receipt.Materials) == false)
            return false;
        foreach (var item in receipt.Movements)
        {
            if (item.MovementSequence <= receipt.SequenceBefore || item.MovementSequence > receipt.SequenceAfter || item.From != new CardLocation(receipt.SelectedZone, draft.VictimSeat) || item.Reason != "skill-program.damage-target-obtain.obtain" || item.IsEquipment != _coreEquipmentContentOperations.EquipmentRules.IsEquipment(item.PrintedKind) || item.To != CardLocation.Hand(draft.OwnerSeat) && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == item.MovementSequence && _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(move, CardLocation.Hand(draft.OwnerSeat))) && !(item.To == CardLocation.OutsideGame && item.From.Zone == CardZoneKind.Equipment && (item.PrintedKind == CardKind.GeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(item.CardId), item.From, CardLocation.Hand(draft.OwnerSeat)))) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.Sequence == item.MovementSequence && move.CardId == item.CardId && move.CardKind == item.PrintedKind && move.From == item.From && move.To == item.To && move.Reason.Value == item.Reason) != 1)
                return false;
        }

        // The atomic callback records every selected entity before any removal
        // hooks run. This interval must be precisely that complete ordered batch.
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > receipt.SequenceBefore && move.Sequence <= receipt.SequenceAfter).Select(move => new ProgramDamageTargetObtainReceipt(move.Sequence, move.CardId, move.CardKind, move.From, move.To, move.Reason.Value, _coreEquipmentContentOperations.EquipmentRules.IsEquipment(move.CardKind))).SequenceEqual(receipt.Movements) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageTargetRegionObtainPaidEvent>().Count(fact => fact.ProgramFrameId == frame.Id && fact.Source == new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) && fact.GameplayHash == frame.GameplayHash && fact.InstructionIndex == frame.InstructionIndex && fact.DamageWindowId == draft.DamageWindowId && fact.DamageFrameId == draft.DamageFrameId && fact.OuterCardUseFrameId == draft.OuterCardUseFrameId && fact.OuterActionId == draft.OuterActionId && fact.VictimSeat == draft.VictimSeat && SameDamageTargetRegionReceipt(fact.Receipt, receipt)) == 1;
    }

    internal int PrepareDamageTargetDuelDamage(long useId, int targetSeat)
    {
        if (_coreShanheS5ShuangxiongOperations.TryGetShanheS5ShuangxiongDuelBaseDamage(useId, out var scaled))
            return scaled;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.DamageTargetDuelOrigin is not { BaseDamage: { } amount } origin)
            return _characterFanJiangZhangDaOlOperations.PrepareJueshengDuelDamage(useId, targetSeat);
        if (amount < 1 || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { } use || !use.TargetSeats.Contains(targetSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ParentProgramFrameId)is not { } parent || !_coreDamageTargetObtainProgramsOperations.MatchesDamageTargetDuelParent(parent, origin) || parent.DamageTargetObtain?.RegionObtain is not { Paid: true } paid || amount != paid.ActualObtainedCount)
            throw new InvalidOperationException("A variable Duel must retain its exact actual obtained-card payment.");
        return amount;
    }

    internal bool DamageTargetDuelBaseMatches(ProgramDamageTargetObtain receipt, ProgramDamageTargetDuelOrigin origin) => receipt.RegionObtain is { Paid: true } region ? region.ContainsEquipment && region.ActualObtainedCount > 0 && origin.BaseDamage == region.ActualObtainedCount : origin.BaseDamage is null;

    internal PromptChoice SelectAiDamageTargetRegion(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.DamageTargetObtain!;
        // Enemy hand identities stay private. Its public count is the entire
        // estimate; public equipment/judgment cards may use printed categories.
        return decision.Choices.OrderByDescending(choice =>
        {
            var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
            var cards = _cardZonesOperations._cardZones.CardsAt(new(zone, draft.VictimSeat));
            return zone == CardZoneKind.Hand ? cards.Count * 8d : cards.Sum(card => card.IsGeneralWeapon ? 0d : _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) ? 10d : 8d);
        }).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
    }
}
