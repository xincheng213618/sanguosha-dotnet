#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal SkillProgramStepOutcome BeginBeiMiHuAwakening(ProgramSkillFrame f)
    {
        if (!ExactBeiMiHuLifecycleParent(f) || !CanOfferBeiMiHuLifecycle(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!))
            throw new InvalidOperationException("Baijia lost its own preparation or actual seven acquired cards.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var r = NewBeiMiHuReceipt(f, "maximum-children")with
        {
            CumulativeDraw = BeiMiHuActualCards(f.OwnerSeat),
            MaximumBefore = owner.MaxHp,
            MaximumAfter = checked(owner.MaxHp + 1)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r });
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeiMiHuAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, r.CumulativeDraw, r.MaximumBefore, r.MaximumAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
