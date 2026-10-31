#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ProgramRuntimeEventServices(KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IProgramRuntimeEvents
    {
        public void AdvanceFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    }
