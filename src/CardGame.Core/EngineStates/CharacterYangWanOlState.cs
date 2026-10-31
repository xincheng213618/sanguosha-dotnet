#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterYangWanOlState : Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess
{
    // Private 追还 arm choices: arm frame id → secretly chosen seat. The map is
    // runtime trusted state; replay rebuilds it from the accepted choice
    // commands exactly like the 闭境 marked-card map.
    private Dictionary<long, int> _zhuihuanArmChoices = [];
    internal ref readonly Dictionary<long, int> ReferenceToZhuihuanArmChoices => ref _zhuihuanArmChoices;

    ref readonly Dictionary<long, int> Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices => ref _zhuihuanArmChoices;
}
