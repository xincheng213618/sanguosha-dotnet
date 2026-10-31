#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineYaodaoChitongRuntime
{
    internal const string YaodaoChitongSkill = "shanhe:yaodao-chitong";
    SkillProgramStepOutcome global::CardGame.Core.IYaodaoChitongHost.BeginYaodaoChitong(ProgramSkillFrame f) => BeginYaodaoChitong(f);
    SkillProgramStepOutcome global::CardGame.Core.IYaodaoChitongHost.ResolveYaodaoChitong(ProgramSkillFrame f, SkillProgramEffect e) => ResolveYaodaoChitong(f, e);
    internal Card? ActiveYaodaoChitong(int seat, string skillId, string instance) => skillId == YaodaoChitongSkill && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.YaodaoChitong) ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).SingleOrDefault(c => c.Kind == CardKind.YaodaoChitong && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(seat, skillId, instance, c)) : null;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 5790)]
    internal int YaodaoChitongBlackSlashBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack is not global::CardGame.Core.NativeOwners.CardAttackHandle || attack.IsSourceLess || attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || attack.SourceSeat != attack.CardUserSeat || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], CardKind.YaodaoChitong))
            return 0;
        // The accepted native action preserves a same-color, mixed-suit
        // material group; its absent common suit must not erase black color.
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action is { } action)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action) == false ? 1 : 0;
        return attack.Card is { } card && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], card)is Suit.Spade or Suit.Club ? 1 : 0;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5791, SkillProgramEffectOp.BeginYaodaoChitong)]
    internal bool CanOfferYaodaoChitong(ProgramTriggerLegalityContext gate) => ActiveYaodaoChitong(gate.OwnerSeat, gate.SkillId, gate.Candidate.SkillInstanceId)is not null && gate.WindowContext is { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } context } && _coreCardActionsOperations.ProgramCardAttack is { } attack && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && attack.SourceSeat == gate.OwnerSeat && context.ParentCardUseFrameId == attack.ResolutionId && attack.EffectiveCardKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
    internal SkillProgramStepOutcome BeginYaodaoChitong(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.YaodaoChitong is not null || f.InstructionIndex != 1 || ActiveYaodaoChitong(f.OwnerSeat, f.SkillId, f.SkillInstanceId)is not { } weapon || f.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } context } window || _coreCardActionsOperations.ProgramCardAttack is not { } attack || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) || attack.SourceSeat != f.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } action } use || use.SourceSeat != f.OwnerSeat || context.ParentCardUseFrameId != use.Id || context.CardActionId != action.ActionId || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != attack.TargetSeat)
            throw new InvalidOperationException("Yaodao Chitong requires the exact equipped weapon and original source-owned Slash target window.");
        var r = new YaodaoChitongReceipt(new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, weapon.Id, use.Id, action.ActionId, window.ParentFrameId, use.TargetIndex, attack.TargetSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YaodaoChitong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YaodaoChitongStartedEvent(f.Id, r));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool YaodaoChitongTarget(CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == attack.TargetSeat || target == use.SourceSeat || use.TargetSeats.Contains(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return false;
        // This is a replacement of an already paid Use, so preserve its exact
        // material/conversion, quota exemption and distance policy invoice.
        var card = attack.Card ?? new Card(0, use.CardKind, use.Action?.EffectiveSuit ?? Suit.None, use.Action?.EffectiveRank ?? 0);
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target], card, attack.ConversionSource, use.CardKind, existingUseFrameId: use.Id, physicalCardIds: use.Action!.PhysicalCards.Select(c => c.CardId).ToArray());
    }

    internal SkillProgramStepOutcome ResolveYaodaoChitong(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.YaodaoChitong is not { Resolved: false } r || f.InstructionIndex != 3 || f.JudgmentOutcomeBindings?.Where(o => o.Name == effect.SourceBind).ToArray()is not [var outcome] || outcome.TargetSeat != f.OwnerSeat || outcome.Reason != "shanhe.yaodao-chitong" || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, outcome) || _coreCardActionsOperations.ProgramCardAttack is not { } attack || attack.ResolutionId != r.CardUseId || attack.TargetSeat != r.OriginalTargetSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseId)is not { } use || use.TargetIndex != r.TargetIndex)
            throw new InvalidOperationException("Yaodao Chitong lost the exact completed native judgment and its unchanged original Slash target.");
        var candidates = outcome.Suit is Suit.Spade or Suit.Club && _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.OriginalTargetSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? _coreMatchParticipantsPlayersAccess._players.Where(p => YaodaoChitongTarget(use, attack, p.Seat)).Select(p => p.Seat).Order().ToArray() : [];
        var before = _randomOperations._random.State;
        int? selected = candidates.Length == 0 ? null : candidates[_randomOperations._random.Next(candidates.Length)];
        var after = _randomOperations._random.State;
        // The one RNG draw is paid before any later target or cleanup children.
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YaodaoChitong = r with { Resolved = true }, YaodaoChitongRandom = new(before, after, candidates.Length, selected) });
        if (selected is { } next)
        {
            attack.SetDamageParticipants(attack.SourceSeat, next);
            attack.SetIgnoresArmor(_coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]) || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], _coreMatchParticipantsPlayersAccess._players[next], use.CardKind));
            _coreNativeCardUseFramesOperations.RedirectCardUseTarget(use.Id, r.OriginalTargetSeat, next);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YaodaoChitongResolvedEvent(f.Id, use.Id, r.ActionId, r.TargetIndex, r.OriginalTargetSeat, selected, outcome.JudgmentFrameId, outcome.Suit, candidates.Length));
        return SkillProgramStepOutcome.Continue;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5791)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5791)]
    internal bool ContinueIssuedYaodaoChitong(ProgramSkillFrame f)
    {
        if (f.YaodaoChitong is null)
            return false;
        return true;
    }
}
