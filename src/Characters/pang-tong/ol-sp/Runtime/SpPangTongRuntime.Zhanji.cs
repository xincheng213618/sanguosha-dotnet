using System.Globalization;
namespace CardGame.Core;
internal sealed partial class SpPangTongRuntime
{
 private int[] MatchingZhanjiDrawIndexes(CardMovementBatchContext batch,ProgramTriggerCandidate candidate)
 {
  if(batch.NativeDrawInvocation is not{}p||p.LastBatchId!=batch.Id||p.ParentFrameId!=batch.ParentFrameId||p.OwnerSeat!=candidate.OwnerSeat||p.ActualCount<1||p.ActualTurnNumber!=batch.TurnNumber||p.ActualTurnOwnerSeat!=candidate.OwnerSeat||p.PhaseActorSeat!=candidate.OwnerSeat||p.Phase?.Kind!=ActualDiscardRecoveryPhaseKind.Play||!drawInvoices.IsValid(p)||!ZhanjiDrawEntitlementRuntime.HasEntitlement(p,candidate.OwnerSeat,candidate.SkillId,candidate.BindingId,candidate.SkillInstanceId,candidate.GameplayHash)||p.DirectProducer is{}producer&&producer.OwnerSeat==candidate.OwnerSeat&&producer.SkillId==candidate.SkillId)return [];
  if(history.Read<SpPangTongSegmentPaidEvent>().Any(e=>e.Source.OwnerSeat==candidate.OwnerSeat&&e.Source.SkillId==candidate.SkillId&&e.DrawInvocation==p.InvocationId))return [];
  var last=p.Materials[^1];return batch.Movements.Select((m,i)=>(m,i)).Where(x=>x.m.Sequence==last.MovementSequence&&x.m.CardId==last.CardId&&x.m.CardKind==last.PrintedKind&&x.m.From==CardLocation.DrawPile&&x.m.To==CardLocation.Hand(candidate.OwnerSeat)&&x.m.Reason==p.Reason&&movements.Read().Contains(x.m)).Select(x=>x.i).ToArray();
 }
}

internal sealed partial class SpPangTongRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyMovementIndexes, 100)]
    private int[]? SelectWindowIndexes_MatchingZhanjiDrawIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation))
            return MatchingZhanjiDrawIndexes(batch, candidate);
        return null;
    }
}
