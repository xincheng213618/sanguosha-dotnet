#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredBenefitsProgramsState : Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess, Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess
{
    private List<ProgramFactionRecoveryDebt> _programFactionRecoveryDebts = [];
    internal ref readonly List<ProgramFactionRecoveryDebt> ReferenceToProgramFactionRecoveryDebts => ref _programFactionRecoveryDebts;

    private List<ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers = [];
    internal ref readonly List<ProgramNextTurnRuleModifier> ReferenceToProgramNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;

    ref readonly List<ProgramNextTurnRuleModifier> Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;

    ref readonly List<ProgramFactionRecoveryDebt> Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programFactionRecoveryDebts => ref _programFactionRecoveryDebts;

    ref readonly List<ProgramNextTurnRuleModifier> Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers => ref _programNextTurnRuleModifiers;
}
