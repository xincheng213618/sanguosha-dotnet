#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreFixedRecipientBenefitsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreFixedRecipientBenefitsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreFixedRecipientBenefitsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed class CoreFixedRecipientBenefitsCoreOwnedDeathBenefitReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCoreOwnedDeathBenefitReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _targetFactory = targetFactory;
    public bool ExactOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ExactOwnedDeathBenefitReturn(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreFixedRecipientBenefitsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreFixedRecipientBenefitsCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFixedRecipientBenefitsCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public bool PairBenefitSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PairBenefitSourceCurrent(f);
    public global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PairBenefitSource(f);
}
