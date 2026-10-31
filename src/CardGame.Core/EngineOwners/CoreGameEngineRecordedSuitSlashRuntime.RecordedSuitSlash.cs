#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecordedSuitSlashRuntime
{
    internal void RevealRecordedSuitSlash(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context, TargetSeat: { } target } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == context.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != context.CardActionId || action.Type != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActorSeat != frame.OwnerSeat || action.EffectiveDesignatedTargetSeats.Count != 1 || action.EffectiveDesignatedTargetSeats[0] != target)
            throw new InvalidOperationException("Recorded-suit Slash lost its exact sole target and owning action.");
        var mask = ProgramRecordedSuitSlash.Suits.Where(suit => _coreProgramPersistentStateOperations.GetProgramBooleanState(frame, ProgramRecordedSuitSlash.StateId(effect.StateId!, suit))).Aggregate(0, (bits, suit) => bits | ProgramRecordedSuitSlash.Bit(suit));
        var count = Math.Max(0, System.Numerics.BitOperations.PopCount((uint)mask) - 1);
        _coreProgramLifecycleOperations.RevealProgramTopCards(frame.Id, frame.OwnerSeat, count, null, effect.ResultBind!, SkillProgramCardSetVisibility.Public);
        var bonus = _coreProgramLifecycleOperations.GetProgramCardSet(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), effect.ResultBind!).CardIds.Count(id => (ProgramRecordedSuitSlash.Bit(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id).Suit) & mask) != 0);
        var receipt = new ProgramRecordedSuitSlashReceipt(action.ActionId, use.Id, frame.Id, action.ActorSeat, target, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, frame.InstructionIndex - 1, effect.StateId!, mask, bonus);
        use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(f => f.Id == use.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { RecordedSuitSlashReceipts = Array.AsReadOnly((use.RecordedSuitSlashReceipts ?? []).Append(receipt).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRecordedSuitSlashIssuedEvent(receipt));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name)}】，亮出 {(count)} 张牌，此【杀】伤害+{(bonus)}，目标不能使用已记录花色的牌响应。", frame.OwnerSeat);
    }

    internal IEnumerable<ProgramRecordedSuitSlashReceipt> RecordedSuitSlashReceipts(long frameId, int target) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == frameId)is { Action: { } action, RecordedSuitSlashReceipts: { } receipts } ? receipts.Where(r => r.CardUseFrameId == frameId && r.ActionId == action.ActionId && r.ActorSeat == action.ActorSeat && r.TargetSeat == target && action.EffectiveDesignatedTargetSeats.Contains(target)) : [];
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 40)]
    internal int RecordedSuitSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is global::CardGame.Core.NativeOwners.CardAttackHandle && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) ? RecordedSuitSlashReceipts(attack.ResolutionId, attack.TargetSeat).Sum(r => r.DamageBonus) : 0;
    internal bool IsRecordedSuitResponseAllowed(CharacterState responder, IReadOnlyList<Card> cards, CardKind kind)
    {
        if (kind != CardKind.Dodge || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) || responder.Seat != attack.TargetSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat != responder.Seat)
            return true;
        var bit = ProgramRecordedSuitSlash.Bit(_coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(responder, cards));
        return !RecordedSuitSlashReceipts(attack.ResolutionId, attack.TargetSeat).Any(r => (r.SuitMask & bit) != 0);
    }

    [NativeUseValidation(10)]
    internal void ClearRecordedSuitSlash(CardUseFrame use)
    {
        foreach (var receipt in use.RecordedSuitSlashReceipts ?? [])
            foreach (var suit in ProgramRecordedSuitSlash.Suits)
                _coreProgramPersistentStateOperations.SetProgramBooleanState(receipt.ActorSeat, receipt.SkillId, receipt.SkillInstanceId, ProgramRecordedSuitSlash.StateId(receipt.StatePrefix, suit), false);
    }

    void global::CardGame.Core.IRecordedSuitSlashProgramHost.RevealRecordedSuitSlash(ProgramSkillFrame frame, SkillProgramEffect effect) => RevealRecordedSuitSlash(frame, effect);
}
