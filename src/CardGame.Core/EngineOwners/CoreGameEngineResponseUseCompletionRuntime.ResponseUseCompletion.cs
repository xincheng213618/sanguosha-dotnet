#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseUseCompletionRuntime
{
    internal bool TryGetCompletedNullificationDyingCosts(ProgramCardTriggerWindowFrame? window, out int[] costs)
    {
        costs = [];
        if (window?.CompletedResponseReturn?.Kind != ProgramCompletedResponseKind.Nullification || _coreDyingCursorRuntimeOperations.ActiveDying is not { Continuation: DyingContinuationKind.ProgramSkill, KillerSeat: null } dying)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == window.Id);
        if (index < 0 || index + 2 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame program || program.WindowContext?.ParentFrameId != window.Id || program.Id != dying.ParentFrameId || _resolutionStackOperations._resolutionStack[index + 2] is not DyingFrame child || child.Id != dying.Id || child.ParentFrameId != program.Id || child.VictimSeat != dying.VictimSeat || child.KillerSeat != dying.KillerSeat || child.ResponderIndex != dying.ResponderIndex || !child.ResponderSeats.SequenceEqual(dying.ResponderSeats))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.LoseHp || global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(program, effect.Target) != dying.VictimSeat)
            return false;
        if (index + 3 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 3] is CardUseFrame rescue)
        {
            if (rescue.DyingResponse is not { } response || response.ResolutionId != dying.Id || response.ResponderSeat != dying.ResponderSeat || rescue.SourceSeat != response.ResponderSeat || rescue.Action is not { Type: CardActionType.Use } action || action.ActorSeat != rescue.SourceSeat || action.ProviderSeat != rescue.SourceSeat || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.CardKind is not (CardKind.Peach or CardKind.Alcohol) || action.EffectiveKind != rescue.CardKind || !action.EffectiveDesignatedTargetSeats.SequenceEqual(rescue.TargetSeats) || !action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(rescue.PhysicalCardIds ?? [rescue.CardId]) || (rescue.CardKind == CardKind.Peach ? !response.UsedPeach : !response.UsedAlcohol || rescue.SourceSeat != dying.VictimSeat))
                return false;
            costs = (rescue.PhysicalCardIds ?? [rescue.CardId]).Where(id => _cardZonesOperations._cardZones.GetLocation(id).Zone == CardZoneKind.Processing).ToArray();
        }

        return true;
    }

    internal static bool IsNullificationResponseProgramWindow(ProgramCardTriggerWindowFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ResponseUseCompletion.IsNullificationResponseProgramWindow(frame);
    internal bool IsCompletedResponseUse(CardActionContext action, ProgramCardContinuation continuation) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ResponseUseCompletion.IsCompletedResponseUse(action, continuation, _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action));
    internal bool IsCompletedResponseParentConsistent(ProgramCardTriggerWindowFrame window)
    {
        if (window.CompletedResponseReturn?.Kind == ProgramCompletedResponseKind.Nullification)
        {
            var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow;
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is NullificationWindowFrame suspended && _characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCounterspell(suspended))
                pending = suspended;
            if (pending is null || window.ParentFrameId != pending.Id || window.Action.OpponentSeat != pending.SourceSeat)
                return false;
            return _coreDelayedNullificationOperations.HasNullificationEffectOwner(pending) && window.Action.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(pending);
        }

        if (window.CompletedResponseReturn?.Kind != ProgramCompletedResponseKind.Dodge || window.AttackOwnerFrameId is not { } ownerId || window.ParentFrameId != ownerId)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == ownerId);
        return use?.CardAttack is { } attack && window.Action.ActorSeat == attack.TargetSeat && window.Action.OpponentSeat == attack.SourceSeat && window.Action.ParentActionId == use.Action?.ActionId;
    }

    internal bool IsCompletedResponseUse(CardActionContext action, ProgramCompletedResponseKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ResponseUseCompletion.IsCompletedResponseUse(action, kind, _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action));
    internal bool HasResponseUseCompletionObserver(CardActionContext action, ProgramCardContinuation continuation) => IsCompletedResponseUse(action, continuation) && (_programPipelineHooksOperations._programPipelineHooks.ObservesResponseUseCompletion(action) || _characterLiFengOlOperations.HasJiyunResponseCompletion(action, continuation) || _coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && (_coreNationalSkillsOperations.GetSkillBindingShard(owner)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted) ?? []).Any(binding => binding.Trigger.IncludeResponseUses)));
    internal bool TryBeginCompletedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        _characterCaoXiOlOperations.CompleteCaoXiNativeCounterspellBeforeUseCompleted(action, continuation);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !HasResponseUseCompletionObserver(action, continuation))
            return false;
        // Arrow Barrage Dodge and supplied faction cards are response-only. A Dodge use
        // must be the responder's own defense against the actual Slash parent.
        if (continuation == ProgramCardContinuation.Dodge && (attack is null || attack.EffectiveCardKind is not { } incomingKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) || attack.TargetSeat != action.ActorSeat))
            return false;
        var kind = continuation == ProgramCardContinuation.Dodge ? ProgramCompletedResponseKind.Dodge : ProgramCompletedResponseKind.Nullification;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCompleted, [], continuation: null, cardUseCausedDamage: false, completedResponseReturn: new(kind));
    }

    internal Suit? FreezeCompletedResponseUseSuit(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind)
    {
        _characterHuangGaiMouMobileOperations.PrepareMouZhaJiangResponse(owner, cards.Select(c => c.Id).ToArray(), kind);
        if (!_characterMaChaoAnniversaryGodOperations.HasHengwuCapability && !_characterLiFengOlOperations.HasLiFengCapability && !_characterMaMiDiOlOperations.HasMaMiDiCapability && kind is not (CardKind.Dodge or CardKind.Nullification) || !(_characterMaChaoAnniversaryGodOperations.HasHengwuCapability || _characterZouShiLeOperations.HasLeZouShiCapability || _characterMiHengLeOperations.HasSiruiCapability || _characterLiFengOlOperations.HasLiFengCapability || _characterCuiYanOlOperations.HasCuiYanCapability || _characterMaMiDiOlOperations.HasMaMiDiCapability || _characterWuAnGuoOlOperations.HasWuAnGuoCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.CapturesCompletedResponseSuit || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RevealRecordedSuitSlash) || _coreCurrentTurnUseKindsOperations.TracksCurrentTurnUseKinds && kind == CardKind.Nullification || (_coreCurrentTurnUseKindsOperations.TracksActualTurnUseOrdinal || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal) && kind == CardKind.Dodge))
            return null;
        var suits = cards.Select(card => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)).Distinct().ToArray();
        return suits.Length == 1 ? suits[0] : null;
    }

    internal void ContinueNullificationAfterResponseUse(CardActionContext action)
    {
        if (_coreResponseCompletionOperations.TryBeginCardResponseCompleted(null, action, ProgramCardContinuation.NullificationResponse))
            return;
        if (!TryBeginCompletedResponseUsePrograms(null, action, ProgramCardContinuation.NullificationResponse))
            _coreNativeNullificationOperations.ContinueNullificationWindow(_coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("The completed counterspell lost its original chain.")));
    }

    internal void ContinueCompletedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame frame)
    {
        var completed = frame.CompletedResponseReturn ?? throw new InvalidOperationException("The completed response use lost its typed continuation.");
        if (!IsCompletedResponseUse(frame.Action, completed.Kind))
            throw new InvalidOperationException("A completed response use changed its actor or card kind.");
        if (completed.Kind == ProgramCompletedResponseKind.Nullification)
            _coreNativeNullificationOperations.ContinueNullificationWindow(_coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("The completed counterspell lost its original chain.")));
        else
            _coreNativeDodgeOperations.ContinueFinishedCardResponse(attack ?? throw new InvalidOperationException("The completed Dodge lost its original Slash."), frame.Action, ProgramCardContinuation.Dodge);
    }
}
