namespace CardGame.Core;
public sealed partial class GameEngine
{
    private readonly ProgramRuleObservers _programRuleObservers;
    private ref global::CardGame.Core.RuleProgressionRuntime? _ruleProgression => ref _ruleProgression2.ReferenceToRuleProgression;
    private RuleProgressionRuntime RuleProgression => _ruleProgression ??= new(_programRuleObservers, this.ComposeNativeRuleProgressionOperations());

    private void AdvanceEventRulesAndQueueFact(IGameEvent payload) => RuleProgression.AdvanceFact(payload);
    private void AdvanceRulesAndPublishState() => RuleProgression.AdvanceState();
}
