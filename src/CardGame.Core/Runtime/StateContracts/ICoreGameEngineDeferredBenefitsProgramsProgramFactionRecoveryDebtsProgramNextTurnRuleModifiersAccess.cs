#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramFactionRecoveryDebt> _programFactionRecoveryDebts { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers { get; }
}
