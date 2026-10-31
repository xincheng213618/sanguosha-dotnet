#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoPuOlRuntime
{
    internal bool IsZhenyingDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ZhenyingDuel: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal bool MatchesZhenyingDuel(ProgramSkillFrame f, ZhenyingDuelOrigin d) => ValidZhenying(f) && f.Zhenying is { Stage: ZhenyingStage.DuelIssued, Duel: { } issued } && issued == (d with
    {
        AttackStarted = false
    }

    );
    internal bool IsZhenyingDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing) => attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && processing.Count == 0 && attack.PhysicalCards.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ZhenyingDuel is { AttackStarted: true } origin && attack.ProgramSkillCardUseFrameId == origin.ProgramFrameId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == origin.ProgramFrameId && MatchesZhenyingDuel(f, origin));
    [NativeUseReturn(26500)]
    internal void ReturnZhenyingDuel(CardUseFrame use)
    {
        if (use.ZhenyingDuel is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || use.Id != origin.CardUseFrameId || !MatchesZhenyingDuel(f, origin))
            throw new InvalidOperationException("Zhenying Duel lost its exact canceled or skipped native use return.");
        FinishZhenying(f, true);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 26500)]
    internal bool ReturnZhenyingDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ZhenyingDuel is not { } origin)
            return false;
        if (!origin.AttackStarted || completion.ProgramFrameId != origin.ProgramFrameId || completion.ResolutionId != origin.CardUseFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ProgramFrameId || !MatchesZhenyingDuel(f, origin))
            throw new InvalidOperationException("Zhenying Duel lost its exact native response, damage or dying return.");
        FinishZhenying(f, true);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 26501)]
    internal void AssertZhenyingDuels(ProgramSkillFrame frame)
    {
        if (frame.Zhenying is null)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.ZhenyingDuel is not null))
        {
            var origin = use.ZhenyingDuel!;
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame f || !MatchesZhenyingDuel(f, origin) || use.Id != origin.CardUseFrameId || !IsZhenyingDuelUse(use.Id) || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveIsRed: false })
                throw new InvalidOperationException("Zhenying requires its exact issued zero-entity native Duel and owning paid adjustment.");
        }
    }
}
