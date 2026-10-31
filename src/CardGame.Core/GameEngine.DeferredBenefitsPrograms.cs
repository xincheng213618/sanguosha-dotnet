namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers => ref _deferredBenefitsPrograms.ReferenceToProgramNextTurnRuleModifiers;
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramFactionRecoveryDebt> _programFactionRecoveryDebts => ref _deferredBenefitsPrograms.ReferenceToProgramFactionRecoveryDebts;
}
