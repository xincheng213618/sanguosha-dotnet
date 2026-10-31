#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddMouXqJueyanRequestChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AddMouXqJueyanRequestChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddWeijieRequestChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AddWeijieRequestChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddZonghuRequestChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AddZonghuRequestChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddDrawFundedDistinctBasicChoices(global::CardGame.Core.PendingDecision? prompt) => this._targetFactory().AddDrawFundedDistinctBasicChoices(prompt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AppendBoundaryLiuFengQinlingChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AppendBoundaryLiuFengQinlingChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddHuanpeiRequestChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AddHuanpeiRequestChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddRequestedDeckBasicChoices(global::CardGame.Core.PendingDecision? p) => this._targetFactory().AddRequestedDeckBasicChoices(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CorePendingDecisionPolicyPipelineCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePendingDecisionPolicyPipelineCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? AddChainedStateBasicChoices(global::CardGame.Core.PendingDecision? prompt) => this._targetFactory().AddChainedStateBasicChoices(prompt);
}
