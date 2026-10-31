#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDistanceNativeQueriesRuntime
{
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 60)]
    internal bool DistanceBorrowedPaidWeaponSlash60GetCombatDistance(ProgramSlashDistanceQuery q) => _coreNativeRuleQueriesOperations.GetCombatDistance(q.Actor.Seat, q.Target.Seat) <= 1;
}
