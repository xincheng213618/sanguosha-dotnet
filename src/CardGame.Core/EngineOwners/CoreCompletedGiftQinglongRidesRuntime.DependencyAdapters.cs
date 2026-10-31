#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime))]
internal sealed class CoreCompletedGiftQinglongRidesCoreCompletedCardGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedGiftQinglongRidesCoreCompletedCardGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> _targetFactory = targetFactory;
    public bool IsValidCompletedGiftTargetSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsValidCompletedGiftTargetSelection(frame);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CompletedCardGiftChoices(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CompletedCardGiftChoices(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreCompletedGiftQinglongRidesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedGiftQinglongRidesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCompletedGiftQinglongRidesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedGiftQinglongRidesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}
