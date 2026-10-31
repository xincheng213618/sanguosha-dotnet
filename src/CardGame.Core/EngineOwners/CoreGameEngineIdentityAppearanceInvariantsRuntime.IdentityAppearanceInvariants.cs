#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineIdentityAppearanceInvariantsRuntime
{
    internal bool HasIdentityAppearanceAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        foreach (var appearance in _resolutionStackOperations._resolutionStack.OfType<IdentityAppearanceFrame>())
        {
            if (appearance.Receipt.Entry != IdentityAppearanceEntry.DamageDeduction || appearance.Receipt.ParentFrameId != attack.ResolutionId || !_coreIdentityConcealmentOperations.IdentityAppearanceAttackMatches(appearance.Receipt, attack))
                continue;
            _coreIdentityConcealmentOperations.RequireIdentityAppearance(appearance);
            if (appearance.ActiveWindowId is not { } windowId)
                return true;
            return _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(window => window.Id == windowId && window.IdentityAppearanceFrameId == appearance.Id && window.Window == SkillProgramTriggerWindow.IdentityGeneralAppeared && window.Continuation == ProgramLifecycleContinuation.ResumeIdentityAppearance);
        }

        return false;
    }
}
