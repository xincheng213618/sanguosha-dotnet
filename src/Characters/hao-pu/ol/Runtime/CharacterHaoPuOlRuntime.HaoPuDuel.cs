#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoPuOlRuntime
{
    internal bool IsZhenyingDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ZhenyingDuel: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal bool MatchesZhenyingDuel(ProgramSkillFrame f, ZhenyingDuelOrigin d) => ValidZhenying(f) && f.Zhenying is { Stage: ZhenyingStage.DuelIssued, Duel: { } issued } && issued == (d with
    {
        AttackStarted = false
    }

    );
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
    internal bool ReturnZhenyingDuelAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ZhenyingDuel is not { } origin)
            return false;
        if (!origin.AttackStarted || completion.ProgramFrameId != origin.ProgramFrameId || completion.ResolutionId != origin.CardUseFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ProgramFrameId || !MatchesZhenyingDuel(f, origin))
            throw new InvalidOperationException("Zhenying Duel lost its exact native response, damage or dying return.");
        FinishZhenying(f, true);
        return true;
    }
}
