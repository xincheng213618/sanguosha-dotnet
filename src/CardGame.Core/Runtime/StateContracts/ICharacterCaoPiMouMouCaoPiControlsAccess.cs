#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterCaoPiMouMouCaoPiControlsAccess : IProgramRuntimeOperations
{
    ref readonly List<MouCaoPiControl> _mouCaoPiControls { get; }
}
