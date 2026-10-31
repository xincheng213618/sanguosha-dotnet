#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePendingDecisionPolicyPipelineCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddMouXqJueyanRequestChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddWeijieRequestChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddZonghuRequestChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddDrawFundedDistinctBasicChoices(global::CardGame.Core.PendingDecision? prompt);
}

internal interface ICorePendingDecisionPolicyPipelineCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AppendBoundaryLiuFengQinlingChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddHuanpeiRequestChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddRequestedDeckBasicChoices(global::CardGame.Core.PendingDecision? p);
}

internal interface ICorePendingDecisionPolicyPipelineCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? AddChainedStateBasicChoices(global::CardGame.Core.PendingDecision? prompt);
}
