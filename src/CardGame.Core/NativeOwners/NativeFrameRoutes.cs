#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class NativeFrameRoutes(GetNativeTryAdvanceTarget getTryAdvanceTarget) : INativeFrameRoutes
    {
        public bool TryAdvance(ResolutionFrame frame) => getTryAdvanceTarget()(frame);
    }
