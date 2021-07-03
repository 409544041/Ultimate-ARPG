﻿using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public interface ISocketItemUIElementInsert
    {
        void SetSocket(Socket socket);
        Socket GetSocket();
        int GetIndex();
        void SetIndex(int index);
        void UpdateSocket();
    }
    public class SocketItemInsertUIElement : MonoBehaviour, ISocketItemUIElementInsert
    {

        public Image ThingImage = default;
        public TextMeshProUGUI ThingNameText = default;
        public TextMeshProUGUI ThingDescriptionText = default;
        public Sprite EmptySprite = default;

        Socket socket;
        int index;

        public int GetIndex()
        {
            return index;
        }

        public Socket GetSocket()
        {
            return socket;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public void SetSocket(Socket socket)
        {
            this.socket = socket;
            Setup(socket);
        }

        public void UpdateSocket()
        {
            Setup(socket);
        }

        protected virtual void Setup(Socket socket)
        {
            if (socket == null)
            {
                Debug.LogWarning("socket shouldn't be null and have an instance of it. Something went wrong");
                return;
            }

            SocketItem socketitem = socket.SocketedThing;
            if (socketitem != null)
            {
                ThingImage.sprite = socketitem.GetSprite();
                ThingNameText.SetText(socketitem.GetGeneratedItemName());
                ThingDescriptionText.SetText(socketitem.GetUserDescription());
            }
            else
            {
                ThingImage.sprite = EmptySprite;

            }



        }
      
    }
}