#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess : IProgramRuntimeOperations
{
    ref readonly List<ProgramFactionRecoveryDebt> _programFactionRecoveryDebts { get; }

    ref readonly List<ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers { get; }
}
