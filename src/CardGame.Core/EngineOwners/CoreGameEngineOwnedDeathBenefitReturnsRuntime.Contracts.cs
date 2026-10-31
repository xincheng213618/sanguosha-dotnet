#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOwnedDeathBenefitReturnsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreOwnedDeathBenefitReturnsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreOwnedDeathBenefitReturnsCoreFixedRecipientBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactFixedRecipientDeath(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.ProgramDeathTriggerWindowFrame window);
    global::CardGame.Core.SkillProgramStepOutcome SelectIssuedFixedRecipient(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect);
    global::CardGame.Core.FixedRecipientBenefitIssuedEvent? OriginalFixedRecipient(int owner, string skill, string hash, string state);
    bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreOwnedDeathBenefitReturnsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreOwnedDeathBenefitReturnsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreOwnedDeathBenefitReturnsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreOwnedDeathBenefitReturnsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreOwnedDeathBenefitReturnsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreOwnedDeathBenefitReturnsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOwnedDeathBenefitReturnsCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f);
}
