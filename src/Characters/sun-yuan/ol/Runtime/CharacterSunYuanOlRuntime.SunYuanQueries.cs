#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal bool HasSunYuanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSayingPlayStartObtainAndReply) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDongxinDamageDiscardAndReclaim);

    internal static CardConversionSource SunYuanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_yuan_ol_SunYuanQueries.SunYuanSource(f);
    internal static string SunYuanHash<T>(T value) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_yuan_ol_SunYuanQueries.SunYuanHash<T>(value);
    internal bool SunYuanParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (c.Window == SkillProgramTriggerWindow.PlayPhaseStarting)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame w || w.Id != c.ParentFrameId || w.OwnerSeat != f.OwnerSeat || w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.Items[w.ItemIndex].Candidate is not { } a)
                return false;
            return a.OwnerSeat == f.OwnerSeat && a.SkillId == f.SkillId && a.BindingId == f.TriggerId && a.SkillInstanceId == f.SkillInstanceId && a.GameplayHash == f.GameplayHash && a.OccurrenceIndex == c.OccurrenceIndex;
        }

        if (c.Window != SkillProgramTriggerWindow.AfterDamageApplied || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame d || d.Id != c.ParentFrameId || d.ParentFrameId != c.DamageFrameId || d.TriggerWindow != c.Window || d.CandidateIndex < 0 || d.CandidateIndex >= d.Candidates.Count)
            return false;
        var candidate = d.Candidates[d.CandidateIndex].ToProgramCandidate();
        return candidate.OwnerSeat == f.OwnerSeat && candidate.SkillId == f.SkillId && candidate.BindingId == f.TriggerId && candidate.SkillInstanceId == f.SkillInstanceId && candidate.GameplayHash == f.GameplayHash && candidate.OccurrenceIndex == c.OccurrenceIndex && d.TargetSeat == c.TargetSeat && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(a => a.Id == d.ParentFrameId && a.TargetSeat == c.TargetSeat && a.Amount == c.Amount && a.SourceSeat == c.SourceSeat);
    }

    internal IReadOnlyList<Card> SunYuanEligibleCards(int chooser, int holder, bool obtain) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(holder)).Concat(obtain ? _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[holder]) : []).Where(card => (!obtain || !card.IsGeneralWeapon) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, _cardZonesOperations._cardZones.GetLocation(card.Id), obtain ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Discard) && (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(chooser) || obtain || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, card, CardLocation.Hand(chooser), OwnedCardMoveIntent.Discard)) && (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Equipment(chooser) || !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OwnerSeat == chooser && f.SunYuan is not null && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(chooser, f.SkillId, f.SkillInstanceId, card)))).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 27490)]
    internal bool CanOfferSunYuan(SkillProgramTrigger t, ProgramSkillWindowContext c, int owner) => !t.Effects.Any(e => SunYuanComposition.Handles(e.Op)) || _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && (t.Effects[0].Op == SkillProgramEffectOp.ResolveSayingPlayStartObtainAndReply ? c.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == owner && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner && SunYuanEligibleCards(owner, p.Seat, true).Count > 0) : DongxinContextLegal(owner, t.Id, c));
}
