#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanHaiBridgesRuntime
{
    internal Suit? ProgramMultiCardSuit(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? Suit.None : _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards);
    internal bool? ProgramMultiCardColor(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? null : _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards);
}
