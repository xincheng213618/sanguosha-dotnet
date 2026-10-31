#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongMobileRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IMobileGuYongHost.CompleteMixedHand(ProgramSkillFrame supplied, string binding)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "mobile:bingyi" || frame.TriggerId != "ending-reveal-and-share" || frame.InstructionIndex != 3 || binding != "shown-hand" || _coreProgramLifecycleOperations.GetProgramCardSet(frame, binding).Visibility != SkillProgramCardSetVisibility.Public)
            throw new InvalidOperationException("Bingyi requires its original already-revealed whole-hand binding.");
        if (_coreProgramCategoryChallengesOperations.AreProgramBoundCardsSameColor(frame, binding))
            return SkillProgramStepOutcome.Continue;
        _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: true);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
