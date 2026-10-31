#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangGaiMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuangGaiMouMobileCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }
}
