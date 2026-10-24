namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasShiPxCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiPxXuye);

    private static bool IsShiPxRiderTrigger(SkillProgramTrigger trigger) =>
        trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShiPxRider);

    // The rider watches both swap sides: one candidate per committed batch whose
    // hand movements complete either side's full loss of its swapped-in cards.
    private IEnumerable<ProgramTriggerCandidate> CollectShiPxRiderCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _players.Where(player => player.IsAlive))
        foreach (var candidate in CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
        {
            var trigger = GetProgramTrigger(candidate);
            if (!IsShiPxRiderTrigger(trigger)) continue;
            var swap = ShiPxActiveSwap(candidate.OwnerSeat);
            if (swap is null || !HasRuntimeSkill(_players[candidate.OwnerSeat], ShiPangXiComposition.KuangxiangSkill)) continue;
            if (ShiPxRiderSide(swap, batch.Movements) is null) continue;
            yield return candidate;
            break;
        }
    }
}
