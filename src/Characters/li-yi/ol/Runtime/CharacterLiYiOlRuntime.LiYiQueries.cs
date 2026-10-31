#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiYiOlRuntime
{
    internal const string ChanshuangDiscardReason = "skill-program.li-yi.chanshuang.discard";
    internal bool HasLiYiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveChanshuangEnding);

    internal bool HasLiYiVirtualStoneAxe(CharacterState player, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null) => player.IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.EmptyWeaponStoneAxe) && !_corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Any(c => c.Id != excludedEquipmentId && (excludedEquipmentSlot is null || _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot != excludedEquipmentSlot) && _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.Weapon);
    internal CardConversionSource ChanshuangSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static string ChanshuangHash<T>(T value) => global::CardGame.Core.RuntimeSkills.RuntimeRules_li_yi_ol_LiYiQueries.ChanshuangHash<T>(value);
    internal IGameEvent[] ChanshuangActualTurnHistory()
    {
        if (!HasLiYiCapability || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            return[];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var boundary = Array.FindLastIndex(history, e => e is ActualTurnCursorReturnedEvent);
        if (boundary >= 0 && history[boundary] is ActualTurnCursorReturnedEvent end && end.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && end.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return[];
        return history.Skip(boundary + 1).ToArray();
    }

    internal int CurrentChanshuangMask(int actor)
    {
        var history = ChanshuangActualTurnHistory();
        var mask = 0;
        if (history.OfType<NativeRecastInvocationRecordedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ExecutorSeat == actor && e.ActualRecastCount == 1))
            mask |= 1;
        if (history.OfType<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ExecutorSeat == actor && e.ActualCount == 2))
            mask |= 4;
        foreach (var group in history.OfType<CardUseDeclaredEvent>().Where(e => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(e.CardKind)).GroupBy(e => e.ResolutionId))
        {
            var declared = group.First();
            if (group.Any(e => e != declared))
                throw new InvalidOperationException("Chanshuang requires one consistent native Slash declaration.");
            var seat = declared.SourceSeat;
            int? provider = null;
            foreach (var change in history.OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == declared.ResolutionId))
            {
                if (change.PreviousActorSeat != seat || change.OwnerSeat != seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ProviderSeat) || provider is { } previous && previous != change.ProviderSeat)
                    throw new InvalidOperationException("Chanshuang lost the final actual Slash actor's native replacement chain.");
                seat = change.ActorSeat;
                provider = change.ProviderSeat;
            }

            if (seat == actor)
            {
                mask |= 2;
                break;
            }
        }

        return mask;
    }

    internal static int ChanshuangItemCount(ChanshuangReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_li_yi_ol_LiYiQueries.ChanshuangItemCount(r);
    internal static ChanshuangVote ChanshuangItem(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_li_yi_ol_LiYiQueries.ChanshuangItem(f);
    internal int ChanshuangChooser(ProgramSkillFrame f) => f.Chanshuang is { Stage: ChanshuangStage.Votes, Votes.Count: 1 } r ? r.PeerSeat : f.Chanshuang!.Stage == ChanshuangStage.Votes ? f.OwnerSeat : ChanshuangItem(f).Seat;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27481)]
    internal bool CanOfferChanshuangEnding(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveChanshuangEnding }] || context.Window == SkillProgramTriggerWindow.TurnEnding && context.OwnerSeat == c.OwnerSeat && context.SourceSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == c.OwnerSeat && CurrentChanshuangMask(c.OwnerSeat) != 0;
    internal bool TryGetChanshuangSlashRequest(ProgramSkillFrame f, out int actorSeat, out IReadOnlyList<int> targets)
    {
        actorSeat = -1;
        targets = [];
        if (f.Chanshuang is not { SlashTarget: { } target } r || r.Stage is not (ChanshuangStage.SlashChoice or ChanshuangStage.SlashIssuing or ChanshuangStage.NativeUse or ChanshuangStage.NativeChildren) || r.Cursor >= ChanshuangItemCount(r) || ChanshuangItem(f)is not { Option: ChanshuangOption.Slash } item || r.Source != ChanshuangSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == r.Operation && e.ActualTurn == r.ActualTurn && e.ActualTurnOwner == r.ActualTurnOwner && e.PeerSeat == r.PeerSeat && e.FrozenMask == r.FrozenMask) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangSlashSelectedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.ActorSeat == item.Seat && e.TargetSeat == target) != 1)
            return false;
        actorSeat = item.Seat;
        targets = Array.AsReadOnly(new[] { target });
        return true;
    }

    internal bool IsChanshuangSlashRequest(ProgramSkillFrame f) => f.Chanshuang?.Stage == ChanshuangStage.SlashChoice && TryGetChanshuangSlashRequest(f, out _, out _);
    internal bool IsChanshuangSlashQuery(CharacterState actor) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Chanshuang: { } r } f && r.Stage is ChanshuangStage.SlashTarget or ChanshuangStage.SlashChoice or ChanshuangStage.SlashIssuing && r.Cursor < ChanshuangItemCount(r) && ChanshuangItem(f)is { Option: ChanshuangOption.Slash } item && item.Seat == actor.Seat && r.Source == ChanshuangSource(f) && r.GameplayHash == f.GameplayHash;
    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeBaseTerms, 10)]
    internal void AddLiYiAttackRangeBaseTerms(ProgramAttackRangeQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (HasLiYiVirtualStoneAxe(query.Player, query.ExcludedEquipmentId, query.ExcludedEquipmentSlot) && terms[0].Value < 3)
            terms[0] = new RuleQueryBaseTerm($"skill:{(query.Player.Seat)}:zhanjin:attack-range", 3);
    }

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 60)]
    internal bool ChanshuangSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => IsChanshuangSlashQuery(query.Actor);
}
