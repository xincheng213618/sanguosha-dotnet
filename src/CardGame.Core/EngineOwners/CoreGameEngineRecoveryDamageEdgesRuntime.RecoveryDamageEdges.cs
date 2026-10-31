#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecoveryDamageEdgesRuntime
{
    internal bool TryDescribeRecoveryDamage(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? sourceReference, DamageNature? nature, bool sourceLess, int? explicitSourceSeat, out RecoveryDamageOrigin? origin, out long? damageWindowId) => _coreNativeRecoveryDamageOperations.NativeHealthRecovery.TryDescribeRecoveryDamage(frame, target, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out origin, out damageWindowId);
    internal bool ExactRecoveryProducerEdge(int hpIndex, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.NativeHealthRecovery.ExactRecoveryProducerEdge(hpIndex, history);
    internal bool PausedRecoveryDamageMatches(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? sourceReference, DamageNature? nature) => _coreNativeRecoveryDamageOperations.NativeHealthRecovery.PausedRecoveryDamageMatches(frame, target, amount, sourceReference, nature);
    internal void AssertRecoveryDamageOrigin(ProgramSkillFrame frame) => _coreNativeRecoveryDamageOperations.NativeHealthRecovery.AssertRecoveryDamageOrigin(frame);
}
