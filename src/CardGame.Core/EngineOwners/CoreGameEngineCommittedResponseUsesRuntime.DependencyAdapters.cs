#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreCommittedResponseUsesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreCommittedResponseUsesCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueAcceptedCardResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCommittedResponseUsesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreCommittedResponseUsesCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().IsCompletedResponseUse(action, continuation);
    public void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().ContinueNullificationAfterResponseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime))]
internal sealed class CoreCommittedResponseUsesCharacterSunHaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCharacterSunHaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> _targetFactory = targetFactory;
    public bool HasSunHaoCanshiResponseObligation(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasSunHaoCanshiResponseObligation(action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreCommittedResponseUsesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreCommittedResponseUsesCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginCaoChunPaidResponseDebt(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().TryBeginCaoChunPaidResponseDebt(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreCommittedResponseUsesCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public void CaptureMouGuquResponseUse(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().CaptureMouGuquResponseUse(parent, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreCommittedResponseUsesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommittedResponseUsesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}
