#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredBenefitsProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.ProgramFactionRecoveryDebt> _programFactionRecoveryDebts = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramFactionRecoveryDebt> ReferenceToProgramFactionRecoveryDebts => ref _programFactionRecoveryDebts;

    private global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> ReferenceToProgramNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramFactionRecoveryDebt> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programFactionRecoveryDebts => ref _programFactionRecoveryDebts;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;
}
