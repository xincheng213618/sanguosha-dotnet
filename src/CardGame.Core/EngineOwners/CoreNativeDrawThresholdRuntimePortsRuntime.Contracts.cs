#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDrawThresholdRuntimePortsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreNativeDrawThresholdRuntimePortsCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MobileShiKuangguThresholdDrawMatches(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.NativeDrawInvocationProof proof);
}
