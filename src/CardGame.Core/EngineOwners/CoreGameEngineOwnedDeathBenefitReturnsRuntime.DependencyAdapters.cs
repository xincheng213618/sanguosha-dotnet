#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreFixedRecipientBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreFixedRecipientBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _targetFactory = targetFactory;
    public bool ExactFixedRecipientDeath(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.ProgramDeathTriggerWindowFrame window) => this._targetFactory().ExactFixedRecipientDeath(f, out window);
    public global::CardGame.Core.SkillProgramStepOutcome SelectIssuedFixedRecipient(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().SelectIssuedFixedRecipient(supplied, effect);
    public global::CardGame.Core.FixedRecipientBenefitIssuedEvent? OriginalFixedRecipient(int owner, string skill, string hash, string state) => this._targetFactory().OriginalFixedRecipient(owner, skill, hash, state);
    public bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidFixedRecipientReceipt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreOwnedDeathBenefitReturnsCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDeathBenefitReturnsCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PairBenefitSource(f);
}
