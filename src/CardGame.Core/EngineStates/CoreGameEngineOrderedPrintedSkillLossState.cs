#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineOrderedPrintedSkillLossState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.OrderedPrintedSkillLossLease> _orderedPrintedSkillLossLeases = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OrderedPrintedSkillLossLease> ReferenceToOrderedPrintedSkillLossLeases => ref _orderedPrintedSkillLossLeases;

    private bool _orderedPrintedSkillLossStateStarted = default !;
    internal ref bool ReferenceToOrderedPrintedSkillLossStateStarted => ref _orderedPrintedSkillLossStateStarted;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OrderedPrintedSkillLossLease> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess._orderedPrintedSkillLossLeases => ref _orderedPrintedSkillLossLeases;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OrderedPrintedSkillLossLease> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossLeases => ref _orderedPrintedSkillLossLeases;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossStateStarted => ref _orderedPrintedSkillLossStateStarted;
}
