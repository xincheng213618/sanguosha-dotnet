#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDamageCursorRuntimeRuntime
{
    internal DamageTriggerWindowFrame? ActiveDamageTrigger => _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack ? _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(window => _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(damage => damage.Id == window.ParentFrameId && damage.ParentFrameId == attack.ResolutionId && damage.SourceSeat == window.SourceSeat && damage.TargetSeat == window.TargetSeat) && window.SourceCardId == attack.Card?.Id && window.SourceCard == attack.EffectiveCardKind) : null;
}
