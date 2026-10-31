#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ProgramRuntimeEventServices(global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IProgramRuntimeEvents
    {
        public void AdvanceFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    }
