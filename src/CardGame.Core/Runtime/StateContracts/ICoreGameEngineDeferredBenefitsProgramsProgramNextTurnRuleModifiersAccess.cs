#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers { get; }
}
