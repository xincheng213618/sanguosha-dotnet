#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNationalGeneralRevealRuntime
{
    internal void RevealNationalGeneral(CharacterState actor, GeneralSelectionSlot slot)
    {
        if (!_coreNativeDamageHandlesOperations.IsNationalWarMode)
        {
            throw new InvalidOperationException("General reveal is only available in national-war lite mode.");
        }

        var general = slot switch
        {
            GeneralSelectionSlot.Primary => actor.GeneralSelected && !actor.GeneralRevealed ? actor.General : null,
            GeneralSelectionSlot.Secondary => actor.SecondaryGeneralSelected && !actor.SecondaryGeneralRevealed ? actor.SecondaryGeneral : null,
            _ => null
        };
        if (general is null)
        {
            throw new InvalidOperationException("The selected national general is already public or unavailable.");
        }

        if (slot == GeneralSelectionSlot.Primary)
        {
            actor.GeneralRevealed = true;
        }
        else
        {
            actor.SecondaryGeneralRevealed = true;
        }

        var firstFactionReveal = !actor.FactionRevealed;
        actor.FactionRevealed = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NationalGeneralRevealedEvent(actor.Seat, slot, general.Id));
        if (firstFactionReveal)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NationalFactionRevealedEvent(actor.Seat, actor.NationalFactionId!));
        }

        _coreMatchLogOperations.AddLog("NationalGeneralRevealed", $"{(actor.Name)} 明置{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(actor.NationalFactionId!))}势力的【{(general.Name)}】。", actor.Seat);
    }

    internal void RevealNationalInformation(CharacterState player)
    {
        if (!_coreNativeDamageHandlesOperations.IsNationalWarMode || player.NationalFactionId is null)
        {
            return;
        }

        if (player.GeneralSelected && !player.GeneralRevealed)
        {
            player.GeneralRevealed = true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NationalGeneralRevealedEvent(player.Seat, GeneralSelectionSlot.Primary, player.General.Id));
        }

        if (player.SecondaryGeneralSelected && player.SecondaryGeneral is { } secondary && !player.SecondaryGeneralRevealed)
        {
            player.SecondaryGeneralRevealed = true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NationalGeneralRevealedEvent(player.Seat, GeneralSelectionSlot.Secondary, secondary.Id));
        }

        if (!player.FactionRevealed)
        {
            player.FactionRevealed = true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NationalFactionRevealedEvent(player.Seat, player.NationalFactionId));
        }
    }

    internal IReadOnlyList<LegalAction> BuildNationalRevealActions(CharacterState actor)
    {
        var actions = new List<LegalAction>();
        if (!_coreNativeDamageHandlesOperations.IsNationalWarMode || !actor.IsAlive)
        {
            return actions;
        }

        if (actor.GeneralSelected && !actor.GeneralRevealed)
        {
            actions.Add(new LegalAction(LegalActionKind.RevealGeneral, null, null, $"明置【{(actor.General.Name)}】") { GeneralSlot = GeneralSelectionSlot.Primary });
        }

        if (actor.SecondaryGeneralSelected && !actor.SecondaryGeneralRevealed && actor.SecondaryGeneral is not null)
        {
            actions.Add(new LegalAction(LegalActionKind.RevealGeneral, null, null, $"明置【{(actor.SecondaryGeneral.Name)}】") { GeneralSlot = GeneralSelectionSlot.Secondary });
        }

        return actions;
    }
}
