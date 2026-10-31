#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiangChongOlRuntime
{
    internal bool MuzhenUsed(int owner, string branch, long token) => _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongBranchPaidEvent>().Any(e => e.OwnerSeat == owner && e.Branch == branch && e.PhaseToken == token);
    internal bool MuzhenTarget(ProgramSkillFrame? f, int owner, int target, string branch) => target != owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && (branch == "gift" ? _corePhysicalOwnedCardQueriesOperations.GetEquipment2(target).Count > 0 && XiangOwned(owner, f).Count >= 2 : XiangOwned(owner, f).Any(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(owner, target, _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), c.From)));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27140, SkillProgramEffectOp.ResolveMuzhenExchange)]
    internal bool AllowsMuzhen(ProgramActivationLegalityContext gate) => XiangPlayToken(gate.OwnerSeat)is> 0 and var token && gate.Activation.Effects is [{ StateId: { } branch }] && !MuzhenUsed(gate.OwnerSeat, branch, token) && _coreMatchParticipantsPlayersAccess._players.Any(t => MuzhenTarget(null, gate.OwnerSeat, t.Seat, branch));
    internal void PlaceMuzhen(ProgramSkillFrame f)
    {
        var r = f.XiangChong!;
        var c = r.Selected.Single();
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId);
        if (card.IsGeneralWeapon || !_coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(f.OwnerSeat, r.Recipient, card, c.From))
            throw new InvalidOperationException("Muzhen's true equipment placement is no longer legal.");
        var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment2(r.Recipient).Where(x => _coreEquipmentContentOperations.EquipmentRules.Get(x.Kind).Slot == slot).ToArray();
        var old = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[r.Recipient].EquipmentSlotCapacity(slot) ? equipped[0] : null;
        var all = r.Selected.Concat(old is null ? Array.Empty<XiangChongCard>() : [new(old.Id, old.Kind, CardLocation.Equipment(r.Recipient))]);
        var grain = all.Where(x => x.Kind == CardKind.WoodenOx && x.From.OwnerSeat is not null && x.From.Zone == CardZoneKind.Equipment).SelectMany(x => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[x.From.OwnerSeat!.Value]).Select(y => new XiangChongCard(y.Id, y.Kind, CardLocation.WoodenOxGrain(x.From.OwnerSeat.Value)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Paid = true, Stage = XiangChongStage.PaymentChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Payments = r.Selected, Replaced = old is null ? null : new(old.Id, old.Kind, CardLocation.Equipment(r.Recipient)), Auxiliaries = grain } });
        if (old is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, old);
            _corePhysicalMovementFacadesOperations.MoveCard(old, CardLocation.Equipment(r.Recipient), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, c.From, CardLocation.Equipment(r.Recipient), CardMoveReasons.EquipmentEnter);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = f.XiangChong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, r.Recipient, slot, card.Id, card.Kind, old?.Id));
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > f.XiangChong!.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEntityMovedEvent(f.Id, "placement", m.CardId, m.CardKind, m.From, m.To, m.Sequence));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongBranchPaidEvent(f.Id, f.OwnerSeat, r.Recipient, r.Branch, r.PhaseToken, f.XiangChong!.Before, f.XiangChong.After));
    }
}
