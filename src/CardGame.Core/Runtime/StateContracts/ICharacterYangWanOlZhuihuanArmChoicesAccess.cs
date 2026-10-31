#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterYangWanOlZhuihuanArmChoicesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<long, int> _zhuihuanArmChoices { get; }
}
