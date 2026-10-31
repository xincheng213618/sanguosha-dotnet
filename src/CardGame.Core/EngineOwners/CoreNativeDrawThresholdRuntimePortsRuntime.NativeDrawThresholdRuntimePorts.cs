#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawThresholdRuntimePortsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.INativeDrawThresholdNativeOperations.BeginDamage(ProgramSkillFrame frame, int target, int amount) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount);
    bool global::CardGame.Core.INativeDrawThresholdNativeOperations.CompletedNativeDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => _characterWeiYanMobileShiOperations.MobileShiKuangguThresholdDrawMatches(producer, proof);
}
