#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuAnGuoOlRuntime
{
    internal bool CanIssueWuAnGuoLiyongDuel(int actor, int target) => _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor);
    internal void BeginWuAnGuoLiyongDuel(ProgramSkillFrame frame, WuAnGuoLiyongDuelOrigin origin)
    {
        if (!MatchesWuAnGuoLiyong(frame, origin) || origin.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 || !CanIssueWuAnGuoLiyongDuel(origin.ActorSeat, origin.TargetSeat) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Liyong requires its exact accepted Yin operation, frozen native gain and legal opposing Duel actor.");
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, origin.ActorSeat, [origin.TargetSeat], CardKind.Duel, physicalCardIds: [], wuAnGuoLiyong: origin);
        if (id != origin.CardUseFrameId)
            throw new InvalidOperationException("Liyong native Duel changed its reserved frame.");
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, origin.ActorSeat, [origin.TargetSeat], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    internal bool IsWuAnGuoLiyongDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { WuAnGuoLiyong: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    [NativeUseReturn(27010)]
    internal void ReturnWuAnGuoLiyongUse(CardUseFrame use)
    {
        if (use.WuAnGuoLiyong is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || use.Id != origin.CardUseFrameId || !MatchesWuAnGuoLiyong(frame, origin))
            throw new InvalidOperationException("Liyong lost its canceled native Duel return.");
        FinishWuAnGuoLiyong(frame, true);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27010)]
    internal bool ReturnWuAnGuoLiyongAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.WuAnGuoLiyong is not { } origin)
            return false;
        if (!origin.AttackStarted || completion.ProgramFrameId != origin.ProgramFrameId || completion.ResolutionId != origin.CardUseFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || !MatchesWuAnGuoLiyong(frame, origin))
            throw new InvalidOperationException("Liyong lost its native response, damage or dying completion.");
        FinishWuAnGuoLiyong(frame, true);
        return true;
    }

    internal bool HasWuAnGuoLiyongCompletedUseParent(long useId) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { WuAnGuoLiyong: { Stage: WuAnGuoLiyongStage.DuelIssued, Duel: { } origin } } frame && origin.CardUseFrameId == useId && MatchesWuAnGuoLiyong(frame, origin) && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardId == 0 && e.CardKind == CardKind.Duel) == 1;
}
