namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OrderedPrintedSkillLossLease> _orderedPrintedSkillLossLeases => ref _orderedPrintedSkillLoss.ReferenceToOrderedPrintedSkillLossLeases;
    private ref bool _orderedPrintedSkillLossStateStarted => ref _orderedPrintedSkillLoss.ReferenceToOrderedPrintedSkillLossStateStarted;
}
