using Unity.Netcode;
using Unity.Netcode.Components;

public class ClientNetwork : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

}
