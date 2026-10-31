#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryDamageEdgesEntries
{
    internal void AssertRecoveryDamageOrigin(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRecoveryDamageOrigin(frame);
    internal bool ExactRecoveryProducerEdge(int hpIndex, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ExactRecoveryProducerEdge(hpIndex, history);
    internal bool PausedRecoveryDamageMatches(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference, global::CardGame.Core.DamageNature? nature) => _resolve().PausedRecoveryDamageMatches(frame, target, amount, sourceReference, nature);
    internal bool TryDescribeRecoveryDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSourceSeat, scoped out global::CardGame.Core.RecoveryDamageOrigin? origin, scoped out long? damageWindowId) => _resolve().TryDescribeRecoveryDamage(frame, target, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out origin, out damageWindowId);
}
