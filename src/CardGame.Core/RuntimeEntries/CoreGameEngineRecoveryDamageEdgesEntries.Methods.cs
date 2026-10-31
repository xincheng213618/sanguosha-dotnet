#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryDamageEdgesEntries
{
    internal void AssertRecoveryDamageOrigin(ProgramSkillFrame frame) => _resolve().AssertRecoveryDamageOrigin(frame);
    internal bool ExactRecoveryProducerEdge(int hpIndex, IReadOnlyList<IGameEvent> history) => _resolve().ExactRecoveryProducerEdge(hpIndex, history);
    internal bool PausedRecoveryDamageMatches(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? sourceReference, DamageNature? nature) => _resolve().PausedRecoveryDamageMatches(frame, target, amount, sourceReference, nature);
    internal bool TryDescribeRecoveryDamage(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? sourceReference, DamageNature? nature, bool sourceLess, int? explicitSourceSeat, scoped out RecoveryDamageOrigin? origin, scoped out long? damageWindowId) => _resolve().TryDescribeRecoveryDamage(frame, target, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out origin, out damageWindowId);
}
