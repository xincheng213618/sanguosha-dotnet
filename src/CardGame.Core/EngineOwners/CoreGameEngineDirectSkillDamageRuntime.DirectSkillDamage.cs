#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDirectSkillDamageRuntime
{
    internal bool IsDirectSkillDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle && !attack.IsSourceLess && !attack.IsChainPropagation && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == attack.ResolutionId && frame.OwnerSeat == attack.SourceSeat && frame.AttackAttempt is not null);
}
