using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Client2.ConnectToServer();

        StartCoroutine(Pinging());
    }

    private IEnumerator Pinging()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1.0f);

            using (var packet = new Packet((int)ClientPackets.PingRequest))
            {
                Client2.sw.Restart();
                packet.Write(Client2.ID);
                Client2.SendData(packet);
            }
        }
    }
}
